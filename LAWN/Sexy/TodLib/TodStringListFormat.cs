namespace Sexy.TodLib
{
	internal class TodStringListFormat
	{
		public SexyColor mBaseColor = default(SexyColor);

		public string mFormatName;

		public Font mNewFont;

		public SexyColor mNewColor = default(SexyColor);

		public int mLineSpacingOffset;

		public uint mFormatFlags;

		public TodStringListFormat()
		{
			Reset();
		}

		public void Reset()
		{
			mFormatName = null;
			mNewFont = null;
			mNewColor = default(SexyColor);
			mBaseColor = default(SexyColor);
			mLineSpacingOffset = 0;
			mFormatFlags = 0u;
		}

		public TodStringListFormat(string aFormatName, Font aNewFont, SexyColor aNewColor, int aLineSpacingOffset, uint aFormatFlags)
		{
			mFormatName = aFormatName;
			mNewFont = aNewFont;
			mNewColor = aNewColor;
			mBaseColor = aNewColor;
			mLineSpacingOffset = aLineSpacingOffset;
			mFormatFlags = aFormatFlags;
		}
	}
}
