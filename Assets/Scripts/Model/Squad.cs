using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class Squad
{
	public int faction;
	public List<int> chars = new List<int>();
	public Vector3 location;
	public Vector3 destination;
	private SquadPawn view;
}
