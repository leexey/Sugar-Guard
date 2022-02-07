using System;
using System.Collections.Generic;

namespace SugarGuard.Protector.Protections.ControlFlow
{
	// Token: 0x02000029 RID: 41
	public static class Utils
	{
		// Token: 0x060000C3 RID: 195 RVA: 0x0000A91C File Offset: 0x00008B1C
		public static void AddListEntry<TKey, TValue>(this IDictionary<TKey, List<TValue>> self, TKey key, TValue value)
		{
			bool flag = key == null;
			if (flag)
			{
				throw new ArgumentNullException("key");
			}
			List<TValue> list;
			bool flag2 = !self.TryGetValue(key, out list);
			if (flag2)
			{
				list = (self[key] = new List<TValue>());
			}
			list.Add(value);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x0000A96C File Offset: 0x00008B6C
		public static IList<T> RemoveWhere<T>(this IList<T> self, Predicate<T> match)
		{
			for (int i = self.Count - 1; i >= 0; i--)
			{
				bool flag = match(self[i]);
				if (flag)
				{
					self.RemoveAt(i);
				}
			}
			return self;
		}
	}
}
