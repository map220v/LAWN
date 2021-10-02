namespace Sexy.TodLib
{
	internal class TrailDefinition
	{
		public Image mImage;

		public string mImageName;

		public int mMaxPoints;

		public float mMinPointDistance;

		public int mTrailFlags;

		public FloatParameterTrack mTrailDuration = new FloatParameterTrack();

		public FloatParameterTrack mWidthOverLength = new FloatParameterTrack();

		public FloatParameterTrack mWidthOverTime = new FloatParameterTrack();

		public FloatParameterTrack mAlphaOverLength = new FloatParameterTrack();

		public FloatParameterTrack mAlphaOverTime = new FloatParameterTrack();

		public TrailDefinition()
		{
			mMaxPoints = 2;
			mMinPointDistance = 1f;
			mTrailFlags = 0;
			mImage = null;
		}

		public void Dispose()
		{
		}
	}
}
