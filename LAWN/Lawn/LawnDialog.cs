using Sexy;
using Sexy.TodLib;
using System;

namespace Lawn
{
	internal class LawnDialog : Dialog
	{
		public LawnApp mApp;

		public int mButtonDelay;

		public ReanimationWidget mReanimation;

		public bool mDrawStandardBack;

		public LawnStoneButton mLawnYesButton;

		public LawnStoneButton mLawnNoButton;

		public bool mTallBottom;

		public bool mVerticalCenterText;

		protected int mButtonMinWidth;

		public int mMinWidth;

		public int GetLeft()
		{
			return mContentInsets.mLeft + mBackgroundInsets.mLeft;
		}

		public int GetWidth()
		{
			return mWidth - mContentInsets.mLeft - mContentInsets.mRight - mBackgroundInsets.mLeft - mBackgroundInsets.mRight;
		}

		public int GetTop()
		{
			return mContentInsets.mTop + mBackgroundInsets.mTop + (int)Constants.InvertAndScale(54f) + Constants.DIALOG_HEADER_OFFSET;
		}

		public LawnDialog(LawnApp theApp, Image theButtonComponentImage, int theId, bool isModal, string theDialogHeader, string theDialogLines, string theDialogFooter, int theButtonMode)
			: base(null, theButtonComponentImage, theId, isModal, theDialogHeader, theDialogLines, string.Empty, 0)
		{
			mTextAlign = 2;
			mApp = theApp;
			mButtonDelay = -1;
			mReanimation = new ReanimationWidget();
			mReanimation.mLawnDialog = this;
			mDrawStandardBack = true;
			mTallBottom = false;
			mVerticalCenterText = true;
			mDialogHeader = TodStringFile.TodStringTranslate(theDialogHeader);
			mDialogLines = TodStringFile.TodStringTranslate(theDialogLines);
			SetColors(GameConstants.gLawnDialogColors, 7);
			SetHeaderFont(Resources.FONT_DWARVENTODCRAFT15);
			SetLinesFont(Resources.FONT_DWARVENTODCRAFT15);
			mContentInsets = Constants.LawnDialog_Insets;
			SetColor(0, new SexyColor(224, 187, 98));
			SetColor(1, new SexyColor(224, 187, 98));
			mSpaceAfterHeader = (int)Constants.InvertAndScale(10f);
			switch (theButtonMode)
			{
			case 1:
				mLawnYesButton = GameButton.MakeButton(1000, this, "[BUTTON_YES]");
				mLawnNoButton = GameButton.MakeButton(1001, this, "[BUTTON_NO]");
				break;
			case 2:
				mLawnYesButton = GameButton.MakeButton(1000, this, "[BUTTON_OK]");
				mLawnNoButton = GameButton.MakeButton(1001, this, "[BUTTON_CANCEL]");
				break;
			case 3:
				mLawnYesButton = GameButton.MakeButton(1000, this, theDialogFooter);
				mLawnNoButton = null;
				break;
			default:
				mLawnYesButton = null;
				mLawnNoButton = null;
				break;
			}
		}

		public override void Dispose()
		{
			if (mReanimation != null)
			{
				mReanimation.Dispose();
			}
			if (mLawnYesButton != null)
			{
				mLawnYesButton.Dispose();
			}
			if (mLawnNoButton != null)
			{
				mLawnNoButton.Dispose();
			}
		}

		public virtual void SetButtonDelay(int theDelay)
		{
			mButtonDelay = theDelay;
			if (mLawnYesButton != null)
			{
				mLawnYesButton.SetDisabled(true);
			}
			if (mLawnNoButton != null)
			{
				mLawnNoButton.SetDisabled(true);
			}
		}

		public override void Update()
		{
			base.Update();
			if (mUpdateCnt == mButtonDelay)
			{
				if (mLawnYesButton != null)
				{
					mLawnYesButton.SetDisabled(false);
				}
				if (mLawnNoButton != null)
				{
					mLawnNoButton.SetDisabled(false);
				}
			}
			MarkDirty();
		}

		public override void ButtonPress(int theId)
		{
			mApp.PlaySample(Resources.SOUND_GRAVEBUTTON);
		}

		public override bool BackButtonPress()
		{
			int theId = (mLawnNoButton == null || mId == 55) ? mLawnYesButton.mId : mLawnNoButton.mId;
			ButtonPress(theId);
			ButtonDepress(theId);
			return true;
		}

		public override void ButtonDepress(int theId)
		{
			if (mUpdateCnt > mButtonDelay)
			{
				base.ButtonDepress(theId);
			}
		}

