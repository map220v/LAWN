using System.Collections.Generic;

namespace Sexy
{
	internal class PreModalInfo
	{
		public Widget mBaseModalWidget;

		public Widget mPrevBaseModalWidget;

		public Widget mPrevFocusWidget;

		public FlagsMod mPrevBelowModalFlagsMod = default(FlagsMod);

		private static Stack<PreModalInfo> unusedObjects = new Stack<PreModalInfo>();

		public void PrepareForReuse()
		{
			Reset();
			unusedObjects.Push(this);
		}

		public static PreModalInfo GetNewPreModalInfo()
		{
			if (unusedObjects.Count > 0)
			{
				return unusedObjects.Pop();
			}
			return new PreModalInfo();
		}

		private void Reset()
		{
			mBaseModalWidget = null;
			mPrevBaseModalWidget = null;
			mPrevFocusWidget = null;
			mPrevBelowModalFlagsMod = default(FlagsMod);
		}

		private PreModalInfo()
		{
			Reset();
		}
	}
}
