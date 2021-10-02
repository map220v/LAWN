using Lawn;
using Microsoft.Xna.Framework;
using System;

namespace Sexy.TodLib
{
	internal static class TodCommon
	{
		public delegate string gGetCurrentLevelNameDelegate();

		public delegate bool gAppCloseRequestDelegate();

		public delegate bool gAppHasUsedCheatKeysDelegate();

		public delegate bool gExtractResourcesByNameDelegate(ResourceManager theManager, string theName, bool doUnpackAtlasImages);

		public const uint SEXY_RAND_MAX = 2147483647u;

		public const int D3DImageFlag_NeedsSanding = 4096;

		internal const int POOL_SIZE = 4096;

		public const int MAX_GLOBAL_ALLOCATORS = 128;

		public static float DEG_TO_RAD = (float)Math.PI / 180f;

		public static float RAD_TO_DEG = 57.29578f;

		public static bool OffsetForGraphicsTranslation = true;

		public static gGetCurrentLevelNameDelegate gGetCurrentLevelName = TodGetCurrentLevelName;

		public static gAppCloseRequestDelegate gAppCloseRequest = TodAppCloseRequest;

		public static gAppHasUsedCheatKeysDelegate gAppHasUsedCheatKeys = TodAppHasUsedCheatKeys;

		public static gExtractResourcesByNameDelegate gExtractResourcesByName = null;

		public static int gNumGobalAllocators = 0;

		public static float PixelAligned(float num)
		{
			int num2 = (int)(num * Constants.S + 0.5f);
			return (float)num2 * Constants.IS;
		}

		public static float FloatLerp(float start, float end, float t)
		{
			return start + t * (end - start);
		}

		public static byte ClampByte(byte num, byte minNum, byte maxNum)
		{
			if (num <= minNum)
			{
				return minNum;
			}
			if (num >= maxNum)
			{
				return maxNum;
			}
			return num;
		}

		public static int ClampInt(int num, int minNum, int maxNum)
		{
			if (num <= minNum)
			{
				return minNum;
			}
			if (num >= maxNum)
			{
				return maxNum;
			}
			return num;
		}

		public static float ClampFloat(float num, float minNum, float maxNum)
		{
			if (num <= minNum)
			{
				return minNum;
			}
			if (num >= maxNum)
			{
				return maxNum;
			}
			return num;
		}

		public static int FloatRoundToInt(float theFloatValue)
		{
			if (theFloatValue > 0f)
			{
				return (int)(theFloatValue + 0.5f);
			}
			return (int)(theFloatValue - 0.5f);
		}

		public static bool FloatApproxEqual(float theFloatValue1, float theFloatValue2)
		{
			if (Math.Abs(theFloatValue1 - theFloatValue2) < 1E-06f)
			{
				return true;
			}
			return false;
		}

		public static float Distance2D(float x1, float y1, float x2, float y2)
		{
			return (float)Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
		}

		public static float DegToRad(float theAngle)
		{
			return theAngle * DEG_TO_RAD;
		}

		public static float RadToDeg(float theAngle)
		{
			return theAngle * RAD_TO_DEG;
		}

		public static void SetBit(ref uint theNumber, int theBitIndex, int theValue)
		{
			if (theValue != 0)
			{
				theNumber |= (uint)(1 << theBitIndex);
			}
			else
			{
				theNumber &= (uint)(1 << theBitIndex);
			}
		}

		public static bool TestBit(uint theNumber, int theBitIndex)
		{
			if (((int)theNumber & (1 << theBitIndex)) != 0)
			{
				return true;
			}
			return false;
		}

		public static float TodCurveS(float theTime)
		{
			return 3f * theTime * theTime - 2f * theTime * theTime * theTime;
		}

