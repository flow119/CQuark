using System;
using System.Collections.Generic;
using System.Text;
namespace CQuark
{
    public partial class CQ_Expression_Compiler 
    {
        public static ICQ_Expression Compiler_Expression_Define(IList<Token> tlist, int pos, int posend)
        {
            CQ_Expression_Define define = new CQ_Expression_Define(pos, posend, tlist[pos].line, tlist[posend].line);
            if (tlist[pos].text == "bool")
            {
                define.value_type = typeof(bool);
            }
            else
            {
				IType type = CQuark.AppDomain.GetTypeByKeyword(tlist[pos].text);
                define.value_type = type.typeBridge;
            }
            define.value_name = tlist[pos+1].text;
            return define;
        }

        public static ICQ_Expression Compiler_Expression_DefineArray(IList<Token> tlist, int pos, int posend)
        {
            CQ_Expression_Define define = new CQ_Expression_Define(pos, posend, tlist[pos].line, tlist[posend].line);
            {
				IType type = CQuark.AppDomain.GetTypeByKeyword(tlist[pos].text + "[]");
                define.value_type = type.typeBridge;
            }
            define.value_name = tlist[pos + 3].text;
            return define;
        }
        public static ICQ_Expression Compiler_Expression_Lambda(IList<Token> tlist, int pos, int posend)
        {
            int b1;
            int fs1 = pos;
            int fe1 = FindCodeAny(tlist, ref fs1, out b1);
            CQ_Expression_Lambda value = new CQ_Expression_Lambda(pos, posend, tlist[pos].line, tlist[posend].line);

            int testbegin = fs1 + 1;
            if (b1 != 1)
            {
                return null;
            }
            //(xxx)=>{...}
            CQ_Expression_Block block = new CQ_Expression_Block(fs1, fe1, tlist[fs1].line, tlist[fe1].line);
            do
            {

                int fe2 = FindCodeAny(tlist, ref testbegin, out b1);


                ICQ_Expression subvalue;
                bool succ = Compiler_Expression(tlist, testbegin, fe2, out subvalue);
                if (!succ) break;
                if (subvalue != null)
                {
                    block.listParam.Add(subvalue);
                    testbegin = fe2 + 2;
                }
                else
                {
                    block.listParam.Add(null);
                    testbegin = fe2 + 2;
                }
            }
            while (testbegin <= fe1);

            value.listParam.Add(block);
            //(...)=>{}
            ICQ_Expression subvalueblock;

            int b2;
            int fs2 = fe1 + 2;
            int fecode = FindCodeAny(tlist, ref fs2, out b2);
            bool succ2 = Compiler_Expression_Block(tlist, fs2, fecode, out subvalueblock);
            if (succ2)
            {
                //value.tokenEnd = fecode;
                //value.lineEnd = tlist[fecode].line;
                value.listParam.Add(subvalueblock);
                return value;
            }
            return null;
        }
        public static ICQ_Expression Compiler_Expression_DefineAndSet(IList<Token> tlist, int pos, int posend)
        {
            int expbegin =pos+3;
            int bdep;
            int expend = FindCodeAny(tlist, ref expbegin, out bdep);
            if(expend!=posend)
            {
                expend = posend;
            }
            ICQ_Expression v;
            bool succ = Compiler_Expression(tlist, expbegin, expend, out v);
            if(succ&&v!=null)
            {
                CQ_Expression_Define define = new CQ_Expression_Define(pos, posend, tlist[pos].line, tlist[posend].line);
                if (tlist[pos].text == "bool")
                {
                    define.value_type = typeof(bool);
                }
                else
                {
					IType type = CQuark.AppDomain.GetTypeByKeyword(tlist[pos].text);
                    define.value_type = type.typeBridge;
                }
                define.value_name = tlist[pos + 1].text;
                define.listParam.Add(v);
                return define;
            }
            LogError(tlist,"不正确的定义表达式:" , pos,posend);
            return null;
        }
        public static ICQ_Expression Compiler_Expression_DefineAndSetArray(IList<Token> tlist, int pos, int posend)
        {
            int expbegin = pos + 5;
            int bdep;
            int expend = FindCodeAny(tlist, ref expbegin, out bdep);
            if (expend != posend)
            {
                expend = posend;
            }
            ICQ_Expression v;
            bool succ = Compiler_Expression(tlist, expbegin, expend, out v);
            if (succ && v != null)
            {
                CQ_Expression_Define define = new CQ_Expression_Define(pos, posend, tlist[pos].line, tlist[posend].line);
                {
					IType type = AppDomain.GetTypeByKeyword(tlist[pos].text+"[]");
                    define.value_type = type.typeBridge;
                }
                define.value_name = tlist[pos + 3].text;
                define.listParam.Add(v);
                return define;
            }
            LogError(tlist, "不正确的定义表达式:", pos, posend);
            return null;
        }
        public static ICQ_Expression Compiler_Expression_Set(IList<Token> tlist, int pos, int posend)
        {
            int expbegin = pos + 2;
            int bdep;
            int expend = FindCodeAny(tlist, ref expbegin, out bdep);
            if (expend != posend)
            {
              
                expend = posend;
            }
            ICQ_Expression v;
            bool succ = Compiler_Expression(tlist, expbegin, expend, out v);
            if (succ && v != null)
            {
                CQ_Expression_SetValue define = new CQ_Expression_SetValue(pos, expend, tlist[pos].line, tlist[expend].line);
                define.value_name = tlist[pos].text;
				define.listParam.Add(v);
                return define;
            }
            LogError(tlist,"不正确的定义表达式:" ,pos,posend);
            return null;
        }
    }
}