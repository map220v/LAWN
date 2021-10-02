using System;
using System.Collections.Generic;

namespace Sexy.TodLib
{
	internal class FilterEffect
	{
		public static List<Dictionary<Image, Image>> gFilterMap = new List<Dictionary<Image, Image>>();

		public static void FilterEffectInitForApp()
		{
		}

		public static void FilterEffectDisposeForApp()
		{
		}

		public static Image FilterEffectGetImage(Image theImage, FilterEffectType theFilterEffect)
		{
			return theImage;
		}

		private static Image FilterEffectCreateImage(Image theImage, FilterEffectType theFilterEffect)
		{
			MemoryImage memoryImage = new MemoryImage();
			memoryImage.mWidth = theImage.mWidth;
			memoryImage.mWidth = theImage.mHeight;
			Graphics @new = Graphics.GetNew(memoryImage);
			@new.DrawImage(theImage, 0, 0);
			switch (theFilterEffect)
			{
			case FilterEffectType.FILTER_EFFECT_WASHED_OUT:
				FilterEffectDoWashedOut(memoryImage);
				break;
			case FilterEffectType.FILTER_EFFECT_LESS_WASHED_OUT:
				FilterEffectDoLessWashedOut(memoryImage);
				break;
			case FilterEffectType.FILTER_EFFECT_WHITE:
				FilterEffectDoWhite(memoryImage);
				break;
			}
			memoryImage.mNumCols = theImage.mNumCols;
			memoryImage.mNumRows = theImage.mNumRows;
			@new.PrepareForReuse();
			return memoryImage;
		}

		private static void FilterEffectDoWashedOut(MemoryImage theImage)
		{
			FilterEffectDoLumSat(theImage, 1.8f, 0.2f);
		}

		private static void FilterEffectDoLessWashedOut(MemoryImage theImage)
		{
			FilterEffectDoLumSat(theImage, 1.2f, 0.3f);
		}

		private static void FilterEffectDoWhite(MemoryImage theImage)
		{
			int[] array = new int[theImage.mWidth * theImage.mHeight];
			theImage.Texture.GetData(array);
			for (int i = 0; i < theImage.mHeight; i++)
			{
				for (int j = 0; j < theImage.mWidth; j++)
				{
					array[j + i * theImage.mWidth] |= 16777215;
				}
			}
			theImage.Texture.SetData(array);
		}

		private static void FilterEffectDoLumSat(MemoryImage theImage, float aLum, float aSat)
		{
			int[] array = new int[theImage.mWidth * theImage.mHeight];
			theImage.Texture.GetData(array);
			for (int i = 0; i < theImage.mHeight; i++)
			{
				for (int j = 0; j < theImage.mWidth; j++)
				{
					int num = array[j + i * theImage.mWidth];
					char c = (char)(num & 0xFF);
					char c2 = (char)((num >> 8) & 0xFF);
					char c3 = (char)((num >> 16) & 0xFF);
					char c4 = (char)(num >> 24);
					float r = (float)(int)c / 255f;
					float g = (float)(int)c2 / 255f;
					float b = (float)(int)c3 / 255f;
					float h;
					float s;
					float l;
					RGB_to_HSL(r, g, b, out h, out s, out l);
					s *= aSat;
					l *= aLum;
					HSL_to_RGB(h, s, l, out r, out g, out b);
					int num2 = TodCommon.ClampInt((int)(r * 255f), 0, 255);
					int num3 = TodCommon.ClampInt((int)(g * 255f), 0, 255);
					int num4 = TodCommon.ClampInt((int)(b * 255f), 0, 255);
					array[j + i * theImage.mWidth] = ((int)((uint)c4 << 24) | (num4 << 16) | (num3 << 8) | num2);
				}
			}
			theImage.Texture.SetData(array);
		}

		private static void RGB_to_HSL(float r, float g, float b, out float h, out float s, out float l)
		{
			float val = Math.Max(r, g);
			val = Math.Max(val, b);
			float val2 = Math.Min(r, g);
			val2 = Math.Min(val2, b);
			h = (l = (s = 0f));
			float num;
			if (!((l = (val2 + val) / 2f) <= 0f) && (s = (num = val - val2)) > 0f)
			{
				s /= ((l <= 0.5f) ? (val + val2) : (2f - val - val2));
				float num2 = (val - r) / num;
				float num3 = (val - g) / num;
				float num4 = (val - b) / num;
				if (r == val)
				{
					h = ((g == val2) ? (5f + num4) : (1f - num3));
				}
				else if (g == val)
				{
					h = ((b == val2) ? (1f + num2) : (3f - num4));
				}
				else
				{
					h = ((r == val2) ? (3f + num3) : (5f - num2));
				}
				h /= 6f;
			}
		}

		private static void HSL_to_RGB(float h, float sl, float l, out float r, out float g, out float b)
		{
			r = (g = (b = 0f));
			float num = (l <= 0.5f) ? (l * (1f + sl)) : (l + sl - l * sl);
			if (num <= 0f)
			{
				r = (g = (b = 0f));
				return;
			}
			float num2 = l + l - num;
			float num3 = (num - num2) / num;
			h *= 6f;
			int num4 = TodCommon.ClampInt((int)h, 0, 5);
			float num5 = h - (float)num4;
			float num6 = num * num3 * num5;
			float num7 = num2 + num6;
			float num8 = num - num6;
			switch (num4)
			{
			case 0:
				r = num;
				g = num7;
				b = num2;
				break;
			case 1:
				r = num8;
				g = num;
				b = num2;
				break;
			case 2:
				r = num2;
				g = num;
				b = num7;
				break;
			case 3:
				r = num2;
				g = num8;
				b = num;
				break;
			case 4:
				r = num7;
				g = num2;
				b = num;
				break;
			case 5:
				r = num;
				g = num2;
				b = num8;
				break;
			}
		}
	}
}
