using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceTool : MonoBehaviour
{
	public GameObject fencePrefab;
	public Fence fence;

	private void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
				fence = (Instantiate(fencePrefab, hit.point, Quaternion.identity) as GameObject).GetComponent<Fence>();
		}
		if (Input.GetMouseButton(0) && fence != null)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
				fence.post2.position = hit.point;
		}
		if (Input.GetMouseButtonUp(0) && fence != null)
		{
			fence.enabled = true;
			fence = null;
		}
	}
}