		public static float TodCurveEvaluate(float theTime, float thePositionStart, float thePositionEnd, TodCurves theCurve)
		{
			float num = 0f;
			switch (theCurve)
			{
			case TodCurves.CURVE_CONSTANT:
				num = 0f;
				break;
			case TodCurves.CURVE_LINEAR:
				num = theTime;
				break;
			case TodCurves.CURVE_EASE_IN:
				num = TodCurveQuad(theTime);
				break;
			case TodCurves.CURVE_EASE_OUT:
				num = TodCurveInvQuad(theTime);
				break;
			case TodCurves.CURVE_EASE_IN_OUT:
				num = TodCurveS(TodCurveS(theTime));
				break;
			case TodCurves.CURVE_EASE_IN_OUT_WEAK:
				num = TodCurveS(theTime);
				break;
			case TodCurves.CURVE_FAST_IN_OUT:
				num = TodCurveInvQuadS(TodCurveInvQuadS(theTime));
				break;
			case TodCurves.CURVE_FAST_IN_OUT_WEAK:
				num = TodCurveInvQuadS(theTime);
				break;
			case TodCurves.CURVE_BOUNCE:
				num = TodCurveBounce(theTime);
				break;
			case TodCurves.CURVE_BOUNCE_FAST_MIDDLE:
				num = TodCurveQuad(TodCurveBounce(theTime));
				break;
			case TodCurves.CURVE_BOUNCE_SLOW_MIDDLE:
				num = TodCurveInvQuad(TodCurveBounce(theTime));
				break;
			case TodCurves.CURVE_SIN_WAVE:
				num = (float)Math.Sin(theTime * (float)Math.PI * 2f);
				break;
			case TodCurves.CURVE_EASE_SIN_WAVE:
				num = (float)Math.Sin(TodCurveS(theTime) * (float)Math.PI * 2f);
				break;
			default:
				Debug.ASSERT(false);
				break;
			}
			return thePositionStart + (thePositionEnd - thePositionStart) * num;
		}

		public static float TodAnimateCurveFloatTime(float theTimeStart, float theTimeEnd, float theTimeAge, float thePositionStart, float thePositionEnd, TodCurves theCurve)
		{
			float num = theTimeAge - theTimeStart;
			float num2 = theTimeEnd - theTimeStart;
			float theTime = num / num2;
			return TodCurveEvaluateClamped(theTime, thePositionStart, thePositionEnd, theCurve);
		}

		public static float TodAnimateCurveFloat(int theTimeStart, int theTimeEnd, int theTimeAge, float thePositionStart, float thePositionEnd, TodCurves theCurve)
		{
			int num = theTimeAge - theTimeStart;
			int num2 = theTimeEnd - theTimeStart;
			float theTime = (float)num / (float)num2;
			return TodCurveEvaluateClamped(theTime, thePositionStart, thePositionEnd, theCurve);
		}

		public static int TodAnimateCurve(int theTimeStart, int theTimeEnd, int theTimeAge, int thePositionStart, int thePositionEnd, TodCurves theCurve)
		{
			return FloatRoundToInt(TodAnimateCurveFloat(theTimeStart, theTimeEnd, theTimeAge, thePositionStart, thePositionEnd, theCurve));
		}

		public static object TodPickFromWeightedArray(TodWeightedArray[] theArray, int theCount)
		{
			TodWeightedArray todWeightedArray = TodPickArrayItemFromWeightedArray(theArray, theCount);
			return todWeightedArray.mItem;
		}

		public static TodWeightedArray TodPickArrayItemFromWeightedArray(TodWeightedArray[] theArray, int theCount)
		{
			int num = 0;
			for (int i = 0; i < theCount; i++)
			{
				num += theArray[i].mWeight;
			}
			Debug.ASSERT(num > 0);
			int num2 = RandomNumbers.NextNumber(num);
			int num3 = 0;
			for (int j = 0; j < theCount; j++)
			{
				num3 += theArray[j].mWeight;
				if (num2 < num3)
				{
					return theArray[j];
				}
			}
			Debug.ASSERT(false);
			return null;
		}

		public static TodWeightedGridArray TodPickFromWeightedGridArray(TodWeightedGridArray[] theArray, int theCount)
		{
			int num = 0;
			for (int i = 0; i < theCount; i++)
			{
				num += theArray[i].mWeight;
			}
			Debug.ASSERT(num > 0);
			int num2 = RandomNumbers.NextNumber(num);
			int num3 = 0;
			for (int j = 0; j < theCount; j++)
			{
				num3 += theArray[j].mWeight;
				if (num2 < num3)
				{
					return theArray[j];
				}
			}
			Debug.ASSERT(false);
			return null;
		}

