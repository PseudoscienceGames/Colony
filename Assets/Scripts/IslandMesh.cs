using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class IslandMesh : MonoBehaviour
{
	public List<Vector3> verts = new List<Vector3>();
	private List<int> tris = new List<int>();
	private List<Vector2> uvs = new List<Vector2>();
	private int vertNumber = 0;
	public bool addNoise;
	public float noiseScale;

	IslandData data;

	public void GenMesh()
	{
		data = GetComponent<IslandData>();

		AddTiles();
		CollapseDoubles();
		if (addNoise)
			AddNoise();
		ExpandDoubles();
		//CollapseDoubles();
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		mesh.vertices = verts.ToArray();
		mesh.triangles = tris.ToArray();
		mesh.uv = uvs.ToArray();
		mesh.RecalculateNormals();
		GetComponent<MeshCollider>().sharedMesh = mesh;
	}

	void AddTiles()
	{
		foreach (Tile tile in GetComponent<IslandData>().tiles.Values)
		{
			AddTop(tile);
			AddSide(tile);
		}
	}
	void AddTop(Tile tile)
	{
		verts.Add(tile.worldLoc);
		for (int i = 0; i <= 5; i++)
		{
			Vector3 vertex1WorldLoc = tile.worldLoc + (Quaternion.Euler(0, (60 * i), 0) * Vector3.forward * Grid.hexRadius);
			vertex1WorldLoc = FindOffset(tile, vertex1WorldLoc, i);
			//int cT = 1;
			float height = tile.height;
			//if (IslandData.Instance.tiles.ContainsKey(Grid.MoveTo(tile.gridLoc, i)))
			//{
			//	cT++;
			//	height += IslandData.Instance.tiles[Grid.MoveTo(tile.gridLoc, i)].height;
			//}
			//if (IslandData.Instance.tiles.ContainsKey(Grid.MoveTo(tile.gridLoc, Grid.MoveDirFix(i + 1))))
			//{
			//	cT++;
			//	height += IslandData.Instance.tiles[Grid.MoveTo(tile.gridLoc, Grid.MoveDirFix(i + 1))].height;
			//}
			//vertex1WorldLoc.y = (height / cT) * Grid.tileHeight;
			verts.Add(vertex1WorldLoc);
			tris.Add(vertNumber);
			tris.Add(vertNumber + Grid.MoveDirFix(i) + 1);
			tris.Add(vertNumber + Grid.MoveDirFix(i + 1) + 1);
		}
		vertNumber += 7;
		uvs.Add(new Vector2(0.5f, 0.5f));
		uvs.Add(new Vector2(0.5f, 0));
		uvs.Add(new Vector2(1, 0));
		uvs.Add(new Vector2(1, 1));
		uvs.Add(new Vector2(0.5f, 1));
		uvs.Add(new Vector2(0, 1));
		uvs.Add(new Vector2(0, 0));
	}
	void AddSide(Tile tile)
	{
		for (int i = 0; i <= 5; i++)
		{
			if (!IslandData.Instance.tiles.ContainsKey(Grid.MoveTo(tile.gridLoc, i)) ||
				IslandData.Instance.tiles[Grid.MoveTo(tile.gridLoc, i)].height < tile.height)
			{
				int s = 0;
				if (IslandData.Instance.tiles.ContainsKey(Grid.MoveTo(tile.gridLoc, i)))
					s = (int)IslandData.Instance.tiles[Grid.MoveTo(tile.gridLoc, i)].height + 1;
				for (int h = s; h <= tile.height; h++)
				{
					Vector3 vertex1WorldLoc = tile.worldLoc + (Quaternion.Euler(0, (60 * Grid.MoveDirFix(i - 1)), 0) * Vector3.forward * Grid.hexRadius);
					Vector3 vertex2WorldLoc = tile.worldLoc + (Quaternion.Euler(0, (60 * i), 0) * Vector3.forward * Grid.hexRadius);
					vertex1WorldLoc = FindOffset(tile, vertex1WorldLoc, Grid.MoveDirFix(i - 1));
					vertex2WorldLoc = FindOffset(tile, vertex2WorldLoc, i);

					Vector3 vertex3WorldLoc = vertex2WorldLoc;
					Vector3 vertex4WorldLoc = vertex1WorldLoc;

					vertex1WorldLoc.y = h * Grid.tileHeight;
					vertex2WorldLoc.y = h * Grid.tileHeight;
					vertex3WorldLoc.y = (h - 1f) * Grid.tileHeight;
					vertex4WorldLoc.y = (h - 1f) * Grid.tileHeight;
					//if (IslandData.Instance.tiles.ContainsKey(Grid.MoveTo(tile.gridLoc, i)))
					//{
					//	vertex3WorldLoc.y = IslandData.Instance.tiles[Grid.MoveTo(tile.gridLoc, i)].height * Grid.tileHeight;
					//	vertex4WorldLoc.y = IslandData.Instance.tiles[Grid.MoveTo(tile.gridLoc, i)].height * Grid.tileHeight;
					//}

					verts.Add(vertex2WorldLoc);
					verts.Add(vertex1WorldLoc);
					verts.Add(vertex4WorldLoc);
					verts.Add(vertex2WorldLoc);
					verts.Add(vertex4WorldLoc);
					verts.Add(vertex3WorldLoc);

					tris.Add(vertNumber);
					tris.Add(vertNumber + 1);
					tris.Add(vertNumber + 2);
					tris.Add(vertNumber + 3);
					tris.Add(vertNumber + 4);
					tris.Add(vertNumber + 5);
					vertNumber += 6;

					uvs.Add(new Vector2(1, 0));
					uvs.Add(new Vector2(1, 1));
					uvs.Add(new Vector2(0, 1));
					uvs.Add(new Vector2(1, 0));
					uvs.Add(new Vector2(0, 1));
					uvs.Add(new Vector2(0, 0));
				}
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
			pos = pos - (pos - tile.worldLoc) / 4f;
		else if (otherTile1 == null && otherTile2 != null)
			pos = pos - (pos - Grid.GridToWorld(Grid.MoveTo(tile.gridLoc, vertDir), tile.height)) / 4f;
		else if (otherTile2 == null && otherTile1 != null)
			pos = pos - (pos - Grid.GridToWorld(Grid.MoveTo(tile.gridLoc, vertDir + 1), tile.height)) / 4f;
		else if(otherTile1.height == otherTile2.height && otherTile2.height != tile.height)
			pos = pos - (pos - tile.worldLoc) / 4f;
		else if(tile.height == otherTile1.height && tile.height != otherTile2.height)
			pos = pos - (pos - otherTile2.worldLoc) / 4f;
		else if (tile.height == otherTile2.height && tile.height != otherTile1.height)
			pos = pos - (pos - otherTile1.worldLoc) / 4f;
		pos.y = tile.height * Grid.tileHeight;
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