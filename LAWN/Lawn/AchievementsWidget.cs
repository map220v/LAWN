using Sexy;

namespace Lawn
{
	internal class AchievementsWidget : Widget
	{
		public LawnApp mApp;

		public static TRect BackButtonRect = Constants.AchievementWidget_BackButton_Rect;

		public AchievementsWidget(LawnApp theApp)
		{
			mApp = theApp;
			mWidth = Constants.BOARD_WIDTH;
			mHeight = Resources.IMAGE_SELECTORSCREEN_ACHIEVEMENTS_HOLE.mHeight * Constants.AchievementWidget_HOLE_DEPTH + Resources.IMAGE_SELECTORSCREEN_ACHIEVEMENTS_HOLE_CHINA.mHeight + Constants.AchievementWidget_Background_Offset_Y;
		}

		public override void Draw(Graphics g)
		{
			int num = Constants.AchievementWidget_Background_Offset_Y;
			for (int i = 0; i < Constants.AchievementWidget_HOLE_DEPTH; i++)
			{
				g.DrawImage(Resources.IMAGE_SELECTORSCREEN_ACHIEVEMENTS_HOLE, 0, num);
				num += Resources.IMAGE_SELECTORSCREEN_ACHIEVEMENTS_HOLE.mHeight;
			}
			int num2 = 16;
			g.DrawImage(AtlasResources.IMAGE_PIPE, Constants.AchievementWidget_Pipe_Offset.X, num2 * AtlasResources.IMAGE_PIPE.mHeight + Constants.AchievementWidget_Pipe_Offset.Y, new TRect(0, 0, AtlasResources.IMAGE_PIPE.mWidth, Resources.IMAGE_SELECTORSCREEN_ACHIEVEMENTS_HOLE.mHeight - 1));
			int num3 = 21;
			g.DrawImage(AtlasResources.IMAGE_WORM, Constants.AchievementWidget_Worm_Offset.X, num3 * Resources.IMAGE_SELECTORSCREEN_ACHIEVEMENTS_HOLE.mHeight + Constants.AchievementWidget_Worm_Offset.Y, new TRect(0, 0, AtlasResources.IMAGE_WORM.mWidth - 1, AtlasResources.IMAGE_WORM.mHeight - 1));
			g.DrawImage(AtlasResources.IMAGE_ZOMBIE_WORM, Constants.AchievementWidget_ZombieWorm_Offset.X, num3 * Resources.IMAGE_SELECTORSCREEN_ACHIEVEMENTS_HOLE.mHeight + Constants.AchievementWidget_ZombieWorm_Offset.Y);
			int num4 = 53;
			g.DrawImage(AtlasResources.IMAGE_GEMS_LEFT, Constants.AchievementWidget_GemLeft_Offset.X, num4 * Resources.IMAGE_SELECTORSCREEN_ACHIEVEMENTS_HOLE.mHeight + Constants.AchievementWidget_GemLeft_Offset.Y);
			g.DrawImage(AtlasResources.IMAGE_GEMS_RIGHT, Constants.AchievementWidget_GemRight_Offset.X, num4 * Resources.IMAGE_SELECTORSCREEN_ACHIEVEMENTS_HOLE.mHeight + Constants.AchievementWidget_GemRight_Offset.Y);
			int num5 = 90;
			g.DrawImage(AtlasResources.IMAGE_FOSSIL, Constants.AchievementWidget_Fossile_Offset.X, num5 * Resources.IMAGE_SELECTORSCREEN_ACHIEVEMENTS_HOLE.mHeight + Constants.AchievementWidget_Fossile_Offset.Y, new TRect(0, 0, AtlasResources.IMAGE_FOSSIL.mWidth - 1, AtlasResources.IMAGE_FOSSIL.mHeight - 1));
			g.DrawImage(Resources.IMAGE_SELECTORSCREEN_ACHIEVEMENTS_HOLE_CHINA, 0, num);
			g.DrawImage(Resources.IMAGE_SELECTORSCREEN_ACHIEVEMENTS_TOP_BACKGROUND, 0, 0);
			Image theImage = ((!mIsDown || !BackButtonRect.Contains(new TPoint(mWidgetManager.mLastMouseX, mWidgetManager.mLastMouseY))) ? AtlasResources.IMAGE_REANIM_SELECTORSCREEN_ACHIEVEMENTS_BACK_BUTTON : AtlasResources.IMAGE_REANIM_SELECTORSCREEN_ACHIEVEMENTS_BACK_HIGHLIGHT);
			g.DrawImage(theImage, Constants.AchievementWidget_BackButton_X, Constants.AchievementWidget_BackButton_Y);
			g.SetScale(1f);
		}


		public override void MouseDown(int x, int y, int theClickCount)
		{
			if (BackButtonRect.Contains(x, y))
			{
				mApp.PlaySample(Resources.SOUND_GRAVEBUTTON);
			}
		}

		public override void MouseUp(int x, int y, int theClickCount)
		{
			if (BackButtonRect.Contains(x, y))
			{
				mApp.mGameSelector.ButtonDepress(118);
				return;
			}
			ScrollWidget scrollWidget = (ScrollWidget)mParent;
			scrollWidget.ScrollToMin(true);
		}
	}
}
