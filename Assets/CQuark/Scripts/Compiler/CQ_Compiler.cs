using System;
using System.Collections.Generic;
using System.Text;
namespace CQuark
{
    public partial class CQ_Expression_Compiler 
    {

        public static ICQ_Expression Compile(IList<Token> tlist, CQ_Environment env)
        {
            ICQ_Expression value;

            int expbegin = 0;
            int expend = FindCodeBlock(tlist, expbegin);
            if (expend != tlist.Count - 1)
            {
                LogError(tlist, "CodeBlock 识别问题,异常结尾", expbegin, expend);
                return null;
            }
            bool succ = Compiler_Expression_Block(tlist, env, expbegin, expend, out value);
            if (succ)
            {
                if (value == null)
                {
                    DebugUtil.LogWarning("编译为null:");
                }
                return value;

            }
            else
            {
                LogError(tlist, "编译失败:", expbegin, expend);
                return null;
            }
        }

        public static ICQ_Expression Compile_NoBlock(IList<Token> tlist, CQ_Environment env)
        {
            ICQ_Expression value;
            int expbegin = 0;
            int expend = tlist.Count - 1;
            bool succ = Compiler_Expression(tlist, env, expbegin, expend, out value);
            if (succ)
            {
                if (value == null)
                {
                    DebugUtil.LogWarning("编译为null:");
                }
                return value;
            }
            else
            {
                LogError(tlist, "编译失败:", expbegin, expend);
                return null;
            }
        }

        public static IList<ICQ_Type> FileCompile(CQ_Environment env, string filename, IList<Token> tlist, bool embDebugToken)
        {
            return _FileCompiler(filename, tlist, embDebugToken, env, false);
        }

        public static IList<ICQ_Type> FilePreCompile(CQ_Environment env, string filename, IList<Token> tlist)
        {
            return _FileCompiler(filename, tlist, false, env, true);
        }
    }
}