namespace Lawn
{
	internal class LevelStats
	{
		public int mUnusedLawnMowers;

		public LevelStats()
		{
			Reset();
		}

		public void Reset()
		{
			mUnusedLawnMowers = 0;
		}
	}
}