		public virtual void CheckboxChecked(int theId, bool cheked)
		{
			mApp.PlaySample(Resources.SOUND_BUTTONCLICK);
		}

		public override void KeyDown(KeyCode theKey)
		{
			if (mId == 19 && mApp.mBoard != null)
			{
				mApp.mBoard.DoTypingCheck(theKey);
			}
			if (mId != 3)
			{
				if (theKey == KeyCode.KEYCODE_SPACE || theKey == KeyCode.KEYCODE_RETURN || (ushort)theKey == 121 || (ushort)theKey == 89)
				{
					base.ButtonDepress(1000);
				}
				else if ((theKey == KeyCode.KEYCODE_ESCAPE || (ushort)theKey == 110 || (ushort)theKey == 78) && mLawnNoButton != null)
				{
					base.ButtonDepress(1001);
				}
			}
		}

		public override void AddedToManager(WidgetManager theWidgetManager)
		{
			base.AddedToManager(theWidgetManager);
			AddWidget(mReanimation);
			if (mLawnYesButton != null)
			{
				AddWidget(mLawnYesButton);
			}
			if (mLawnNoButton != null)
			{
				AddWidget(mLawnNoButton);
			}
		}

		public override void RemovedFromManager(WidgetManager theWidgetManager)
		{
			base.RemovedFromManager(theWidgetManager);
			if (mLawnYesButton != null)
			{
				RemoveWidget(mLawnYesButton);
			}
			if (mLawnNoButton != null)
			{
				RemoveWidget(mLawnNoButton);
			}
			RemoveWidget(mReanimation);
			mReanimation.Dispose();
		}

		public override void Resize(int theX, int theY, int theWidth, int theHeight)
		{
			base.Resize(theX, theY, theWidth, theHeight);
			Image image = mTallBottom ? AtlasResources.IMAGE_DIALOG_BIGBOTTOMLEFT : AtlasResources.IMAGE_DIALOG_BOTTOMLEFT;
			int num = mContentInsets.mLeft + mBackgroundInsets.mLeft - (int)Constants.InvertAndScale(5f);
			int num2 = theHeight - image.mHeight;
			num2 = ((!mTallBottom) ? (num2 + (int)Constants.InvertAndScale(16f)) : (num2 + (int)Constants.InvertAndScale(44f)));
			int num3 = mWidth - mContentInsets.mLeft - mBackgroundInsets.mLeft - mContentInsets.mRight - mBackgroundInsets.mRight + (int)Constants.InvertAndScale(8f);
			int mHeight = AtlasResources.IMAGE_BUTTON_LEFT.mHeight;
			int num4 = (int)Constants.InvertAndScale(10f);
			int num5 = (num3 - num4) / 2;
			int num6 = Math.Max(mButtonMinWidth, AtlasResources.IMAGE_BUTTON_LEFT.mWidth + AtlasResources.IMAGE_BUTTON_MIDDLE.mWidth + AtlasResources.IMAGE_BUTTON_RIGHT.mWidth);
			if (mLawnYesButton != null && mLawnNoButton != null)
			{
				int num7 = (int)(0.8f * (float)Math.Max(mLawnYesButton.mFont.StringWidth(mLawnYesButton.mLabel), mLawnNoButton.mFont.StringWidth(mLawnNoButton.mLabel)));
				if (num7 > num6)
				{
					num6 = num7;
				}
			}
			int num8 = num5 - num6 - AtlasResources.IMAGE_BUTTON_MIDDLE.mWidth + 1;
			if (num8 < num6)
			{
				num8 = num6;
			}
			if (mLawnYesButton != null && mLawnNoButton != null)
			{
				num8 += (int)Constants.InvertAndScale(8f);
				int num9 = num;
				int i;
				for (i = num + num3 - num8; num9 + num8 > i; i += 10)
				{
					num9 -= 10;
				}
				mLawnYesButton.Resize(num9, num2, num8, mHeight);
				mLawnNoButton.Resize(i, num2, num8, mHeight);
			}
			else if (mLawnYesButton != null)
			{
				int num10 = num3 - AtlasResources.IMAGE_BUTTON_MIDDLE.mWidth + 1;
				num += (num3 - num10) / 2;
				mLawnYesButton.Resize(num, num2, num10, mHeight);
			}
			if (mReanimation.mReanim != null)
			{
				mReanimation.Resize((int)mReanimation.mPosX, (int)((float)Constants.DIALOG_HEADER_OFFSET + mReanimation.mPosY), mReanimation.mWidth, mReanimation.mHeight);
			}
			mX = mApp.mWidth / 2 - mWidth / 2;
			mY = mApp.mHeight / 2 - base.mHeight / 2;
		}

