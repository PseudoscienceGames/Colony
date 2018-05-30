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
		StartCoroutine(AddTiles());
		//GenHeights();
		
	}

	IEnumerator AddTiles()
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
				//islandMesh.GenMesh();
				//yield return null;
			}
			
		}
		islandMesh.GenMesh();
		Debug.Log(Time.timeSinceLevelLoad);
		yield return null;
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
