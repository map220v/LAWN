using Sexy;

namespace Lawn
{
	internal class PlantGalleryWidget : Widget
	{
		public AlmanacDialog mDialog;

		public PlantGalleryWidget(AlmanacDialog theDialog)
		{
			mDialog = theDialog;
			mWidth = Constants.PlantGallerySize.X;
			mHeight = Constants.PlantGallerySize.Y;
		}

		public override void MouseUp(int x, int y, int theClickCount)
		{
			SeedType seedType = SeedHitTest(x, y);
			if (seedType != SeedType.SEED_NONE)
			{
				mDialog.PlantSelected(seedType);
			}
		}

		public SeedType SeedHitTest(int x, int y)
		{
			for (int i = 0; i < GameConstants.NUM_ALMANAC_SEEDS; i++)
			{
				SeedType seedType = (SeedType)i;
				if (mDialog.mApp.HasSeedType(seedType))
				{
					int x2 = 0;
					int y2 = 0;
					GetSeedPosition(seedType, ref x2, ref y2);
					int sMALL_SEEDPACKET_WIDTH = Constants.SMALL_SEEDPACKET_WIDTH;
					int sMALL_SEEDPACKET_HEIGHT = Constants.SMALL_SEEDPACKET_HEIGHT;
					if (x >= x2 && y >= y2 && x < x2 + sMALL_SEEDPACKET_WIDTH && y < y2 + sMALL_SEEDPACKET_HEIGHT)
					{
						return seedType;
					}
				}
			}
			return SeedType.SEED_NONE;
		}

		public void GetSeedPosition(SeedType theSeedType, ref int x, ref int y)
		{
			if (theSeedType == SeedType.SEED_IMITATER)
			{
				x = Constants.Almanac_ImitatorPosition.X;
				y = Constants.Almanac_ImitatorPosition.Y;
			}
			else
			{
				x = Constants.Almanac_SeedOffset.X + (int)theSeedType % 4 * (Constants.SMALL_SEEDPACKET_WIDTH + Constants.Almanac_SeedSpace.X);
				y = Constants.Almanac_SeedOffset.X + (int)theSeedType / 4 * (Constants.SMALL_SEEDPACKET_HEIGHT + Constants.Almanac_SeedSpace.Y);
			}
		}

		public override void Draw(Graphics g)
		{
			g.HardwareClip();
			bool flag = false;
			bool flag2 = true;
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < GameConstants.NUM_ALMANAC_SEEDS; j++)
				{
					SeedType seedType = (SeedType)j;
					int x = 0;
					int y = 0;
					GetSeedPosition(seedType, ref x, ref y);
					if (!mDialog.mApp.HasSeedType(seedType))
					{
						g.DrawImage(AtlasResources.IMAGE_ALMANAC_PLANTBLANK, x, y);
					}
					else
					{
						SeedPacket.DrawSmallSeedPacket(g, x, y, seedType, SeedType.SEED_NONE, 0f, 255, seedType != SeedType.SEED_IMITATER && flag, false, flag2, seedType != SeedType.SEED_IMITATER && flag2);
					}
				}
				flag = true;
				flag2 = false;
			}
			g.EndHardwareClip();
		}
	}
}
