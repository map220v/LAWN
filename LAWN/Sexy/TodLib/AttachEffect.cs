using System.Collections.Generic;

namespace Sexy.TodLib
{
	internal class AttachEffect
	{
		public object mEffectID;

		public EffectType mEffectType;

		public SexyTransform2D mOffset = default(SexyTransform2D);

		public bool mDontDrawIfParentHidden;

		public bool mDontPropogateColor;

		private static Stack<AttachEffect> unusedObjects = new Stack<AttachEffect>(100);

		public static AttachEffect GetNewAttachEffect()
		{
			if (unusedObjects.Count > 0)
			{
				return unusedObjects.Pop();
			}
			return new AttachEffect();
		}

		public void PrepareForReuse()
		{
			Reset();
			unusedObjects.Push(this);
		}

		private AttachEffect()
		{
			Reset();
		}

		public void Reset()
		{
			mEffectID = null;
			mEffectType = EffectType.EFFECT_PARTICLE;
			mOffset = default(SexyTransform2D);
			mDontDrawIfParentHidden = false;
			mDontPropogateColor = false;
		}
	}
}