		public static int TodPickFromArray(int[] theArray, int theCount)
		{
			Debug.ASSERT(theCount > 0);
			int num = RandomNumbers.NextNumber(theCount);
			return theArray[num];
		}

		public static int TodPickFromSmoothArray(TodSmoothArray[] theArray, int theCount)
		{
			float num = 0f;
			for (int i = 0; i < theCount; i++)
			{
				num += theArray[i].mWeight;
			}
			Debug.ASSERT(num > 0f);
			float num2 = 1f / num;
			float num3 = 0f;
			for (int j = 0; j < theCount; j++)
			{
				num3 += TodCalcSmoothWeight(theArray[j].mWeight * num2, theArray[j].mLastPicked, theArray[j].mSecondLastPicked);
			}
			Debug.ASSERT(num3 > 0f);
			float num4 = RandomNumbers.NextNumber(num3);
			float num5 = 0f;
			int k;
			for (k = 0; k < theCount - 1; k++)
			{
				TodSmoothArray todSmoothArray = theArray[k];
				num5 += TodCalcSmoothWeight(todSmoothArray.mWeight * num2, todSmoothArray.mLastPicked, todSmoothArray.mSecondLastPicked);
				if (num4 <= num5)
				{
					break;
				}
			}
			TodUpdateSmoothArrayPick(theArray, theCount, k);
			return theArray[k].mItem;
		}

		public static void TodUpdateSmoothArrayPick(TodSmoothArray[] theArray, int theCount, int thePickIndex)
		{
			for (int i = 0; i < theCount; i++)
			{
				if (theArray[i].mWeight > 0f)
				{
					theArray[i].mLastPicked += 1f;
					theArray[i].mSecondLastPicked += 1f;
				}
			}
			theArray[thePickIndex].mSecondLastPicked = theArray[thePickIndex].mLastPicked;
			theArray[thePickIndex].mLastPicked = 0f;
		}

		public static float RandRangeFloat(float theMin, float theMax)
		{
			return theMin + RandomNumbers.NextNumber(theMax - theMin);
		}

		public static int RandRangeInt(int theMin, int theMax)
		{
			Debug.ASSERT(theMin <= theMax);
			return theMin + RandomNumbers.NextNumber(theMax - theMin + 1);
		}

		public static void TodDrawString(Graphics g, string theText, int thePosX, int thePosY, Font theFont, SexyColor theColor, DrawStringJustification theJustification)
		{
			TodDrawString(g, theText, thePosX, thePosY, theFont, theColor, theJustification, 1f);
		}

		public static void TodDrawString(Graphics g, string theText, int thePosX, int thePosY, Font theFont, SexyColor theColor, int maxWidth, DrawStringJustification theJustification)
		{
			float num = 1f;
			float num2 = theFont.StringWidth(TodStringFile.TodStringTranslate(theText));
			if (num2 > (float)maxWidth)
			{
				num = (float)maxWidth / num2;
			}
			TodDrawString(g, theText, thePosX, thePosY - (int)((num - 1f) * (float)theFont.GetHeight() / 2f), theFont, theColor, theJustification, num);
		}

		public static void TodDrawString(Graphics g, string theText, int thePosX, int thePosY, Font theFont, SexyColor theColor, DrawStringJustification theJustification, float scale)
		{
			theFont.mScaleX = (theFont.mScaleY = scale);
			string theString = TodStringFile.TodStringTranslate(theText);
			int num = thePosX + g.mTransX;
			int theY = thePosY + g.mTransY;
			switch (theJustification)
			{
			case DrawStringJustification.DS_ALIGN_RIGHT:
			case DrawStringJustification.DS_ALIGN_RIGHT_VERTICAL_MIDDLE:
			{
				int num3 = theFont.StringWidth(theString);
				num -= num3;
				break;
			}
			case DrawStringJustification.DS_ALIGN_CENTER:
			case DrawStringJustification.DS_ALIGN_CENTER_VERTICAL_MIDDLE:
			{
				int num2 = theFont.StringWidth(theString);
				num -= num2 / 2;
				break;
			}
			}
			theFont.DrawString(g, num, theY, theString, theColor);
			theFont.mScaleX = (theFont.mScaleY = 1f);
		}

