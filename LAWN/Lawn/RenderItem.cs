using Sexy.TodLib;
using System;
using System.Collections.Generic;

namespace Lawn
{
	internal class RenderItem : IComparable
	{
		public RenderObjectType mRenderObjectType;

		public int mZPos;

		public GameObject mGameObject;

		public Plant mPlant;

		public Zombie mZombie;

		public Coin mCoin;

		public Projectile mProjectile;

		public CursorPreview mCursorPreview;

		public TodParticleSystem mParticleSytem;

		public Reanimation mReanimation;

		public GridItem mGridItem;

		public LawnMower mMower;

		public BossPart mBossPart;

		public int mBoardGridY;

		private static long nextId;

		public long id;

		private static Stack<RenderItem> unusedObjects = new Stack<RenderItem>(2048);

		public static void PreallocateMemory()
		{
			for (int i = 0; i < 2048; i++)
			{
				new RenderItem().PrepareForReuse();
			}
		}

		public static RenderItem GetNewRenderItem()
		{
			if (unusedObjects.Count > 0)
			{
				return unusedObjects.Pop();
			}
			return new RenderItem();
		}

		public void PrepareForReuse()
		{
			Reset();
			unusedObjects.Push(this);
		}

		private RenderItem()
		{
			Reset();
		}

		private void Reset()
		{
			id = nextId++;
			if (nextId == long.MaxValue)
			{
				nextId = 0L;
			}
			mRenderObjectType = (RenderObjectType)0;
			mZPos = 0;
			mGameObject = null;
			mPlant = null;
			mZombie = null;
			mCoin = null;
			mProjectile = null;
			mCursorPreview = null;
			mParticleSytem = null;
			mReanimation = null;
			mGridItem = null;
			mMower = null;
			mGameObject = null;
			mPlant = null;
			mZombie = null;
			mCoin = null;
			mProjectile = null;
			mCursorPreview = null;
			mParticleSytem = null;
			mReanimation = null;
			mGridItem = null;
			mMower = null;
			mBossPart = BossPart.BOSS_PART_BACK_LEG;
			mBoardGridY = 0;
		}

		public static int CompareByZ(RenderItem a, RenderItem b)
		{
			if (a == null && b == null)
			{
				return 0;
			}
			if (a == null)
			{
				return -1;
			}
			if (b == null)
			{
				return 1;
			}
			if (a.mZPos == b.mZPos)
			{
				return (int)(a.id - b.id);
			}
			return a.mZPos - b.mZPos;
		}

		int IComparable.CompareTo(object toCompare)
		{
			RenderItem renderItem = (RenderItem)toCompare;
			if (mZPos == renderItem.mZPos)
			{
				return id.CompareTo(renderItem.id);
			}
			return mZPos.CompareTo(renderItem.mZPos);
		}
	}
}
