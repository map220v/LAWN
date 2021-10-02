using Microsoft.Xna.Framework;
using Sexy;
using Sexy.TodLib;
using System;
using System.Collections.Generic;

namespace Lawn
{
	internal class MessageWidget
	{
		public LawnApp mApp;

		public string mLabel = string.Empty;

		public string mLabelString;

		public List<string> mLabelStringList = new List<string>();

		public int mDisplayTime;

		public int mDuration;

		public MessageStyle mMessageStyle;

		public Reanimation[] mTextReanimID = new Reanimation[128];

		public ReanimationType mReanimType;

		public int mSlideOffTime;

		public Image mIcon;

		public char[] mLabelNext = new char[128];

		public MessageStyle mMessageStyleNext;

		public MessageWidget(LawnApp theApp)
		{
			mApp = theApp;
			mDuration = 0;
			mLabel = string.Empty;
			mMessageStyle = MessageStyle.MESSAGE_STYLE_OFF;
			mLabelNext[0] = '\uffff';
			mMessageStyleNext = MessageStyle.MESSAGE_STYLE_OFF;
			mSlideOffTime = 100;
			mIcon = null;
			for (int i = 0; i < 128; i++)
			{
				mTextReanimID[i] = null;
			}
		}

		public void Dispose()
		{
			ClearReanim();
		}

		public void SetLabel(string theNewLabel, MessageStyle theMessageStyle)
		{
			SetLabel(theNewLabel, theMessageStyle, null);
		}

		public void SetLabel(string theNewLabel, MessageStyle theMessageStyle, Image theIcon)
		{
			string text = TodStringFile.TodStringTranslate(theNewLabel);
			Debug.ASSERT(text.length() < 127);
			if (mReanimType != ReanimationType.REANIM_NONE && mDuration > 0)
			{
				mMessageStyleNext = theMessageStyle;
				mLabelNext = string.Copy(text).ToCharArray();
				mDuration = Math.Min(mDuration, 100 + mSlideOffTime + 1);
				return;
			}
			ClearReanim();
			mLabel = text;
			mLabelString = string.Copy(text);
			mLabelStringList.Clear();
			for (int i = 0; i < mLabel.Length; i++)
			{
				mLabelStringList.Add(mLabel[i].ToString());
			}
			mMessageStyle = theMessageStyle;
			mReanimType = ReanimationType.REANIM_NONE;
			switch (theMessageStyle)
			{
			case MessageStyle.MESSAGE_STYLE_HINT_LONG:
			case MessageStyle.MESSAGE_STYLE_HINT_TALL_LONG:
			case MessageStyle.MESSAGE_STYLE_BIG_MIDDLE:
			case MessageStyle.MESSAGE_STYLE_ZEN_GARDEN_LONG:
				mDuration = 1500;
				break;
			case MessageStyle.MESSAGE_STYLE_HINT_TALL_UNLOCKMESSAGE:
				mDuration = 500;
				break;
			case MessageStyle.MESSAGE_STYLE_TUTORIAL_LEVEL1:
			case MessageStyle.MESSAGE_STYLE_TUTORIAL_LEVEL2:
			case MessageStyle.MESSAGE_STYLE_TUTORIAL_LATER:
			case MessageStyle.MESSAGE_STYLE_HINT_FAST:
			case MessageStyle.MESSAGE_STYLE_HINT_TALL_FAST:
			case MessageStyle.MESSAGE_STYLE_BIG_MIDDLE_FAST:
				mDuration = 500;
				break;
			case MessageStyle.MESSAGE_STYLE_ACHIEVEMENT:
				mDuration = 250;
				break;
			case MessageStyle.MESSAGE_STYLE_TUTORIAL_LEVEL1_STAY:
			case MessageStyle.MESSAGE_STYLE_TUTORIAL_LATER_STAY:
			case MessageStyle.MESSAGE_STYLE_HINT_STAY:
				mDuration = 10000;
				break;
			case MessageStyle.MESSAGE_STYLE_HOUSE_NAME:
				mDuration = 250;
				break;
			case MessageStyle.MESSAGE_STYLE_HUGE_WAVE:
				mDuration = 750;
				mReanimType = ReanimationType.REANIM_TEXT_FADE_ON;
				break;
			case MessageStyle.MESSAGE_STYLE_SLOT_MACHINE:
				mDuration = 750;
				break;
			default:
				Debug.ASSERT(false);
				break;
			}
			if (mReanimType != ReanimationType.REANIM_NONE)
			{
				LayoutReanimText();
			}
			mDisplayTime = mDuration;
			mIcon = theIcon;
		}

