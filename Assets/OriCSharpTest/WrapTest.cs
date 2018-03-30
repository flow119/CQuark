using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapTest : MonoBehaviour{

	public WrapTest(int a){

	}

	public int m_filed;
	public int m_get{
		get{
			return 0;
		}
	}

	public int m_set{
		set{
			_set = value;
		}
	}
	private int _set;

	public int get_g{
		get;set;
	}

	public int set_g{
		get;set;
	}

	public void PubMet(){

	}

	public static void StaMet(){
		
	}

	private void PriMet(){

	}

	public void set_a(int value){

	}

	public int b{
		set; get;
	}

	public IEnumerator Corout(){
		yield return new WaitForSeconds(1);
	}

    //public void Log (int a) {
    //    Debug.Log("A");
    //}

    public void Log (float a) {
        Debug.Log("F");
    }

    public void Log (int a, bool b = false) {
        Debug.Log("B");
    }

   

    //public void Log (float a = 2, float b = 3) {
    //    Debug.Log("FF");
    //}

    void Start () {
        //类型、数量完全匹配是第一优先
        //类型完全匹配，但参数有默认值是第二优先
        //类型隐式转换是第三优先
        Log(2);
    }
}
