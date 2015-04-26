using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	SixenseInput.Controller leftHydra;
	SixenseInput.Controller rightHydra;

	CharacterController charControl;

	public GameObject cameras;
	public float f_speed = 0.0f;
	public bool b_keyboard = false;

	float f_gravity = 9.8f;
	float f_maxSpeed = 0.0f;
	float f_minSpeed = 0.0f;
	float f_sprintEnergy = 10.0f;
	bool b_sneaking = false;

	void Awake()
	{
		charControl = this.GetComponent<CharacterController>();
	}

	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		leftHydra = SixenseInput.Controllers [0];
		rightHydra = SixenseInput.Controllers [1];

		Vector3 moveVec = new Vector3(0,0,0);
		CalculateMovementSpeed(leftHydra, rightHydra);
		moveVec = (transform.forward * 100.0f) * f_speed;
		moveVec.y = -f_gravity;
		
		charControl.Move(moveVec * Time.deltaTime);
	}

	void FixedUpdate()
	{
		if(leftHydra != null)
		{
			RotateY (leftHydra, rightHydra);
		}
	}

	void CalculateMovementSpeed(SixenseInput.Controller leftHydra, SixenseInput.Controller rightHydra)
	{

		if (rightHydra.GetButtonDown (SixenseButtons.BUMPER) || Input.GetButtonDown("SneakToggle")) 
		{
			ToggleSneak();
		}

		if (rightHydra.GetButton (SixenseButtons.TRIGGER) || leftHydra.GetButton (SixenseButtons.TRIGGER) || Input.GetButton("Jump")) 
		{

			if(((rightHydra.GetButton (SixenseButtons.TRIGGER) && leftHydra.GetButton (SixenseButtons.TRIGGER)) || Input.GetButton("Sprint")) && f_sprintEnergy > 0.0f) 
			{
				f_maxSpeed = 0.5f;
				f_sprintEnergy -= Time.deltaTime;
				f_speed += Time.deltaTime * 1.5f;
				if(b_sneaking)
				{
					ToggleSneak();
				}
				//print ("SPRINT");
			}
			else if(b_sneaking)
			{

				f_maxSpeed = 0.15f;
				f_sprintEnergy += Time.deltaTime;
				f_speed += Time.deltaTime;
				//print ("SNEAK");
			}
			else if(rightHydra.Rotation.x < -0.4f && leftHydra.Rotation.x < -0.4f || Input.GetAxis("Vertical") < 0.0f)
			{
				f_minSpeed = -0.15f;
				f_speed -= Time.deltaTime;
			}
			else if(f_maxSpeed > 0.5f)
			{
				f_maxSpeed -= Time.deltaTime;
			}
			else
			{
				f_maxSpeed = 0.25f;
				f_sprintEnergy += Time.deltaTime/2;
				f_speed += Time.deltaTime;
				//print ("WALK");
			}
		}
		else
		{
			f_sprintEnergy += Time.deltaTime;
			f_speed -= Time.deltaTime * 2.0f;
			if(f_minSpeed < 0.0f)
			{
				f_minSpeed += Time.deltaTime;
			}
			else
			{
				f_minSpeed = 0.0f;
			}
			//print ("STAND");
		}
		
		Mathf.Clamp(f_sprintEnergy,0.0f,10.0f);
		f_speed = Mathf.Clamp(f_speed,f_minSpeed,f_maxSpeed);
	}

	void RotateY (SixenseInput.Controller leftHydra, SixenseInput.Controller rightHydra)
	{
		float inputRotationY = 0.0f;

		if (b_keyboard) 
		{
			inputRotationY = Input.GetAxis ("Horizontal") * 0.5f;
		}
		else 
		{
			inputRotationY = (rightHydra.Rotation.y + leftHydra.Rotation.y) / 2.0f;
		}
		if(inputRotationY < -0.03f || inputRotationY > 0.03f)
		{
			float yRot = transform.eulerAngles.y + (inputRotationY * 4);
			transform.rotation = Quaternion.Euler (new Vector3(0,yRot,0));
		}
	}

	void ToggleSneak()
	{
		if(!b_sneaking)
		{
			cameras.transform.localPosition = new Vector3(0,-0.8f,0);
		}
		else
		{
			cameras.transform.localPosition = new Vector3(0,3.7f,0);
		}
		b_sneaking = !b_sneaking;
	}
}
