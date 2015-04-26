using UnityEngine;
using System.Collections;

public class FlyingRigid : MonoBehaviour
{
	float f_speed = 0.4f;
	float f_sprint = 0.0f;
	float f_startSpeed = 0.0f;
	bool b_collision = false;
	bool b_landed = true;
	bool b_firstperson = true;
	protected Rigidbody rigidBody = null;
	Animation anim;
	public GameObject playerModel;
	public GameObject cameras;
	public GameObject schnabel;
	public bool b_keyboard, b_mobile;
	SixenseInput.Controller leftHydra;
	SixenseInput.Controller rightHydra;
	float f_timeSinceLastTouch = 0.0f;

	void Awake ()
	{
		rigidBody = gameObject.GetComponent<Rigidbody> ();
	}

	void Start ()
	{
		anim = GetComponentInChildren<Animation> ();

		leftHydra = SixenseInput.Controllers [0];
		rightHydra = SixenseInput.Controllers [1];

		if(b_mobile)
		{
			changeCameraPerspective();
		}
	}

	void Update ()
	{
#if UNITY_ANDROID
		if (getTouchTap () > 0.0f) {
#else
		if (rightHydra.GetButtonDown (SixenseButtons.TRIGGER) || leftHydra.GetButtonDown (SixenseButtons.TRIGGER) || Input.GetAxis ("Jump") > 0.0f) {
#endif

			if (b_landed) {
				f_startSpeed = 0.75f;
			}
				
			b_collision = false;
			f_sprint = 0.5f;
				
			b_landed = false;
			rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
			rigidBody.useGravity = false;
			anim.Play ("BirtFlap2");
		}

		if (leftHydra.GetButtonDown (SixenseButtons.START)) {
			resetGame ();
		}
		
		if (rightHydra.GetButtonDown (SixenseButtons.START) || Input.GetKeyDown (KeyCode.V) || Input.GetKeyDown (KeyCode.JoystickButton6)) {
			changeCameraPerspective ();
		}

	}

	void FixedUpdate ()
	{
		if (f_startSpeed > 0.0f) {
			this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y + f_startSpeed, this.transform.position.z);
			f_startSpeed -= 0.01f;
		}

		RotateX (leftHydra, rightHydra);
		RotateY (leftHydra, rightHydra);
		CalculateSpeed (leftHydra, rightHydra);

	}

	float getTouchTap ()
	{
		foreach (Touch touch in Input.touches) {
			if (touch.phase == TouchPhase.Began) {
				f_timeSinceLastTouch = 1.0f;
			}
		}

		f_timeSinceLastTouch -= Time.deltaTime;

		return f_timeSinceLastTouch;
	}

	void changeCameraPerspective ()
	{
		if (b_firstperson) {
			cameras.transform.localPosition = new Vector3 (0.0f, 7.0f, -19.0f);
			b_firstperson = false;
			schnabel.gameObject.SetActive (false);
		} else {
			cameras.transform.localPosition = new Vector3 (0.0f, 0.0f, 3.7f);
			b_firstperson = true;
			schnabel.gameObject.SetActive (true);
		}
	}

	void resetGame ()
	{
		b_landed = true;
		this.transform.position = new Vector3 (0.0f, 352.0f, 0.0f);
		this.transform.rotation = Quaternion.Euler (new Vector3 (0.0f, 0.0f, 0.0f));
		playerModel.transform.localRotation = Quaternion.Euler (new Vector3 (0.0f, 0.0f, 0.0f));
	}

