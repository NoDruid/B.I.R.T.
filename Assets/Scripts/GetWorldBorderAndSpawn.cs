using UnityEngine;
using System.Collections;

public class GetWorldBorderAndSpawn : MonoBehaviour {

	GameObject spawnOnThis;
	public Transform viech;
	Vector3 maxValues = Vector3.zero;
	Vector3 minValues = Vector3.zero;

	[Range(0,200)]
	public int maxViechers;


	// Use this for initialization
	void Start () {

		spawnOnThis = GameObject.Find ("SpawnArea");

		MeshFilter mshFilt = spawnOnThis.GetComponent<MeshFilter>();
		Mesh gridMesh = mshFilt.mesh;

		Bounds bounds = gridMesh.bounds;

		Vector3 scale = spawnOnThis.transform.localScale;

		maxValues = Vector3.Scale(bounds.max, scale);
		minValues = Vector3.Scale(bounds.min, scale);

		maxValues.x += spawnOnThis.transform.position.x;
		minValues.x += spawnOnThis.transform.position.x;
		
		maxValues.y += spawnOnThis.transform.position.y;
		minValues.y += spawnOnThis.transform.position.y;
		
		maxValues.z += spawnOnThis.transform.position.z;
		minValues.z += spawnOnThis.transform.position.z;



		int spawnedNumber = 0;

		for (int i=0; i<maxViechers; i++) {
			float x = 0.0f;
			float z = 0.0f; 
			bool spawned = false;
			
			
			RaycastHit hit;
			
			while(!spawned)
			{
				x = Random.Range(minValues.x,maxValues.x);
				z = Random.Range(minValues.z,maxValues.z);
				Physics.Raycast(new Vector3(x,2000,z),new Vector3(0,-1,0),out hit);
				if(hit.point.y < maxValues.y && hit.point.y > minValues.y)
				{
					spawned = true;
					//Debug.Log(hit.point.y+" at "+spawnedNumber);
				}
			}
			//Debug.Log(hit.point.y + " " +maxValues.y + " " + minValues.y);

				
				Vector3 startPosi = new Vector3(x,hit.point.y+10,z);
				Instantiate (viech, startPosi,Quaternion.identity);
				spawnedNumber += 1;




			//Debug.Log(spawnedNumber+" at "+(startPosi.y-5));

		}

		//Debug.Log (spawnedNumber);

	}
	
	// Update is called once per frame
	void Update () {

	
	}

	public Vector3 getMaxCoordinate()
	{
		return maxValues;
	}

	public Vector3 getMinCoordinate()
	{
		return minValues;
	}
}
