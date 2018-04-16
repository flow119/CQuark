using CQuark;
using System.Collections;

/// <summary>
/// Block是一个代码块（虽然可能正式项目中不会用到）
/// 它演示了西瓜工作的基本原理，它包括一个CQ_content
/// 演示包括如何注册一个函数，如何调用，如何修改CQ_content
/// </summary>
public class CQuarkBlock {
    CQ_Content content;// = new CQ_Content();

    public CQuarkBlock () {
        content = new CQ_Content();
        content.DepthAdd();
    }

    public object Execute (string script) {
        var expr = CQuark.AppDomain.BuildBlock(script);//语法分析
        CQ_Value value = expr.ComputeValue(content);//执行表达式
        if(value == CQ_Value.Null)
            return null;
        return value.value;
    }
    public IEnumerator StartCoroutine (string script, UnityEngine.MonoBehaviour coroutine) {
        var expr = CQuark.AppDomain.BuildBlock(script);//语法分析
        yield return coroutine.StartCoroutine(expr.CoroutineCompute(content, coroutine));
    }

    public CQ_Value GetValue (string name) {
        return content.Get(name);
    }
    public void SetValue (string name, object v) {
        content.DefineAndSet(name, v.GetType(), v);
    }
    public void ClearValue () {
        content = new CQ_Content();
    }
}