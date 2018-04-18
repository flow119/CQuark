using System.Collections.Generic;
using CQuark;

/// <summary>
/// CQ_Value[]的对象池，由于调用方法时创建数组太频繁了，用对象池去处理
/// </summary>
public class CQValueArray  {

    static Dictionary<int, Stack<CQ_Value[]>> _pool = new Dictionary<int,Stack<CQ_Value[]>>();

    public static CQ_Value[] Pop (int length) {
        if(!_pool.ContainsKey(length)) {
            _pool[length] = new Stack<CQ_Value[]>();
            return new CQ_Value[length];
        }

        if(_pool[length].Count > 0)
            return _pool[length].Pop();
        else
            return new CQ_Value[length];
    }

    public static void Push (CQ_Value[] arr) {
        _pool[arr.Length].Push(arr);
    }
}
