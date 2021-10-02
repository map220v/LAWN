namespace Sexy.TodLib
{
	internal class FoleyTypeData
	{
		public FoleyInstance[] mFoleyInstances = new FoleyInstance[8];

		public int mLastVariationPlayed;

		public FoleyTypeData()
		{
			mLastVariationPlayed = -1;
			for (int i = 0; i < 8; i++)
			{
				mFoleyInstances[i] = new FoleyInstance();
			}
		}
	}
}
