using UnityEngine;

public class SelfRotation : MonoBehaviour {

  public float speed = 1.0f;

  private bool stop = false;
	
	// Update is called once per frame
	void Update () {

    if (Input.GetKeyDown(KeyCode.E))
      stop = (stop) ? false : true;

    if(!stop)
    {
      transform.Rotate(Vector3.up * Time.deltaTime * speed);
    }
	}
}
