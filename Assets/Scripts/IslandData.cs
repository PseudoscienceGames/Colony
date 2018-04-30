using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class IslandData : MonoBehaviour
{
	public Dictionary<Vector2, Tile> tiles = new Dictionary<Vector2, Tile>();
	public int mapGridRadius;
	public int heightMin;
	public int heightMax;
	public int heightSections;

	public static IslandData Instance;
	void Awake() { Instance = this; }

	void Start()
	{
		GenData();
	}

	void GenData()
	{
		AddTiles();
		GenHeights();
		
	}

	void AddTiles()
	{
		tiles.Add(Vector2.zero, new Tile(Vector2.zero, 0));
		for (int fRadius = 1; fRadius <= mapGridRadius; fRadius++)
		{
			//Set initial hex grid location
			Vector2 gridLoc = new Vector2(fRadius, -fRadius);

			int dir = 2;
			//Find data for each hex in the ring (each ring has 6 more hexes than the last)
			for (int fHex = 0; fHex < 6 * fRadius; fHex++)
			{
				tiles.Add(gridLoc, new Tile(gridLoc, 0));
				//Finds next hex in ring
				gridLoc = Grid.MoveTo(gridLoc, dir);
				if (gridLoc.x == 0 || gridLoc.y == 0 || gridLoc.x == -gridLoc.y)
				{
					dir++;
				}
			}
		}
	}

	void GenHeights()
	{
		foreach (Tile t in tiles.Values)
			t.SetHeight(Random.Range(heightMin, heightMax));
		//Dictionary<Tile, float> possibleTiles = new Dictionary<Tile, float>();
		//List<Tile> assignedTiles = new List<Tile>();
		//for(int i = 1; i <= heightSections; i++)
		//{
		//	Tile t = tiles[tiles.Keys.ToList<Vector2>()[Random.Range(0, tiles.Count)]];
		//	t.SetHeight(i);
		//	assignedTiles.Add(t);
		//	foreach (Vector2 g in Grid.FindAdjacentGridLocs(t.gridLoc))
		//	{
		//		if(tiles.ContainsKey(g) && !possibleTiles.ContainsKey(tiles[g]) && !assignedTiles.Contains(tiles[g]))
		//			possibleTiles.Add(tiles[g], t.height);
		//	}
		//	Debug.Log(t.gridLoc + " " + i);
		//}
		//while(possibleTiles.Count > 0)
		//{
		//	Tile t = possibleTiles.Keys.ToList<Tile>()[Random.Range(0, possibleTiles.Count)];
		//	t.SetHeight(possibleTiles[t]);
		//	possibleTiles.Remove(t);
		//	assignedTiles.Add(t);
		//	foreach (Vector2 g in Grid.FindAdjacentGridLocs(t.gridLoc))
		//	{
		//		if (tiles.ContainsKey(g) && !assignedTiles.Contains(tiles[g]) && !possibleTiles.ContainsKey(tiles[g]))
		//			possibleTiles.Add(tiles[g], t.height);
		//	}
		//}
		GetComponent<IslandMesh>().GenMesh();
	}
}
