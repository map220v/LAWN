using Sexy;
using Sexy.TodLib;
using System;

namespace Lawn
{
	internal class LoadingScreen : Widget
	{
		private float mCurBarWidth;

		private float mTotalBarWidth;

		public static bool IsLoading;

		public static LoadingScreen gLoadingScreen = new LoadingScreen();

		public override void Draw(Graphics g)
		{
			base.Draw(g);
			int num = mWidth / 2;
			int num2 = mHeight / 2 - (int)Constants.InvertAndScale(34f);
			g.DrawImage(AtlasResources.IMAGE_LOADBAR_DIRT, num, (float)num2 + Constants.InvertAndScale(18f));
			if (mCurBarWidth >= mTotalBarWidth)
			{
				g.DrawImage(AtlasResources.IMAGE_LOADBAR_GRASS, num, num2);
				return;
			}
			Graphics @new = Graphics.GetNew(g);
			@new.ClipRect(num, num2, (int)mCurBarWidth, AtlasResources.IMAGE_LOADBAR_GRASS.mHeight);
			@new.DrawImage(AtlasResources.IMAGE_LOADBAR_GRASS, num, num2);
			float num3 = mCurBarWidth * 0.94f;
			float rad = (0f - num3) / 180f * (float)Math.PI * 2f;
			float num4 = TodCommon.TodAnimateCurveFloatTime(0f, mTotalBarWidth, mCurBarWidth, 1f, 0.5f, TodCurves.CURVE_LINEAR);
			SexyTransform2D sexyTransform2D = default(SexyTransform2D);
			TodCommon.TodScaleRotateTransformMatrix(ref sexyTransform2D.mMatrix, (float)num + Constants.InvertAndScale(11f) + num3, (float)num2 - Constants.InvertAndScale(3f) - Constants.InvertAndScale(35f) * num4 + Constants.InvertAndScale(35f), rad, num4, num4);
			TodCommon.TodBltMatrix(theSrcRect: new TRect(0, 0, AtlasResources.IMAGE_REANIM_LOAD_SODROLLCAP.mWidth, AtlasResources.IMAGE_REANIM_LOAD_SODROLLCAP.mHeight), g: g, theImage: AtlasResources.IMAGE_REANIM_LOAD_SODROLLCAP, theTransform: sexyTransform2D.mMatrix, theClipRect: ref g.mClipRect, theColor: SexyColor.White, theDrawMode: g.mDrawMode);
			@new.PrepareForReuse();
		}
	}
}
