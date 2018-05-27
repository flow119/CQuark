using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark {
    public enum TokenType {
        UNKNOWN,
        KEYWORD,        //关键字
        COMMENT,        //注释
        VALUE,          //数值
        STRING,         //字符串
		PUNCTUATION,    //标点

		IDENTIFIER,     //标识符 变量与函数
		NAMESPACE,		//命名空间，1.0.1新增(编译中间使用，等同于IDENTIFIER)
		TYPE,           //类型
		CLASS,			//类， 1.0.1新增(编译中间使用，等同于TYPE)

		ATTRIBUTE,		//Attribute 180523新增
    }



    public struct Token {
        public string text;
        public int pos;
        public int line;
        public TokenType type;
        public override string ToString () {
			return type + "|" + text + "|" + line + "," + pos;
        }
        public string SourcePos () {
            return string.Format("@line{0},pos{1}", line, pos);

			//asdf"\r\n"//
        }
    }

}
