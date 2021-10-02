using Sexy;

public class DataArray<T> where T : class, new()
{
	public T[] mBlock;

	public uint mMaxUsedCount;

	public uint mMaxSize;

	public int mFreeListHead;

	public uint mSize;

	public uint mNextKey;

	public string mName;

	public DataArray()
	{
		mBlock = null;
		mMaxUsedCount = 0u;
		mMaxSize = 0u;
		mFreeListHead = 0;
		mSize = 0u;
		mNextKey = 1u;
		mName = null;
	}

	public void Dispose()
	{
		DataArrayDispose();
	}

	public void DataArrayInitialize(uint theMaxSize, string theName)
	{
		Debug.ASSERT(mBlock == null);
		Debug.ASSERT(theMaxSize <= 65536);
		if (mBlock == null || mBlock.Length != theMaxSize)
		{
			mBlock = new T[theMaxSize];
		}
		for (int i = 0; i < mBlock.Length; i++)
		{
			mBlock[i] = null;
		}
		mMaxSize = theMaxSize;
		mName = theName;
		mNextKey = 1u;
	}

	public void DataArrayDispose()
	{
		if (mBlock != null)
		{
			DataArrayFreeAll();
			mBlock = null;
			mMaxUsedCount = 0u;
			mMaxSize = 0u;
			mFreeListHead = 0;
			mSize = 0u;
			mName = null;
		}
	}

	public void DataArrayFreeAll()
	{
		for (int i = 0; i < mMaxSize; i++)
		{
			mBlock[i] = null;
		}
		mMaxUsedCount = 0u;
		mFreeListHead = 0;
	}

	public bool IterateNext(ref T theItem)
	{
		if (theItem == null)
		{
			theItem = DataArrayGet(1u);
			return true;
		}
		int nextValidIndex = GetNextValidIndex((uint)(DataArrayGetID(theItem) - 1));
		if (nextValidIndex >= mMaxUsedCount)
		{
			theItem = null;
			return false;
		}
		theItem = mBlock[nextValidIndex];
		return true;
	}

	private int GetNextValidIndex(uint index)
	{
		for (int i = (int)(index + 1); i < mMaxSize; i++)
		{
			if (mBlock[i] != null)
			{
				return i;
			}
		}
		return -1;
	}

	private int GetNextFreeIndex(uint index)
	{
		for (int i = (int)(index + 1); i < mMaxSize; i++)
		{
			if (mBlock[i] == null)
			{
				return i;
			}
		}
		return -1;
	}

	public T DataArrayAlloc()
	{
		uint num;
		if (mFreeListHead == mMaxUsedCount)
		{
			num = mMaxUsedCount;
			mMaxUsedCount++;
			mFreeListHead = (int)mMaxUsedCount;
		}
		else
		{
			num = (uint)mFreeListHead;
			mFreeListHead = GetNextFreeIndex((uint)mFreeListHead);
		}
		T val = new T();
		mBlock[num] = val;
		return val;
	}

	public void DataArrayFree(T theItem)
	{
		int num = 0;
		while (true)
		{
			if (num < mMaxSize)
			{
				if (mBlock[num] == theItem)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		mBlock[num] = null;
		if (num < mFreeListHead)
		{
			mFreeListHead = num;
		}
	}

	public T DataArrayGet(uint theId)
	{
		return mBlock[theId];
	}

	public T DataArrayTryToGet(uint theId)
	{
		if (theId >= mMaxSize)
		{
			return null;
		}
		return mBlock[theId];
	}

	public int DataArrayGetID(T theItem)
	{
		for (int i = 0; i < mMaxSize; i++)
		{
			if (mBlock[i] == theItem)
			{
				return i;
			}
		}
		return -1;
	}
}
