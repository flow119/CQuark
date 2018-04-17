using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    class Type_Byte : Type_Numeric
    {
        public Type_Byte()
            : base(typeof(byte), "byte",false)
        {
            //function = new RegHelper_TypeFunction(typeof(uint));
        }

        public override object ConvertTo(object src, TypeBridge targetType)
        {
            bool convertSuccess = false;
            object convertedObject = TryConvertTo<byte>(src, targetType, out convertSuccess);
            if (convertSuccess) {
                return convertedObject;
            }

			return base.ConvertTo(src, targetType);
        }

		public override CQ_Value Math2Value(char code, object left, CQ_Value right)
        {
            CQ_Value returnValue = CQ_Value.Null;

            if (Math2Value<byte>(code, left, right, out returnValue)) {
                return returnValue;
            }

            return base.Math2Value(code, left, right);
        }

		public override bool MathLogic(LogicToken code, object left, CQ_Value right)
        {
            bool mathLogicSuccess = false;
            bool value = MathLogic<byte>(code, left, right, out mathLogicSuccess);
            if (mathLogicSuccess) {
                return value;
            }

			return base.MathLogic(code, left, right);
        }

        public override object defaultValue
        {
            get { return (byte)0; }
        }
    }
}
