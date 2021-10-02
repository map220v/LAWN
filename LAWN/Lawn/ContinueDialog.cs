using Sexy;
using Sexy.TodLib;

namespace Lawn
{
	internal class ContinueDialog : LawnDialog
	{
		public DialogButton mContinueButton;

		public DialogButton mNewGameButton;

		public ContinueDialog(LawnApp theApp)
			: base(theApp, null, 37, true, "[CONTINUE_GAME_HEADER]", "", "[DIALOG_BUTTON_CANCEL]", 3)
		{
			if (theApp.IsAdventureMode() || theApp.IsQuickPlayMode())
			{
				mDialogLines = TodStringFile.TodStringTranslate("[CONTINUE_GAME_OR_RESTART]");
				mContinueButton = GameButton.MakeButton(0, this, "[CONTINUE_BUTTON]");
				mNewGameButton = GameButton.MakeButton(1, this, "[RESTART_BUTTON]");
			}
			else
			{
				mDialogLines = TodStringFile.TodStringTranslate("[CONTINUE_GAME]");
				mContinueButton = GameButton.MakeButton(0, this, "[CONTINUE_BUTTON]");
				mNewGameButton = GameButton.MakeButton(1, this, "[NEW_GAME_BUTTON]");
			}
			mTallBottom = true;
			CalcSize(50, 70);
		}

		public override void Dispose()
		{
			mContinueButton.Dispose();
			mNewGameButton.Dispose();
			base.Dispose();
		}

		public override int GetPreferredHeight(int theWidth)
		{
			int preferredHeight = base.GetPreferredHeight(theWidth);
			return preferredHeight + (int)Constants.InvertAndScale(40f);
		}

		public override void Resize(int theX, int theY, int theWidth, int theHeight)
		{
			base.Resize(theX, theY, theWidth, theHeight);
			int num = AtlasResources.IMAGE_BUTTON_LEFT.mWidth + AtlasResources.IMAGE_BUTTON_MIDDLE.mWidth * 4 + AtlasResources.IMAGE_BUTTON_RIGHT.mWidth;
			int mHeight = mLawnYesButton.mHeight;
			int theY2 = mLawnYesButton.mY - mHeight - 2;
			int num2 = mLawnYesButton.mX - 20;
			int i;
			for (i = mLawnYesButton.mX + mLawnYesButton.mWidth - num + 24; num2 + num > i; i += 20)
			{
				num2 -= 20;
			}
			mContinueButton.Resize(num2, theY2, num, mHeight);
			mNewGameButton.Resize(i, mContinueButton.mY, num, mHeight);
			mLawnYesButton.Resize(theWidth / 2 - num / 2, mLawnYesButton.mY, num, mHeight);
		}

		public override void AddedToManager(WidgetManager theWidgetManager)
		{
			base.AddedToManager(theWidgetManager);
			AddWidget(mContinueButton);
			AddWidget(mNewGameButton);
		}

		public override void RemovedFromManager(WidgetManager theWidgetManager)
		{
			base.RemovedFromManager(theWidgetManager);
			RemoveWidget(mContinueButton);
			RemoveWidget(mNewGameButton);
		}

		public override void ButtonDepress(int theId)
		{
			switch (theId)
			{
			case 0:
				if (mApp.mBoard.mNextSurvivalStageCounter != 1)
				{
					string savedGameName = LawnCommon.GetSavedGameName(mApp.mGameMode, (int)mApp.mPlayerInfo.mId);
					mApp.EraseFile(savedGameName);
				}
				mApp.RestartLoopingSounds();
				mApp.KillDialog(mId);
				mApp.mMusic.StartGameMusic();
				return;
			case 1:
				if (mApp.IsAdventureMode() || mApp.IsQuickPlayMode())
				{
					LawnDialog lawnDialog = mApp.DoDialog(39, true, "[RESTART_LEVEL_HEADER]", "[RESTART_LEVEL]", "", 2);
					lawnDialog.mMinWidth = (int)Constants.InvertAndScale(350f);
					lawnDialog.mLawnYesButton.mLabel = TodStringFile.TodStringTranslate("[RESTART_BUTTON]");
					lawnDialog.CalcSize(0, 0);
				}
				else
				{
					LawnDialog lawnDialog2 = mApp.DoDialog(39, true, "[NEW_GAME_HEADER]", "[NEW_GAME]", "", 2);
					lawnDialog2.mMinWidth = (int)Constants.InvertAndScale(250f);
					lawnDialog2.mLawnYesButton.mLabel = TodStringFile.TodStringTranslate("[NEW_GAME_BUTTON]");
					lawnDialog2.CalcSize(0, 0);
				}
				return;
			}
			mApp.KillDialog(mId);
			if (mApp.IsAdventureMode() || mApp.IsQuickPlayMode())
			{
				mApp.ShowGameSelector();
			}
			else if (mApp.IsSurvivalMode())
			{
				mApp.KillBoard();
				mApp.ShowGameSelectorQuickPlay(false);
			}
			else if (mApp.IsPuzzleMode())
			{
				mApp.KillBoard();
				mApp.ShowGameSelectorQuickPlay(false);
			}
			else
			{
				mApp.KillBoard();
				mApp.ShowGameSelectorQuickPlay(false);
			}
		}
	}
}
