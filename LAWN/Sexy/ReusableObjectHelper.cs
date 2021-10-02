using System.Collections.Generic;

namespace Sexy
{
	internal class ReusableObjectHelper<T> where T : IReusable, new()
	{
		private Stack<T> unusedInstances = new Stack<T>();

		public T GetNew()
		{
			if (unusedInstances.Count == 0)
			{
				return new T();
			}
			return unusedInstances.Pop();
		}

		public void PushOnToReuseStack(T obj)
		{
			obj.Reset();
			unusedInstances.Push(obj);
		}
	}
}
