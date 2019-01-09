using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

  public GameObject orbitObject;
	
  // Use this for initialization
	void Start () {
    

  }

  // Update is called once per frame
  void Update () {
    Vector3 globalOrgin = (orbitObject)? orbitObject.transform.transform.position : transform.transform.position;
    transform.RotateAround(-globalOrgin, Vector3.up, Time.deltaTime * 5.0f);
  }
}
