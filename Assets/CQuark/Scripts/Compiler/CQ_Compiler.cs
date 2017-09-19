using System;
using System.Collections.Generic;
using System.Text;
namespace CQuark
{
    public partial class CQ_Expression_Compiler : ICQ_Expression_Compiler
    {
        ICQ_Logger logger;
        public CQ_Expression_Compiler(ICQ_Logger logger)
        {
            this.logger = logger;
        }
        public ICQ_Expression Compile(IList<Token> tlist, ICQ_Environment content)
        {
            ICQ_Expression value;

            int expbegin = 0;
            int expend = FindCodeBlock(tlist, expbegin);
            if (expend != tlist.Count - 1)
            {
                LogError(tlist, "CodeBlock 识别问题,异常结尾", expbegin, expend);
                return null;
            }
            bool succ = Compiler_Expression_Block(tlist, content, expbegin, expend, out value);
            if (succ)
            {
                if (value == null)
                {
                    logger.Log_Warn("编译为null:");
                }
                return value;

            }
            else
            {
                LogError(tlist, "编译失败:", expbegin, expend);
                return null;
            }



        }

        public ICQ_Expression Compile_NoBlock(IList<Token> tlist, ICQ_Environment content)
        {
            ICQ_Expression value;
            int expbegin = 0;
            int expend = tlist.Count - 1;
            bool succ = Compiler_Expression(tlist, content, expbegin, expend, out value);
            if (succ)
            {
                if (value == null)
                {
                    logger.Log_Warn("编译为null:");
                }
                return value;

            }
            else
            {
                LogError(tlist, "编译失败:", expbegin, expend);
                return null;
            }


        }
        public ICQ_Expression Optimize(ICQ_Expression value, ICQ_Environment env)
        {
            ICQ_Expression expr = value as ICQ_Expression;
            if (expr == null) return value;
            else return OptimizeDepth(expr, new CQ_Content(env));
        }
        ICQ_Expression OptimizeDepth(ICQ_Expression expr, CQ_Content content)
        {
            //先进行深入优化
            if (expr.listParam != null)
            {
                for (int i = 0; i < expr.listParam.Count; i++)
                {
                    ICQ_Expression subexpr = expr.listParam[i] as ICQ_Expression;
                    if (subexpr != null)
                    {
                        expr.listParam[i] = OptimizeDepth(subexpr, content);
                    }
                }
            }


            return OptimizeSingle(expr, content);

        }
        ICQ_Expression OptimizeSingle(ICQ_Expression expr, CQ_Content content)
        {

            if (expr is CQ_Expression_Math2Value || expr is CQ_Expression_Math2ValueAndOr || expr is CQ_Expression_Math2ValueLogic)
            {

                if (expr.listParam[0] is ICQ_Value &&
                expr.listParam[1] is ICQ_Value)
                {
                    CQ_Content.Value result = expr.ComputeValue(content);
                    if ((Type)result.type == typeof(bool))
                    {
                        CQ_Value_Value<bool> value = new CQ_Value_Value<bool>();
                        value.value_value = (bool)result.value;
                        value.tokenBegin = expr.listParam[0].tokenBegin;
                        value.tokenEnd = expr.listParam[1].tokenEnd;
                        value.lineBegin = expr.listParam[0].lineBegin;
                        value.lineEnd = expr.listParam[1].lineEnd;
                        return value;
                    }
                    else
                    {
                        ICQ_Type v = content.environment.GetType(result.type);
                        ICQ_Value value = v.MakeValue(result.value);
                        value.tokenBegin = expr.listParam[0].tokenBegin;
                        value.tokenEnd = expr.listParam[1].tokenEnd;
                        value.lineBegin = expr.listParam[0].lineBegin;
                        value.lineEnd = expr.listParam[1].lineEnd;
                        return value;
                    }


                }
            }
            if (expr is CQ_Expression_Math3Value)
            {
                CQ_Content.Value result = expr.listParam[0].ComputeValue(content);
                if ((Type)result.type == typeof(bool))
                {
                    bool bv = (bool)result.value;
                    if (bv)
                        return expr.listParam[1];
                    else
                        return expr.listParam[2];
                }
            }

            return expr;
        }


        public IList<ICQ_Type> FileCompile(ICQ_Environment env,string filename,IList<Token> tlist, bool embDebugToken)
        {
            return _FileCompiler(filename, tlist, embDebugToken, env, false);
        }
        public IList<ICQ_Type> FilePreCompile(ICQ_Environment env, string filename, IList<Token> tlist)
        {
            return _FileCompiler(filename, tlist, false, env, true);
        }
    }
}