		public void Update()
		{
			if (mApp.mBoard == null || mApp.mBoard.mPaused)
			{
				return;
			}
			if (mDuration < 10000 && mDuration > 0)
			{
				mDuration -= 3;
				if (mDuration >= 0 && mDuration < 3)
				{
					mMessageStyle = MessageStyle.MESSAGE_STYLE_OFF;
					mIcon = null;
					if (mMessageStyleNext != 0)
					{
						SetLabel(new string(mLabelNext), mMessageStyleNext);
						mMessageStyleNext = MessageStyle.MESSAGE_STYLE_OFF;
					}
				}
			}
			for (int i = 0; i < mLabel.Length; i++)
			{
				Reanimation reanimation = mApp.ReanimationTryToGet(mTextReanimID[i]);
				if (reanimation == null)
				{
					break;
				}
				int theTimeEnd = 50;
				int num = 1;
				if (mReanimType == ReanimationType.REANIM_TEXT_FADE_ON)
				{
					num = 100;
				}
				if (mDuration > mSlideOffTime)
				{
					if (mReanimType == ReanimationType.REANIM_TEXT_FADE_ON)
					{
						reanimation.mAnimRate = 60f;
					}
					else
					{
						int num2 = (mDisplayTime - mDuration) * num;
						reanimation.mAnimRate = TodCommon.TodAnimateCurveFloat(0, theTimeEnd, num2 - i, 0f, 40f, TodCurves.CURVE_LINEAR);
					}
				}
				else
				{
					if (mDuration >= mSlideOffTime && mDuration < mSlideOffTime + 3)
					{
						reanimation.PlayReanim(GlobalMembersReanimIds.ReanimTrackId_anim_leave, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 0, 0f);
					}
					int num3 = (mSlideOffTime - mDuration) * num;
					reanimation.mAnimRate = TodCommon.TodAnimateCurveFloat(0, theTimeEnd, num3 - i, 0f, 40f, TodCurves.CURVE_LINEAR);
				}
				reanimation.Update();
			}
		}

