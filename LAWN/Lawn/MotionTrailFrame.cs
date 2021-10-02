using Sexy;

namespace Lawn
{
	internal class MotionTrailFrame
	{
		public float mPosX;

		public float mPosY;

		public float mAnimTime;

		public void SaveToFile(Buffer b)
		{
			b.WriteFloat(mPosX);
			b.WriteFloat(mPosY);
			b.WriteFloat(mAnimTime);
		}

		public void LoadFromFile(Buffer b)
		{
			mPosX = b.ReadFloat();
			mPosY = b.ReadFloat();
			mAnimTime = b.ReadFloat();
		}
	}
}
