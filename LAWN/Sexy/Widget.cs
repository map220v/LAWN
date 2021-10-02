using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Sexy
{
	internal class Widget : WidgetContainer
	{
		public bool mVisible;

		public bool mMouseVisible;

		public bool mDisabled;

		public bool mHasFocus;

		public bool mIsDown;

		public bool mIsOver;

		public bool mHasTransparencies;

		public List<Color> mColors = new List<Color>();

		public Insets mMouseInsets = default(Insets);

		public bool mDoFinger;

		public bool mWantsFocus;

		public Widget mTabPrev;

		public Widget mTabNext;

		public static bool mWriteColoredString = true;

		public void WidgetRemovedHelper()
		{
			if (mWidgetManager != null)
			{
				foreach (Widget mWidget in mWidgets)
				{
					mWidget.WidgetRemovedHelper();
				}
				mWidgetManager.DisableWidget(this);
				foreach (PreModalInfo mPreModalInfo in mWidgetManager.mPreModalInfoList)
				{
					if (mPreModalInfo.mPrevBaseModalWidget == this)
					{
						mPreModalInfo.mPrevBaseModalWidget = null;
					}
					if (mPreModalInfo.mPrevFocusWidget == this)
					{
						mPreModalInfo.mPrevFocusWidget = null;
					}
				}
				RemovedFromManager(mWidgetManager);
				MarkDirtyFull(this);
				mWidgetManager = null;
			}
		}

		public Widget()
		{
			Reset();
		}

		public virtual bool DoScroll(_Touch touch)
		{
			return true;
		}

		protected virtual void Reset()
		{
			mWidgetManager = null;
			mVisible = true;
			mDisabled = false;
			mIsDown = false;
			mIsOver = false;
			mDoFinger = false;
			mMouseVisible = true;
			mHasFocus = false;
			mHasTransparencies = false;
			mWantsFocus = false;
			mTabPrev = null;
			mTabNext = null;
		}

		public override void Dispose()
		{
			mColors.Clear();
		}

		public virtual void OrderInManagerChanged()
		{
		}

		public virtual void SetVisible(bool isVisible)
		{
			if (mVisible != isVisible)
			{
				mVisible = isVisible;
				if (mVisible)
				{
					MarkDirty();
				}
				else
				{
					MarkDirtyFull();
				}
				if (mWidgetManager != null)
				{
					mWidgetManager.RehupMouse();
				}
			}
		}

		public virtual void SetColors(int[,] theColors, int theNumColors)
		{
			mColors.Clear();
			for (int i = 0; i < theNumColors; i++)
			{
				SetColor(i, new Color(theColors[i, 0], theColors[i, 1], theColors[i, 2]));
			}
			MarkDirty();
		}

		public virtual void SetColor(int theIdx, SexyColor theColor)
		{
			SetColor(theIdx, theColor.Color);
		}

		public virtual void SetColor(ButtonWidget.ColorType theIdx, Color theColor)
		{
			SetColor((int)theIdx, theColor);
		}

		public virtual void SetColor(int theIdx, Color theColor)
		{
			if (theIdx >= mColors.Count)
			{
				mColors.Add(theColor);
			}
			else
			{
				mColors[theIdx] = theColor;
			}
			MarkDirty();
		}

		public virtual Color GetColor(Dialog.DialogColour theIdx)
		{
			return GetColor((int)theIdx);
		}

		public virtual Color GetColor(int theIdx)
		{
			Color result = default(Color);
			if (theIdx < mColors.Count)
			{
				return mColors[theIdx];
			}
			return result;
		}

		public virtual Color GetColor(Dialog.DialogColour theIdx, Color theDefaultColor)
		{
			return GetColor((int)theIdx, theDefaultColor);
		}

		public virtual Color GetColor(int theIdx, Color theDefaultColor)
		{
			if (theIdx < mColors.Count)
			{
				return mColors[theIdx];
			}
			return theDefaultColor;
		}

		public virtual void SetDisabled(bool isDisabled)
		{
			if (mDisabled != isDisabled)
			{
				mDisabled = isDisabled;
				if (isDisabled && mWidgetManager != null)
				{
					mWidgetManager.DisableWidget(this);
				}
				MarkDirty();
				if (!isDisabled && mWidgetManager != null && Contains(mWidgetManager.mLastMouseX, mWidgetManager.mLastMouseY))
				{
					mWidgetManager.MousePosition(mWidgetManager.mLastMouseX, mWidgetManager.mLastMouseY);
				}
			}
		}

		public virtual void ShowFinger(bool on)
		{
		}

		public virtual void Resize(int theX, int theY, int theWidth, int theHeight)
		{
			if (mX != theX || mY != theY || mWidth != theWidth || mHeight != theHeight)
			{
				MarkDirtyFull();
				mX = theX;
				mY = theY;
				mWidth = theWidth;
				mHeight = theHeight;
				MarkDirty();
				if (mWidgetManager != null)
				{
					mWidgetManager.RehupMouse();
				}
			}
		}

		public virtual void Resize(TRect theRect)
		{
			Resize(theRect.mX, theRect.mY, theRect.mWidth, theRect.mHeight);
		}

		public virtual void Move(int theNewX, int theNewY)
		{
			Resize(theNewX, theNewY, mWidth, mHeight);
		}

		public virtual bool WantsFocus()
		{
			return mWantsFocus;
		}

		public override void Draw(Graphics g)
		{
		}

		public virtual void DrawOverlay(Graphics g)
		{
		}

		public virtual void DrawOverlay(Graphics g, int thePriority)
		{
			DrawOverlay(g);
		}

		public override void Update()
		{
			base.Update();
		}

		public override void UpdateF(float theFrac)
		{
		}

		public virtual void GotFocus()
		{
			mHasFocus = true;
		}

		public virtual void LostFocus()
		{
			mHasFocus = false;
		}

		public virtual void KeyChar(SexyChar theChar)
		{
		}

		public virtual void KeyDown(KeyCode theKey)
		{
			if (theKey != KeyCode.KEYCODE_TAB)
			{
				return;
			}
			if (mWidgetManager.mKeyDown[16])
			{
				if (mTabPrev != null)
				{
					mWidgetManager.SetFocus(mTabPrev);
				}
			}
			else if (mTabNext != null)
			{
				mWidgetManager.SetFocus(mTabNext);
			}
		}

		public virtual void KeyUp(KeyCode theKey)
		{
		}

		public virtual void MouseEnter()
		{
		}

		public virtual void MouseLeave()
		{
		}

		public virtual void MouseMove(int x, int y)
		{
		}

		public virtual void MouseDown(int x, int y, int theMagicCode)
		{
			if (theMagicCode == 3)
			{
				MouseDown(x, y, 2, 1);
			}
			else if (theMagicCode >= 0)
			{
				MouseDown(x, y, 0, theMagicCode);
			}
			else
			{
				MouseDown(x, y, 1, -theMagicCode);
			}
		}

		public virtual void MouseDown(int x, int y, int theBtnNum, int theClickCount)
		{
		}

		public virtual void MouseUp(int x, int y, int theMagicCode)
		{
			if (theMagicCode == 3)
			{
				MouseUp(x, y, 2, 1);
			}
			else if (theMagicCode >= 0)
			{
				MouseUp(x, y, 0, theMagicCode);
			}
			else
			{
				MouseUp(x, y, 1, -theMagicCode);
			}
		}

		public virtual void MouseUp(int x, int y, int theBtnNum, int theClickCount)
		{
		}

		public virtual void MouseDrag(int x, int y)
		{
		}

		public virtual void MouseWheel(int theDelta)
		{
		}

		public virtual void TouchBegan(_Touch touch)
		{
			int x = (int)touch.location.X;
			int y = (int)touch.location.Y;
			MouseDown(x, y, 1);
		}

		public virtual void TouchMoved(_Touch touch)
		{
			int x = (int)touch.location.X;
			int y = (int)touch.location.Y;
			MouseDrag(x, y);
		}

		public virtual void TouchEnded(_Touch touch)
		{
			int x = (int)touch.location.X;
			int y = (int)touch.location.Y;
			MouseUp(x, y, 1);
		}

		public virtual void TouchesCanceled()
		{
		}

		public virtual bool IsPointVisible(int x, int y)
		{
			return true;
		}

		public virtual TRect WriteCenteredLine(Graphics g, int anOffset, string theLine)
		{
			Font font = g.GetFont();
			int num = font.StringWidth(theLine);
			int theX = (mWidth - num) / 2;
			g.DrawString(theLine, theX, anOffset);
			return new TRect(theX, anOffset - font.GetAscent(), num, font.GetHeight());
		}

		public virtual TRect WriteCenteredLine(Graphics g, int anOffset, string theLine, Color theColor1, Color theColor2)
		{
			return WriteCenteredLine(g, anOffset, theLine, theColor1, theColor2, new TPoint(1, 2));
		}

		public virtual TRect WriteCenteredLine(Graphics g, int anOffset, string theLine, Color theColor1, Color theColor2, TPoint theShadowOffset)
		{
			Font font = g.GetFont();
			int num = font.StringWidth(theLine);
			int num2 = (mWidth - num) / 2;
			g.SetColor(theColor2);
			g.DrawString(theLine, (mWidth - num) / 2 + theShadowOffset.mX, anOffset + theShadowOffset.mY);
			g.SetColor(theColor1);
			g.DrawString(theLine, (mWidth - num) / 2, anOffset);
			return new TRect(num2 + Math.Min(0, theShadowOffset.mX), anOffset - font.GetAscent() + Math.Min(0, theShadowOffset.mY), num + Math.Abs(theShadowOffset.mX), font.GetHeight() + Math.Abs(theShadowOffset.mY));
		}

		public virtual int WriteString(Graphics g, string theString, int theX, int theY, int theWidth, int theJustification, bool drawString, int theOffset)
		{
			return WriteString(g, theString, theX, theY, theWidth, theJustification, drawString, theOffset, -1);
		}

		public virtual int WriteString(Graphics g, string theString, int theX, int theY, int theWidth, int theJustification, bool drawString)
		{
			return WriteString(g, theString, theX, theY, theWidth, theJustification, drawString, 0, -1);
		}

		public virtual int WriteString(Graphics g, string theString, int theX, int theY, int theWidth, int theJustification)
		{
			return WriteString(g, theString, theX, theY, theWidth, theJustification, true, 0, -1);
		}

		public virtual int WriteString(Graphics g, string theString, int theX, int theY, int theWidth)
		{
			return WriteString(g, theString, theX, theY, theWidth, -1, true, 0, -1);
		}

		public virtual int WriteString(Graphics g, string theString, int theX, int theY)
		{
			return WriteString(g, theString, theX, theY, -1, -1, true, 0, -1);
		}

		public virtual int WriteString(Graphics g, string theString, int theX, int theY, int theWidth, int theJustification, bool drawString, int theOffset, int theLength)
		{
			bool flag = g.mWriteColoredString;
			g.mWriteColoredString = mWriteColoredString;
			int result = g.WriteString(theString, theX, theY, theWidth, theJustification, drawString, theOffset, theLength);
			g.mWriteColoredString = flag;
			return result;
		}

		public virtual int WriteWordWrapped(Graphics g, TRect theRect, string theLine, int theLineSpacing, int theJustification)
		{
			bool flag = g.mWriteColoredString;
			g.mWriteColoredString = mWriteColoredString;
			int result = g.WriteWordWrapped(theRect, theLine, theLineSpacing, theJustification);
			g.mWriteColoredString = flag;
			return result;
		}

		public virtual int GetWordWrappedHeight(Graphics g, int theWidth, string theLine, int aLineSpacing)
		{
			int theMaxWidth = 0;
			int theMaxChars = 0;
			return g.GetWordWrappedHeight(theWidth, theLine, aLineSpacing, ref theMaxWidth, theMaxChars);
		}

		public virtual int GetNumDigits(int theNumber)
		{
			int num = 10;
			int num2 = 1;
			while (theNumber >= num)
			{
				num2++;
				num *= 10;
			}
			return num2;
		}

		public virtual void WriteNumberFromStrip(Graphics g, int theNumber, int theX, int theY, Image theNumberStrip, int aSpacing)
		{
			int num = 10;
			int num2 = 1;
			while (theNumber >= num)
			{
				num2++;
				num *= 10;
			}
			if (theNumber == 0)
			{
				num = 10;
			}
			int num3 = theNumberStrip.GetWidth() / 10;
			for (int i = 0; i < num2; i++)
			{
				num /= 10;
				int num4 = theNumber / num % 10;
				Graphics graphics = g.Create();
				graphics.ClipRect(theX + i * (num3 + aSpacing), theY, num3, theNumberStrip.GetHeight());
				graphics.DrawImage(theNumberStrip, theX + i * (num3 + aSpacing) - num4 * num3, theY);
				graphics.Dispose();
			}
		}

		public virtual bool Contains(int theX, int theY)
		{
			if (theX >= mX && theX < mX + mWidth && theY >= mY)
			{
				return theY < mY + mHeight;
			}
			return false;
		}

		public virtual TRect GetInsetRect()
		{
			return new TRect(mX + mMouseInsets.mLeft, mY + mMouseInsets.mTop, mWidth - mMouseInsets.mLeft - mMouseInsets.mRight, mHeight - mMouseInsets.mTop - mMouseInsets.mBottom);
		}

		public void DeferOverlay()
		{
			DeferOverlay(0);
		}

		public void DeferOverlay(int thePriority)
		{
			mWidgetManager.DeferOverlay(this, thePriority);
		}

		public int Left()
		{
			return mX;
		}

		public int Top()
		{
			return mY;
		}

		public int Right()
		{
			return mX + mWidth;
		}

		public int Bottom()
		{
			return mY + mHeight;
		}

		public int Width()
		{
			return mWidth;
		}

		public int Height()
		{
			return mHeight;
		}

		public void Layout(int theLayoutFlags, Widget theRelativeWidget, int theLeftPad, int theTopPad, int theWidthPad)
		{
			Layout(theLayoutFlags, theRelativeWidget, theLeftPad, theTopPad, theWidthPad, 0);
		}

		public void Layout(int theLayoutFlags, Widget theRelativeWidget, int theLeftPad, int theTopPad)
		{
			Layout(theLayoutFlags, theRelativeWidget, theLeftPad, theTopPad, 0, 0);
		}

		public void Layout(int theLayoutFlags, Widget theRelativeWidget, int theLeftPad)
		{
			Layout(theLayoutFlags, theRelativeWidget, theLeftPad, 0, 0, 0);
		}

		public void Layout(int theLayoutFlags, Widget theRelativeWidget)
		{
			Layout(theLayoutFlags, theRelativeWidget, 0, 0, 0, 0);
		}

		public void Layout(int theLayoutFlags, Widget theRelativeWidget, int theLeftPad, int theTopPad, int theWidthPad, int theHeightPad)
		{
			int num = theRelativeWidget.Left();
			int num2 = theRelativeWidget.Top();
			if (theRelativeWidget == mParent)
			{
				num = 0;
				num2 = 0;
			}
			int num3 = theRelativeWidget.Width();
			int num4 = theRelativeWidget.Height();
			int num5 = num + num3;
			int num6 = num2 + num4;
			int num7 = Left();
			int num8 = Top();
			int num9 = Width();
			int num10 = Height();
			for (int num11 = 1; num11 < 4194304; num11 <<= 1)
			{
				if ((theLayoutFlags & num11) != 0)
				{
					switch (num11)
					{
					case 1:
						num9 = num3 + theWidthPad;
						break;
					case 2:
						num10 = num4 + theHeightPad;
						break;
					case 256:
						num8 = num2 - num10 + theTopPad;
						break;
					case 512:
						num8 = num6 + theTopPad;
						break;
					case 1024:
						num7 = num5 + theLeftPad;
						break;
					case 2048:
						num7 = num - num9 + theLeftPad;
						break;
					case 4096:
						num7 = num + theLeftPad;
						break;
					case 8192:
						num7 = num5 - num9 + theLeftPad;
						break;
					case 16384:
						num8 = num2 + theTopPad;
						break;
					case 32768:
						num8 = num6 - num10 + theTopPad;
						break;
					case 65536:
						num9 = num5 - num7 + theWidthPad;
						break;
					case 131072:
						num9 = num - num7 + theWidthPad;
						break;
					case 262144:
						num10 = num2 - num8 + theHeightPad;
						break;
					case 524288:
						num10 = num6 - num8 + theHeightPad;
						break;
					case 16:
						num7 = theLeftPad;
						break;
					case 32:
						num8 = theTopPad;
						break;
					case 64:
						num9 = theWidthPad;
						break;
					case 128:
						num10 = theHeightPad;
						break;
					case 1048576:
						num7 = num + (num3 - num9) / 2 + theLeftPad;
						break;
					case 2097152:
						num8 = num2 + (num4 - num10) / 2 + theTopPad;
						break;
					}
				}
			}
			Resize(num7, num8, num9, num10);
		}
	}
}
