using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class TriGridMesh : MonoBehaviour
{
	int vertNum = 0;
	public void GenMesh(ref Dictionary<TriGridLoc, TriTile> grid)
	{
		List<Vector3> verts = new List<Vector3>();
		List<int> tris = new List<int>();
		List<Vector2> uvs = new List<Vector2>();

		vertNum = 0;
		foreach(KeyValuePair<TriGridLoc, TriTile> kvp in grid)
		{
			TriTile t = kvp.Value;
			if (t.gridLoc.left)
			{
				verts.Add(t.WorldLoc() + new Vector3(0, 0, Mathf.Sqrt(3) / 3f));
				verts.Add(t.WorldLoc() + new Vector3(0.5f, 0, -Mathf.Sqrt(3) / 6f));
				verts.Add(t.WorldLoc() + new Vector3(-0.5f, 0, -Mathf.Sqrt(3) / 6f));
			}
			else
			{
				verts.Add(t.WorldLoc() + new Vector3(0, 0, -Mathf.Sqrt(3) / 3f));
				verts.Add(t.WorldLoc() + new Vector3(-0.5f, 0, Mathf.Sqrt(3) / 6f));
				verts.Add(t.WorldLoc() + new Vector3(0.5f, 0, Mathf.Sqrt(3) / 6f));
			}
			tris.Add(vertNum);
			tris.Add(vertNum + 1);
			tris.Add(vertNum + 2);
			uvs.Add(new Vector2(0.5f, 1f));
			uvs.Add(new Vector2(1f, 0));
			uvs.Add(new Vector2(0f, 0f));
			vertNum += 3;

			List<TriGridLoc> connections = t.FindConnections();
			for (int i = 0; i < 3; i++)
			{
				if (grid.Contains(connections[i]) && )
				{

				}
			}
		}
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		mesh.vertices = verts.ToArray();
		mesh.triangles = tris.ToArray();
		mesh.uv = uvs.ToArray();
		mesh.RecalculateNormals();
	}
}
