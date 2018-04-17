using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark {
    //运算类型，相当于把各种类分成几个大类
    public interface IType {
        string keyword {
            get;
        }
        string _namespace {
            get;
        }
        TypeBridge typeBridge {
            get;
        }
        object defaultValue {
            get;
        }
        IClass _class {
            get;
        }

        //自动转型能力
        object ConvertTo (object src, TypeBridge targetType);

        //数学计算能力
        CQ_Value Math2Value (char code, object left, CQ_Value right);

        //逻辑计算能力
        bool MathLogic (LogicToken code, object left, CQ_Value right);
    }
}
