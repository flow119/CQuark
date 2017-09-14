using System;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{


    public class RegHelper_DeleAction<T> : RegHelper_Type, ICLS_Type_Dele
    {
        public RegHelper_DeleAction(Type type, string setkeyword)
            : base(type, setkeyword, true)
        {
            //var ms = type.GetMethods();

        }

        public override object Math2Value(CLS_Content env, char code, object left, CLS_Content.Value right, out CLType returntype)
        {
            returntype = null;

            if (left is DeleEvent)
            {
                DeleEvent info = left as DeleEvent;
                Delegate calldele = null;

                //!--exist bug.
                /*if (right.value is DeleFunction) calldele = CreateDelegate(env.environment, right.value as DeleFunction);
                else if (right.value is DeleLambda) calldele = CreateDelegate(env.environment, right.value as DeleLambda);
                else if (right.value is Delegate) calldele = right.value as Delegate;*/

                object rightValue = right.value;
                if (rightValue is DeleFunction)
                {
                    if (code == '+')
                    {
                        calldele = CreateDelegate(env.environment, rightValue as DeleFunction);
                    }
                    else if (code == '-')
                    {
                        calldele = CreateDelegate(env.environment, rightValue as DeleFunction);
                    }
                }
                else if (rightValue is DeleLambda)
                {
                    if (code == '+')
                    {
                        calldele = CreateDelegate(env.environment, rightValue as DeleLambda);
                    }
                    else if (code == '-')
                    {
                        calldele = CreateDelegate(env.environment, rightValue as DeleLambda);
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
                    calldele = CreateDelegate(env.environment, right.value as DeleFunction);
                else if (right.value is DeleLambda)
                    calldele = CreateDelegate(env.environment, right.value as DeleLambda);
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


        public Delegate CreateDelegate(ICLS_Environment env, DeleFunction delefunc)
        {
            DeleFunction _func = delefunc;
            Delegate _dele = delefunc.cacheFunction(this._type, null);
            if (_dele != null) return _dele;
            Action<T> dele = (T param0) =>
            {
                var func = _func.calltype.functions[_func.function];
                if (func.expr_runtime != null)
                {
                    CLS_Content content = new CLS_Content(env,true);
                    try
                    {
                        content.DepthAdd();
                        content.CallThis = _func.callthis;
                        content.CallType = _func.calltype;
                        content.function = _func.function;

                        content.DefineAndSet(func._paramnames[0], func._paramtypes[0].type, param0);

                        func.expr_runtime.ComputeValue(content);
                        content.DepthRemove();
                    }
                    catch (Exception err)
                    {
                        string errinfo = "Dump Call in:";
                        if (_func.calltype != null) errinfo += _func.calltype.Name + "::";
                        if (_func.function != null) errinfo += _func.function;
                        errinfo += "\n";
                        env.logger.Log(errinfo + content.Dump()); 
                        throw err;
                    }
                }
            };
            Delegate d = dele as Delegate;
            if ((Type)this.type != typeof(Action))
            {
                _dele = Delegate.CreateDelegate(this.type, d.Target, d.Method);
            }
            else
            {
                _dele = dele;
            }
            return delefunc.cacheFunction(this._type, _dele);
        }


        public Delegate CreateDelegate(ICLS_Environment env, DeleLambda lambda)
        {
            CLS_Content content = lambda.content.Clone();
            var pnames = lambda.paramNames;
            var expr = lambda.expr_func;
            Action<T> dele = (T param0) =>
                {
                    if (expr != null)
                    {
                        try
                        {
                            content.DepthAdd();


                            content.DefineAndSet(pnames[0], typeof(T), param0);

                            expr.ComputeValue(content);

                            content.DepthRemove();
                        }
                        catch (Exception err)
                        {
                            string errinfo = "Dump Call lambda in:";
                            if (content.CallType != null) errinfo += content.CallType.Name + "::";
                            if (content.function != null) errinfo += content.function;
                            errinfo += "\n";
                            env.logger.Log(errinfo + content.Dump()); 
                            throw err;
                        }
                    }
                };
            Delegate d = dele as Delegate;
            if ((Type)this.type != typeof(Action<T>))
            {
                return Delegate.CreateDelegate(this.type, d.Target, d.Method);
            }
            else
            {
                return dele;
            }
        }
    }
}
