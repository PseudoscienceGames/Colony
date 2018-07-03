using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class Squad
{
	public int faction;
	public Vector3 target;
	public NavMeshAgent nav;
	public List<int> chars = new List<int>();
}
