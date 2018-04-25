using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CQuark;
using System;

//这个类由工具自动生成，不需要手动修改

namespace CQuark{
	public partial class Wrap {
		#region Types
			
		#endregion
		
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
	}
}

