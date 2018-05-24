using CQuark;
using System.Collections;

/// <summary>
/// Block是一个代码块（虽然可能正式项目中不会用到）
/// 它演示了西瓜工作的基本原理，它包括一个CQ_content
/// 演示包括如何注册一个函数，如何调用，如何修改CQ_content
/// </summary>
public class CQuarkParagraph {
    CQ_Content content;// = new CQ_Content();

    public CQuarkParagraph () {
		content = CQ_ObjPool.PopContent();
    }

	public CQ_Value Execute (string script) {
		var expr = CQuark.CQ_Compiler.CompileBlock(script);//语法分析
        CQ_Value value = expr.ComputeValue(content);//执行表达式
        return value;
    }
    public IEnumerator StartCoroutine (string script, UnityEngine.MonoBehaviour coroutine) {
		var expr = CQuark.CQ_Compiler.CompileBlock(script);//语法分析
        yield return coroutine.StartCoroutine(expr.CoroutineCompute(content, coroutine));
    }

    public CQ_Value GetValue (string name) {
        return content.Get(name);
    }
    public void SetValue (string name, object v) {
        CQ_Value val = new CQ_Value();
        val.SetObject(v.GetType(), v);
		content.DefineAndSet(name, v.GetType(), val);
    }
    public void ClearValue () {
		content.Restore();
    }
}