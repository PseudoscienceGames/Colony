using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionPawn : MonoBehaviour
{
	public Faction faction;

	private void Start()
	{
		faction.GenChars();
		GameObject s = Instantiate(Resources.Load("Squad")) as GameObject;
		s.GetComponent<SquadPawn>().squad = faction.squads[0];
		if (faction.teamIndex == 0)
		{
			GameObject.Find("ModeController").GetComponent<ModeControl>().playerFaction = faction;
			GameObject.Find("SquadMode").GetComponent<SquadMode>().playerFaction = faction;
		}
	}
}
