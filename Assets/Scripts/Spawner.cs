using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public int treeCount;
	public GameObject pine;
	public GameObject palm;
	public GameObject blob;
	IslandData data;

	public void SpawnTrees()
	{
		data = GetComponent<IslandData>();
		for (int i = 0; i < treeCount; i++)
		{
			Tile t = new List<Tile>(data.tiles.Values)[Random.Range(0, data.tiles.Count)];
			if (t.landType == 0)
			{
				Instantiate(palm, t.verts[6], Quaternion.Euler(0, Random.Range(0, 360), 0));
			}
			if (t.landType == 1)
			{
				Instantiate(blob, t.verts[6], Quaternion.Euler(0, Random.Range(0, 360), 0));
			}
			if (t.landType == 2)
			{
				Instantiate(pine, t.verts[6], Quaternion.Euler(0, Random.Range(0, 360), 0));
			}

		}
	}
}