		public void Draw(Graphics g)
		{
			if (mDuration <= 3)
			{
				return;
			}
			Font font = GetFont();
			Font font2 = null;
			int num = Constants.BOARD_WIDTH / 2;
			int num2 = 596;
			int num3 = 255;
			SexyColor theColor = new SexyColor(250, 250, 0, 255);
			SexyColor theColor2 = new SexyColor(0, 0, 0, 255);
			bool flag = false;
			int num4 = 0;
			int num5 = 0;
			if (font == Resources.FONT_CONTINUUMBOLD14)
			{
				font2 = Resources.FONT_CONTINUUMBOLD14OUTLINE;
			}
			switch (mMessageStyle)
			{
			case MessageStyle.MESSAGE_STYLE_TUTORIAL_LEVEL1:
			case MessageStyle.MESSAGE_STYLE_TUTORIAL_LEVEL1_STAY:
				num2 = 476;
				num4 = (int)Constants.InvertAndScale(60f);
				theColor = new SexyColor(253, 245, 173);
				num3 = 192;
				break;
			case MessageStyle.MESSAGE_STYLE_TUTORIAL_LEVEL2:
			case MessageStyle.MESSAGE_STYLE_TUTORIAL_LATER:
			case MessageStyle.MESSAGE_STYLE_TUTORIAL_LATER_STAY:
				num2 = 476;
				num4 = (int)Constants.InvertAndScale(60f);
				theColor = new SexyColor(253, 245, 173);
				num3 = 192;
				break;
			case MessageStyle.MESSAGE_STYLE_HINT_TALL_FAST:
			case MessageStyle.MESSAGE_STYLE_HINT_TALL_UNLOCKMESSAGE:
			case MessageStyle.MESSAGE_STYLE_HINT_TALL_LONG:
				num2 = 476;
				num4 = (int)Constants.InvertAndScale(70f);
				theColor = new SexyColor(253, 245, 173);
				num3 = 192;
				num5 = 30;
				break;
			case MessageStyle.MESSAGE_STYLE_ACHIEVEMENT:
				num2 = 466;
				num4 = (int)Constants.InvertAndScale(40f);
				theColor = new SexyColor(253, 245, 173);
				num3 = 192;
				break;
			case MessageStyle.MESSAGE_STYLE_HINT_LONG:
			case MessageStyle.MESSAGE_STYLE_HINT_FAST:
			case MessageStyle.MESSAGE_STYLE_HINT_STAY:
				num2 = 527;
				num4 = (int)Constants.InvertAndScale(40f);
				theColor = new SexyColor(253, 245, 173);
				num3 = 192;
				break;
			case MessageStyle.MESSAGE_STYLE_BIG_MIDDLE:
			case MessageStyle.MESSAGE_STYLE_BIG_MIDDLE_FAST:
				num2 = 476;
				num4 = (int)Constants.InvertAndScale(60f);
				theColor = new SexyColor(253, 245, 173);
				num3 = 192;
				break;
			case MessageStyle.MESSAGE_STYLE_HOUSE_NAME:
				num2 = 550;
				theColor = new SexyColor(255, 255, 255, 255);
				flag = true;
				break;
			case MessageStyle.MESSAGE_STYLE_HUGE_WAVE:
				num2 = 290;
				theColor = new SexyColor(255, 0, 0);
				break;
			case MessageStyle.MESSAGE_STYLE_ZEN_GARDEN_LONG:
				num2 = 514;
				num4 = (int)Constants.InvertAndScale(40f);
				theColor = new SexyColor(253, 245, 173);
				num3 = 192;
				break;
			case MessageStyle.MESSAGE_STYLE_SLOT_MACHINE:
				num = mApp.mWidth / 2 + Constants.Board_Offset_AspectRatio_Correction;
				num2 = Constants.MessageWidget_SlotMachine_Y;
				num3 = 64;
				break;
			default:
				Debug.ASSERT(false);
				break;
			}
			num2 = (int)((float)num2 * Constants.S);
			if (mReanimType != ReanimationType.REANIM_NONE)
			{
				if (font == Resources.FONT_CONTINUUMBOLD14)
				{
					DrawReanimatedText(g, Resources.FONT_CONTINUUMBOLD14OUTLINE, SexyColor.Black, num2);
				}
				DrawReanimatedText(g, font, theColor, num2);
				return;
			}
			if (num3 != 255)
			{
				theColor.mAlpha = TodCommon.TodAnimateCurve(75, 0, mApp.mBoard.mMainCounter % 75, num3, 255, TodCurves.CURVE_BOUNCE_SLOW_MIDDLE);
				theColor2.mAlpha = theColor.mAlpha;
			}
			if (flag)
			{
				theColor.mAlpha = TodCommon.ClampInt(mDuration * 15, 0, 255);
				theColor2.mAlpha = theColor.mAlpha;
			}
			if (num4 > 0)
			{
				num2 -= (int)Constants.InvertAndScale(30f);
				TRect theRect = new TRect(-mApp.mBoard.mX, num2, Constants.BOARD_WIDTH, num4);
				g.SetColor(new SexyColor(0, 0, 0, 128));
				g.SetColorizeImages(true);
				g.FillRect(theRect);
				theRect.mY -= (int)Constants.InvertAndScale(3f);
				theRect.mX += num5;
				TodStringFile.TodDrawStringWrapped(g, mLabel, theRect, font, theColor, DrawStringJustification.DS_ALIGN_CENTER_VERTICAL_MIDDLE);
			}
			else
			{
				TRect theRect2 = new TRect(num - mApp.mWidth / 2 - mApp.mBoard.mX, num2 + font.GetAscent(), mApp.mWidth, mApp.mHeight);
				if (font2 != null)
				{
					TodStringFile.TodDrawStringWrapped(g, mLabel, theRect2, font2, theColor2, DrawStringJustification.DS_ALIGN_CENTER);
				}
				TodStringFile.TodDrawStringWrapped(g, mLabel, theRect2, font, theColor, DrawStringJustification.DS_ALIGN_CENTER);
			}
			if (mMessageStyle == MessageStyle.MESSAGE_STYLE_HOUSE_NAME)
			{
				string text = string.Empty;
				if (mApp.IsSurvivalMode() && mApp.mBoard.mChallenge.mSurvivalStage > 0)
				{
					int numWavesPerFlag = mApp.mBoard.GetNumWavesPerFlag();
					int num6 = mApp.mBoard.mChallenge.mSurvivalStage * mApp.mBoard.GetNumWavesPerSurvivalStage() / numWavesPerFlag;
					string theStringToSubstitute = mApp.Pluralize(num6, "[ONE_FLAG]", "[COUNT_FLAGS]");
					text = ((num6 != 1) ? TodCommon.TodReplaceString("[FLAGS_COMPLETED_PLURAL]", "{FLAGS}", theStringToSubstitute) : TodCommon.TodReplaceString("[FLAGS_COMPLETED]", "{FLAGS}", theStringToSubstitute));
				}
				if (text.length() > 0)
				{
					TodCommon.TodDrawString(g, text, mApp.mWidth / 2 - mApp.mBoard.mX, num2 + 26, Resources.FONT_HOUSEOFTERROR16, new SexyColor(224, 187, 98, theColor.mAlpha), DrawStringJustification.DS_ALIGN_CENTER);
				}
			}
			if (mMessageStyle == MessageStyle.MESSAGE_STYLE_ACHIEVEMENT && mIcon != null)
			{
				g.DrawImage(mIcon, 10, num2 - 10);
			}
			g.SetColor(SexyColor.White);
			g.SetDrawMode(Graphics.DrawMode.DRAWMODE_NORMAL);
		}

