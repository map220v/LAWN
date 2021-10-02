using System.Collections.Generic;

namespace Sexy
{
	internal class SexyMathHermite
	{
		public struct SPoint
		{
			public float mX;

			public float mFx;

			public float mFxPrime;

			public SPoint(float inX, float inFx, float inFxPrime)
			{
				mX = inX;
				mFx = inFx;
				mFxPrime = inFxPrime;
			}
		}

		protected class SPiece
		{
			public float[] mCoeffs = new float[4];
		}

		public List<SPoint> mPoints = new List<SPoint>();

		protected List<SPiece> mPieces = new List<SPiece>();

		protected bool mIsBuilt;

		public SexyMathHermite()
		{
			mIsBuilt = false;
		}

		public void Rebuild()
		{
			mIsBuilt = false;
		}

		public float Evaluate(float inX)
		{
			if (!mIsBuilt)
			{
				if (!BuildCurve())
				{
					return 0f;
				}
				mIsBuilt = true;
			}
			uint count = (uint)mPieces.Count;
			for (int i = 0; i < count; i++)
			{
				if (inX < mPoints[i + 1].mX)
				{
					return EvaluatePiece(inX, mPoints, i, mPieces[i]);
				}
			}
			return mPoints[mPoints.Count - 1].mFx;
		}

		protected void CreatePiece(List<SPoint> inPoints, int inPointsIndex, List<SPiece> outPiece, int outPieceIndex)
		{
			float[][] array = new float[4][];
			for (uint num = 0u; num < 4; num++)
			{
				array[num] = new float[4];
			}
			float[] array2 = new float[4];
			for (int i = inPointsIndex; i <= inPointsIndex + 1; i++)
			{
				int num2 = 2 * i;
				array2[num2] = inPoints[i].mX;
				array2[num2 + 1] = inPoints[i].mX;
				array[num2][0] = inPoints[i].mFx;
				array[num2 + 1][0] = inPoints[i].mFx;
				array[num2 + 1][1] = inPoints[i].mFxPrime;
				if (i != 0)
				{
					array[num2][1] = (array[num2][0] - array[num2 - 1][0]) / (array2[num2] - array2[num2 - 1]);
				}
			}
			for (uint num3 = 2u; num3 < 4; num3++)
			{
				for (uint num4 = 2u; num4 <= num3; num4++)
				{
					array[num3][num4] = (array[num3][num4 - 1] - array[num3 - 1][num4 - 1]) / (array2[num3] - array2[num3 - num4]);
				}
			}
			for (uint num5 = 0u; num5 < 4; num5++)
			{
				outPiece[outPieceIndex].mCoeffs[num5] = array[num5][num5];
			}
		}

		protected float EvaluatePiece(float inX, List<SPoint> inPoints, int inPointsIndex, SPiece inPiece)
		{
			float[] array = new float[2]
			{
				inX - inPoints[0].mX,
				inX - inPoints[1].mX
			};
			float num = 1f;
			float num2 = inPiece.mCoeffs[0];
			for (uint num3 = 1u; num3 < 4; num3++)
			{
				num *= array[(num3 - 1) / 2u];
				num2 += num * inPiece.mCoeffs[num3];
			}
			return num2;
		}

		protected bool BuildCurve()
		{
			mPieces.Clear();
			int count = mPoints.Count;
			if (count < 2)
			{
				return false;
			}
			int num = count - 1;
			mPieces.Capacity = num;
			for (int i = 0; i < num; i++)
			{
				CreatePiece(mPoints, i, mPieces, i);
			}
			return true;
		}
	}
}
