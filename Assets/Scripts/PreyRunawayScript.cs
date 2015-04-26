using UnityEngine;
using System.Collections;

public class PreyRunawayScript : MonoBehaviour
{
	Prey preyScript;

	void Start ()
	{

	}

	void OnTriggerEnter (Collider info)
	{
		if (info.name == "Player") {
			preyScript = this.GetComponent<Prey> ();
			
			preyScript.setScared (true);
		}
	}

	void OnTriggerStay (Collider info)
	{
		if (info.name == "Player") {
			preyScript.playerPosition = info.transform.position;
		}
	}
}
