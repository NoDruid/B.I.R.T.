using UnityEngine;
using System.Collections;

public class Prey : MonoBehaviour
{
	public Vector3 playerPosition;
	GameObject border;
	Transform cave;
	bool changeDirection = true;
	Vector3 walking = new Vector3 (0, 0, 0.2f);
	float walkForinSeconds = 0.0f;
	bool scared = false;
	bool wasScared = false;
	bool returnToCave = false;
	
	//bool eagleSight = false;

	void Start ()
	{
		cave = this.transform.parent;
		border = cave.FindChild ("border").gameObject;
	}

	void Update ()
	{
		walkForinSeconds -= Time.deltaTime;

		if (walkForinSeconds < 0.0f) {
			//Debug.Log("returnToCave: "+returnToCave+" wasScared: "+wasScared);
			if (returnToCave && !wasScared) {
				//Debug.Log ("returnToCave");
				WalkCave ();
			} else if (wasScared) {
				Debug.Log ("umguck");
				this.transform.Rotate (new Vector3 (0, 180, 0));
				walkForinSeconds = 1.0f;
				wasScared = false;
				scared = false;
			} else {
				//Debug.Log ("Walk Random");
				WalkRandom ();
				changeDirection = true;
				walkForinSeconds = 2.0f;
			}
		}

		if (scared) {
			scared = false;
			ScaredOfPlayer (playerPosition);
			walkForinSeconds = 5.0f;
			wasScared = true;
		} 




		//Debug.DrawRay(this.transform.position, cave.position - this.transform.position);
	}

	void FixedUpdate()
	{
		transform.Translate (walking);
	}

	void OnCollisionEnter (Collision info)
	{
		if (info.collider.gameObject.Equals (border)) {
			Debug.Log ("Border Trigger Enter");

		}
	}

	void OnTriggerExit (Collider info)
	{
		if (info.gameObject.Equals (border)) {
			//Debug.Log ("Border Collision");
			if (!wasScared) {
				walkForinSeconds = 2.0f;
				WalkCave ();
			} 
			else
			{
				returnToCave = true;
			}
		}
	}

	void WalkRandom ()
	{
		changeDirection = false;
		this.transform.rotation = Quaternion.Euler (new Vector3 (0, Random.Range (0, 360), 0));
	}

	void WalkCave ()
	{
		Vector3 relativePos = cave.position - transform.position;
		relativePos.y = 0.0f;
		Quaternion rotation = Quaternion.LookRotation (relativePos);
		transform.rotation = rotation;
		//Debug.Log (this.transform.rotation);
	}

	public void ScaredOfPlayer (Vector3 playerPosition)
	{
		Vector3 relativePos = transform.position - playerPosition;
		relativePos.y = 0.0f;
		Quaternion rotation = Quaternion.LookRotation (relativePos);
		transform.rotation = rotation;

	}

	public void setScared (bool scaredStatus)
	{
		//Debug.Log ("setscared");
		scared = scaredStatus;
	}

	public void comeHome()
	{
		Debug.Log ("comeHome");
		returnToCave = false;
		walkForinSeconds = 2.0f;
		WalkCave();
	}
}
