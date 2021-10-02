namespace Sexy.TodLib
{
	internal class FoleyInstance
	{
		public XNASoundInstance mInstance;

		public int mRefCount;

		private bool _paused;

		public int mStartTime;

		public int mPauseOffset;

		public bool mPaused
		{
			get
			{
				if (_paused)
				{
					return !mInstance.IsReleased();
				}
				return false;
			}
			set
			{
				_paused = value;
			}
		}

		public FoleyInstance()
		{
			mInstance = null;
			mRefCount = 0;
			mPaused = false;
			mStartTime = 0;
			mPauseOffset = 0;
		}
	}
}
