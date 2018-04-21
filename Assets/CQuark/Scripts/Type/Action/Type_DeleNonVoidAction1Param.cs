using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    /// <summary>
    /// 支持有返回值的带 1 个参数的委托注册.
    /// 注意这里和void类型委托注册的用法有些区别：
    /// 这里的类模板第一个参数是返回类型.
    ///    比如有个返回bool型的委托定义如下：
    ///    public class Class {
    ///         public delegate bool BoolParam1Delegate(int param);
    ///    }
    ///    那么注册方式如下：
    ///    env.RegType(new RegHelper_DeleNonVoidAction<bool, int>(typeof(Class.BoolParam1Delegate), "Class.BoolParam1Delegate"));
    /// </summary>
	public class Type_DeleNonVoidAction<ReturnType, T> : Type_Action
    {
        /// <summary>
        /// 有返回值,同时带 1个 参数的委托.
        /// </summary>
        /// <returns></returns>
        public delegate ReturnType NonVoidDelegate(T param);

        public Type_DeleNonVoidAction(Type type, string setkeyword)
            : base(type, setkeyword, true)
        {

        }

        public override CQ_Value Math2Value(char code, CQ_Value left, CQ_Value right)
        {
            if(left.GetValue() is DeleEvent)
            {
                DeleEvent info = left.GetValue() as DeleEvent;
                Delegate calldele = null;

                //!--exist bug.
                /*if (right.value is DeleFunction) calldele = CreateDelegate(right.value as DeleFunction);
                else if (right.value is DeleLambda) calldele = CreateDelegate(right.value as DeleLambda);
                else if (right.value is Delegate) calldele = right.value as Delegate;*/

                object rightValue = right.GetValue();
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
                    ret.SetNoneTypeValue(info);
                    return ret;
                }
                else if (code == '-')
                {
                    info._event.RemoveEventHandler(info.source, calldele);
                    //if (!(rightValue is Delegate)) {
                    //    Dele_Map_Delegate.Destroy(rightValue as IDeleBase);
                    //}
                    CQ_Value ret = new CQ_Value();//type保持null
                    ret.SetNoneTypeValue(info);
                    return ret;
                }

            }
            else if(left.GetValue() is Delegate || left.GetValue() == null)
            {
                Delegate info = left.GetValue() as Delegate;
                Delegate calldele = null;
                if(right.GetValue() is DeleFunction)
                    calldele = CreateDelegate(right.GetValue() as DeleFunction);
                else if(right.GetValue() is DeleLambda)
                    calldele = CreateDelegate(right.GetValue() as DeleLambda);
                else if(right.GetValue() is Delegate)
                    calldele = right.GetValue() as Delegate;
                if (code == '+')
                {
                    CQ_Value ret = new CQ_Value();//type保持null
                    ret.SetNoneTypeValue(Delegate.Combine(info, calldele));
                    return ret;
                }
                else if (code == '-')
                {
                    CQ_Value ret = new CQ_Value();//type保持null
                    ret.SetNoneTypeValue(Delegate.Remove(info, calldele));
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
            NonVoidDelegate dele = delegate(T param)
            {
                var func = _func.calltype.functions[_func.function];
                if (func.expr_runtime != null)
                {
					CQ_Content content = CQ_ObjPool.PopContent();
                    try
                    {
                        content.DepthAdd();
                        content.CallThis = _func.callthis;
                        content.CallType = _func.calltype;
						#if CQUARK_DEBUG
                        content.function = _func.function;
						#endif

                        CQ_Value p0 = new CQ_Value();
                        p0.SetValue(func._paramtypes[0].typeBridge, param);
                        content.DefineAndSet(func._paramnames[0], p0);

                        CQ_Value retValue = func.expr_runtime.ComputeValue(content);
                        content.DepthRemove();

                        return (ReturnType)retValue.GetValue();
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
                return default(ReturnType);
            };
            _dele = Delegate.CreateDelegate(this.typeBridge, dele.Target, dele.Method);
            return delefunc.cacheFunction(this._type, _dele);
        }

        public override Delegate CreateDelegate(DeleLambda lambda)
        {
            CQ_Content content = lambda.content.Clone();
            var pnames = lambda.paramNames;
            var expr = lambda.expr_func;

            NonVoidDelegate dele = delegate(T param)
            {
                if (expr != null)
                {
                    try
                    {

                        content.DepthAdd();

                        CQ_Value p0 = new CQ_Value();
                        p0.SetValue(typeof(T), param);
                        content.DefineAndSet(pnames[0], p0);

                        CQ_Value retValue = expr.ComputeValue(content);

                        content.DepthRemove();

                        return (ReturnType)retValue.GetValue();
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
                return default(ReturnType);
            };

            Delegate d = dele as Delegate;
            return Delegate.CreateDelegate(this.typeBridge, d.Target, d.Method);
        }
    }
}