		public static void TodDrawStringLayer(Graphics g, string theText, int thePosX, int thePosY, Font theFont, SexyColor theColor, DrawStringJustification theJustification, float scale, int layer)
		{
			theFont.mScaleX = (theFont.mScaleY = scale);
			string theString = TodStringFile.TodStringTranslate(theText);
			int num = thePosX + g.mTransX;
			int theY = thePosY + g.mTransY;
			switch (theJustification)
			{
			case DrawStringJustification.DS_ALIGN_RIGHT:
			case DrawStringJustification.DS_ALIGN_RIGHT_VERTICAL_MIDDLE:
			{
				int num3 = theFont.StringWidth(theString);
				num -= num3;
				break;
			}
			case DrawStringJustification.DS_ALIGN_CENTER:
			case DrawStringJustification.DS_ALIGN_CENTER_VERTICAL_MIDDLE:
			{
				int num2 = theFont.StringWidth(theString);
				num -= num2 / 2;
				break;
			}
			}
			theFont.DrawStringLayer(g, num, theY, theString, theColor, layer);
			theFont.mScaleX = (theFont.mScaleY = 1f);
		}

		public static void TodDrawStringCenterBy(Graphics g, string theText, int thePosX, int thePosY, Font theFont, SexyColor theColor, DrawStringJustification theJustification, float scale, string centerString)
		{
			theFont.mScaleX = (theFont.mScaleY = scale);
			string theString = TodStringFile.TodStringTranslate(theText);
			int num = thePosX + g.mTransX;
			int theY = thePosY + g.mTransY;
			switch (theJustification)
			{
			case DrawStringJustification.DS_ALIGN_RIGHT:
			case DrawStringJustification.DS_ALIGN_RIGHT_VERTICAL_MIDDLE:
			{
				int num3 = theFont.StringWidth(centerString);
				num -= num3;
				break;
			}
			case DrawStringJustification.DS_ALIGN_CENTER:
			case DrawStringJustification.DS_ALIGN_CENTER_VERTICAL_MIDDLE:
			{
				int num2 = theFont.StringWidth(centerString);
				num -= num2 / 2;
				break;
			}
			}
			theFont.DrawString(g, num, theY, theString, theColor);
			theFont.mScaleX = (theFont.mScaleY = 1f);
		}

		public static void TodDrawStringMatrix(Graphics g, Font theFont, Matrix theMatrix, string theString, SexyColor theColor)
		{
			g.SetFont(theFont);
			g.SetColor(theColor);
			g.DrawString(theString, (int)theMatrix.M41, (int)theMatrix.M42);
		}

		public static void TodDrawImageCelScaled(Graphics g, Image theImageStrip, int thePosX, int thePosY, int theCelCol, int theCelRow, float theScaleX, float theScaleY)
		{
			Debug.ASSERT(theCelCol >= 0 && theCelCol < theImageStrip.mNumCols);
			Debug.ASSERT(theCelCol >= 0 && theCelCol < theImageStrip.mNumCols);
			Debug.ASSERT(theCelRow >= 0 && theCelRow < theImageStrip.mNumRows);
			int celWidth = theImageStrip.GetCelWidth();
			int celHeight = theImageStrip.GetCelHeight();
			TRect theSrcRect = new TRect(celWidth * theCelCol, celHeight * theCelRow, celWidth, celHeight);
			TRect theDestRect = new TRect(thePosX, thePosY, FloatRoundToInt((float)celWidth * theScaleX), FloatRoundToInt((float)celHeight * theScaleY));
			g.DrawImage(theImageStrip, theDestRect, theSrcRect);
		}

		public static void TodDrawImageCelF(Graphics g, Image theImageStrip, float thePosX, float thePosY, int theCelCol, int theCelRow)
		{
			Debug.ASSERT(theCelCol >= 0 && theCelCol < theImageStrip.mNumCols);
			Debug.ASSERT(theCelRow >= 0 && theCelRow < theImageStrip.mNumRows);
			int celWidth = theImageStrip.GetCelWidth();
			int celHeight = theImageStrip.GetCelHeight();
			TRect theSrcRect = new TRect(celWidth * theCelCol, celHeight * theCelRow, celWidth, celHeight);
			g.DrawImageF(theImageStrip, thePosX, thePosY, theSrcRect);
		}

