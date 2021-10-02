using System.Collections.Generic;

namespace Sexy.TodLib
{
	internal class TodWeightedGridArray
	{
		public int mX;

		public int mY;

		public int mWeight;

		private static Stack<TodWeightedGridArray> unusedObjects = new Stack<TodWeightedGridArray>();

		public static TodWeightedGridArray GetNewTodWeightedGridArray()
		{
			if (unusedObjects.Count > 0)
			{
				return unusedObjects.Pop();
			}
			return new TodWeightedGridArray();
		}

		private TodWeightedGridArray()
		{
		}

		public void Reset()
		{
			mX = (mY = (mWeight = 0));
		}

		public void PrepareForReuse()
		{
			Reset();
			unusedObjects.Push(this);
		}
	}
}
