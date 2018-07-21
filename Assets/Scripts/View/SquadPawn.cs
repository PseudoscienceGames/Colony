using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SquadPawn : MonoBehaviour
{
	public Squad squad;
	public NavMeshAgent nav;

	public void SetDestination(Vector3 des)
	{
		if (nav == null)
			nav = GetComponent<NavMeshAgent>();
		nav.SetDestination(des);
		squad.destination = des;
	}

	private void Update()
	{
		squad.location = transform.position;
	}

}
