// *
// *****************************************************************************
// *
// * Copyright (c) 2025 Deskasoft International
// *
// * Permission is hereby granted, free of charge, to any person obtaining a copy
// * of this software and associated documentation files (the "Software"), to deal
// * in the Software without restriction, including without limitation the rights
// * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// * copies of the Software, and to permit persons to whom the Software is
// * furnished to do so, subject to the following conditions:
// *
// * The above copyright notice and this permission notice shall be included in all
// * copies or substantial portions of the Software.
// *
// * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// * SOFTWARE.
// *
// *
// * Please refer to LICENSE file.
// *
// ******************************************************************************
// *

using System;
using System.Threading;
using System.Threading.Tasks;

namespace HashifyNet.Core
{
	/// <summary>
	/// Base implementation for an internal state of an iteratively computable hash function value.
	/// <para>Provides buffering and cancellation handling features.</para>
	/// </summary>
	/// <typeparam name="TSelf">The final derived type to use when cloning.</typeparam>
	public abstract partial class BlockTransformerBase<TSelf>
		: IBlockTransformer
		where TSelf : BlockTransformerBase<TSelf>, new()
	{
		/// <summary>
		/// The default number of bytes to process before re-checking the cancellation token.
		/// </summary>
		protected const int DefaultCancellationBatchSize = 4096;

		/// <summary>
		/// The default block size to pass to <see cref="TransformByteGroupsInternal(ReadOnlySpan{byte})"/>.
		/// </summary>
		protected const int DefaultInputBlockSize = 1;

		private readonly int _cancellationBatchSize;
		private readonly int _inputBlockSize;

		private readonly byte[] _leftover = null;
		private int _leftoverCount = 0;
		private bool _isCorrupted = false;

		/// <summary>
		/// Construct <see cref="BlockTransformerBase{TSelf}"/> with optional parameters to configure features.
		/// </summary>
		/// <param name="cancellationBatchSize">Maximum number of bytes to process before re-checking the cancellation token.</param>
		/// <param name="inputBlockSize">Block size to pass to <see cref="TransformByteGroupsInternal(ReadOnlySpan{byte})"/>.</param>
		protected BlockTransformerBase(int cancellationBatchSize = DefaultCancellationBatchSize, int inputBlockSize = DefaultInputBlockSize)
		{
			_cancellationBatchSize = cancellationBatchSize;
			_inputBlockSize = inputBlockSize;

			_leftover = new byte[_inputBlockSize];

			// Ensure _cancellationBatchSize is a multiple of _inputBlockSize, preferrably rounding down.
			{
				var blockSizeRemainder = _cancellationBatchSize % _inputBlockSize;

				if (blockSizeRemainder > 0)
				{
					_cancellationBatchSize -= blockSizeRemainder;

					if (_cancellationBatchSize <= 0)
					{
						_cancellationBatchSize += _inputBlockSize;
					}
				}
			}
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="data"><inheritdoc/></param>
		public void TransformBytes(byte[] data) => TransformBytes(data, CancellationToken.None);

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="data"><inheritdoc/></param>
		/// <param name="cancellationToken"><inheritdoc/></param>
		/// <exception cref="ArgumentNullException"><inheritdoc/></exception>
		public void TransformBytes(byte[] data, CancellationToken cancellationToken)
		{
			ThrowIfCorrupted();

			if (data == null)
			{
				throw new ArgumentNullException(nameof(data));
			}

			TransformBytes(data, 0, data.Length, CancellationToken.None);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="data"><inheritdoc/></param>
		/// <param name="offset"><inheritdoc/></param>
		/// <param name="count"><inheritdoc/></param>
		public void TransformBytes(byte[] data, int offset, int count) =>
			TransformBytes(data, 0, data.Length, CancellationToken.None);

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="data"><inheritdoc/></param>
		/// <param name="offset"><inheritdoc/></param>
		/// <param name="count"><inheritdoc/></param>
		/// <param name="cancellationToken"><inheritdoc/></param>
		/// <exception cref="ArgumentNullException"><inheritdoc/></exception>
		/// <exception cref="ArgumentException"><inheritdoc/></exception>
		/// <exception cref="ArgumentOutOfRangeException"><inheritdoc/></exception>
		public void TransformBytes(byte[] data, int offset, int count, CancellationToken cancellationToken)
		{
			ThrowIfCorrupted();

			if (data == null)
			{
				throw new ArgumentNullException(nameof(data));
			}

			TransformBytes(data.AsSpan(offset, count), cancellationToken);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="data"><inheritdoc/></param>
		public void TransformBytes(ReadOnlySpan<byte> data) =>
			TransformBytes(data, CancellationToken.None);

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="data"><inheritdoc/></param>
		/// <param name="cancellationToken"><inheritdoc/></param>
		/// <exception cref="ArgumentException"><inheritdoc/></exception>
		public void TransformBytes(ReadOnlySpan<byte> data, CancellationToken cancellationToken)
		{
			if (data.IsEmpty)
			{
				throw new ArgumentException("data must point to a non-empty valid ReadOnlySpan<byte>.", nameof(data));
			}

			TransformBytesInternal(data, cancellationToken);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public IHashValue FinalizeHashValue() =>
			FinalizeHashValue(CancellationToken.None);

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="cancellationToken"><inheritdoc/></param>
		/// <returns><inheritdoc/></returns>
		public IHashValue FinalizeHashValue(CancellationToken cancellationToken)
		{
			ThrowIfCorrupted();

			try
			{
				cancellationToken.ThrowIfCancellationRequested();
				try
				{
					return FinalizeHashValueInternal(FinalizeInputBuffer(), cancellationToken);
				}
				finally
				{
					cancellationToken.ThrowIfCancellationRequested();
				}
			}
			catch (TaskCanceledException)
			{
				MarkSelfCorrupted();
				throw;
			}
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public IBlockTransformer Clone()
		{
			ThrowIfCorrupted();

			var clone = new TSelf();
			CopyStateTo(clone);

			return clone;
		}

		/// <summary>
		/// Copies the internal state of the current instance to the provided instance.
		/// </summary>
		/// <param name="other">The instance to copy the internal state into.</param>
		/// <remarks>All overriders should ensure base.CopyStateTo(other) is called.</remarks>
		protected virtual void CopyStateTo(TSelf other)
		{
			if (_leftoverCount > 0)
			{
				Buffer.BlockCopy(_leftover, 0, other._leftover, 0, _leftoverCount);
			}
		}

		/// <summary>
		/// Throws an <see cref="InvalidOperationException"/> if this transformer is marked as being corrupted.
		/// </summary>
		/// <exception cref="InvalidOperationException">A previous transformation cancellation has resulted in an undefined internal state.</exception>
		protected void ThrowIfCorrupted()
		{
			if (_isCorrupted)
			{
				throw new InvalidOperationException("A previous transformation cancellation has resulted in an undefined internal state.");
			}
		}

		/// <summary>
		/// Marks this instance as corrupted and will prevent all calls on this instance from completing successfully.
		/// </summary>
		protected void MarkSelfCorrupted()
		{
			_isCorrupted = true;
		}

		/// <summary>
		/// Updates the internal state of this transformer with the given data.
		/// </summary>
		/// <param name="data">The data to process into this transformer's internal state.</param>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/> to cease processing of the provided data.</param>
		protected void TransformBytesInternal(ReadOnlySpan<byte> data, CancellationToken cancellationToken)
		{
			try
			{
				if (_leftoverCount > 0)
				{
					int fit = _inputBlockSize - _leftoverCount;

					Span<byte> newLeftOverS = _leftover.AsSpan().Slice(_leftoverCount, fit);
					if (data.Length >= fit)
					{
						ReadOnlySpan<byte> fill = data.Slice(0, fit);
						fill.CopyTo(newLeftOverS);
						TransformByteGroupsInternal(new ReadOnlySpan<byte>(_leftover, 0, _inputBlockSize));
						_leftoverCount = 0;
						data = data.Slice(fit);

						// If no more data, return now.
						if (data.IsEmpty)
						{
							return;
						}
					}
					else
					{
						data.CopyTo(newLeftOverS);
						_leftoverCount += data.Length;
						return;
					}
				}

				int bytesSinceLastCancelCheck = 0;
				while (data.Length >= _inputBlockSize)
				{
					ReadOnlySpan<byte> blockToProcess = data.Slice(0, _inputBlockSize);
					TransformByteGroupsInternal(blockToProcess);

					bytesSinceLastCancelCheck += _inputBlockSize;
					if (bytesSinceLastCancelCheck >= _cancellationBatchSize)
					{
						cancellationToken.ThrowIfCancellationRequested();
						bytesSinceLastCancelCheck = 0;
					}

					data = data.Slice(_inputBlockSize);
				}

				// Ensure a single call at least to make sure no cancellation is requested.
				cancellationToken.ThrowIfCancellationRequested();

				if (data.Length > 0)
				{
					data.CopyTo(_leftover);
					_leftoverCount = data.Length;
				}
			}
			catch (TaskCanceledException)
			{
				MarkSelfCorrupted();
				throw;
			}
		}

		private ReadOnlySpan<byte> FinalizeInputBuffer()
		{
			if (_leftoverCount < 1)
			{
				return ReadOnlySpan<byte>.Empty;
			}

			var result = new ReadOnlySpan<byte>(_leftover, 0, _leftoverCount);
			_leftoverCount = 0;
			return result;
		}

		/// <summary>
		/// Updates the internal state of this transformer with the given data.
		/// 
		/// The data's size will be a multiple of the provided inputBlockSize in the constructor.
		/// </summary>
		/// <param name="data">The data to process into this transformer's internal state.</param>
		protected abstract void TransformByteGroupsInternal(ReadOnlySpan<byte> data);

		/// <summary>
		/// Completes any finalization processing and returns the resulting <see cref="IHashValue"/>.
		/// </summary>
		/// <remarks>Internal state will remain unmodified, therefore this method will not invalidate future calls to any other TransformBytes or FinalizeTransformation calls.</remarks>
		/// <exception cref="InvalidOperationException">A previous transformation cancellation has resulted in an undefined internal state.</exception>
		protected abstract IHashValue FinalizeHashValueInternal(ReadOnlySpan<byte> leftover, CancellationToken cancellationToken);
	}
}
