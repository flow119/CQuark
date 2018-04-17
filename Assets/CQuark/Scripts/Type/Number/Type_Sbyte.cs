using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    class Type_Sbyte : Type_Numeric
    {
        public Type_Sbyte()
            : base(typeof(sbyte), "sbyte",false)
        {

        }

        public override object ConvertTo(object src, TypeBridge targetType)
        {
            bool convertSuccess = false;
            object convertedObject = TryConvertTo<sbyte>(src, targetType, out convertSuccess);
            if (convertSuccess) {
                return convertedObject;
            }

            return base.ConvertTo(src, targetType);
        }

        public override CQ_Value Math2Value (char code, object left, CQ_Value right) {
            CQ_Value returnValue = CQ_Value.Null;

            if(Math2Value<sbyte>(code, left, right, out returnValue)) {
                return returnValue;
            }

            return base.Math2Value(code, left, right);
        }

        public override bool MathLogic(LogicToken code, object left, CQ_Value right)
        {
            bool mathLogicSuccess = false;
            bool value = MathLogic<sbyte>(code, left, right, out mathLogicSuccess);
            if (mathLogicSuccess) {
                return value;
            }

            return base.MathLogic(code, left, right);
        }

        public override object defaultValue
        {
            get { return (sbyte)0; }
        }
    }
}
