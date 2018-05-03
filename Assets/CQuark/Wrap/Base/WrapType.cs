using CQuark;
using System;

namespace CQuark{
	public partial class Wrap {
        private static bool MatchType (CQ_Value[] param, Type[] types, bool mustEqual) {
            //这里没有做长度判断，因为外面判断过了
            if(mustEqual) {
                for(int i = 0; i < types.Length; i++) {
                    if(!param[i].EqualType(types[i]))
                        return false;
                }
            }
            else {
                for(int i = 0; i < types.Length; i++) {
                    if(!param[i].EqualOrImplicateType(types[i]))
                        return false;
                }
            }
            return true;
        }

		private static bool MatchType (CQ_Value[] param, Type type, bool mustEqual) {
			//这里没有做长度判断，因为外面判断过了
			if(mustEqual) {
				if(!param[0].EqualType(type))
						return false;
			}
			else {
				if(!param[0].EqualOrImplicateType(type))
						return false;
			}
			return true;
		}
	}
}

