using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CQuark;
using System;

//这个类由工具自动生成，不需要手动修改

namespace CQuark{
	public partial class Wrap {
		#region Types
			
			static Type[] ta780f8dc = new Type[]{typeof(float), typeof(float), typeof(float)};
			static Type[] tc3c31980 = new Type[]{typeof(float), typeof(float)};
			static Type[] t2e78c2fc = new Type[]{typeof(UnityEngine.Vector3), typeof(UnityEngine.Vector3), typeof(float)};
			static Type[] tdc85dce0 = new Type[]{typeof(UnityEngine.Vector3), typeof(UnityEngine.Vector3), typeof(float), typeof(float)};
			static Type[] tb15d7b60 = new Type[]{typeof(UnityEngine.Vector3), typeof(UnityEngine.Vector3)};
			static Type[] ta475c239 = new Type[]{typeof(UnityEngine.Vector3)};
			static Type[] t4fe7dbc3 = new Type[]{typeof(UnityEngine.Vector3), typeof(float)};
			static Type[] tc300a33f = new Type[]{typeof(object)};
			static Type[] tcad56011 = new Type[]{typeof(string)};
			static Type[] t000197ef = new Type[]{typeof(int)};
			static Type[] t05d0225c = new Type[]{typeof(float)};
			static Type[] tce2c8527 = new Type[]{typeof(ushort)};
			static Type[] td25106be = new Type[]{typeof(float[])};
			static Type[] tb97145a0 = new Type[]{typeof(int), typeof(int)};
			static Type[] t05fb6391 = new Type[]{typeof(int[])};
			static Type[] t2496ee4f = new Type[]{typeof(int), typeof(int), typeof(int)};
			static Type[] t081ed015 = new Type[]{typeof(UnityEngine.Transform)};
			static Type[] t3d8cf87f = new Type[]{typeof(UnityEngine.Transform), typeof(bool)};
			static Type[] tcbb99d0e = new Type[]{typeof(UnityEngine.Vector3), typeof(UnityEngine.Quaternion)};
			static Type[] t2621e916 = new Type[]{typeof(UnityEngine.Vector3), typeof(UnityEngine.Space)};
			static Type[] t290c6cd3 = new Type[]{typeof(float), typeof(float), typeof(float), typeof(UnityEngine.Space)};
			static Type[] t79fcdb7c = new Type[]{typeof(UnityEngine.Vector3), typeof(UnityEngine.Transform)};
			static Type[] t385538b9 = new Type[]{typeof(float), typeof(float), typeof(float), typeof(UnityEngine.Transform)};
			static Type[] tc2a8d4cc = new Type[]{typeof(UnityEngine.Vector3), typeof(float), typeof(UnityEngine.Space)};
			static Type[] t3a0b8804 = new Type[]{typeof(UnityEngine.Transform), typeof(UnityEngine.Vector3)};
			static Type[] t05287799 = new Type[]{typeof(System.Type)};
			static Type[] tc6af3e03 = new Type[]{typeof(System.Type), typeof(bool)};
			static Type[] t002e3aea = new Type[]{typeof(bool)};
			static Type[] t6cb40646 = new Type[]{typeof(string), typeof(object), typeof(UnityEngine.SendMessageOptions)};
			static Type[] t3397f290 = new Type[]{typeof(string), typeof(object)};
			static Type[] t21c19c07 = new Type[]{typeof(string), typeof(UnityEngine.SendMessageOptions)};

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

