using Sexy;
using Sexy.TodLib;

namespace Lawn
{
	internal class BungeeDropGrid
	{
		public TodWeightedGridArray[] mGridArray = new TodWeightedGridArray[Constants.GRIDSIZEX * Constants.MAX_GRIDSIZEY];

		public int mGridArrayCount;

		public BungeeDropGrid()
		{
			for (int i = 0; i < mGridArray.Length; i++)
			{
				mGridArray[i] = TodWeightedGridArray.GetNewTodWeightedGridArray();
			}
		}
	}
}
