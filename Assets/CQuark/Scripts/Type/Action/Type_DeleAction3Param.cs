using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
	public class Type_DeleAction<T, T1, T2> : Type_Action
    {
        public Type_DeleAction(Type type, string setkeyword)
            : base(type, setkeyword, true)
        {

        }


        public override CQ_Value Math2Value(char code, CQ_Value left, CQ_Value right)
        {
            if(left.GetObject() is DeleEvent)
            {
                DeleEvent info = left.GetObject() as DeleEvent;
                Delegate calldele = null;

                //!--exist bug.
                /*if (right.value is DeleFunction) calldele = CreateDelegate(right.value as DeleFunction);
                else if (right.value is DeleLambda) calldele = CreateDelegate(right.value as DeleLambda);
                else if (right.value is Delegate) calldele = right.value as Delegate;*/

                object rightValue = right.GetObject();
                if (rightValue is DeleFunction)
                {
                    if (code == '+')
                    {
                        calldele = CreateDelegate(rightValue as DeleFunction);
                    }
                    else if (code == '-')
                    {
                        calldele = CreateDelegate(rightValue as DeleFunction);
                    }
                }
                else if (rightValue is DeleLambda)
                {
                    if (code == '+')
                    {
                        calldele = CreateDelegate(rightValue as DeleLambda);
                    }
                    else if (code == '-')
                    {
                        calldele = CreateDelegate(rightValue as DeleLambda);
                    }
                }
                else if (rightValue is Delegate)
                {
                    calldele = rightValue as Delegate;
                }

                if (code == '+')
                {
                    info._event.AddEventHandler(info.source, calldele);
                    //if (!(rightValue is Delegate)) {
                    //    Dele_Map_Delegate.Map(rightValue as IDeleBase, calldele);
                    //}
                    CQ_Value ret = new CQ_Value();//type保持null
                    ret.SetNoneTypeObject(info);
                    return ret;
                }
                else if (code == '-')
                {
                    info._event.RemoveEventHandler(info.source, calldele);
                    //if (!(rightValue is Delegate)) {
                    //    Dele_Map_Delegate.Destroy(rightValue as IDeleBase);
                    //}
                    CQ_Value ret = new CQ_Value();//type保持null
                    ret.SetNoneTypeObject(info);
                    return ret;
                }

            }
            else if(left.GetObject() is Delegate || left.GetObject() == null)
            {
                Delegate info = left.GetObject() as Delegate;
                Delegate calldele = null;
                if(right.GetObject() is DeleFunction)
                    calldele = CreateDelegate(right.GetObject() as DeleFunction);
                else if(right.GetObject() is DeleLambda)
                    calldele = CreateDelegate(right.GetObject() as DeleLambda);
                else if(right.GetObject() is Delegate)
                    calldele = right.GetObject() as Delegate;
                if (code == '+')
                {
                    CQ_Value ret = new CQ_Value();//type保持null
                    ret.SetNoneTypeObject(Delegate.Combine(info, calldele));
                    return ret;
                }
                else if (code == '-')
                {
                    CQ_Value ret = new CQ_Value();//type保持null
                    ret.SetNoneTypeObject(Delegate.Remove(info, calldele));
                    return ret;
                }
            }
            throw new NotSupportedException();
        }

        public override Delegate CreateDelegate(DeleFunction delefunc)
        {
            DeleFunction _func = delefunc;
            Delegate _dele = delefunc.cacheFunction(this._type, null);
            if (_dele != null) return _dele;
            Action<T, T1, T2> dele = (T param0, T1 param1, T2 param2) =>
            {
                var func = _func.calltype.functions[_func.function];
                if (func.expr_runtime != null)
                {
					CQ_Content content = CQ_ObjPool.PopContent();
                    try
                    {
                        content.CallThis = _func.callthis;
                        content.CallType = _func.calltype;
						#if CQUARK_DEBUG
                        content.function = _func.function;
						#endif

                        CQ_Value p0 = new CQ_Value();
                        p0.SetObject(func._paramtypes[0].typeBridge, param0);
						content.DefineAndSet(func._paramnames[0], func._paramtypes[0].typeBridge, p0);

                        CQ_Value p1 = new CQ_Value();
                        p1.SetObject(func._paramtypes[1].typeBridge, param1);
						content.DefineAndSet(func._paramnames[1], func._paramtypes[1].typeBridge, p1);

                        CQ_Value p2 = new CQ_Value();
                        p2.SetObject(func._paramtypes[2].typeBridge, param2);
						content.DefineAndSet(func._paramnames[2], func._paramtypes[2].typeBridge, p2);

                        func.expr_runtime.ComputeValue(content);
                        content.DepthRemove();
                    }
                    catch (Exception err)
                    {
                        string errinfo = "Dump Call in:";
                        if (_func.calltype != null) errinfo += _func.calltype.Name + "::";
                        if (_func.function != null) errinfo += _func.function;
                        errinfo += "\n";
                        DebugUtil.Log(errinfo + content.Dump()); 
                        throw err;
                    }
					CQ_ObjPool.PushContent(content);
                }
            };
            Delegate d = dele as Delegate;
            if ((Type)this.typeBridge != typeof(Action))
            {
                _dele = Delegate.CreateDelegate(this.typeBridge, d.Target, d.Method);
            }
            else
            {
                _dele = dele;
            }
            return delefunc.cacheFunction(this._type, _dele);
        }


        public override Delegate CreateDelegate(DeleLambda lambda)
        {
            CQ_Content content = lambda.content.Clone();
            var pnames = lambda.paramNames;
            var expr = lambda.expr_func;
            Action<T, T1, T2> dele = (T param0, T1 param1, T2 param2) =>
            {
                if (expr != null)
                {
                    try
                    {
                        content.DepthAdd();

                        CQ_Value p0 = new CQ_Value();
                        p0.SetObject(typeof(T), param0);
						content.DefineAndSet(pnames[0], typeof(T), p0);

                        CQ_Value p1 = new CQ_Value();
                        p1.SetObject(typeof(T1), param1);
						content.DefineAndSet(pnames[1], typeof(T1), p1);

                        CQ_Value p2 = new CQ_Value();
                        p2.SetObject(typeof(T2), param2);
						content.DefineAndSet(pnames[2], typeof(T2), p2);

                        expr.ComputeValue(content);

                        content.DepthRemove();
                    }
                    catch (Exception err)
                    {
                        string errinfo = "Dump Call lambda in:";
                        if (content.CallType != null) errinfo += content.CallType.Name + "::";
						#if CQUARK_DEBUG
                        if (content.function != null) errinfo += content.function;
						#endif
                        errinfo += "\n";
                        DebugUtil.Log(errinfo + content.Dump());
                        throw err;
                    }
                }
            };
            Delegate d = dele as Delegate;
            if ((Type)this.typeBridge != typeof(Action<T, T1, T2>))
            {
                return Delegate.CreateDelegate(this.typeBridge, d.Target, d.Method);
            }
            else
            {
                return dele;
            }
        }
    }
}
