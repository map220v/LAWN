using Microsoft.Xna.Framework.Audio;

namespace Sexy
{
	internal class XNASoundEntry
	{
		public float mBaseVolume = 1f;

		public float mBasePan;

		public SoundEffect mSound;

		public void Dispose()
		{
			mSound.Dispose();
		}
	}
}
