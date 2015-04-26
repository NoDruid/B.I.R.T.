using UnityEngine;
using System.Collections;

public class ComeHome : MonoBehaviour
{
	void OnTriggerEnter (Collider info)
	{
		if (info.transform.parent != null) 
		{
			if (info.transform.parent.Equals (this.transform.parent)) 
			{
				Debug.Log ("enterisChild");
				if (info.name == "Hase(Clone)") 
				{
					Debug.Log ("isHase");
					info.GetComponent<Prey> ().comeHome ();
				}
			}
		}
	}
}


