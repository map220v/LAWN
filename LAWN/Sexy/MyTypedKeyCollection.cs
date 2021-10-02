using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sexy
{
	internal class MyTypedKeyCollection<T> : KeyedCollection<int, TypedKey<T>> where T : struct
	{
		private List<int> keys = new List<int>(50);

		public new void Add(TypedKey<T> item)
		{
			base.Add(item);
			keys.Add(GetKeyForItem(item));
			keys.Sort();
		}

		public new void Clear()
		{
			base.Clear();
			keys.Clear();
		}

		protected override int GetKeyForItem(TypedKey<T> item)
		{
			return item.KeyIdentifier;
		}

		protected override void SetItem(int index, TypedKey<T> item)
		{
			item.KeyIdentifier = index;
			base.SetItem(index, item);
		}

		public int GetKeyAfter(int key)
		{
			int lastKey = GetLastKey();
			foreach (int key2 in keys)
			{
				if (base[key2].tick > key && base[key2].tick < lastKey)
				{
					return base[key2].tick;
				}
			}
			return lastKey;
		}

		public int GetKeyBefore(int key)
		{
			int num = GetFirstKey();
			foreach (int key2 in keys)
			{
				if (base[key2].tick < key && base[key2].tick > num)
				{
					num = base[key2].tick;
				}
				else if (base[key2].tick >= key)
				{
					return num;
				}
			}
			return num;
		}

		public int GetFirstKey()
		{
			if (keys.Count > 0)
			{
				return keys[0];
			}
			return 0;
		}

		public int GetLastKey()
		{
			return keys[keys.Count - 1];
		}

		public bool empty()
		{
			return base.Count == 0;
		}
	}
}
