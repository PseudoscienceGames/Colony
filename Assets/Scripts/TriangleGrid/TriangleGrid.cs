using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleGrid : MonoBehaviour
{
	Dictionary<TriGridLoc, TriTile> grid = new Dictionary<TriGridLoc, TriTile>();

	private void Start()
	{
		for(int x = 0; x < 5; x++)
		{
			for (int y = 0; y < 5; y++)
			{
				grid.Add(new TriGridLoc(x, y, true), new TriTile(x, y, true, Random.Range(0,4)));
				grid.Add(new TriGridLoc(x, y, false), new TriTile(x, y, false, Random.Range(0, 4)));
			}
		}
		GetComponent<TriGridMesh>().GenMesh(ref grid);
	}
}

public struct TriGridLoc
{
	public int x;
	public int y;
	public bool left;

	public TriGridLoc(int x, int y, bool left)
	{
		this.x = x;
		this.y = y;
		this.left = left;
	}
}

public struct TriTile
{
	public TriGridLoc gridLoc;
	public int height;

	public Vector3 WorldLoc()
	{
		Vector3 pos = new Vector3();
		pos.x = gridLoc.x + (float)gridLoc.y / 2f;
		pos.z = gridLoc.y * (Mathf.Sqrt(3) / 2f);
		pos.y = (float)height / 2f;
		if (!gridLoc.left)
			pos += new Vector3(0.5f, 0, (Mathf.Sqrt(3) / 6f));
		return pos;
	}

	public TriTile(TriGridLoc gridLoc, int height)
	{
		this.gridLoc = gridLoc;
		this.height = height;
	}
	public TriTile(int x, int y, bool left, int height)
	{
		gridLoc.x = x;
		gridLoc.y = y;
		gridLoc.left = left;
		this.height = height;
	}


	public void SetHeight(int height)
	{
		this.height = height;
	}
	public List<TriGridLoc> FindConnections()
	{
		List<TriGridLoc> connections = new List<TriGridLoc>();
		if(gridLoc.left)
		{
			connections.Add(new TriGridLoc(gridLoc.x - 1, gridLoc.y, false));
			connections.Add(new TriGridLoc(gridLoc.x, gridLoc.y, false));
			connections.Add(new TriGridLoc(gridLoc.x, gridLoc.y - 1, false));
		}
		else
		{
			connections.Add(new TriGridLoc(gridLoc.x + 1, gridLoc.y, true));
			connections.Add(new TriGridLoc(gridLoc.x, gridLoc.y, true));
			connections.Add(new TriGridLoc(gridLoc.x, gridLoc.y + 1, true));
		}
		return connections;
	}
}
