﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    class CQ_Type_UInt : RegHelper_Type
    {
        public CQ_Type_UInt()
            : base(typeof(uint), "uint",false)
        {
            //function = new RegHelper_TypeFunction(typeof(uint));
        }

        public override object ConvertTo(object src, CQType targetType)
        {
            bool convertSuccess = false;
            object convertedObject = NumericTypeUtils.TryConvertTo<uint>(src, targetType, out convertSuccess);
            if (convertSuccess) {
                return convertedObject;
            }

            return base.ConvertTo(src, targetType);
        }

        public override object Math2Value(char code, object left, CQ_Content.Value right, out CQType returntype)
        {
            bool math2ValueSuccess = false;
            object value = NumericTypeUtils.Math2Value<uint>(code, left, right, out returntype, out math2ValueSuccess);
            if (math2ValueSuccess) {
                return value;
            }

            return base.Math2Value(code, left, right, out returntype);
        }

        public override bool MathLogic(LogicToken code, object left, CQ_Content.Value right)
        {
            bool mathLogicSuccess = false;
            bool value = NumericTypeUtils.MathLogic<uint>(code, left, right, out mathLogicSuccess);
            if (mathLogicSuccess) {
                return value;
            }

            return base.MathLogic(code, left, right);
        }

        public override object DefValue
        {
            get { return (uint)0; }
        }
    }
}