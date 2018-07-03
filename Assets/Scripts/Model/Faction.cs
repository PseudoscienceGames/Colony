using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Faction
{
	public string factionName;
	public int teamIndex;
	public Color color;

	public List<Vector2Int> territory = new List<Vector2Int>();
	public List<Squad> squads = new List<Squad>();
	public List<Character> chars = new List<Character>();
}
