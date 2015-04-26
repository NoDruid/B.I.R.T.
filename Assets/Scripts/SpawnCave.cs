using UnityEngine;
using System.Collections;

public class SpawnCave : MonoBehaviour
{
	[Range(1,30)]
	public int maxAnimals;
	int currentAnimals = 0;
	public GameObject animal;
	float spawnTimer = 0.0f;
	[Range(1,100)]
	public float timeToSpawn;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (spawnTimer < 0.0f) {
			if (currentAnimals < maxAnimals) {
				Spawn ();
				spawnTimer = timeToSpawn + Random.Range (-timeToSpawn / 5.0f, timeToSpawn / 5.0f);
			}
		} else {
			spawnTimer -= Time.deltaTime;
		}
	}

	void Spawn ()
	{
		GameObject clone;
		clone = Instantiate (animal, this.transform.position, Quaternion.identity) as GameObject;
		clone.transform.SetParent (this.transform);
		currentAnimals += 1;
	}


}