		public override void Draw(Graphics g)
		{
			if (!mDrawStandardBack)
			{
				return;
			}
			Image image = AtlasResources.IMAGE_DIALOG_BOTTOMLEFT;
			Image image2 = AtlasResources.IMAGE_DIALOG_BOTTOMMIDDLE;
			Image image3 = AtlasResources.IMAGE_DIALOG_BOTTOMRIGHT;
			if (mTallBottom)
			{
				image = AtlasResources.IMAGE_DIALOG_BIGBOTTOMLEFT;
				image2 = AtlasResources.IMAGE_DIALOG_BIGBOTTOMMIDDLE;
				image3 = AtlasResources.IMAGE_DIALOG_BIGBOTTOMRIGHT;
			}
			int theWidth = mWidth - AtlasResources.IMAGE_DIALOG_TOPLEFT.mWidth - AtlasResources.IMAGE_DIALOG_TOPRIGHT.mWidth;
			int dIALOG_HEADER_OFFSET = Constants.DIALOG_HEADER_OFFSET;
			g.DrawImage(AtlasResources.IMAGE_DIALOG_TOPLEFT, 0, dIALOG_HEADER_OFFSET);
			LawnCommon.TileImageHorizontally(g, AtlasResources.IMAGE_DIALOG_TOPMIDDLE, AtlasResources.IMAGE_DIALOG_TOPLEFT.mWidth, dIALOG_HEADER_OFFSET, theWidth);
			g.DrawImage(AtlasResources.IMAGE_DIALOG_TOPRIGHT, mWidth - AtlasResources.IMAGE_DIALOG_TOPRIGHT.mWidth, dIALOG_HEADER_OFFSET);
			dIALOG_HEADER_OFFSET += AtlasResources.IMAGE_DIALOG_TOPRIGHT.mHeight;
			int theWidth2 = mWidth - AtlasResources.IMAGE_DIALOG_CENTERLEFT.mWidth - AtlasResources.IMAGE_DIALOG_CENTERRIGHT.mWidth;
			int num = mHeight - dIALOG_HEADER_OFFSET - image2.mHeight;
			Graphics @new = Graphics.GetNew(g);
			@new.SetClipRect(0, dIALOG_HEADER_OFFSET, mWidth, num);
			int num2 = (num + AtlasResources.IMAGE_DIALOG_CENTERLEFT.mHeight - 1) / AtlasResources.IMAGE_DIALOG_CENTERLEFT.mHeight;
			while (num2-- > 0)
			{
				@new.DrawImage(AtlasResources.IMAGE_DIALOG_CENTERLEFT, 0, dIALOG_HEADER_OFFSET);
				LawnCommon.TileImageHorizontally(@new, AtlasResources.IMAGE_DIALOG_CENTERMIDDLE, AtlasResources.IMAGE_DIALOG_CENTERLEFT.mWidth, dIALOG_HEADER_OFFSET, theWidth2);
				@new.DrawImage(AtlasResources.IMAGE_DIALOG_CENTERRIGHT, mWidth - AtlasResources.IMAGE_DIALOG_CENTERRIGHT.mWidth, dIALOG_HEADER_OFFSET);
				dIALOG_HEADER_OFFSET += AtlasResources.IMAGE_DIALOG_CENTERLEFT.mHeight;
			}
			@new.PrepareForReuse();
			int theWidth3 = mWidth - image.mWidth - image3.mWidth;
			dIALOG_HEADER_OFFSET = mHeight - image2.mHeight;
			g.DrawImage(image, 0, dIALOG_HEADER_OFFSET);
			LawnCommon.TileImageHorizontally(g, image2, image.mWidth, dIALOG_HEADER_OFFSET, theWidth3);
			g.DrawImage(image3, mWidth - image3.mWidth, dIALOG_HEADER_OFFSET);
			g.DrawImage(AtlasResources.IMAGE_DIALOG_HEADER, (mWidth - AtlasResources.IMAGE_DIALOG_HEADER.mWidth) / 2, 0);
			int num3 = Constants.DIALOG_HEADER_OFFSET + mContentInsets.mTop + mBackgroundInsets.mTop;
			if (mDialogHeader.Length > 0)
			{
				g.SetFont(mHeaderFont);
				g.SetColor(mColors[0]);
				WriteCenteredLine(g, num3, mDialogHeader);
				num3 += mHeaderFont.GetHeight();
				num3 += mSpaceAfterHeader;
			}
			g.SetFont(mLinesFont);
			g.SetColor(mColors[1]);
			int theWidth4 = mWidth - mContentInsets.mLeft - mContentInsets.mRight - mBackgroundInsets.mLeft - mBackgroundInsets.mRight - (int)Constants.InvertAndScale(4f);
			TRect theRect = new TRect(mBackgroundInsets.mLeft + mContentInsets.mLeft + (int)Constants.InvertAndScale(2f), num3, theWidth4, 0);
			if (mVerticalCenterText)
			{
				int wordWrappedHeight = GetWordWrappedHeight(g, theWidth4, mDialogLines, mLinesFont.GetLineSpacing() + mLineSpacingOffset);
				int num4 = mHeight - mContentInsets.mBottom - mBackgroundInsets.mBottom - num3 - mButtonHeight - (int)Constants.InvertAndScale(20f);
				if (mTallBottom)
				{
					num4 -= (int)Constants.InvertAndScale(40f);
				}
				theRect.mY += (num4 - wordWrappedHeight) / 2;
			}
			num3 += WriteWordWrapped(g, theRect, mDialogLines, mLineSpacingOffset, mTextAlign);
		}

