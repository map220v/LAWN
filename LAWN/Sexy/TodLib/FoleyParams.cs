namespace Sexy.TodLib
{
	internal class FoleyParams
	{
		public FoleyType mFoleyType;

		public float mPitchRange;

		public int[] mSfxID = new int[10];

		public uint mFoleyFlags;

		public FoleyParams(FoleyType aFoleyType, float aPitchRange, int[] aIDs, uint aFoleyFlags)
		{
			mFoleyType = aFoleyType;
			mPitchRange = aPitchRange;
			mSfxID = aIDs;
			mFoleyFlags = aFoleyFlags;
		}
	}
}
