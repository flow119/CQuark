using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark {
    public enum TokenType {
        UNKNOWN,
        KEYWORD,        //关键字
        PUNCTUATION,    //标点
        IDENTIFIER,     //标识符 变量与函数
        TYPE,           //类型
        COMMENT,        //注释
        VALUE,          //数值
        STRING,         //字符串
    }

    public struct Token {
        public string text;
        public int pos;
        public int line;
        public TokenType type;
        public override string ToString () {
            return type.ToString() + "|" + text + "|" + pos.ToString();
        }
        public string SourcePos () {
            return string.Format("@line{0},pos{1}", line, pos);
        }
    }
}
