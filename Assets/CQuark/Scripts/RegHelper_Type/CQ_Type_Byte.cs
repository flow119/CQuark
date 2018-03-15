using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    class CQ_Type_Byte : RegHelper_Type
    {
        public CQ_Type_Byte()
            : base(typeof(byte), "byte",false)
        {
            //function = new RegHelper_TypeFunction(typeof(uint));
        }

        public override object ConvertTo(CQ_Content content, object src, CQType targetType)
        {
            bool convertSuccess = false;
            object convertedObject = NumericTypeUtils.TryConvertTo<byte>(src, targetType, out convertSuccess);
            if (convertSuccess) {
                return convertedObject;
            }

			return base.ConvertTo(content, src, targetType);
        }

		public override object Math2Value(CQ_Content content, char code, object left, CQ_Content.Value right, out CQType returntype)
        {
            bool math2ValueSuccess = false;
            object value = NumericTypeUtils.Math2Value<byte>(code, left, right, out returntype, out math2ValueSuccess);
            if (math2ValueSuccess) {
                return value;
            }

			return base.Math2Value(content, code, left, right, out returntype);
        }

		public override bool MathLogic(CQ_Content content, LogicToken code, object left, CQ_Content.Value right)
        {
            bool mathLogicSuccess = false;
            bool value = NumericTypeUtils.MathLogic<byte>(code, left, right, out mathLogicSuccess);
            if (mathLogicSuccess) {
                return value;
            }

			return base.MathLogic(content, code, left, right);
        }

        public override object DefValue
        {
            get { return (byte)0; }
        }
    }
}
