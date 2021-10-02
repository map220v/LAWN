namespace Lawn
{
	internal class ProjectileDefinition
	{
		public ProjectileType mProjectileType;

		public int mImageRow;

		public int mDamage;

		public ProjectileDefinition(ProjectileType theType, int theRow, int theDamage)
		{
			mProjectileType = theType;
			mImageRow = theRow;
			mDamage = theDamage;
		}
	}
}
