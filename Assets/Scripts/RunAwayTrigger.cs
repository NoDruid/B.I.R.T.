using UnityEngine;
using System.Collections;

public class RunAwayTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider info)
	{
		if (info.name == "Player") {
			//Debug.Log (info.name);
			Attack attack = (Attack) info.GetComponent(typeof(Attack));
			attack.InAttackRange(this.gameObject);
		}
	}
}
