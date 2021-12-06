using Sexy.TodLib;

namespace Sexy
{
	internal class ReanimRes : BaseRes
	{
		public ReanimatorDefinition mReanim;

		public ReanimRes()
		{
			mType = ResType.ResType_Reanim;
		}

		public override void DeleteResource()
		{
			if (mReanim != null)
			{
				mReanim = null;
			}
			base.DeleteResource();
		}
	}
}