		public void ClearReanim()
		{
			for (int i = 0; i < 128; i++)
			{
				Reanimation reanimation = mApp.ReanimationTryToGet(mTextReanimID[i]);
				if (reanimation != null)
				{
					reanimation.ReanimationDie();
					mTextReanimID[i] = null;
				}
			}
		}

		public void ClearLabel()
		{
			if (mReanimType != ReanimationType.REANIM_NONE)
			{
				if (mApp.mGameMode == GameMode.GAMEMODE_CHALLENGE_SPEED)
				{
					mDuration = Math.Min(mDuration, 51);
				}
				else
				{
					mDuration = Math.Min(mDuration, 100 + mSlideOffTime + 1);
				}
			}
			else
			{
				mDuration = 0;
			}
			mIcon = null;
		}

		public bool IsBeingDisplayed()
		{
			if (mDuration >= 0 && mDuration < 3)
			{
				return false;
			}
			return true;
		}

		public Font GetFont()
		{
			return Resources.FONT_HOUSEOFTERROR16;
		}

		public void DrawReanimatedText(Graphics g, Font theFont, SexyColor theColor, float thePosY)
		{
			int length = mLabel.Length;
			for (int i = 0; i < mLabelStringList.Count; i++)
			{
				if (!string.IsNullOrEmpty(mLabelStringList[i]))
				{
					Reanimation reanimation = mApp.ReanimationTryToGet(mTextReanimID[i]);
					if (reanimation == null)
					{
						break;
					}
					ReanimatorTransform aTransformCurrent;
					reanimation.GetCurrentTransform(2, out aTransformCurrent, false);
					int num = TodCommon.ClampInt(TodCommon.FloatRoundToInt(aTransformCurrent.mAlpha * (float)theColor.mAlpha), 0, 255);
					if (num <= 0)
					{
						break;
					}
					SexyColor theColor2 = theColor;
					theColor2.mAlpha = num;
					aTransformCurrent.mTransX += reanimation.mOverlayMatrix.mMatrix.M41 + (float)Constants.ReanimTextCenterOffsetX - (float)(Constants.Board_Offset_AspectRatio_Correction / 2);
					aTransformCurrent.mTransY += reanimation.mOverlayMatrix.mMatrix.M42 + thePosY - 300f * Constants.S;
					if (mReanimType == ReanimationType.REANIM_TEXT_FADE_ON && mDisplayTime - mDuration < mSlideOffTime)
					{
						float num2 = 1f - reanimation.mAnimTime;
						aTransformCurrent.mTransX += reanimation.mOverlayMatrix.mMatrix.M41 * num2;
					}
					Matrix theMatrix = default(Matrix);
					Reanimation.MatrixFromTransform(aTransformCurrent, out theMatrix);
					TodCommon.TodDrawStringMatrix(g, theFont, theMatrix, mLabelStringList[i], theColor2);
					aTransformCurrent.PrepareForReuse();
				}
			}
		}

		public void LayoutReanimText()
		{
			float[] array = new float[5];
			int num = 0;
			float val = 0f;
			int num2 = 0;
			Font font = GetFont();
			int length = mLabel.Length;
			mSlideOffTime = 100 + length;
			for (int i = 0; i <= length; i++)
			{
				if (i == length || mLabel[i] == '\n')
				{
					Debug.ASSERT(num < 5);
					int count = i - num2;
					int index = num2;
					num2 = i + 1;
					string theString = new string(mLabel[index], count);
					array[num] = font.StringWidth(theString);
					val = Math.Max(val, array[num]);
					num++;
				}
			}
			num = 0;
			float num3 = (0f - array[num]) * 0.5f;
			float num4 = 0f;
			for (int j = 0; j < length; j++)
			{
				Reanimation reanimation = mApp.AddReanimation(num3 * Constants.IS, num4 * Constants.IS, 0, mReanimType);
				reanimation.mIsAttachment = true;
				reanimation.PlayReanim(GlobalMembersReanimIds.ReanimTrackId_anim_enter, ReanimLoopType.REANIM_PLAY_ONCE_AND_HOLD, 0, 0f);
				mTextReanimID[j] = mApp.ReanimationGetID(reanimation);
				num3 += (float)font.CharWidth(mLabel[j]);
				if (mLabel[j] == '\n')
				{
					num++;
					Debug.ASSERT(num < 5);
					num3 = (0f - array[num]) * 0.5f;
					num4 += (float)font.GetLineSpacing();
				}
			}
		}
	}
}