		public static void TodDrawImageCelScaledF(Graphics g, Image theImageStrip, float thePosX, float thePosY, int theCelCol, int theCelRow, float theScaleX, float theScaleY)
		{
			Debug.ASSERT(theCelCol >= 0 && theCelCol < theImageStrip.mNumCols);
			int celWidth = theImageStrip.GetCelWidth();
			int celHeight = theImageStrip.GetCelHeight();
			TRect theSrcRect = new TRect(celWidth * theCelCol, celHeight * theCelRow, celWidth, celHeight);
			if (theScaleX == 1f && theScaleY == 1f)
			{
				g.DrawImageF(theImageStrip, thePosX, thePosY, theSrcRect);
				return;
			}
			Matrix theTransform = Matrix.Identity;
			theTransform.M41 = (float)celWidth * 0.5f * theScaleX + thePosX + (float)g.mTransX;
			theTransform.M42 = (float)celHeight * 0.5f * theScaleY + thePosY + (float)g.mTransY;
			theTransform.M11 = theScaleX;
			theTransform.M22 = theScaleY;
			SexyColor aColor = g.mColorizeImages ? new SexyColor(g.mColor) : SexyColor.White;
			g.DrawImageTransformed(theImageStrip, ref theTransform, true, aColor, theSrcRect, false);
		}

		public static void TodDrawImageCelCenterScaledF(Graphics g, Image theImageStrip, float thePosX, float thePosY, int theCelCol, float theScaleX, float theScaleY)
		{
			Debug.ASSERT(theCelCol >= 0 && theCelCol < theImageStrip.mNumCols);
			int celWidth = theImageStrip.GetCelWidth();
			int celHeight = theImageStrip.GetCelHeight();
			TRect theSrcRect = new TRect(celWidth * theCelCol, 0, celWidth, celHeight);
			if (theScaleX == 1f && theScaleY == 1f)
			{
				g.DrawImageF(theImageStrip, thePosX, thePosY, theSrcRect);
				return;
			}
			Matrix theTransform = Matrix.Identity;
			theTransform.M41 = (float)celWidth * 0.5f + thePosX + (float)g.mTransX;
			theTransform.M42 = (float)celHeight * 0.5f + thePosY + (float)g.mTransY;
			theTransform.M11 = theScaleX;
			theTransform.M22 = theScaleY;
			SexyColor aColor = g.mColorizeImages ? new SexyColor(g.mColor) : SexyColor.White;
			g.DrawImageTransformed(theImageStrip, ref theTransform, true, aColor, theSrcRect, false);
		}

		public static void TodDrawImageScaledF(Graphics g, Image theImage, float thePosX, float thePosY, float theScaleX, float theScaleY)
		{
			if (theScaleX == 1f && theScaleY == 1f)
			{
				g.DrawImageF(theImage, thePosX, thePosY);
				return;
			}
			TRect theSrcRect = new TRect(0, 0, theImage.mWidth, theImage.mHeight);
			Matrix theTransform = Matrix.Identity;
			theTransform.M41 = (float)theImage.mWidth * 0.5f * theScaleX + thePosX + (float)g.mTransX;
			theTransform.M42 = (float)theImage.mHeight * 0.5f * theScaleY + thePosY + (float)g.mTransY;
			theTransform.M11 = theScaleX;
			theTransform.M22 = theScaleY;
			SexyColor aColor = g.mColorizeImages ? new SexyColor(g.mColor) : SexyColor.White;
			g.DrawImageTransformed(theImage, ref theTransform, true, aColor, theSrcRect, false);
		}

		public static void TodDrawImageCenterScaledF(Graphics g, Image theImage, float thePosX, float thePosY, float theScaleX, float theScaleY)
		{
			TodDrawImageCenterScaledF(g, theImage, thePosX, thePosY, theScaleX, theScaleY, 0, false);
		}

