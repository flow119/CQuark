using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CQuark
{
    //脚本实例
    public class SInstance
    {
        public CQ_Type type;
        public Dictionary<string, CQ_Value> member = new Dictionary<string, CQ_Value>();//成员
        public Dictionary<string, Dictionary<Type, Delegate>> deles = new Dictionary<string, Dictionary<Type, Delegate>>();
    }
}
