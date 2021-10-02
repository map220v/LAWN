using Sexy;

namespace Lawn
{
	internal class CheatDialog : LawnDialog, EditListener
	{
		public EditWidget mLevelEditWidget;

		public CheatDialog(LawnApp theApp)
			: base(theApp, null, 35, true, "CHEAT", "Enter New Level:", "", 2)
		{
			mApp = theApp;
			mVerticalCenterText = false;
			mLevelEditWidget = LawnCommon.CreateEditWidget(0, this, this, "Cheat", "");
			mLevelEditWidget.mMaxChars = 12;
			mLevelEditWidget.SetFont(Resources.FONT_BRIANNETOD12);
			string empty = string.Empty;
			if (mApp.mGameMode != 0)
			{
				empty = Common.StrFormat_("C{0}", mApp.mGameMode);
			}
			else if (mApp.HasFinishedAdventure())
			{
				int level = theApp.mPlayerInfo.GetLevel();
				empty = Common.StrFormat_("F{0}", mApp.GetStageString(level));
			}
			else
			{
				int level2 = theApp.mPlayerInfo.GetLevel();
				empty = mApp.GetStageString(level2);
			}
			mLevelEditWidget.SetText(empty);
			CalcSize(110, 40);
		}

		public override void Dispose()
		{
			mLevelEditWidget.Dispose();
			base.Dispose();
		}

		public override int GetPreferredHeight(int theWidth)
		{
			return base.GetPreferredHeight(theWidth) + 40;
		}

		public override void Resize(int theX, int theY, int theWidth, int theHeight)
		{
			base.Resize(theX, theY, theWidth, theHeight);
			mLevelEditWidget.Resize(mContentInsets.mLeft + 12, mHeight - 155, mWidth - mContentInsets.mLeft - mContentInsets.mRight - 24, 28);
		}

		public override void AddedToManager(WidgetManager theWidgetManager)
		{
			base.AddedToManager(theWidgetManager);
			AddWidget(mLevelEditWidget);
			theWidgetManager.SetFocus(mLevelEditWidget);
		}

		public override void RemovedFromManager(WidgetManager theWidgetManager)
		{
			base.RemovedFromManager(theWidgetManager);
			RemoveWidget(mLevelEditWidget);
		}

		public override void Draw(Graphics g)
		{
			base.Draw(g);
			LawnCommon.DrawEditBox(g, mLevelEditWidget);
		}

		public virtual void EditWidgetText(int theId, string theString)
		{
			mApp.ButtonDepress(2000 + mId);
		}

		public virtual bool AllowChar(int theId, char theChar)
		{
			if (char.IsDigit(theChar) || theChar == '-' || theChar == 'c' || theChar == 'C' || theChar == 'f' || theChar == 'F')
			{
				return true;
			}
			return false;
		}

		public bool ApplyCheat()
		{
			int result = -1;
			int mFinishedAdventure = 0;
			string mString = mLevelEditWidget.mString;
			int num = mString.ToLower().IndexOf("f");
			if (num >= 0)
			{
				mFinishedAdventure = 1;
			}
			mString = mString.ToLower().Replace("f", "");
			string[] array = mString.Split('-');
			if (array.Length > 1)
			{
				int result2;
				int.TryParse(array[0], out result2);
				int result3;
				int.TryParse(array[1], out result3);
				result = (result2 - 1) * 10 + result3;
			}
			else
			{
				int.TryParse(mString, out result);
			}
			if (result <= 0)
			{
				mApp.DoDialog(36, true, "Enter Level", "Invalid Level. Do 'number' or 'area-subarea' or 'Cnumber' or 'Farea-subarea'.", "OK", 3);
				return false;
			}
			mApp.mGameMode = GameMode.GAMEMODE_ADVENTURE;
			mApp.mPlayerInfo.SetLevel(result);
			mApp.mPlayerInfo.mFinishedAdventure = mFinishedAdventure;
			mApp.WriteCurrentUserConfig();
			mApp.mBoard.PickBackground();
			return true;
		}

		public bool ShouldClear()
		{
			return false;
		}

		public bool AllowText(int theId, ref string theText)
		{
			return false;
		}
	}
}
