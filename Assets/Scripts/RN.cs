using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RN : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		foreach (Vector3 n in GetComponent<MeshFilter>().mesh.normals)
			Debug.Log("1" + n);
		GetComponent<MeshFilter>().mesh.RecalculateNormals();
		foreach (Vector3 n in GetComponent<MeshFilter>().mesh.normals)
			Debug.Log("2" + n);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
