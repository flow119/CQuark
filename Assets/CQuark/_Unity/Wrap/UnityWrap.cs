using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CQuark;
using System;

public class UnityWrap {
    //TODO 把所有类型分开
    public static CQ_Value New (IType type, List<CQ_Value> param) {
        CQ_Value cqVal = new CQ_Value();
        switch(type.keyword) {
            case "Vector3":
                cqVal.type = typeof(Vector3);
                cqVal.value = new Vector3(param[0].GetFloat(), param[1].GetFloat(), param[2].GetFloat());
                return cqVal;
        }
        return null;
    }

    public static CQ_Value StaticValueGet (IType type, string membername) {
        //if(!string.IsNullOrEmpty(type._namespace) && type._namespace != "UnityEngine")
        //    return null;

        CQ_Value cqVal = new CQ_Value();
        switch(type.keyword) {
            case "Vector3":
                cqVal.type = typeof(Vector3);
                switch(membername) {
                    case "up":
                        cqVal.value = Vector3.up;
                        return cqVal;
                    case "down":
                        cqVal.value = Vector3.down;
                        return cqVal;
                    case "forward":
                        cqVal.value = Vector3.forward;
                        return cqVal;
                    case "back":
                        cqVal.value = Vector3.back;
                        return cqVal;
                    case "left":
                        cqVal.value = Vector3.left;
                        return cqVal;
                    case "right":
                        cqVal.value = Vector3.right;
                        return cqVal;
                    case "one":
                        cqVal.value = Vector3.one;
                        return cqVal;
                    case "zero":
                        cqVal.value = Vector3.zero;
                        return cqVal;
                }
                break;
            case "Time":
                switch(membername) {
                    case "deltaTime":
                        cqVal.type = typeof(float);
                        cqVal.value = Time.deltaTime;
                        return cqVal;
                    case "fixedDeltaTime":
                        cqVal.type = typeof(float);
                        cqVal.value = Time.fixedDeltaTime;
                        return cqVal;
                    case "captureFramerate":
                        cqVal.type = typeof(int);
                        cqVal.value = Time.captureFramerate;
                        return cqVal;
                    case "fixedTime":
                        cqVal.type = typeof(float);
                        cqVal.value = Time.fixedTime;
                        return cqVal;
                    case "fixedUnscaledDeltaTime":
                        cqVal.type = typeof(float);
                        cqVal.value = Time.fixedUnscaledDeltaTime;
                        return cqVal;
                    case "fixedUnscaledTime":
                        cqVal.type = typeof(float);
                        cqVal.value = Time.fixedUnscaledTime;
                        return cqVal;
                    case "frameCount":
                        cqVal.type = typeof(int);
                        cqVal.value = Time.frameCount;
                        return cqVal;
                    case "realtimeSinceStartup":
                        cqVal.type = typeof(float);
                        cqVal.value = Time.realtimeSinceStartup;
                        return cqVal;
                    case "time":
                        cqVal.type = typeof(float);
                        cqVal.value = Time.time;
                        return cqVal;
                }
                break;
        }

        return null;
    }

    public static bool StaticValueSet (IType type, string membername, CQ_Value param) {
        return false;
    }

    public static CQ_Value StaticCall (IType type, string functionname, List<CQ_Value> param) {
        //if(!string.IsNullOrEmpty(type._namespace) && type._namespace != "UnityEngine")
        //    return null;

        CQ_Value cqVal = new CQ_Value();
        switch(type.keyword) {
            case "Mathf":
                switch(functionname) {
                    case "Abs":
                        if((Type)param[0].type == typeof(int)) {
                            cqVal.type = typeof(int);
                            cqVal.value = Mathf.Abs((int)(param[0].value));
                            return cqVal;
                        }
                        else if((Type)param[0].type == typeof(float)) {
                            cqVal.type = typeof(float);
                            cqVal.value = Mathf.Abs((float)(param[0].value));
                            return cqVal;
                        }
                        break;
                    case "Sin":
                        cqVal.type = typeof(float);
                        cqVal.value = Mathf.Sin(NumericTypeUtils.GetFloat(param[0].type, param[0].value));
                        return cqVal;
                }
                break;
        }
        return cqVal;
    }

    public static CQ_Value MemberValueGet (IClass _class, object object_this, string membername) {
        return null;
    }

    public static bool MemberValueSet (IClass _class, object object_this, string membername, CQ_Value param) {
        return false;
    }

    public static bool MemberCall (IClass _class, object object_this, string functionname, IList<CQ_Value> param, out CQ_Value cqVal) {
        cqVal = null;
        if(_class is Class_System) {
            cqVal = new CQ_Value();
            Type type = (_class as Class_System).type;
            if(type == typeof(Transform)) {
                Transform t = (Transform)object_this;
                switch(functionname) {
                    case "Rotate":
                        t.Rotate((Vector3)param[0].value, (float)param[1].value);
                        return true;
                }
            }
        }

        return false;
    }
}
