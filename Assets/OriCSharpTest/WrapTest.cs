using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapTest {

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
}
