using UnityEngine;
using System.Collections;

public class TreeScript : MonoBehaviour {

	GameObject spawnOnThis;
	Vector3 maxValues = Vector3.zero;
	Vector3 minValues = Vector3.zero;
	
	[Range(0,10000)]
	public int maxTrees;

	public GameObject[] trees;

	void Start () 
	{
		spawnOnThis = GameObject.Find ("SpawnArea");
		
		MeshFilter mshFilt = spawnOnThis.GetComponent<MeshFilter>();
		Mesh gridMesh = mshFilt.mesh;
		
		Bounds bounds = gridMesh.bounds;
		
		Vector3 scale = spawnOnThis.transform.localScale;
		
		maxValues = Vector3.Scale(bounds.max, scale);
		minValues = Vector3.Scale(bounds.min, scale);
		Debug.Log ("Min" + minValues);
		maxValues.x += spawnOnThis.transform.position.x;
		minValues.x += spawnOnThis.transform.position.x;

		maxValues.y += spawnOnThis.transform.position.y;
		minValues.y += spawnOnThis.transform.position.y;

		maxValues.z += spawnOnThis.transform.position.z;
		minValues.z += spawnOnThis.transform.position.z;
		
		int spawnedNumber = 0;
		
		for (int i=0; i<maxTrees; i++) {
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
			

			
			Vector3 startPosi = new Vector3(x,hit.point.y,z);


			GameObject baum = trees[Random.Range(0, trees.Length-1)];
			Instantiate (baum, startPosi,Quaternion.identity);
			spawnedNumber += 1;
			
			
			
			
			//Debug.Log(spawnedNumber+" at "+(startPosi.y-5));
			
		}
		
		//Debug.Log (spawnedNumber);
		
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


