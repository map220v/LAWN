using Sexy;

namespace Lawn
{
	internal class MagnetItem
	{
		public float mPosX;

		public float mPosY;

		public float mDestOffsetX;

		public float mDestOffsetY;

		public MagnetItemType mItemType;

		public void Reset()
		{
			mPosX = (mPosY = (mDestOffsetX = (mDestOffsetY = 0f)));
			mItemType = MagnetItemType.MAGNET_ITEM_NONE;
		}

		public bool SaveToFile(Buffer b)
		{
			b.WriteFloat(mDestOffsetX);
			b.WriteFloat(mDestOffsetY);
			b.WriteLong((int)mItemType);
			b.WriteFloat(mPosX);
			b.WriteFloat(mPosY);
			return true;
		}

		public bool LoadFromFile(Buffer b)
		{
			mDestOffsetX = b.ReadFloat();
			mDestOffsetY = b.ReadFloat();
			mItemType = (MagnetItemType)b.ReadLong();
			mPosX = b.ReadFloat();
			mPosY = b.ReadFloat();
			return true;
		}
	}
}
