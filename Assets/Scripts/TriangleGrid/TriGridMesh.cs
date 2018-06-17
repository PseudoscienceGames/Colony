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
			verts.Add(t.verts[0]);
			verts.Add(t.verts[1]);
			verts.Add(t.verts[2]);
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
				if (grid.ContainsKey(connections[i]) )
				{
					TriTile o = grid[connections[i]];
					verts.Add(t.verts[i]);
					if (i == 0)
					{
						verts.Add(t.verts[2]);
						verts.Add(o.verts[2]);
					}
					else
					{
						verts.Add(t.verts[i-1]);
						verts.Add(o.verts[i-1]);
					}
					tris.Add(vertNum);
					tris.Add(vertNum + 1);
					tris.Add(vertNum + 2);
					uvs.Add(new Vector2(0.5f, 1f));
					uvs.Add(new Vector2(1f, 0));
					uvs.Add(new Vector2(0f, 0f));
					vertNum += 3;
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
