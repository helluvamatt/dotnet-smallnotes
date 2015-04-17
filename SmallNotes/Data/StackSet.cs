using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallNotes.Data
{
	public class StackSet<T> : List<T>
	{
		public void Push(T item)
		{ 
			if (Contains(item))
			{
				Remove(item);
			}
			Insert(0, item);
		}

		public T Peek()
		{
			return this[0];
		}

		public T Pop()
		{
			T item = Peek();
			RemoveAt(0);
			return item;
		}

	}
}