		public override int WriteWordWrapped(Graphics g, TRect theRect, string theLine, int theLineSpacing, int theJustification)
		{
			bool mWriteColoredString = g.mWriteColoredString;
			g.mWriteColoredString = Widget.mWriteColoredString;
			int result = TodStringFile.TodDrawStringWrapped(g, theLine, theRect, g.GetFont(), g.mColor, (DrawStringJustification)theJustification);
			g.mWriteColoredString = mWriteColoredString;
			return result;
		}

		public override void MouseDrag(int x, int y)
		{
		}

		public void CalcSize(int theExtraX, int theExtraY)
		{
			CalcSize(theExtraX, theExtraY, 0);
		}

		public void CalcSize(int theExtraX, int theExtraY, int theMinWidth)
		{
			int num = mContentInsets.mLeft + mContentInsets.mRight + mBackgroundInsets.mLeft + mBackgroundInsets.mRight + theExtraX;
			if (mDialogHeader.Length > 0)
			{
				num += mHeaderFont.StringWidth(mDialogHeader);
				if (num < theMinWidth)
				{
					num = theMinWidth;
				}
			}
			int num2 = Math.Max(mMinWidth, AtlasResources.IMAGE_DIALOG_TOPLEFT.mWidth + AtlasResources.IMAGE_DIALOG_TOPRIGHT.mWidth + AtlasResources.IMAGE_DIALOG_TOPMIDDLE.mWidth);
			if (num < num2)
			{
				num = num2;
			}
			int num3 = Constants.DIALOG_HEADER_OFFSET + mContentInsets.mTop + mContentInsets.mBottom + mBackgroundInsets.mTop + mBackgroundInsets.mBottom + theExtraY;
			if (!string.IsNullOrEmpty(mDialogHeader))
			{
				num3 += mHeaderFont.GetHeight();
				num3 += mSpaceAfterHeader;
			}
			if (mDialogLines.Length > 0)
			{
				num += AtlasResources.IMAGE_DIALOG_TOPMIDDLE.mWidth;
				Graphics @new = Graphics.GetNew();
				@new.SetFont(mLinesFont);
				num3 += GetWordWrappedHeight(@new, num - mContentInsets.mLeft - mContentInsets.mRight - mBackgroundInsets.mLeft - mBackgroundInsets.mRight - (int)Constants.InvertAndScale(4f), mDialogLines, mLineSpacingOffset);
				@new.PrepareForReuse();
			}
			num3 += AtlasResources.IMAGE_DIALOG_BOTTOMMIDDLE.GetHeight();
			int num4 = Constants.DIALOG_HEADER_OFFSET + AtlasResources.IMAGE_DIALOG_TOPLEFT.mHeight + AtlasResources.IMAGE_DIALOG_BOTTOMLEFT.mHeight;
			if (mTallBottom)
			{
				num4 = Constants.DIALOG_HEADER_OFFSET + AtlasResources.IMAGE_DIALOG_TOPLEFT.mHeight + AtlasResources.IMAGE_DIALOG_BIGBOTTOMLEFT.mHeight;
			}
			if (num3 < num4)
			{
				num3 = num4;
			}
			Resize(mX, mY, num, num3);
		}

		public override int GetWordWrappedHeight(Graphics g, int theWidth, string theLine, int aLineSpacing)
		{
			return TodStringFile.TodDrawStringWrappedHeight(g, theLine, new TRect(0, 0, theWidth, 0), g.GetFont(), g.mColor, DrawStringJustification.DS_ALIGN_LEFT);
		}
	}
}
