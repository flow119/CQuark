using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark{
	public class DeleFunction// : IDeleBase //指向脚本中的函数
	{
		public DeleFunction(CQ_Type stype, SInstance _this, string function)
		{
			this.calltype = stype;
			this.callthis = _this;
			this.function = function;
		}

		//public string GetKey() {
		//    return calltype.Name + "_" + function;
		//}

		public CQ_Type calltype;
		public SInstance callthis;
		public string function;
		public Delegate cacheFunction(Type deletype, Delegate dele)
		{
			if (dele == null)
			{
				Delegate v = null;
				Dictionary<Type, Delegate> caches = null;
				if (callthis != null)
				{
					if (callthis.deles.TryGetValue(function, out caches))
					{
						caches.TryGetValue(deletype, out v);
					}
				}
				else
				{
					if (calltype.deles.TryGetValue(function, out caches))
					{
						caches.TryGetValue(deletype, out v);
					}
				}
				return v;
			}
			else
			{
				Dictionary<Type, Delegate> caches = null;
				if (callthis != null)
				{
					if (!callthis.deles.TryGetValue(function, out caches))
					{
						caches = new Dictionary<Type, Delegate>();
						callthis.deles[function] = caches;
					}

					caches[deletype] = dele;
				}
				else
				{
					if (!calltype.deles.TryGetValue(function, out caches))
					{
						caches = new Dictionary<Type, Delegate>();
						calltype.deles[function] = caches;
					}

					caches[deletype] = dele;

				}
				return dele;
			}

		}
	}
}
