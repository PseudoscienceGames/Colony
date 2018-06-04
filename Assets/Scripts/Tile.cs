using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Tile
{
	public Vector2Int gridLoc;
	public int landType;
	public int height;
	public List<Vector2Int> connections = new List<Vector2Int>();
	public List<Vector3> verts = new List<Vector3>();
	public List<int> tris = new List<int>();
	public bool coast;

	public Tile(Vector2Int gridLoc)
	{
		this.gridLoc = gridLoc;
	}

	public Tile(Vector2Int gridLoc, int height)
	{
		this.gridLoc = gridLoc;
		SetHeight(height);
	}

	public void SetHeight(int height)
	{
		this.height = height;
		if (this.height < 0 && !coast)
			this.height = 0;
		if (coast)//height <= 0 && Grid.FindGridDistance(Vector2Int.zero, gridLoc) > 15)
			landType = 0;
		else if (height < 6)
			landType = 1;
		else if (height < 10)
			landType = 2;
		else
			landType = 3;
	}
}
