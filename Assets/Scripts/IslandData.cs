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
	public int minIslandTileCount;
	public int maxIslandTileCount;
	public int currentTileCount;
	public int flattenTimes;
	public IslandMesh islandMesh;
	public BoundsInt bounds;
	public int tC;

	void Start()
	{
		islandMesh = GetComponent<IslandMesh>();
		GenData();
	}

	void GenData()
	{
		AddTiles();
		SetFlatConnections();
		AddCoast();
		PatchHoles();
		GenHeights();
		currentTileCount = tiles.Count;
		GetComponent<IslandMesh>().GenMesh();
		GetComponent<Spawner>().SpawnTrees();
	}

	void AddTiles()
	{
		List<Vector2Int> islandTiles = new List<Vector2Int>();
		List<Vector2Int> possibleTiles = new List<Vector2Int>();
		islandTiles.Add(Vector2Int.zero);
		possibleTiles.AddRange(Grid.FindAdjacentGridLocs(Vector2Int.zero));
		tC = Random.Range(minIslandTileCount, maxIslandTileCount);
		for (int t = 0; t < tC; t++)
		{
			Vector2Int gridLoc = possibleTiles[Random.Range(Mathf.RoundToInt(possibleTiles.Count / 2f), possibleTiles.Count)];
			if (gridLoc.x < bounds.xMin)
				bounds.xMin = gridLoc.x;
			if (gridLoc.x > bounds.xMax)
				bounds.xMax = gridLoc.x;
			if (gridLoc.y < bounds.yMin)
				bounds.yMin = gridLoc.y;
			if (gridLoc.y > bounds.yMax)
				bounds.yMin = gridLoc.y;
			islandTiles.Add(gridLoc);
			possibleTiles.Remove(gridLoc);
			foreach (Vector2Int v in Grid.FindAdjacentGridLocs(gridLoc))
			{
				if (!islandTiles.Contains(v) && !possibleTiles.Contains(v))
					possibleTiles.Add(v);
			}
		}
		bounds.xMin -= 2;
		bounds.xMax += 2;
		bounds.yMin -= 2;
		bounds.yMax += 2;
		foreach (Vector2Int v in islandTiles)
		{
			tiles.Add(v, new Tile(v));
		}
	}
	void PatchHoles()
	{
		List<Vector2Int> water = new List<Vector2Int>();
		for (int x = bounds.xMin - 1; x <= bounds.xMax + 1; x++)
		{
			for (int y = bounds.yMin - 1; y <= bounds.yMax + 1; y++)
			{
				if (!tiles.ContainsKey(new Vector2Int(x, y)))
					water.Add(new Vector2Int(x, y));
			}
		}
		List<List<Vector2Int>> bodies = new List<List<Vector2Int>>();
		int c = 0;
		while (water.Count > c)
		{
			List<Vector2Int> body = new List<Vector2Int>();
			List<Vector2Int> tilesToCheck = new List<Vector2Int>();
			tilesToCheck.Add(water[0]);
			while (tilesToCheck.Count > 0)
			{
				Vector2Int gridLoc = tilesToCheck[0];
				body.Add(gridLoc);
				tilesToCheck.RemoveAt(0);
				foreach (Vector2Int v in Grid.FindAdjacentGridLocs(gridLoc))
				{
					if (water.Contains(v) && !body.Contains(v) && !tilesToCheck.Contains(v))
						tilesToCheck.Add(v);
				}
			}
			c += body.Count;
			bodies.Add(body);
		}
		int biggestBodyIndex = 0;
		int biggestBodySize = 0;
		for (int b = 0; b < bodies.Count; b++)
		{
			if (bodies[b].Count > biggestBodySize)
			{
				biggestBodySize = bodies[b].Count;
				biggestBodyIndex = b;
			}
		}
		water = bodies[biggestBodyIndex];
		tiles.Clear();
		for (int x = bounds.xMin; x <= bounds.xMax; x++)
		{
			for (int y = bounds.yMin; y <= bounds.yMax; y++)
			{
				Vector2Int gridLoc = new Vector2Int(x, y);
				if (!water.Contains(gridLoc) && !tiles.ContainsKey(gridLoc))
					tiles.Add(gridLoc, new Tile(gridLoc));
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
		List<Vector2Int> coast = new List<Vector2Int>();
		foreach (Tile t in tiles.Values)
		{
			foreach (Vector2Int gridLoc in Grid.FindAdjacentGridLocs(t.gridLoc))
			{
				if (!tiles.ContainsKey(gridLoc) && !coast.Contains(gridLoc))
				{
					coast.Add(gridLoc);
				}
			}
		}
		foreach (Vector2Int gridLoc in coast)
		{
			tiles.Add(gridLoc, new Tile(gridLoc, 1));
		}
		foreach (Tile t in tiles.Values)
		{
			bool isCoast = false;
			foreach (Vector2Int gridLoc in Grid.FindAdjacentGridLocs(t.gridLoc))
			{
				if (!tiles.ContainsKey(gridLoc))
				{
					isCoast = true;
				}
			}
			if(isCoast)
			{
				t.coast = true;
				t.SetHeight(0);
			}
		}
		//SetConnections();
	}

	//void SetConnections()
	//{
	//	foreach(Tile t in tiles.Values)
	//	{
	//		t.connections.Clear();
	//		foreach(Vector2Int gridLoc in Grid.FindAdjacentGridLocs(t.gridLoc))
	//		{
	//			if (tiles.ContainsKey(gridLoc) && Mathf.Abs(tiles[gridLoc].height - t.height) <= 1)
	//				t.connections.Add(gridLoc);
	//		}
	//	}
	//}

	void GenHeights()
	{
		foreach (Tile t in tiles.Values)
		{
			if (t.connections.Count < 6)
			{
				t.SetHeight(0);
			}
			else
				t.SetHeight(Random.Range(10, peakHeight + 10));// - Grid.FindGridDistance(Vector2Int.zero, t.gridLoc));
		}
		//SetConnections();
		//Flatten();
	}
	void Flatten()
	{
		int times = 0;
		while (times < flattenTimes)
		{
			times++;
			foreach (Tile t in tiles.Values)
			{
				if (t.connections.Count < 3)
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
			//SetConnections();
		}
	}
}
