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

		IDENTIFIER,     //标识符 变量与函数//废弃
		ATTRIBUTE,		//Attribute 180523新增
		NAMESPACE,		//命名空间，1.0.1新增
		CLASS,			//类， 1.0.1新增
		TYPE,           //类型
		CONSTRUCTOR,	//构造函数，既是Type又是Function
		FUNCTION,		//方法，1.0.1新增
		PROPERTY,		//属性（变量，字段，event, delegate），1.0.1新增
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
