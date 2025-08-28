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

namespace HashifyNet.UnitTests.Mocks
{
	/// <summary>
	/// Forces async functions to actually run asynchronously by delaying the read.
	/// </summary>
	public class SlowAsyncStream
		: Stream
	{
		public override bool CanRead { get { return _underlyingStream.CanRead; } }
		public override bool CanSeek { get { return _underlyingStream.CanSeek; } }
		public override bool CanTimeout { get { return _underlyingStream.CanTimeout; } }
		public override bool CanWrite { get { return _underlyingStream.CanTimeout; } }

		public override long Length { get { return _underlyingStream.Length; } }

		public override long Position
		{
			get { return _underlyingStream.Position; }
			set { _underlyingStream.Position = value; }
		}

		public override int ReadTimeout
		{
			get { return _underlyingStream.ReadTimeout; }
			set { _underlyingStream.ReadTimeout = value; }
		}

		public override int WriteTimeout
		{
			get { return _underlyingStream.WriteTimeout; }
			set { _underlyingStream.WriteTimeout = value; }
		}


		private readonly Stream _underlyingStream;

		public SlowAsyncStream(Stream underlyingStream)
		{
			_underlyingStream = underlyingStream;
		}

		public override async Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
		{
			await Task.Yield();

			await _underlyingStream.CopyToAsync(destination, bufferSize, cancellationToken);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_underlyingStream.Dispose();
			}
		}

		public override void Flush()
		{
			_underlyingStream.Flush();
		}

		public override async Task FlushAsync(CancellationToken cancellationToken)
		{
			await Task.Yield();

			await _underlyingStream.FlushAsync(cancellationToken);
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return _underlyingStream.Read(buffer, offset, count);
		}

		public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			await Task.Yield();

			return await _underlyingStream.ReadAsync(buffer, offset, count, cancellationToken);
		}

		public override int ReadByte()
		{
			return _underlyingStream.ReadByte();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return _underlyingStream.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			_underlyingStream.SetLength(value);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			_underlyingStream.Write(buffer, offset, count);
		}

		public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			await Task.Yield();

			await _underlyingStream.WriteAsync(buffer, offset, count, cancellationToken);
		}

		public override void WriteByte(byte value)
		{
			_underlyingStream.WriteByte(value);
		}
	}
}