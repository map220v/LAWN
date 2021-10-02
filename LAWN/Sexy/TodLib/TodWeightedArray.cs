using System.Collections.Generic;

namespace Sexy.TodLib
{
	internal class TodWeightedArray
	{
		public object mItem;

		public int mWeight;

		private static Stack<TodWeightedArray> unusedObjects = new Stack<TodWeightedArray>();

		public static TodWeightedArray GetNewTodWeightedArray()
		{
			if (unusedObjects.Count > 0)
			{
				return unusedObjects.Pop();
			}
			return new TodWeightedArray();
		}

		public void PrepareForReuse()
		{
			Reset();
			unusedObjects.Push(this);
		}

		private TodWeightedArray()
		{
		}

		public void Reset()
		{
			mItem = null;
			mWeight = 0;
		}
	}
}
