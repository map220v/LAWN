namespace Lawn
{
	internal class ZombieAllowedLevels
	{
		public ZombieType mZombieType;

		public int[] mAllowedOnLevel = new int[50];

		public ZombieAllowedLevels(ZombieType aZombieType, int[] levels)
		{
			mZombieType = aZombieType;
			mAllowedOnLevel = levels;
		}
	}
}
