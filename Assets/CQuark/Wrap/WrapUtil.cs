using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CQuark;
using System;

//这个类是Wrap的工具，请不要删除

namespace CQuark{
	public partial class Wrap {
        private static bool MatchType (List<CQ_Value> param, Type[] types, bool mustEqual) {
            //这里没有做长度判断，因为外面判断过了
            for(int i = 0; i < types.Length; i++) {
                if(mustEqual){
                     if(!param[i].EqualType(types[i]))
                        return false;
                }
                else {
                    if(!param[i].EqualOrImplicateType(types[i]))
                        return false;
                }
            }
            return true;
        }
	}
}
