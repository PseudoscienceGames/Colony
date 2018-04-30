using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : MonoBehaviour
{
	public GameObject postPrefab;
	public Transform post1;
	public Transform post2;
	public List<Vector3> posts = new List<Vector3>();
	public float postDistance;

	private void Start()
	{
		Debug.DrawLine(post1.position, post2.position, Color.red, Mathf.Infinity);
		float distance = Vector3.Distance(post1.position, post2.position);
		int numberOfPosts = Mathf.RoundToInt(distance / postDistance);
		float actualDistance = distance / numberOfPosts;
		for(int i = 1; i < numberOfPosts; i++)
		{
			Vector3 pos = post1.position + (post2.position - post1.position).normalized * i * actualDistance;
			Debug.DrawLine(pos, pos + Vector3.up, Color.red, Mathf.Infinity);
			Instantiate(postPrefab, pos, Quaternion.identity);
		}

	}
}
