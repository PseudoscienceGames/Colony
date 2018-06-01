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
	public int currentTileCount;
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
		SetConnections();
		GenHeights();
		GetComponent<IslandMesh>().GenMesh();
	}

	 void AddTiles()
	{
		List<Vector2> heights = new List<Vector2>();
		for (int i = 0; i < peakCount; i++)
		{
			Vector2 peak = Vector2.zero;
			//peaks.Add(peak);
			heights.Add(peak);
			tiles.Add(peak, new Tile(peak, 0));
		}
		while (tiles.Count < tileCount)
		{
			currentTileCount = tiles.Count;
			Vector2 tile = Grid.FindAdjacentGridLocs(heights[Random.Range(0, heights.Count)])[Random.Range(0, 6)];
			if (!heights.Contains(tile))
			{
				tiles.Add(tile, new Tile(tile));
				tiles[tile].SetHeight((int)(((float)(tileCount - heights.Count) / (float)(tileCount * 2)) * peakHeight));
				heights.Add(tile);
			}
		}
	}

	void SetConnections()
	{
		foreach(Tile t in tiles.Values)
		{
			t.connections.Clear();
			foreach(Vector2 gridLoc in Grid.FindAdjacentGridLocs(t.gridLoc))
			{
				if (tiles.ContainsKey(gridLoc) && Mathf.Abs(tiles[gridLoc].height - t.height) <= 1)
					t.connections.Add(gridLoc);
			}
		}
	}

	void GenHeights()
	{
		//foreach tile
		foreach(Tile t in tiles.Values)
		{
			if (t.connections.Count < 6)
			{
				t.SetHeight(0);
				t.coast = true;
			}
			else
				t.SetHeight(Random.Range(1, peakHeight));
		}
		SetConnections();
		StartCoroutine(Flatten());
		//if coast set height to 1
		//else set height to random between 1 and max height
		//while any tile cannot be reached by every other tile
		//pick random tile and move height closer to average of adjacent tiles
	}
	IEnumerator Flatten()
	{
		bool done = false;
		while(!done)
		{
			done = true;
			foreach(Tile t in tiles.Values)
			{
				if(t.connections.Count < 6 && !t.coast)
				{
					done = false;
					int heightSum = 0;
					int count = 0;
					foreach (Vector2 gridLoc in Grid.FindAdjacentGridLocs(t.gridLoc))
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
					if(t.height < h)
						t.SetHeight(t.height + 1);
				}
			}
			GetComponent<IslandMesh>().GenMesh();
			yield return null;
		}
	}
}
