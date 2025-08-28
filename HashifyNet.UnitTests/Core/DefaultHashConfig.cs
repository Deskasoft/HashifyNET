namespace HashifyNet.UnitTests
{
	public sealed class DefaultHashConfig : IHashConfig<DefaultHashConfig>
	{
		public int HashSizeInBits { get; set; } = -1;
		public DefaultHashConfig(int hashSizeInBits)
		{
			HashSizeInBits = hashSizeInBits;
		}

		public DefaultHashConfig()
		{
		}

		public DefaultHashConfig Clone() => new DefaultHashConfig(this.HashSizeInBits);
	}
}
