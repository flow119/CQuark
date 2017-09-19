using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark
{
    public class CQ_Value_Value<T> : ICQ_Value
    {
        public CQType type
        {
            get { return typeof(T); }
        }


        public T value_value;
        public object value
        {
            get
            {
                return value_value;
            }
        }
        public override string ToString()
        {
            return type.Name + "|" + value_value.ToString();
        }


        public List<ICQ_Expression> listParam
        {
            get { return null; }
        }
        public int tokenBegin
        {
            get;
            set;
        }
        public int tokenEnd
        {
            get;
            set;
        }
        public int lineBegin
        {
            get;
            set;
        }
        public int lineEnd
        {
            get;
            set;
        }
		public bool hasCoroutine{
			get{
//				if(listParam == null || listParam.Count == 0)
//					return false;
//				foreach(ICQ_Expression expr in listParam){
//					if(expr.hasCoroutine)
//						return true;
//				}
				return false;
			}
		}
        public CQ_Content.Value ComputeValue(CQ_Content content)
        {
            content.InStack(this);
            CQ_Content.Value v = new CQ_Content.Value();
            v.type = this.type;
            v.value = this.value_value;
            content.OutStack(this);
            return v;
        }
		public IEnumerator CoroutineCompute(CQ_Content content, ICoroutine coroutine)
		{
			throw new Exception ("暂时不支持套用协程");
		}
    }

    public class CQ_Value_ScriptValue : ICQ_Value
    {
        public CQType type
        {
            get { return value_type; }
        }
        public SType value_type;

        public SInstance value_value;
        public object value
        {
            get
            {
                return value_value;
            }
        }
        public override string ToString()
        {
            return type.Name + "|" + value_value.ToString();
        }


        public List<ICQ_Expression> listParam
        {
            get { return null; }
        }
        public int tokenBegin
        {
            get;
            set;
        }
        public int tokenEnd
        {
            get;
            set;
        }
        public int lineBegin
        {
            get;
            set;
        }
        public int lineEnd
        {
            get;
            set;
        }
		public bool hasCoroutine{
			get{
//				if(listParam == null || listParam.Count == 0)
//					return false;
//				foreach(ICQ_Expression expr in listParam){
//					if(expr.hasCoroutine)
//						return true;
//				}
				return false;
			}
		}
        public CQ_Content.Value ComputeValue(CQ_Content content)
        {
            content.InStack(this);
            CQ_Content.Value v = new CQ_Content.Value();
            v.type = this.type;
            v.value = this.value_value;
            content.OutStack(this);
            return v;
        }
		public IEnumerator CoroutineCompute(CQ_Content content, ICoroutine coroutine)
		{
			throw new Exception ("暂时不支持套用协程");
		}
    }

    public class CQ_Value_Null : ICQ_Value
    {
        public CQType type
        {
            get { return null; }
        }

        public string Dump()
        {
            return "<unknown> null";
        }
      
        public object value
        {
            get
            {
                return null;
            }
        }
        public override string ToString()
        {
            return "<unknown> null";
        }


        public List<ICQ_Expression> listParam
        {
            get { return null; }
        }
        public int tokenBegin
        {
            get;
            set;
        }
        public int tokenEnd
        {
            get;
            set;
        }
        public int lineBegin
        {
            get;
            set;
        }
        public int lineEnd
        {
            get;
            set;
        }
		public bool hasCoroutine{
			get{
//				if(listParam == null || listParam.Count == 0)
//					return false;
//				foreach(ICQ_Expression expr in listParam){
//					if(expr.hasCoroutine)
//						return true;
//				}
				return false;
			}
		}
        public CQ_Content.Value ComputeValue(CQ_Content content)
        {
            content.InStack(this);
            CQ_Content.Value v = new CQ_Content.Value();
            v.type = this.type;
            v.value = null;
            content.OutStack(this);
            return v;
        }
		public IEnumerator CoroutineCompute(CQ_Content content, ICoroutine coroutine)
		{
			throw new Exception ("暂时不支持套用协程");
		}
    }

    public class CQ_Value_Object:ICQ_Value
    {
        public CQ_Value_Object(Type type)
        {
            this.type = type;
            this.value_value = null;
        }

        public CQType type
        {
            get;
            private set;
        }

        public object value_value;
        public object value
        {
            get
            {
                return value_value;
            }
        }

        public List<ICQ_Expression> listParam
        {
            get { return null; }
        }
        public int tokenBegin
        {
            get;
            set;
        }
        public int tokenEnd
        {
            get;
            set;
        }
        public int lineBegin
        {
            get;
            set;
        }
        public int lineEnd
        {
            get;
            set;
        }
		public bool hasCoroutine{
			get{
//				if(listParam == null || listParam.Count == 0)
//					return false;
//				foreach(ICQ_Expression expr in listParam){
//					if(expr.hasCoroutine)
//						return true;
//				}
				return false;
			}
		}
        public CQ_Content.Value ComputeValue(CQ_Content content)
        {
            content.InStack(this);
            CQ_Content.Value v = new CQ_Content.Value();

            v.type = this.type;
            v.value = this.value_value;
            content.OutStack(this);
            return v;
        }
		public IEnumerator CoroutineCompute(CQ_Content content, ICoroutine coroutine)
		{
			throw new Exception ("暂时不支持套用协程");
		}
    }
}