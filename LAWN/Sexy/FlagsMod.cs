namespace Sexy
{
	public struct FlagsMod
	{
		public int mAddFlags;

		public int mRemoveFlags;

		public void CopyFrom(FlagsMod source)
		{
			mAddFlags = source.mAddFlags;
			mRemoveFlags = source.mRemoveFlags;
		}
	}
}
