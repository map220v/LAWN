namespace Lawn
{
	internal class Achievement
	{
		public int mImageId;

		public readonly string mName;

		public readonly string mDesc;

		public Achievement(int aImageId, string aName, string aDesc)
		{
			mImageId = aImageId;
			mName = aName;
			mDesc = aDesc;
		}
	}
}
