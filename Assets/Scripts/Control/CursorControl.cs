using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorControl : MonoBehaviour
{
	public List<Transform> selection = new List<Transform>();

	private void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if(Physics.Raycast(ray, out hit))
			{
				if (selection.Count == 0)
				{
					if (hit.transform.tag == "PlayerSquad")
					{
						if (Input.GetKey(KeyCode.LeftShift))
						{
							if (selection.Contains(hit.transform))
								selection.Remove(hit.transform);
							else
								selection.Add(hit.transform);
						}
						else
						{
							selection.Clear();
							selection.Add(hit.transform);
						}
					}
					else
						selection.Clear();
				}
				else
				{
					foreach (Transform t in selection)
					{
						t.GetComponent<SquadPawn>().SetDestination(hit.point);
					}
				}
			}
		}
	}
}
