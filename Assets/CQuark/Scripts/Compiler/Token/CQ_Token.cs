using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    public struct Token
    {
        //asTC_UNKNOWN    = 0,//未知
        //asTC_KEYWORD    = 1,//关键字
        //asTC_VALUE      = 2,//value   值
        //asTC_IDENTIFIER = 3,//标识符  变量
        //asTC_COMMENT    = 4,//注释
        //asTC_WHITESPACE = 5,//空格

        public string text;
        public int pos;
        public int line;
        public TokenType type;
        public override string ToString()
        {
            return type.ToString() + "|" + text + "|" + pos.ToString();
        }
        public string SourcePos()
        {
            return string.Format("@line{0},pos{1}", line, pos);
        }
    }
}
