using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadMode : MonoBehaviour
{
	public Faction playerFaction;

	public void SpawnSquads()
	{
		foreach(Squad s in playerFaction.squads)
		{
			Instantiate(Resources.Load("SquadDisplay"));
		}
	}
}
