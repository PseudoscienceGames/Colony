using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class IslandData : MonoBehaviour
{
	public Dictionary<Vector2, Tile> tiles = new Dictionary<Vector2, Tile>();
	public List<Vector2> peaks = new List<Vector2>();
	public int mapGridRadius;
	public int peakCount;
	public int peakHeight;
	public int tileCount;

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
		List<Vector2> heights = new List<Vector2>();
		for (int i = 0; i < peakCount; i++)
		{
			Vector2 peak = new Vector2(Random.Range(-mapGridRadius, mapGridRadius) / 2, Random.Range(-mapGridRadius, mapGridRadius) / 2);
			//peaks.Add(peak);
			heights.Add(peak);
			tiles.Add(peak, new Tile(peak, peakHeight));
		}
		while (tiles.Count < tileCount)
		{
			Vector2 tile = Grid.FindAdjacentGridLocs(heights[Random.Range(0, heights.Count)])[Random.Range(0, 6)];
			if (!heights.Contains(tile))
			{
				tiles.Add(tile, new Tile(tile));
				tiles[tile].SetHeight((int)(((float)(tileCount - heights.Count) / (float)tileCount) * peakHeight));
				heights.Add(tile);
			}
		}
		//tiles.Add(Vector2.zero, new Tile(Vector2.zero, 0));
		//for (int fRadius = 1; fRadius <= mapGridRadius; fRadius++)
		//{
		//	//Set initial hex grid location
		//	Vector2 gridLoc = new Vector2(fRadius, -fRadius);

		//	int dir = 2;
		//	//Find data for each hex in the ring (each ring has 6 more hexes than the last)
		//	for (int fHex = 0; fHex < 6 * fRadius; fHex++)
		//	{
		//		tiles.Add(gridLoc, new Tile(gridLoc, 0));
		//		//Finds next hex in ring
		//		gridLoc = Grid.MoveTo(gridLoc, dir);
		//		if (gridLoc.x == 0 || gridLoc.y == 0 || gridLoc.x == -gridLoc.y)
		//		{
		//			dir++;
		//		}
		//	}
		//}
	}

	void GenHeights()
	{
		//for (int i = 0; i < peakCount; i++)
		//{
		//	Vector2 peak = new List<Vector2>(tiles.Keys)[Random.Range(0, tiles.Count)];
		//	heights.Add(peak);
		//	tiles[peak].SetHeight(peakHeight);
		//}
		//while (heights.Count < tiles.Count)
		//{
		//	Vector2 tile = Grid.FindAdjacentGridLocs(heights[Random.Range(0, heights.Count)])[Random.Range(0, 6)];
		//	if (!heights.Contains(tile) && tiles.ContainsKey(tile))
		//	{
		//		tiles[tile].SetHeight((int)(((float)(tiles.Count - heights.Count) / (float)tiles.Count) * peakHeight));
		//		heights.Add(tile);
		//	}
		//}
		GetComponent<IslandMesh>().GenMesh();
	}
}
