using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    class Type_Long : Type_Numeric
    {
        public Type_Long()
            : base(typeof(long), "long", false)
        {

        }

        public override object ConvertTo(object src, CQ_Type targetType)
        {
            bool convertSuccess = false;
            object convertedObject = TryConvertTo<long>(src, targetType, out convertSuccess);
            if (convertSuccess) {
                return convertedObject;
            }

            return base.ConvertTo(src, targetType);
        }

        public override object Math2Value(char code, object left, CQ_Value right, out CQ_Type returntype)
        {
            bool math2ValueSuccess = false;
            object value = Math2Value<long>(code, left, right, out returntype, out math2ValueSuccess);
            if (math2ValueSuccess) {
                return value;
            }

            return base.Math2Value(code, left, right, out returntype);
        }

        public override bool MathLogic(LogicToken code, object left, CQ_Value right)
        {
            bool mathLogicSuccess = false;
            bool value = MathLogic<long>(code, left, right, out mathLogicSuccess);
            if (mathLogicSuccess) {
                return value;
            }

            return base.MathLogic(code, left, right);
        }

        public override object defaultValue
        {
            get { return (long)0; }
        }
    }
}