		public static void TodDrawImageCenterScaledF(Graphics g, Image theImage, float thePosX, float thePosY, float theScaleX, float theScaleY, int cel, bool doTexelOffset)
		{
			if (theScaleX == 1f && theScaleY == 1f)
			{
				g.DrawImageCel(theImage, (int)thePosX, (int)thePosY, cel);
				return;
			}
			TRect celRect = theImage.GetCelRect(cel);
			if (doTexelOffset)
			{
				celRect.mX++;
				celRect.mY++;
				celRect.mWidth--;
				celRect.mHeight--;
			}
			Matrix theTransform = Matrix.Identity;
			theTransform.M41 = (float)celRect.mWidth * 0.5f + thePosX + (float)g.mTransX;
			theTransform.M42 = (float)celRect.mHeight * 0.5f + thePosY + (float)g.mTransY;
			theTransform.M11 = theScaleX;
			theTransform.M22 = theScaleY;
			SexyColor aColor = g.mColorizeImages ? new SexyColor(g.mColor) : SexyColor.White;
			g.DrawImageTransformed(theImage, ref theTransform, true, aColor, celRect, false);
		}

		public static void TodBltMatrix(Graphics g, Image theImage, ref Matrix theTransform, TRect theClipRect, SexyColor theColor, Graphics.DrawMode theDrawMode, TRect theSrcRect, bool clip)
		{
			if (g.mDrawMode != theDrawMode)
			{
				g.SetDrawMode(theDrawMode);
			}
			if (g.IsHardWareClipping() && !g.MatchesHardWareClipRect(theClipRect))
			{
				if (OffsetForGraphicsTranslation)
				{
					theClipRect.mX -= g.mTransX;
					theClipRect.mY -= g.mTransY;
				}
				g.HardwareClipRect(theClipRect);
			}
			g.DrawImageTransformed(theImage, ref theTransform, true, theColor, theSrcRect, clip);
		}

		public static void TodBltMatrix(Graphics g, Image theImage, Matrix theTransform, ref TRect theClipRect, SexyColor theColor, Graphics.DrawMode theDrawMode, TRect theSrcRect)
		{
			TodBltMatrix(g, theImage, ref theTransform, theClipRect, theColor, theDrawMode, theSrcRect, false);
		}

		public static string TodReplaceString(string theText, string theStringToFind, string theStringToSubstitute)
		{
			string text = TodStringFile.TodStringTranslate(theText);
			string newValue = TodStringFile.TodStringTranslate(theStringToSubstitute);
			return text.Replace(theStringToFind, newValue);
		}

		public static string TodReplaceNumberString(string theText, string theStringToFind, int theNumber)
		{
			string text = TodStringFile.TodStringTranslate(theText);
			return text.Replace(theStringToFind, LawnApp.ToString(theNumber));
		}

		public static void SexyMatrix3Translation(ref Matrix m, float x, float y)
		{
			m.Translation += new Vector3(x, y, 0f);
		}

		public static void SexyMatrix3Multiply(ref Matrix m, Matrix l, Matrix r)
		{
			m = l * r;
		}

		public static void TodScaleTransformMatrix(ref Matrix m, float x, float y, float theScaleX, float theScaleY)
		{
			Matrix matrix = Matrix.CreateTranslation(new Vector3(x, y, 0f));
			Matrix matrix2 = Matrix.CreateScale(new Vector3(theScaleX, theScaleY, 1f));
			m = matrix2 * matrix;
		}

		public static void TodScaleRotateTransformMatrix(ref Matrix m, float x, float y, float rad, float theScaleX, float theScaleY)
		{
			Matrix matrix = Matrix.CreateTranslation(new Vector3(x, y, 0f));
			Matrix matrix2 = Matrix.CreateRotationZ(0f - rad);
			Matrix matrix3 = Matrix.CreateScale(new Vector3(theScaleX, theScaleY, 1f));
			m = matrix3 * matrix2 * matrix;
		}

		public static SexyColor GetFlashingColor(int theCounter, int theFlashTime)
		{
			int num = theCounter % theFlashTime;
			int num2 = theFlashTime / 2;
			int num3 = ClampInt(55 + Math.Abs(num2 - num) * 200 / num2, 0, 255);
			return new SexyColor(num3, num3, num3, 255);
		}

		public static int ColorComponentMultiply(int theColor1, int theColor2)
		{
			int num = theColor1 * theColor2 / 255;
			return ClampInt(num, 0, 255);
		}

		public static bool TodFindImagePath(Image theImage, ref string thePath)
		{
			TodResourceManager todResourceManager = (TodResourceManager)GlobalStaticVars.gSexyAppBase.mResourceManager;
			return todResourceManager.FindImagePath(theImage, ref thePath);
		}

