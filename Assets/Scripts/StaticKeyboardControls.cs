using UnityEngine;
using System.Collections;

public class StaticKeyboardControls : MonoBehaviour {

	public float mouseSensitivty = 5.0f;
	
	public float verticalRotation = 0;
	public float upDownRange = 60.0f;
	float upDown = 0.0f;

	// Use this for initialization
	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () {
		upDown = 0.0f;

		if (Input.GetKey (KeyCode.Space)) {
			upDown = 1.0f;
		} else if (Input.GetKey (KeyCode.LeftControl)) {
			upDown = -1.0f;
		}


		if(Input.GetKey(KeyCode.LeftShift))
		{
			this.transform.Translate(new Vector3(Input.GetAxis("Horizontal")*4,upDown*4,Input.GetAxis("Vertical")*4));
		}
		else
		{
			this.transform.Translate(new Vector3(Input.GetAxis("Horizontal")*2,upDown*2,Input.GetAxis("Vertical")*2));
		}

		float rotLeftRight = Input.GetAxis("Mouse X") * mouseSensitivty;

		
		verticalRotation = -Input.GetAxis ("Mouse Y") * mouseSensitivty;
		verticalRotation = Mathf.Clamp (verticalRotation, -upDownRange, upDownRange);
		transform.Rotate (verticalRotation, rotLeftRight, 0);
	}
}
