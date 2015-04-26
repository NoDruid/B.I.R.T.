using UnityEngine;
using System.Collections;

public class CloudMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	void FixedUpdate () 
	{
		this.transform.Rotate(new Vector3(0,0.00001f,0));	
	}

}
