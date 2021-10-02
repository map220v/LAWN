using Sexy;
using Sexy.TodLib;
using System;
using System.Collections.Generic;

namespace Lawn
{
	internal class UserDialog : LawnDialog, ListListener, EditListener
	{
		private const int MAX_USERS = 5;

		public ListWidget mUserList;

		public DialogButton mRenameButton;

		public DialogButton mDeleteButton;

		public int mNumUsers;

		public UserDialog(LawnApp theApp)
			: base(theApp, null, 29, true, "[WHO_ARE_YOU]", "", "", 2)
		{
			mVerticalCenterText = false;
			mUserList = new ListWidget(0, Resources.FONT_BRIANNETOD16, this);
			mUserList.SetColors(LawnCommon.gUserListWidgetColors, 5);
			mUserList.mDrawOutline = true;
			mUserList.mJustify = 1;
			mUserList.mItemHeight = (int)Constants.InvertAndScale(24f);
			mButtonMinWidth = (int)Constants.InvertAndScale(130f);
			mRenameButton = GameButton.MakeButton(0, this, "[RENAME_BUTTON]");
			mDeleteButton = GameButton.MakeButton(1, this, "[DELETE_BUTTON]");
			mNumUsers = 0;
			PlayerInfo mPlayerInfo = theApp.mPlayerInfo;
			if (mPlayerInfo != null)
			{
				mUserList.SetSelect(mUserList.AddLine(mPlayerInfo.mName, false));
				mNumUsers++;
			}
			Dictionary<string, PlayerInfo> profileMap = theApp.mProfileMgr.GetProfileMap();
			Dictionary<string, PlayerInfo>.Enumerator enumerator = profileMap.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (mPlayerInfo == null || enumerator.Current.Value.mName != mPlayerInfo.mName)
				{
					mUserList.AddLine(enumerator.Current.Value.mName, false);
					mNumUsers++;
				}
			}
			if (mNumUsers < 5)
			{
				mUserList.AddLine(TodStringFile.TodStringTranslate("[CREATE_NEW_USER]"), false);
			}
			mTallBottom = true;
			int num = Math.Max(Resources.FONT_DWARVENTODCRAFT15.StringWidth("MMMMMMMMMMMM"), Resources.FONT_DWARVENTODCRAFT15.StringWidth(TodStringFile.TodStringTranslate("[CREATE_NEW_USER]")));
			CalcSize((int)Constants.InvertAndScale(70f), (int)Constants.InvertAndScale(140f), AtlasResources.IMAGE_DIALOG_TOPLEFT.mWidth + num + AtlasResources.IMAGE_DIALOG_TOPRIGHT.mWidth);
		}

		public override void Dispose()
		{
			mUserList.Dispose();
			mRenameButton.Dispose();
			mDeleteButton.Dispose();
			base.Dispose();
		}

		public override void Resize(int theX, int theY, int theWidth, int theHeight)
		{
			base.Resize(theX, theY, theWidth, theHeight);
			int theX2 = GetLeft() + 30;
			int theY2 = GetTop() + -20;
			int num = GetWidth() - 60;
			int theHeight2 = 5 * mUserList.mItemHeight + (int)Constants.InvertAndScale(4f) * 2;
			int num2 = 0;
			mUserList.Resize(theX2, theY2, num - num2, theHeight2);
			mRenameButton.Layout(4355, mLawnYesButton, 0, -3);
			mDeleteButton.Layout(4355, mLawnNoButton, 0, -3);
		}

		public override int GetPreferredHeight(int theWidth)
		{
			return GetPreferredHeight(theWidth) + (int)Constants.InvertAndScale(190f);
		}

		public override void AddedToManager(WidgetManager theWidgetManager)
		{
			base.AddedToManager(theWidgetManager);
			AddWidget(mUserList);
			AddWidget(mDeleteButton);
			AddWidget(mRenameButton);
		}

		public override void RemovedFromManager(WidgetManager theWidgetManager)
		{
			base.RemovedFromManager(theWidgetManager);
			RemoveWidget(mUserList);
			RemoveWidget(mDeleteButton);
			RemoveWidget(mRenameButton);
		}

		public virtual void ListClicked(int theId, int theIdx, int theClickCount)
		{
			if (theIdx == mNumUsers)
			{
				mApp.DoCreateUserDialog(false);
				return;
			}
			mUserList.SetSelect(theIdx);
			if (theClickCount == 2)
			{
				mApp.FinishUserDialog(true);
			}
		}

		public override void ButtonDepress(int theId)
		{
			base.ButtonDepress(theId);
			string selName = GetSelName();
			if (!selName.empty())
			{
				switch (theId)
				{
				case 1:
					mApp.DoConfirmDeleteUserDialog(selName);
					break;
				case 0:
					mApp.DoRenameUserDialog(selName);
					break;
				}
			}
		}

		public virtual void EditWidgetText(int theId, string theString)
		{
			mApp.ButtonDepress(2000 + mId);
		}

		public virtual bool AllowChar(int theId, char theChar)
		{
			if (!char.IsDigit(theChar))
			{
				return false;
			}
			return true;
		}

		public override void Draw(Graphics g)
		{
			base.Draw(g);
		}

		public void FinishDeleteUser()
		{
			int mSelectIdx = mUserList.mSelectIdx;
			mUserList.RemoveLine(mUserList.mSelectIdx);
			mSelectIdx--;
			if (mSelectIdx < 0)
			{
				mSelectIdx = 0;
			}
			if (mUserList.GetLineCount() > 1)
			{
				mUserList.SetSelect(mSelectIdx);
			}
			mNumUsers--;
			if (mNumUsers == 4)
			{
				mUserList.AddLine(TodStringFile.TodStringTranslate("[CREATE_NEW_USER]"), false);
			}
		}

		public void FinishRenameUser(string theNewName)
		{
			int mSelectIdx = mUserList.mSelectIdx;
			if (mSelectIdx < mNumUsers)
			{
				mUserList.SetLine(mSelectIdx, theNewName);
			}
		}

		public string GetSelName()
		{
			if (mUserList.mSelectIdx < 0 || mUserList.mSelectIdx >= mNumUsers)
			{
				return "";
			}
			return mUserList.GetStringAt(mUserList.mSelectIdx);
		}

		public void EditWidgetText(int theId, ref string theString)
		{
		}

		public bool AllowText(int theId, ref string theText)
		{
			return false;
		}

		public bool ShouldClear()
		{
			return false;
		}

		public void ListClosed(int theId)
		{
		}

		public void ListHiliteChanged(int theId, int theOldIdx, int theNewIdx)
		{
		}
	}
}
