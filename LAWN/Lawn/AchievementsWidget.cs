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
