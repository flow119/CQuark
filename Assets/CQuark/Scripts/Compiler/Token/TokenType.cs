namespace CQuark
{
	public enum TokenType
	{
		UNKNOWN,
		KEYWORD,        //关键字
		PUNCTUATION,    //标点
		IDENTIFIER,     //标识符 变量与函数
		TYPE,           //类型
		COMMENT,        //注释
		VALUE,          //数值
		STRING,         //字符串
	}
}
