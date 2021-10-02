namespace Lawn
{
	public struct ZombieDescriptor
	{
		public ZombieType type;

		public int x;

		public int y;

		public ZombieDescriptor(ZombieType theType, int aX, int aY)
		{
			type = theType;
			x = aX;
			y = aY;
		}
	}
}
