using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
	public class Type_DeleAction<T, T1> : Type_Action
    {

        public Type_DeleAction(Type type, string setkeyword) :
            base(type, setkeyword, true)
        {

        }


        public override object Math2Value(char code, object left, CQ_Value right, out CQ_Type returntype)
        {
            returntype = null;

            if (left is DeleEvent)
            {
                DeleEvent info = left as DeleEvent;
                Delegate calldele = null;

                //!--exist bug.
                /*if (right.value is DeleFunction) calldele = CreateDelegate(right.value as DeleFunction);
                else if (right.value is DeleLambda) calldele = CreateDelegate(right.value as DeleLambda);
                else if (right.value is Delegate) calldele = right.value as Delegate;*/

                object rightValue = right.value;
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
                    return info;
                }
                else if (code == '-')
                {
                    info._event.RemoveEventHandler(info.source, calldele);
                    //if (!(rightValue is Delegate)) {
                    //    Dele_Map_Delegate.Destroy(rightValue as IDeleBase);
                    //}
                    return info;
                }

            }
            else if (left is Delegate || left == null)
            {
                Delegate info = left as Delegate;
                Delegate calldele = null;
                if (right.value is DeleFunction)
                    calldele = CreateDelegate(right.value as DeleFunction);
                else if (right.value is DeleLambda)
                    calldele = CreateDelegate(right.value as DeleLambda);
                else if (right.value is Delegate)
                    calldele = right.value as Delegate;
                if (code == '+')
                {
                    return Delegate.Combine(info, calldele); ;
                }
                else if (code == '-')
                {
                    return Delegate.Remove(info, calldele);
                }
            }
            return new NotSupportedException();
        }
        public override Delegate CreateDelegate(DeleFunction delefunc)
        {
            DeleFunction _func = delefunc;
            Delegate _dele = delefunc.cacheFunction(this._type, null);
            if (_dele != null) return _dele;
            Action<T, T1> dele = (T param0, T1 param1) =>
            {
                var func = _func.calltype.functions[_func.function];

                if (func.expr_runtime != null)
                {
                    CQ_Content content = new CQ_Content();
                    try
                    {
                        content.DepthAdd();
                        content.CallThis = _func.callthis;
                        content.CallType = _func.calltype;
						#if CQUARK_DEBUG
                        content.function = _func.function;
						#endif

                        content.DefineAndSet(func._paramnames[0], func._paramtypes[0].typeBridge, param0);
                        content.DefineAndSet(func._paramnames[1], func._paramtypes[1].typeBridge, param1);

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
            Action<T, T1> dele = (T param0, T1 param1) =>
            {
                if (expr != null)
                {
                    try
                    {
                        content.DepthAdd();

                        content.DefineAndSet(pnames[0], typeof(T), param0);
                        content.DefineAndSet(pnames[1], typeof(T1), param1);

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
            if ((Type)this.typeBridge != typeof(Action<T, T1>))
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
