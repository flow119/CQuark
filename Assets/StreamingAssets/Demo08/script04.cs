using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

class script04 : MonoBehaviour
{
   

    public void Start()
    {
        Debug.Log("Start" + this.gameObject);
//        gameObject.name = "Cool name.";
    }

    public void Update()
    {
//        Transform trans = this.gameObject.GetComponent("Transform") as Transform;
    
        transform.Rotate(Vector3.up, 180 * Time.deltaTime);
    }

}
