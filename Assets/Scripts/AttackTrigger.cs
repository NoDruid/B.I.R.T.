using UnityEngine;
using System.Collections;

public class AttackTrigger : MonoBehaviour
{

	void OnTriggerStay (Collider info)
	{
		if (info.name == "Player") {
			//Debug.Log (info.name);
			Attack attack = (Attack)info.GetComponent (typeof(Attack));
			attack.InAttackRange (this.gameObject);
		}
	}
}
