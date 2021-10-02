using Sexy;

namespace Lawn
{
	internal class ZombieGalleryWidget : Widget
	{
		public AlmanacDialog mDialog;

		public ZombieGalleryWidget(AlmanacDialog theDialog)
		{
			mDialog = theDialog;
			mWidth = Constants.ZombieGallerySize.X;
			mHeight = Constants.ZombieGallerySize.Y;
		}

		public override void MouseUp(int x, int y, int theClickCount)
		{
			ZombieType zombieType = ZombieHitTest(x, y);
			if (zombieType != ZombieType.ZOMBIE_INVALID)
			{
				mDialog.ZombieSelected(zombieType);
			}
		}

		public ZombieType GetZombieType(int theIndex)
		{
			if (theIndex >= 33)
			{
				return ZombieType.ZOMBIE_INVALID;
			}
			return (ZombieType)theIndex;
		}

		public ZombieType ZombieHitTest(int x, int y)
		{
			for (int i = 0; i < GameConstants.NUM_ALMANAC_ZOMBIES; i++)
			{
				if (i >= 33)
				{
					continue;
				}
				ZombieType zombieType = GetZombieType(i);
				if (zombieType != ZombieType.ZOMBIE_INVALID && ZombieIsShown(zombieType))
				{
					int x2 = 0;
					int y2 = 0;
					GetZombiePosition(zombieType, ref x2, ref y2);
					if (x >= x2 && y >= y2 && (float)x < (float)x2 + Constants.InvertAndScale(76f) && (float)y < (float)y2 + Constants.InvertAndScale(76f))
					{
						return zombieType;
					}
				}
			}
			return ZombieType.ZOMBIE_INVALID;
		}

		public void GetZombiePosition(ZombieType theZombieType, ref int x, ref int y)
		{
			switch (theZombieType)
			{
			case ZombieType.ZOMBIE_BOSS:
				x = Constants.Almanac_BossPosition.X;
				y = Constants.Almanac_BossPosition.Y;
				break;
			case ZombieType.ZOMBIE_IMP:
				x = Constants.Almanac_ImpPosition.X;
				y = Constants.Almanac_ImpPosition.Y;
				break;
			default:
				x = (int)theZombieType % 3 * Constants.Almanac_ZombieSpace.X;
				y = 5 + (int)theZombieType / 3 * Constants.Almanac_ZombieSpace.Y;
				break;
			}
		}

		public bool ZombieIsShown(ZombieType theZombieType)
		{
			ZombieDefinition zombieDefinition = Zombie.GetZombieDefinition(theZombieType);
			int level = mDialog.mApp.mPlayerInfo.GetLevel();
			if (mDialog.mApp.IsTrialStageLocked() && theZombieType > ZombieType.ZOMBIE_SNORKEL)
			{
				return false;
			}
			if (theZombieType == ZombieType.ZOMBIE_YETI)
			{
				if (mDialog.mApp.CanSpawnYetis() || mDialog.ZombieHasSilhouette(ZombieType.ZOMBIE_YETI))
				{
					return true;
				}
				return false;
			}
			if (theZombieType > ZombieType.ZOMBIE_BOSS)
			{
				return false;
			}
			if (mDialog.mApp.HasFinishedAdventure())
			{
				return true;
			}
			if (zombieDefinition.mStartingLevel > level)
			{
				return false;
			}
			if (zombieDefinition.mStartingLevel == level && (theZombieType == ZombieType.ZOMBIE_IMP || theZombieType == ZombieType.ZOMBIE_BOBSLED || theZombieType == ZombieType.ZOMBIE_BACKUP_DANCER) && !AlmanacDialog.gZombieDefeated[(int)theZombieType])
			{
				return false;
			}
			return true;
		}

		public override void Draw(Graphics g)
		{
			for (int i = 0; i < GameConstants.NUM_ALMANAC_ZOMBIES; i++)
			{
				ZombieType zombieType = GetZombieType(i);
				int x = 0;
				int y = 0;
				GetZombiePosition(zombieType, ref x, ref y);
				if (zombieType == ZombieType.ZOMBIE_INVALID)
				{
					continue;
				}
				if (!ZombieIsShown(zombieType))
				{
					g.DrawImage(AtlasResources.IMAGE_ALMANAC_ZOMBIEBLANK, x, y);
					continue;
				}
				g.DrawImage(AtlasResources.IMAGE_ALMANAC_ZOMBIEWINDOW, x + (int)Constants.InvertAndScale(5f), y + (int)Constants.InvertAndScale(6f));
				ZombieType zombieType2 = zombieType;
				Graphics @new = Graphics.GetNew(g);
				@new.ClipRect(x + Constants.ZombieGalleryWidget_Window_Clip.X, y + Constants.ZombieGalleryWidget_Window_Clip.Y, Constants.ZombieGalleryWidget_Window_Clip.Width, Constants.ZombieGalleryWidget_Window_Clip.Height);
				if (mDialog.ZombieHasSilhouette(zombieType))
				{
					@new.SetColor(new SexyColor(0, 0, 0, 64));
					@new.SetColorizeImages(true);
				}
				mDialog.mApp.mReanimatorCache.DrawCachedZombie(@new, x + (int)Constants.InvertAndScale(AlmanacDialog.ZombieOffsets[(int)zombieType2].mX), y + (int)Constants.InvertAndScale(AlmanacDialog.ZombieOffsets[(int)zombieType2].mY), zombieType2);
				@new.SetColorizeImages(false);
				g.DrawImage(AtlasResources.IMAGE_ALMANAC_ZOMBIEWINDOW2, x + Constants.ZombieGalleryWidget_Window_Offset.X, y + Constants.ZombieGalleryWidget_Window_Offset.Y);
				@new.PrepareForReuse();
			}
		}
	}
}
