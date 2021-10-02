using Microsoft.Xna.Framework;
using System;

namespace Sexy
{
	public struct Transform
	{
		private bool isInitialised;

		public static Transform Identity = new Transform
		{
			mMatrix = new SexyTransform2D(Matrix.Identity)
		};

		private SexyTransform2D mMatrix;

		private bool mNeedCalcMatrix;

		public bool mComplex;

		public bool mHaveRot;

		public bool mHaveScale;

		public float mTransX1;

		public float mTransY1;

		public float mTransX2;

		public float mTransY2;

		public float mScaleX;

		public float mScaleY;

		public float mRot;

		private void MakeComplex()
		{
			if (!mComplex)
			{
				mComplex = true;
				CalcMatrix();
			}
		}

		private void CalcMatrix()
		{
			if (!isInitialised)
			{
				Reset();
			}
			if (mNeedCalcMatrix)
			{
				mNeedCalcMatrix = false;
				mMatrix = new SexyTransform2D(Matrix.Identity);
				mMatrix.mMatrix.M13 = mTransX1;
				mMatrix.mMatrix.M23 = mTransY1;
				mMatrix.mMatrix.M33 = 1f;
				if (mHaveScale)
				{
					mMatrix.mMatrix.M11 = mScaleX;
					mMatrix.mMatrix.M22 = mScaleY;
				}
				else if (mHaveRot)
				{
					mMatrix.RotateRad(mRot);
				}
				if (mTransX2 != 0f || mTransY2 != 0f)
				{
					mMatrix.Translate(mTransX2, mTransY2);
				}
			}
		}

		public void Reset()
		{
			mNeedCalcMatrix = true;
			mComplex = false;
			mTransX1 = (mTransY1 = 0f);
			mTransX2 = (mTransY2 = 0f);
			mScaleX = (mScaleY = 1f);
			mRot = 0f;
			mHaveRot = false;
			mHaveScale = false;
			isInitialised = true;
		}

		public void Translate(float tx, float ty)
		{
			if (!isInitialised)
			{
				Reset();
			}
			if (!mComplex)
			{
				mNeedCalcMatrix = true;
				if (mHaveRot || mHaveScale)
				{
					mTransX2 += tx;
					mTransY2 += ty;
				}
				else
				{
					mTransX1 += tx;
					mTransY1 += ty;
				}
			}
			else
			{
				mMatrix.Translate(tx, ty);
			}
		}

		public void RotateRad(float rot)
		{
			if (!isInitialised)
			{
				Reset();
			}
			if (!mComplex)
			{
				if (mHaveScale)
				{
					MakeComplex();
					mMatrix.RotateRad(rot);
				}
				else
				{
					mNeedCalcMatrix = true;
					mHaveRot = true;
					mRot += rot;
				}
			}
			else
			{
				mMatrix.RotateRad(rot);
			}
		}

		public void RotateDeg(float rot)
		{
			if (!isInitialised)
			{
				Reset();
			}
			RotateRad((float)Math.PI * rot / 180f);
		}

		public void Scale(float sx, float sy)
		{
			if (!isInitialised)
			{
				Reset();
			}
			if (!mComplex)
			{
				if (mHaveRot || mTransX1 != 0f || mTransY1 != 0f || (sx < 0f && mScaleX * sx != -1f) || sy < 0f)
				{
					MakeComplex();
					mMatrix.Scale(sx, sy);
					return;
				}
				mNeedCalcMatrix = true;
				mHaveScale = true;
				mScaleX *= sx;
				mScaleY *= sy;
			}
			else
			{
				mMatrix.Scale(sx, sy);
			}
		}

		public SexyTransform2D GetMatrix()
		{
			if (!isInitialised)
			{
				Reset();
			}
			CalcMatrix();
			return mMatrix;
		}
	}
}
