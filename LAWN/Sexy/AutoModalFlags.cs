namespace Sexy
{
	public struct AutoModalFlags
	{
		public ModalFlags mModalFlags;

		public int mOldOverFlags;

		public int mOldUnderFlags;

		public AutoModalFlags(ModalFlags theModalFlags, FlagsMod theFlagMod)
		{
			mModalFlags = theModalFlags;
			mOldOverFlags = theModalFlags.mOverFlags;
			mOldUnderFlags = theModalFlags.mUnderFlags;
			theModalFlags.ModFlags(ref theFlagMod);
		}

		public void Dispose()
		{
			mModalFlags.mOverFlags = mOldOverFlags;
			mModalFlags.mUnderFlags = mOldUnderFlags;
		}
	}
}
