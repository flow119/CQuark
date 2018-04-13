using System;
using System.Collections.Generic;
using System.Text;
namespace CQuark {
    //静态类，把文本的List<Token>编译成表达式
    public partial class CQ_Expression_Compiler {
        public static ICQ_Expression Compile (IList<Token> tlist) {
            ICQ_Expression value;

            int expbegin = 0;
            int expend = FindCodeBlock(tlist, expbegin);
            if(expend != tlist.Count - 1) {
                LogError(tlist, "CodeBlock 识别问题,异常结尾", expbegin, expend);
                return null;
            }
            bool succ = Compiler_Expression_Block(tlist, expbegin, expend, out value);
            if(succ) {
                if(value == null) {
                    DebugUtil.LogWarning("编译为null:");
                }
                return value;

            }
            else {
                LogError(tlist, "编译失败:", expbegin, expend);
                return null;
            }
        }

        public static IList<IType> FileCompile (string filename, IList<Token> tlist, bool embDebugToken) {
            return _FileCompiler(filename, tlist, embDebugToken, false);
        }

        public static IList<IType> FilePreCompile (string filename, IList<Token> tlist) {
            return _FileCompiler(filename, tlist, false, true);
        }
    }
}