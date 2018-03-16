using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    class CQ_Type_Double : RegHelper_Type
    {
        public CQ_Type_Double()
            : base(typeof(double), "double", false)
        {
                    
            //function = new RegHelper_TypeFunction(typeof(double));
        }
  
        public override object ConvertTo(object src, TypeBridge targetType)
        {
            bool convertSuccess = false;
            object convertedObject = NumericTypeUtils.TryConvertTo<double>(src, targetType, out convertSuccess);
            if (convertSuccess) {
                return convertedObject;
            }

            return base.ConvertTo(src, targetType);
        }

        public override object Math2Value(char code, object left, CQ_Content.Value right, out TypeBridge returntype)
        {
            bool math2ValueSuccess = false;
            object value = NumericTypeUtils.Math2Value<double>(code, left, right, out returntype, out math2ValueSuccess);
            if (math2ValueSuccess) {
                return value;
            }

            return base.Math2Value(code, left, right, out returntype);
        }

        public override bool MathLogic(LogicToken code, object left, CQ_Content.Value right)
        {
            bool mathLogicSuccess = false;
            bool value = NumericTypeUtils.MathLogic<double>(code, left, right, out mathLogicSuccess);
            if (mathLogicSuccess) {
                return value;
            }

            return base.MathLogic(code, left, right);
        }

        public override object defaultValue
        {
            get { return (double)0; }
        }
    }
}