		public static bool TodLoadNextResource()
		{
			TodResourceManager todResourceManager = (TodResourceManager)GlobalStaticVars.gSexyAppBase.mResourceManager;
			return todResourceManager.TodLoadNextResource();
		}

		public static bool TodLoadResources(string theGroup)
		{
			if (string.IsNullOrEmpty(theGroup))
			{
				return true;
			}
			bool result = TodLoadResources(theGroup, false);
			Resources.ExtractResourcesByName(GlobalStaticVars.gSexyAppBase.mResourceManager, theGroup);
			SexyAppBase.XnaGame.CompensateForSlowUpdate();
			return result;
		}

		public static bool TodLoadResources(string theGroup, bool doUnpackAtlasImages)
		{
			ResourceManager mResourceManager = GlobalStaticVars.gSexyAppBase.mResourceManager;
			return mResourceManager.TodLoadResources(theGroup, doUnpackAtlasImages);
		}

		public static bool TodIsPointInPolygon(SexyVector2[] thePolygonPoint, int theNumberPolygonPoints, SexyVector2 theCheckPoint)
		{
			Debug.ASSERT(theNumberPolygonPoints >= 3);
			for (int i = 0; i < theNumberPolygonPoints; i++)
			{
				SexyVector2 rhs = thePolygonPoint[i];
				SexyVector2 sexyVector = default(SexyVector2);
				sexyVector = ((i != theNumberPolygonPoints - 1) ? thePolygonPoint[i + 1] : thePolygonPoint[0]);
				SexyVector2 sexyVector2 = (sexyVector - rhs).Perp();
				SexyVector2 v = theCheckPoint - rhs;
				if (sexyVector2.Dot(v) < 0f)
				{
					return false;
				}
			}
			return true;
		}

		public static string TodGetCurrentLevelName()
		{
			return "Unknown Level";
		}

		public static bool TodAppCloseRequest()
		{
			return false;
		}

		public static bool TodAppHasUsedCheatKeys()
		{
			return false;
		}

		public static float TodCalcSmoothWeight(float aWeight, float aLastPicked, float aSecondLastPicked)
		{
			if (aWeight < 1E-06f)
			{
				return 0f;
			}
			float num = 2f;
			float num2 = 1f / aWeight;
			float num3 = num2 * 2f;
			float num4 = aLastPicked + 1f - num2;
			float num5 = aSecondLastPicked + 1f - num3;
			float num6 = 1f + num4 / num2 * num;
			float num7 = 1f + num5 / num3 * num;
			float num8 = ClampFloat(num6 * 0.75f + num7 * 0.25f, 0.01f, 100f);
			return aWeight * num8;
		}

		public static float TodCurveQuad(float theTime)
		{
			return theTime * theTime;
		}

		public static float TodCurveInvQuad(float theTime)
		{
			return 2f * theTime - theTime * theTime;
		}

		public static float TodCurveQuadS(float theTime)
		{
			if (theTime <= 0.5f)
			{
				return TodCurveQuad(theTime * 2f) * 0.5f;
			}
			return TodCurveInvQuad((theTime - 0.5f) * 2f) * 0.5f + 0.5f;
		}

		public static float TodCurveInvQuadS(float theTime)
		{
			if (theTime <= 0.5f)
			{
				return TodCurveInvQuad(theTime * 2f) * 0.5f;
			}
			return TodCurveQuad((theTime - 0.5f) * 2f) * 0.5f + 0.5f;
		}

		public static float TodCurveCubic(float theTime)
		{
			return theTime * theTime * theTime;
		}

		public static float TodCurveInvCubic(float theTime)
		{
			return (theTime - 1f) * (theTime - 1f) * (theTime - 1f) + 1f;
		}

		public static float TodCurveCubicS(float theTime)
		{
			if (theTime <= 0.5f)
			{
				return TodCurveCubic(theTime * 2f) * 0.5f;
			}
			return TodCurveInvCubic((theTime - 0.5f) * 2f) * 0.5f + 0.5f;
		}

		public static float TodCurvePoly(float theTime, float thePoly)
		{
			return (float)Math.Pow(theTime, thePoly);
		}

