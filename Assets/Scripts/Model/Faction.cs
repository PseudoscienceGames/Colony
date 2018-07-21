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

	public void GenChars()
	{
		squads.Add(new Squad());
		for (int i = 0; i < 3; i++)
		{
			chars.Add(new Character());
			chars[i].GenStartingStats();
			squads[0].chars.Add(i);
		}
	}
}
