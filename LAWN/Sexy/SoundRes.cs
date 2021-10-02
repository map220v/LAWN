namespace Sexy
{
	internal class SoundRes : BaseRes
	{
		public int mSoundId;

		public double mVolume;

		public int mPanning;

		public SoundRes()
		{
			mType = ResType.ResType_Sound;
		}

		public override void DeleteResource()
		{
			if (mSoundId >= 0)
			{
				GlobalStaticVars.gSexyAppBase.mSoundManager.ReleaseSound((uint)mSoundId);
				mSoundId = -1;
			}
			base.DeleteResource();
		}
	}
}