	void OnCollisionEnter (Collision info)
	{
		if (info.collider.gameObject.name == "worldborder") {
			this.transform.Rotate (new Vector3 (0.0f, 180.0f, 0.0f));
			this.transform.rotation = Quaternion.Euler (new Vector3 (0.0f, this.transform.rotation.y, this.transform.rotation.z));
		} else if (info.gameObject.transform.parent != null) {
			if (info.gameObject.transform.parent.gameObject.transform.parent != null) {
				if (info.gameObject.transform.parent.gameObject.transform.parent.name == "terrain") {
					if (!b_landed) {
						RaycastHit hit;
						Physics.Raycast (new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z), new Vector3 (0, -1, 0), out hit);
						if (Vector3.Dot (new Vector3 (0, -1, 0), hit.normal) < -0.5f) {
							this.transform.rotation = Quaternion.Euler (new Vector3 (0.0f, this.transform.rotation.eulerAngles.y, this.transform.rotation.eulerAngles.z));
							this.transform.position = new Vector3 (hit.point.x, hit.point.y + 1.0f, hit.point.z);
										
							b_landed = true;
							Debug.Log ("landed");
							rigidBody.useGravity = true;
							rigidBody.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;

						}
					}
				}
			}
		}
	}
	
	void RotateX (SixenseInput.Controller leftHydra, SixenseInput.Controller rightHydra)
	{
		if (!b_landed) {

			float xRot = 0.0f;
			float inputRotationX;
			if (b_keyboard) {
				inputRotationX = Input.GetAxis ("Vertical") / 4.0f;
			}
			else if(b_mobile)
			{
				inputRotationX = -(Input.acceleration.z + 0.7f)/2.0f;
			}
			else 
			{
				inputRotationX = (rightHydra.Rotation.x + leftHydra.Rotation.x) / 2.0f;
			}

			if (transform.rotation.eulerAngles.x < 180) {
				xRot = Mathf.Clamp ((transform.rotation.eulerAngles.x + (inputRotationX * 4)), -1.0f, 85.0f);
			} else {
				xRot = Mathf.Clamp ((transform.rotation.eulerAngles.x + (inputRotationX * 4)), 275.0f, 361.0f);
			}
			
			Vector3 rotation = new Vector3 (xRot, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
			transform.rotation = Quaternion.Euler (rotation);
			playerModel.transform.localRotation = Quaternion.Euler (new Vector3 (xRot, playerModel.transform.rotation.eulerAngles.y, playerModel.transform.rotation.eulerAngles.z));
		} 	
	}
	
	void RotateY (SixenseInput.Controller leftHydra, SixenseInput.Controller rightHydra)
	{
		float hydraDifference;
		if (b_keyboard) {
			hydraDifference = Input.GetAxis ("Horizontal") * 45.0f;
		}
		else if(b_mobile)
		{
			hydraDifference = Input.acceleration.x * 85.0f;
		}
		else 
		{
			hydraDifference = Mathf.Clamp (leftHydra.Position.y - rightHydra.Position.y, -75.0f, 75.0f);
		}

		transform.rotation = Quaternion.Euler (transform.rotation.eulerAngles + new Vector3 (0, hydraDifference / 40.0f, 0));
		playerModel.transform.localRotation = Quaternion.Euler (new Vector3 (0, 0, -(hydraDifference)));
	}
	
	void CalculateSpeed (SixenseInput.Controller leftHydra, SixenseInput.Controller rightHydra)
	{
		float rotationXNorm = transform.rotation.eulerAngles.x / 360.0f;
		float gravityForce = -(f_speed) / 5;

		//runter
		if (rotationXNorm > 0.01 && rotationXNorm < 0.25) {
			f_speed += Map (0.0f, 0.010f, 0.0f, 0.25f, rotationXNorm);
			
		}
		//hoch
		else if (rotationXNorm > 0.75f && rotationXNorm < 0.99f) {
			f_speed += Map (-0.001f, 0, 0.75f, 1.0f, rotationXNorm);
		}
		
		f_speed = Mathf.Clamp (f_speed, 0.1f, 0.25f);

		Vector3 forward = new Vector3 (0, 0, f_speed + f_sprint);
		
		f_sprint -= 0.0005f;
		f_speed -= 0.0005f;
		f_sprint = Mathf.Clamp (f_sprint, 0, 0.25f);

		Vector3 gravity = transform.position;
		gravity.y = gravity.y + gravityForce;

		if (!b_landed) {
			if (!b_collision) {
				transform.position = gravity;
				this.transform.Translate (forward);
			}
		} else if (b_landed) {
			UpdateGroundMovement (leftHydra, rightHydra);
		}
	}
	
	void UpdateGroundMovement (SixenseInput.Controller leftHydra, SixenseInput.Controller rightHydra)
	{
		Vector3 movement = new Vector3 (leftHydra.JoystickX / 10, 0, leftHydra.JoystickY / 10);
		this.transform.Translate (movement);
	}
	
	float Map (float from, float to, float from2, float to2, float value)
	{
		if (value <= from2)
			return from;
		else if (value >= to2)
			return to;
		return (to - from) * ((value - from2) / (to2 - from2)) + from;
	}
}
