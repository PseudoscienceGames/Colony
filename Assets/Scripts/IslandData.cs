using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class IslandData : MonoBehaviour
{
	public Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();
	public List<Vector2Int> peaks = new List<Vector2Int>();
	public int mapGridRadius;
	public int peakCount;
	public int peakHeight;
	public int tileCount;
	public int currentTileCount;
	public int flattenTimes;
	public IslandMesh islandMesh;

	public static IslandData Instance;
	void Awake() { Instance = this; }

	void Start()
	{
		islandMesh = GetComponent<IslandMesh>();
		GenData();
	}

	void GenData()
	{
		AddTiles();
		SetFlatConnections();
		GenHeights();
		AddCoast();
		currentTileCount = tiles.Count;
		GetComponent<IslandMesh>().GenMesh();
		Spawner.Instance.SpawnTrees();
	}

	 void AddTiles()
	{
		List<Vector2Int> heights = new List<Vector2Int>();
		for (int i = 0; i < peakCount; i++)
		{
			Vector2Int peak = Vector2Int.zero;
			heights.Add(peak);
			tiles.Add(peak, new Tile(peak, 0));
		}
		while (tiles.Count < tileCount)
		{
			currentTileCount = tiles.Count;
			Vector2Int tile = Grid.FindAdjacentGridLocs(heights[Random.Range((int)(heights.Count * 0.66666f), heights.Count)])[Random.Range(0, 6)];
			if (!heights.Contains(tile))
			{
				tiles.Add(tile, new Tile(tile));
				heights.Add(tile);
			}
		}
	}

	void SetFlatConnections()
	{
		foreach (Tile t in tiles.Values)
		{
			t.connections.Clear();
			foreach (Vector2Int gridLoc in Grid.FindAdjacentGridLocs(t.gridLoc))
			{
				if (tiles.ContainsKey(gridLoc))
					t.connections.Add(gridLoc);
			}
		}
	}

	void AddCoast()
	{
		SetFlatConnections();
		List<Vector2Int> coast = new List<Vector2Int>();
		foreach (Tile t in tiles.Values)
		{
			foreach (Vector2Int gridLoc in Grid.FindAdjacentGridLocs(t.gridLoc))
			{
				if (!tiles.ContainsKey(gridLoc) && !coast.Contains(gridLoc) && t.height < 2)
				{
					coast.Add(gridLoc);
					t.coast = true;
					t.SetHeight(0);
				}
			}
		}
		foreach(Vector2Int gridLoc in coast)
		{
			tiles.Add(gridLoc, new Tile(gridLoc, -1));
			tiles[gridLoc].coast = true;
			tiles[gridLoc].SetHeight(-1);
		}
		SetConnections();
	}

	void SetConnections()
	{
		foreach(Tile t in tiles.Values)
		{
			t.connections.Clear();
			foreach(Vector2Int gridLoc in Grid.FindAdjacentGridLocs(t.gridLoc))
			{
				if (tiles.ContainsKey(gridLoc) && Mathf.Abs(tiles[gridLoc].height - t.height) <= 1)
					t.connections.Add(gridLoc);
			}
		}
	}

	void GenHeights()
	{
		foreach(Tile t in tiles.Values)
		{
			if (t.connections.Count < 6)
			{
				t.SetHeight(0);
			}
			else
				t.SetHeight(Random.Range(1, peakHeight) - Grid.FindGridDistance(Vector2Int.zero, t.gridLoc));
		}
		SetConnections();
		Flatten();
	}
	void Flatten()
	{
		int times = 0;
		while(times < flattenTimes)
		{
			times++;
			foreach(Tile t in tiles.Values)
			{
				if(t.connections.Count < 3)
				{

					int heightSum = 0;
					int count = 0;
					foreach (Vector2Int gridLoc in Grid.FindAdjacentGridLocs(t.gridLoc))
					{
						if (tiles.ContainsKey(gridLoc))
						{
							heightSum += (int)tiles[gridLoc].height;
							count++;
						}
					}
					int h = heightSum / count;
					if (t.height > h)
						t.SetHeight(t.height - 1);
				}
			}
			SetConnections();
		}
	}
}
