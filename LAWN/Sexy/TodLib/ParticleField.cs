namespace Sexy.TodLib
{
	internal class ParticleField
	{
		public ParticleFieldType mFieldType;

		public FloatParameterTrack mX = new FloatParameterTrack();

		public FloatParameterTrack mY = new FloatParameterTrack();

		public ParticleField()
		{
			mFieldType = ParticleFieldType.FIELD_INVALID;
		}
	}
}
