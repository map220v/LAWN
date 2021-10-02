namespace Sexy.TodLib
{
	internal class TodSmoothArray
	{
		public int mItem;

		public float mWeight;

		public float mLastPicked;

		public float mSecondLastPicked;

		public bool SaveToFile(Buffer b)
		{
			b.WriteLong(mItem);
			b.WriteFloat(mWeight);
			b.WriteFloat(mLastPicked);
			b.WriteFloat(mSecondLastPicked);
			return true;
		}

		public bool LoadFromFile(Buffer b)
		{
			mItem = b.ReadLong();
			mWeight = b.ReadFloat();
			mLastPicked = b.ReadFloat();
			mSecondLastPicked = b.ReadFloat();
			return true;
		}
	}
}
