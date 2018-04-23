using System.Collections.Generic;
using CQuark;

/// <summary>
/// CQ_Value[], CQ_Content 的对象池，由于调用方法时创建数组太频繁了，用对象池去处理
/// </summary>
namespace CQuark{
	public class CQ_ObjPool  {

	    static Dictionary<int, Stack<CQ_Value[]>> _arrayPool = new Dictionary<int,Stack<CQ_Value[]>>();

	    public static CQ_Value[] PopArray (int length) {
			if(!_arrayPool.ContainsKey(length)) {
				_arrayPool[length] = new Stack<CQ_Value[]>();
	            return new CQ_Value[length];
	        }

			if(_arrayPool[length].Count > 0)
				return _arrayPool[length].Pop();
	        else
	            return new CQ_Value[length];
	    }

	    public static void PushArray (CQ_Value[] arr) {
			_arrayPool[arr.Length].Push(arr);
	    }

		static Stack<CQ_Content> _contentPool = new Stack<CQ_Content>();

		public static CQ_Content PopContent(){
			if(_contentPool.Count > 0){
				CQ_Content content = _contentPool.Pop();
				content.Restore();
				return content;
			}	else{
				return new CQ_Content();
			}
		}

		public static void PushContent(CQ_Content content){
			_contentPool.Push(content);
		}
	}
}