		public static float TodCurveInvPoly(float theTime, float thePoly)
		{
			return (float)Math.Pow(theTime - 1f, thePoly) + 1f;
		}

		public static float TodCurvePolyS(float theTime, float thePoly)
		{
			if (theTime <= 0.5f)
			{
				return TodCurvePoly(theTime * 2f, thePoly) * 0.5f;
			}
			return TodCurveInvPoly((theTime - 0.5f) * 2f, thePoly) * 0.5f + 0.5f;
		}

		public static float TodCurveCircle(float theTime)
		{
			if (theTime > 0.999999f)
			{
				return 1f;
			}
			return 1f - (float)Math.Sqrt(1f - theTime * theTime);
		}

		public static float TodCurveInvCircle(float theTime)
		{
			if (theTime < 1E-06f)
			{
				return 0f;
			}
			return (float)Math.Sqrt(1f - (theTime - 1f) * (theTime - 1f));
		}

		public static float TodCurveBounce(float theTime)
		{
			return 1f - Math.Abs(1f - theTime * 2f);
		}

		public static float TodCurveEvaluateClamped(float theTime, float thePositionStart, float thePositionEnd, TodCurves theCurve)
		{
			if (theTime <= 0f)
			{
				return thePositionStart;
			}
			if (theTime >= 1f)
			{
				if (theCurve == TodCurves.CURVE_BOUNCE || theCurve == TodCurves.CURVE_BOUNCE_SLOW_MIDDLE || theCurve == TodCurves.CURVE_BOUNCE_FAST_MIDDLE || theCurve == TodCurves.CURVE_SIN_WAVE || theCurve == TodCurves.CURVE_EASE_SIN_WAVE)
				{
					return thePositionStart;
				}
				return thePositionEnd;
			}
			return TodCurveEvaluate(theTime, thePositionStart, thePositionEnd, theCurve);
		}

		public static void SexyMatrix3Transpose(Matrix m, ref Matrix r)
		{
			r = Matrix.Transpose(m);
		}

		public static void SexyMatrix3Inverse(ref Matrix mat, out Matrix r)
		{
			Matrix.Invert(ref mat, out r);
		}

		public static void SexyMatrix3ExtractScale(Matrix m, ref float theScaleX, ref float theScaleY)
		{
			float num = (float)Math.Atan2(m.M11, m.M21);
			if (Math.Abs(num) < (float)Math.PI / 4f || Math.Abs(num) > (float)Math.PI * 3f / 4f)
			{
				theScaleX = m.M21 / (float)Math.Cos(num);
			}
			else
			{
				theScaleX = m.M11 / (float)Math.Sin(num);
			}
			float num2 = (float)Math.Atan2(m.M22, m.M12);
			if (Math.Abs(num2) < (float)Math.PI / 4f || Math.Abs(num2) > (float)Math.PI * 3f / 4f)
			{
				theScaleY = m.M12 / (float)Math.Cos(num2);
			}
			else
			{
				theScaleY = m.M22 / (float)Math.Sin(num2);
			}
		}

		public static SexyColor ColorAdd(SexyColor theColor1, SexyColor theColor2)
		{
			int num = theColor1.mRed + theColor2.mRed;
			int num2 = theColor1.mGreen + theColor2.mGreen;
			int num3 = theColor1.mBlue + theColor2.mBlue;
			int num4 = theColor1.mAlpha + theColor2.mAlpha;
			num = ClampInt(num, 0, 255);
			num2 = ClampInt(num2, 0, 255);
			num3 = ClampInt(num3, 0, 255);
			num4 = ClampInt(num4, 0, 255);
			return new SexyColor(num, num2, num3, num4);
		}

		public static SexyColor ColorsMultiply(SexyColor theColor1, SexyColor theColor2)
		{
			SexyColor result = default(SexyColor);
			result.mRed = ColorComponentMultiply(theColor1.mRed, theColor2.mRed);
			result.mGreen = ColorComponentMultiply(theColor1.mGreen, theColor2.mGreen);
			result.mBlue = ColorComponentMultiply(theColor1.mBlue, theColor2.mBlue);
			result.mAlpha = ColorComponentMultiply(theColor1.mAlpha, theColor2.mAlpha);
			return result;
		}
	}
}
