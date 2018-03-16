using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    class Type_Int : Type_Operatorable
    {
        public Type_Int()
            : base(typeof(int), "int", false)
        {

        }

        public override object ConvertTo(object src, TypeBridge targetType)
        {
            bool convertSuccess = false;
            object convertedObject = NumericTypeUtils.TryConvertTo<int>(src, targetType, out convertSuccess);
            if (convertSuccess) {
                return convertedObject;
            }

            return base.ConvertTo(src, targetType);
        }

        public override object Math2Value(char code, object left, CQ_Value right, out TypeBridge returntype)
        {
            bool math2ValueSuccess = false;
            object value = NumericTypeUtils.Math2Value<int>(code, left, right, out returntype, out math2ValueSuccess);
            if (math2ValueSuccess) {
                return value;
            }

            return base.Math2Value(code, left, right, out returntype);
        }

        public override bool MathLogic(LogicToken code, object left, CQ_Value right)
        {
            bool mathLogicSuccess = false;
            bool value = NumericTypeUtils.MathLogic<int>(code, left, right, out mathLogicSuccess);
            if (mathLogicSuccess) {
                return value;
            }

            return base.MathLogic(code, left, right);
        }

        public override object defaultValue
        {
            get { return (int)0; }
        }
    }
}
