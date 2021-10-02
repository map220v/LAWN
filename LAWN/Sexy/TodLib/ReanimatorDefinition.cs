namespace Sexy.TodLib
{
	internal class ReanimatorDefinition
	{
		public ReanimatorTrack[] mTracks;

		public short mTrackCount;

		public float mFPS;

		public ReanimAtlas mReanimAtlas;

		public ReanimatorDefinition()
		{
			mFPS = 12f;
			mTrackCount = 0;
			mReanimAtlas = null;
		}

		public void ExtractImages()
		{
			for (int i = 0; i < mTracks.Length; i++)
			{
				mTracks[i].ExtractImages();
			}
		}
	}
}
