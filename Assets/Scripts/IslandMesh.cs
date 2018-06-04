using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class IslandMesh : MonoBehaviour
{
	private List<Vector3> verts = new List<Vector3>();
	private List<int> tris = new List<int>();
	private List<Vector2> uvs = new List<Vector2>();
	private int vertNumber = 0;
	public bool addNoise;
	public float noiseScale;

	public List<Vector2> uvOffsets = new List<Vector2>();

	IslandData data;

	public void GenMesh()
	{
		data = GetComponent<IslandData>();
		verts.Clear();
		tris.Clear();
		uvs.Clear();
		vertNumber = 0;

		AddTiles();
		if(verts.Count != tris.Count || verts.Count != uvs.Count)
		{
			Debug.Log(verts.Count + " " + tris.Count + " " + uvs.Count);
			Debug.Break();
		}
		//CollapseDoubles();
		//if (addNoise)
		//	AddNoise();
		//ExpandDoubles();
		//CollapseDoubles();
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		mesh.Clear();
		mesh.vertices = verts.ToArray();
		mesh.triangles = tris.ToArray();
		mesh.uv = uvs.ToArray();
		mesh.RecalculateNormals();
		GetComponent<MeshCollider>().sharedMesh = mesh;
		Debug.Log(verts.Count);
	}

	void AddTiles()
	{
		foreach (Tile tile in GetComponent<IslandData>().tiles.Values)
		{
			Vector3 center = Grid.GridToWorld(tile.gridLoc, tile.height);
			Vector3 avg = Vector3.zero;
			for (int i = 0; i <= 5; i++)
			{
				Vector3 loc = FindOffset(tile, center + (Quaternion.Euler(0, (60 * i), 0) * Vector3.forward * Grid.hexRadius), i);
				tile.verts.Add(loc);
				avg += loc;
			}
			tile.verts.Add(avg / 6f);
		}
		foreach (Tile tile in GetComponent<IslandData>().tiles.Values)
		{
			AddTop(tile);
			AddSide(tile);
		}
	}
	void AddTop(Tile tile)
	{
		for (int i = 0; i <= 5; i++)
		{
			verts.Add(tile.verts[6]);
			verts.Add(tile.verts[i]);
			verts.Add(tile.verts[Grid.MoveDirFix(i + 1)]);
			tris.Add(vertNumber);
			tris.Add(vertNumber + 1);
			tris.Add(vertNumber + 2);
			vertNumber += 3;
		}
		//for(int i = 0; i < 18; i++)
		uvs.Add((new Vector2(0.5f, 0.5f) / 4f) + uvOffsets[tile.landType]);
		uvs.Add((new Vector2(0.5f, 0.1f) / 4f) + uvOffsets[tile.landType]);
		uvs.Add((new Vector2(0.9f, 0.1f) / 4f) + uvOffsets[tile.landType]);
		uvs.Add((new Vector2(0.5f, 0.5f) / 4f) + uvOffsets[tile.landType]);
		uvs.Add((new Vector2(0.9f, 0.1f) / 4f) + uvOffsets[tile.landType]);
		uvs.Add((new Vector2(0.9f, 0.9f) / 4f) + uvOffsets[tile.landType]);
		uvs.Add((new Vector2(0.5f, 0.5f) / 4f) + uvOffsets[tile.landType]);
		uvs.Add((new Vector2(0.9f, 0.9f) / 4f) + uvOffsets[tile.landType]);
		uvs.Add((new Vector2(0.5f, 0.9f) / 4f) + uvOffsets[tile.landType]);
		uvs.Add((new Vector2(0.5f, 0.5f) / 4f) + uvOffsets[tile.landType]);
		uvs.Add((new Vector2(0.5f, 0.9f) / 4f) + uvOffsets[tile.landType]);
		uvs.Add((new Vector2(0.1f, 0.9f) / 4f) + uvOffsets[tile.landType]);
		uvs.Add((new Vector2(0.5f, 0.5f) / 4f) + uvOffsets[tile.landType]);
		uvs.Add((new Vector2(0.1f, 0.9f) / 4f) + uvOffsets[tile.landType]);
		uvs.Add((new Vector2(0.1f, 0.1f) / 4f) + uvOffsets[tile.landType]);
		uvs.Add((new Vector2(0.5f, 0.5f) / 4f) + uvOffsets[tile.landType]);
		uvs.Add((new Vector2(0.1f, 0.1f) / 4f) + uvOffsets[tile.landType]);
		uvs.Add((new Vector2(0.5f, 0.1f) / 4f) + uvOffsets[tile.landType]);
	}
	void AddSide(Tile tile)
	{
		for (int i = 0; i <= 5; i++)
		{
			if (!IslandData.Instance.tiles.ContainsKey(Grid.MoveTo(tile.gridLoc, i)) ||
				IslandData.Instance.tiles[Grid.MoveTo(tile.gridLoc, i)].height < tile.height)
			{
				Vector3 vertex1WorldLoc = tile.verts[i];
				Vector3 vertex2WorldLoc = tile.verts[Grid.MoveDirFix(i - 1)];

				Vector3 vertex3WorldLoc = tile.verts[Grid.MoveDirFix(i - 1)];
				Vector3 vertex4WorldLoc = tile.verts[i];

				if (!IslandData.Instance.tiles.ContainsKey(Grid.MoveTo(tile.gridLoc, i)))
				{
					vertex3WorldLoc.y -= Grid.tileHeight;
					vertex4WorldLoc.y -= Grid.tileHeight;
				}
				else
				{
					Tile tile2 = IslandData.Instance.tiles[Grid.MoveTo(tile.gridLoc, i)];
					vertex3WorldLoc = tile2.verts[Grid.MoveDirFix(i + 3)];
					vertex4WorldLoc = tile2.verts[Grid.MoveDirFix(i + 2)];
				}

				verts.Add(vertex1WorldLoc);
				verts.Add(vertex2WorldLoc);
				verts.Add(vertex4WorldLoc);
				verts.Add(vertex2WorldLoc);
				verts.Add(vertex3WorldLoc);
				verts.Add(vertex4WorldLoc);

				tris.Add(vertNumber);
				tris.Add(vertNumber + 1);
				tris.Add(vertNumber + 2);
				tris.Add(vertNumber + 3);
				tris.Add(vertNumber + 4);
				tris.Add(vertNumber + 5);
				vertNumber += 6;

				int offset = 3;
				if (tile.landType < 3)
					offset = 2;

				uvs.Add((new Vector2(0.1f, 0.9f) / 4f) + uvOffsets[offset]);
				uvs.Add((new Vector2(0.9f, 0.9f) / 4f) + uvOffsets[offset]);
				uvs.Add((new Vector2(0.1f, 0.1f) / 4f) + uvOffsets[offset]);

				uvs.Add((new Vector2(0.9f, 0.9f) / 4f) + uvOffsets[offset]);
				uvs.Add((new Vector2(0.9f, 0.1f) / 4f) + uvOffsets[offset]);
				uvs.Add((new Vector2(0.1f, 0.1f) / 4f) + uvOffsets[offset]);
				//}
			}
		}
	}
	Vector3 FindOffset(Tile tile, Vector3 pos, int vertDir)
	{
		vertDir = Grid.MoveDirFix(vertDir);
		Tile otherTile1 = null;
		Tile otherTile2 = null;
		if (data.tiles.ContainsKey(Grid.MoveTo(tile.gridLoc, vertDir)))
			otherTile1 = data.tiles[Grid.MoveTo(tile.gridLoc, vertDir)];
		if (data.tiles.ContainsKey(Grid.MoveTo(tile.gridLoc, vertDir + 1)))
			otherTile2 = data.tiles[Grid.MoveTo(tile.gridLoc, vertDir + 1)];


		if (otherTile1 == null && otherTile2 == null)
			pos = pos - (pos - Grid.GridToWorld(tile.gridLoc, tile.height)) / 4f;
		else if (otherTile1 == null && otherTile2 != null)
			pos = pos - (pos - Grid.GridToWorld(Grid.MoveTo(tile.gridLoc, vertDir), tile.height)) / 4f;
		else if (otherTile2 == null && otherTile1 != null)
			pos = pos - (pos - Grid.GridToWorld(Grid.MoveTo(tile.gridLoc, vertDir + 1), tile.height)) / 4f;
		else if(otherTile1.height == otherTile2.height && otherTile2.height != tile.height)
			pos = pos - (pos - Grid.GridToWorld(tile.gridLoc, tile.height)) / 4f;
		else if(tile.height == otherTile1.height && tile.height != otherTile2.height)
			pos = pos - (pos - Grid.GridToWorld(otherTile2.gridLoc, otherTile2.height)) / 4f;
		else if (tile.height == otherTile2.height && tile.height != otherTile1.height)
			pos = pos - (pos - Grid.GridToWorld(otherTile1.gridLoc, otherTile1.height)) / 4f;
		pos.y = tile.height * Grid.tileHeight;
		if (otherTile1 != null && otherTile2 != null &&
			((tile.height - 1 == otherTile1.height  && tile.height - 1 == otherTile2.height) ||
			(tile.height - 1 == otherTile1.height && tile.height == otherTile2.height) ||
			(tile.height == otherTile1.height && tile.height - 1 == otherTile2.height) ||
			(tile.height > otherTile1.height + 1 && tile.height > otherTile2.height + 1)))
			pos.y = (tile.height - .5f) * Grid.tileHeight;
		if (otherTile1 != null && otherTile2 != null &&
			((tile.height + 1 == otherTile1.height && tile.height + 1 == otherTile2.height) ||
			(tile.height + 1 == otherTile1.height && tile.height == otherTile2.height) ||
			(tile.height == otherTile1.height && tile.height + 1 == otherTile2.height)))
			pos.y = (tile.height + .5f) * Grid.tileHeight;
		if(otherTile1 != null && otherTile2 != null && 
			((tile.height - 1 == otherTile1.height && otherTile2.height < otherTile1.height) ||
			(tile.height - 1 == otherTile2.height && otherTile1.height < otherTile2.height)))
			pos.y = (tile.height - 1f) * Grid.tileHeight;
		if (otherTile1 != null && otherTile2 != null &&
			((tile.height + 1 == otherTile1.height && otherTile2.height > otherTile1.height) ||
			(tile.height + 1 == otherTile2.height && otherTile1.height > otherTile2.height)))
			pos.y = (tile.height + 1f) * Grid.tileHeight;
		if (otherTile1 == null || otherTile2 == null)
			pos.y = (tile.height - 1f) * Grid.tileHeight;
		return pos;
	}
	void AddNoise()
	{
		for(int i = 0; i < verts.Count; i++)
		{
			Vector3 noise = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
			noise *= noiseScale;
			verts[i] += noise;
		}
		
	}
	void CollapseDoubles()
	{
		List<Vector3> newVerts = new List<Vector3>();
		List<int> newTris = new List<int>();
		List<Vector2> newUVs = new List<Vector2>();
		foreach (int tri in tris)
		{
			bool add = true;
			foreach (Vector3 v in newVerts)
			{
				if (Vector3.Distance(v, verts[tri]) < 0.01f)
				{
					newTris.Add(newVerts.IndexOf(v));
					add = false;
				}
			}
			if (add)
			{
				newVerts.Add(verts[tri]);
				newTris.Add(newVerts.Count - 1);
			}
		}
		for (int i = 0; i < newVerts.Count; i++)
			newUVs.Add(Vector2.zero);
		uvs = newUVs;
		verts = newVerts;
		tris = newTris;
		
	}
	void ExpandDoubles()
	{
		List<Vector3> newVerts = new List<Vector3>();
		List<int> newTris = new List<int>();
		List<Vector2> newUVs = new List<Vector2>();
		foreach (int tri in tris)
		{
			newVerts.Add(verts[tri]);
			newTris.Add(newVerts.Count - 1);
			newUVs.Add(uvs[tri]);
		}
		uvs = newUVs;
		verts = newVerts;
		tris = newTris;
	}
}