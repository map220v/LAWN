using Sexy;
using Sexy.TodLib;

namespace Lawn
{
	internal class ReanimatorCache
	{
		public LawnApp mApp;

		public void ReanimatorCacheInitialize()
		{
		}

		public void ReanimatorCacheDispose()
		{
		}

		public void DrawCachedPlant(Graphics g, float centerX, float btmY, SeedType theSeedType, DrawVariation theDrawVariation)
		{
			Debug.ASSERT(theSeedType >= SeedType.SEED_PEASHOOTER && theSeedType < SeedType.NUM_SEED_TYPES);
			Image image = (theSeedType != SeedType.SEED_SPROUT) ? AtlasResources.GetImageInAtlasById((int)(10300 + theSeedType)) : AtlasResources.IMAGE_CACHED_MARIGOLD;
			int num = (int)(centerX - (float)(image.mWidth / 2) * g.mScaleX);
			int num2 = (int)(btmY - (float)image.mHeight * g.mScaleY);
			TodCommon.TodDrawImageScaledF(g, image, num, num2, g.mScaleX, g.mScaleY);
		}

		public void DrawCachedZombie(Graphics g, float thePosX, float thePosY, ZombieType theZombieType)
		{
			Debug.ASSERT(theZombieType >= ZombieType.ZOMBIE_NORMAL && theZombieType < ZombieType.NUM_CACHED_ZOMBIE_TYPES);
			Image imageInAtlasById = AtlasResources.GetImageInAtlasById((int)(10349 + theZombieType));
			TodCommon.TodDrawImageScaledF(g, imageInAtlasById, thePosX, thePosY, g.mScaleX, g.mScaleY);
		}
	}
}
