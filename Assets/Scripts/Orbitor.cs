using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbitor : MonoBehaviour {

  public GameObject orbitObject;

  private bool stop = false;
  // Use this for initialization
	void Start () {
  
  }

  // Update is called once per frame
  void Update () {

    if (Input.GetKeyDown(KeyCode.E))
      stop = (stop) ? false : true;

    if (!stop)
    {
      Vector3 globalOrgin = (orbitObject) ? orbitObject.transform.transform.position : transform.transform.position;
      transform.RotateAround(-globalOrgin, Vector3.up, Time.deltaTime * 5.0f);
    }
  }
}
