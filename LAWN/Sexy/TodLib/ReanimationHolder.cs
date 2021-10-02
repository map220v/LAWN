using System.Collections.Generic;

namespace Sexy.TodLib
{
	internal class ReanimationHolder
	{
		public List<Reanimation> mReanimations = new List<Reanimation>();

		public void InitializeHolder()
		{
		}

		public void DisposeHolder()
		{
		}

		public Reanimation AllocReanimation(float theX, float theY, int theRenderOrder, ReanimationType theReanimationType)
		{
			Reanimation newReanimation = Reanimation.GetNewReanimation();
			newReanimation.mReanimationHolder = this;
			newReanimation.mRenderOrder = theRenderOrder;
			newReanimation.ReanimationInitializeType(theX, theY, theReanimationType);
			newReanimation.mActive = true;
			mReanimations.Add(newReanimation);
			return newReanimation;
		}
	}
}
