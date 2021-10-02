using Microsoft.Xna.Framework;

namespace Sexy.TodLib
{
	internal struct DrawCall
	{
		public TRect mClipRect;

		public SexyColor mColor;

		public TRect mSrcRect;

		public Vector2 mPosition;

		public double mRotation;

		public Vector2 mScale;

		public void SetTransform(ReanimatorTransform transform)
		{
			mPosition = new Vector2(transform.mTransX, transform.mTransY);
			mRotation = 0.0;
			mScale = new Vector2(transform.mScaleX, transform.mScaleY);
		}
	}
}
