using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    class Type_UShort : Type_Numeric
    {
        public Type_UShort()
            : base(typeof(ushort), "ushort",false)
        {
            //function = new RegHelper_TypeFunction(typeof(uint));
        }

        public override object ConvertTo(object src, CQ_Type targetType)
        {
            bool convertSuccess = false;
            object convertedObject = TryConvertTo<ushort>(src, targetType, out convertSuccess);
            if (convertSuccess) {
                return convertedObject;
            }

            return base.ConvertTo(src, targetType);
        }

        public override object Math2Value(char code, object left, CQ_Value right, out CQ_Type returntype)
        {
            bool math2ValueSuccess = false;
            object value = Math2Value<ushort>(code, left, right, out returntype, out math2ValueSuccess);
            if (math2ValueSuccess) {
                return value;
            }

            return base.Math2Value(code, left, right, out returntype);
        }

        public override bool MathLogic(LogicToken code, object left, CQ_Value right)
        {
            bool mathLogicSuccess = false;
            bool value = MathLogic<ushort>(code, left, right, out mathLogicSuccess);
            if (mathLogicSuccess) {
                return value;
            }

            return base.MathLogic(code, left, right);
        }

        public override object defaultValue
        {
            get { return (ushort)0; }
        }
    }
}
