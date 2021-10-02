using System.Collections.Generic;

namespace Sexy.TodLib
{
	internal class TodTriangleGroup
	{
		public static bool gTodTriangleDrawAdditive = false;

		private Image mImage;

		private List<DrawCall> mDrawCalls = new List<DrawCall>();

		private int mTriangleCount;

		private Graphics.DrawMode mDrawMode;

		private static Stack<TodTriangleGroup> unusedObjects = new Stack<TodTriangleGroup>();

		public static TodTriangleGroup GetNewTodTriangleGroup()
		{
			if (unusedObjects.Count > 0)
			{
				return unusedObjects.Pop();
			}
			return new TodTriangleGroup();
		}

		public void PrepareForReuse()
		{
			Reset();
			unusedObjects.Push(this);
		}

		private TodTriangleGroup()
		{
			Reset();
		}

		private void Reset()
		{
			mImage = null;
			mTriangleCount = 0;
			mDrawMode = Graphics.DrawMode.DRAWMODE_NORMAL;
		}

		public void DrawGroup(Graphics g)
		{
			if (mImage != null && mTriangleCount != 0)
			{
				if (!GlobalStaticVars.gSexyAppBase.Is3DAccelerated() && mDrawMode == Graphics.DrawMode.DRAWMODE_ADDITIVE)
				{
					gTodTriangleDrawAdditive = true;
				}
				g.mDrawMode = mDrawMode;
				for (int i = 0; i < mTriangleCount; i++)
				{
					DrawCall drawCall = mDrawCalls[i];
					g.mClipRect = drawCall.mClipRect;
					g.DrawImageRotated(mImage, (int)drawCall.mPosition.X, (int)drawCall.mPosition.Y, drawCall.mRotation, drawCall.mSrcRect);
				}
				mTriangleCount = 0;
				gTodTriangleDrawAdditive = false;
			}
		}

		public void AddTriangle(Graphics g, Image theImage, ReanimatorTransform theTransform, TRect theClipRect, SexyColor theColor, Graphics.DrawMode theDrawMode, TRect theSrcRect)
		{
			if (mTriangleCount > 0 && (mDrawMode != theDrawMode || mImage != theImage))
			{
				DrawGroup(g);
			}
			DrawCall item = default(DrawCall);
			item.SetTransform(theTransform);
			item.mClipRect = theClipRect;
			item.mColor = theColor;
			item.mSrcRect = theSrcRect;
			mDrawCalls.Add(item);
			mImage = theImage;
			mDrawMode = theDrawMode;
			mTriangleCount++;
		}
	}
}
