using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour
{
	SixenseInput.Controller leftHydra;
	SixenseInput.Controller rightHydra;
	float cooldown = 0.0f;
	float f_timeSinceLastDoubleTouch = 0.0f;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		leftHydra = SixenseInput.Controllers [0];
		rightHydra = SixenseInput.Controllers [1];
		cooldown -= Time.deltaTime;
		//Debug.Log (cooldown);

#if UNITY_ANDROID
		if(f_timeSinceLastDoubleTouch > 0.0f)
#else
		if ((leftHydra.GetButton (SixenseButtons.BUMPER) && rightHydra.GetButton (SixenseButtons.BUMPER)) || Input.GetAxis ("Fire1") > 0.0f)
#endif
		{
			cooldown = 3.0f;
		}

		checkTouch();
	}

	public void InAttackRange (GameObject animal)
	{
		if (cooldown <= 0.0f) 
		{
#if UNITY_ANDROID
			if(f_timeSinceLastDoubleTouch > 0.0f)
#else
			if ((leftHydra.GetButton (SixenseButtons.BUMPER) && rightHydra.GetButton (SixenseButtons.BUMPER)) || Input.GetAxis ("Fire1") > 0.0f)
#endif
			{
				Debug.Log ("Attack Succesfull!");
				animal.transform.parent = this.transform;
				animal.GetComponentInChildren<Prey> ().enabled = false;
				Rigidbody animalRigidBody = animal.GetComponent<Rigidbody> ();
				animalRigidBody.constraints = RigidbodyConstraints.FreezeAll;
				animalRigidBody.GetComponent<Rigidbody> ().detectCollisions = false;
				animalRigidBody.GetComponent<Rigidbody> ().useGravity = false;
				animal.transform.localPosition = Vector3.zero;
				animal.GetComponentInChildren<MeshCollider> ().enabled = false;
				//Destroy(animal.gameObject);
			}
		}
	}

	void checkTouch()
	{
		int fingerCount = 0;
		
		f_timeSinceLastDoubleTouch -= Time.deltaTime;
		
		foreach (Touch touch in Input.touches) {
			if (touch.phase == TouchPhase.Began && touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled) {
				fingerCount++;
			}
		}
		
		//Debug.Log(fingerCount);
		
		if(fingerCount > 1)
		{
			Debug.Log("Two finger touched!");
			f_timeSinceLastDoubleTouch = 1.0f;
		}
	}
}
