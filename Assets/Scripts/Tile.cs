using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Tile
{
	public Vector2 gridLoc;
	public float height;
	public Vector3 worldLoc;
	public List<Vector2> connections = new List<Vector2>();
	public List<Vector3> verts = new List<Vector3>();
	public List<int> tris = new List<int>();

	public Tile(Vector2 gridLoc)
	{
		this.gridLoc = gridLoc;
	}

	public Tile(Vector2 gridLoc, float height)
	{
		this.gridLoc = gridLoc;
		SetHeight(height);
	}

	public void SetHeight(float height)
	{
		this.height = height;
		worldLoc = Grid.GridToWorld(gridLoc, height);
	}
}
