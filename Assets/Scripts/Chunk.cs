using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(MeshFilter))]
[RequireComponent (typeof(MeshCollider))]

public class Chunk : MonoBehaviour {

  public int ground_height = 8;
  public int height = 100;
  public byte[,,] map;

  protected int width;
  protected bool initialized = false;

  protected Mesh mesh;
  protected MeshCollider meshCollider;
  protected List<Vector3> verts = new List<Vector3>();
  protected List<int> tris = new List<int>();
  protected List<Vector2> uv = new List<Vector2>();

  /* Instantiate the map, and the mesh filter
  ------------------------------------------------------------------------------*/

  void Start ()
  {

    Terrain terrain = Camera.main.GetComponent<Terrain>();
    width = terrain.chunkSize;

    map = new byte[width,height,width];
    for (int x = 0; x < width; x++)
      for (int y = 0; y < ground_height; y++)
        for (int z = 0; z < width; z++)
          map[x, y, z] = 1;

    mesh = new Mesh();
    GetComponent<MeshFilter>().mesh = mesh;
    meshCollider = GetComponent<MeshCollider>();

    initialized = true;
    Regenerate();
  }

  /* Regenerate the mesh
  ------------------------------------------------------------------------------*/

  public void Regenerate()
  {
    if (!initialized) return;

    verts.Clear();
    tris.Clear();
    uv.Clear();
    mesh.triangles = tris.ToArray();

    for (int y = 0; y < height; y++)
      for (int x = 0; x < width; x++)
        for (int z = 0; z < width; z++)
        {
          byte block = map[x,y,z];
          if (block != 0) DrawBrick(x, y, z, block);
        }

    mesh.vertices = verts.ToArray();
    mesh.triangles = tris.ToArray();
    mesh.uv = uv.ToArray();
    mesh.RecalculateNormals();

    meshCollider.sharedMesh = null;
    meshCollider.sharedMesh = mesh;
  }

  /* Check if any of the faces are considered transparent
  ------------------------------------------------------------------------------*/

  public bool IsTranparent(int x, int y, int z)
  {
    return (((x < 0) || (y < 0) || (z < 0) || (x >= width) || (y >= height) || (z >= width)) || (map[x,y,z] == 0));
  }

  /* Draw the block
  ------------------------------------------------------------------------------*/

  public void DrawBrick(int x, int y, int z, byte block)
  {
    Vector3 start = new Vector3(x, y, z);
    Vector3 offset1, offset2;

    if (IsTranparent(x, y - 1, z))
    {
      offset1 = Vector3.left;
      offset2 = Vector3.back;
      DrawFace(start + Vector3.right, offset1, offset2, block);
    }

    if (IsTranparent(x, y + 1, z))
    {
      offset1 = Vector3.right;
      offset2 = Vector3.back;
      DrawFace(start + Vector3.up, offset1, offset2, block);
    }

    if (IsTranparent(x - 1, y, z))
    {
      offset1 = Vector3.up;
      offset2 = Vector3.back;
      DrawFace(start, offset1, offset2, block);
    }

    if (IsTranparent(x + 1, y, z))
    {
      offset1 = Vector3.down;
      offset2 = Vector3.back;
      DrawFace(start + Vector3.right + Vector3.up, offset1, offset2, block);
    }

    if (IsTranparent(x, y, z - 1))
    {
      offset1 = Vector3.left;
      offset2 = Vector3.up;
      DrawFace(start + Vector3.right + Vector3.back, offset1, offset2, block);
    }

    if (IsTranparent(x, y, z + 1))
    {
      offset1 = Vector3.right;
      offset2 = Vector3.up;
      DrawFace(start, offset1, offset2, block);
    }

  }

  /* Draw the block faces
  ------------------------------------------------------------------------------*/

  public void DrawFace(Vector3 start, Vector3 offset1, Vector3 offset2, byte block)
  {
    int index = verts.Count;

    verts.Add(start);
    verts.Add(start + offset1);
    verts.Add(start + offset2);
    verts.Add(start + offset1 + offset2);

    Vector2 uvBase;
    switch (block)
    {
      default:
        uvBase = new Vector2(0.25f,0.25f);
        break;
    }

    int modifier = ((offset1 == Vector3.right) && (offset2 == Vector3.back)) ? 1 : -1;

    uv.Add (uvBase);
    uv.Add (uvBase + new Vector2(1, 0));
    uv.Add (uvBase + new Vector2(0, 1));
    uv.Add (uvBase + new Vector2(1, 1));

    tris.Add(index + 0);
    tris.Add(index + 1);
    tris.Add(index + 2);
    tris.Add(index + 3);
    tris.Add(index + 2);
    tris.Add(index + 1);

  }

  public void SetBrick(int x, int y, int z, byte block)
  {
    x -= Mathf.RoundToInt(transform.position.x);
    y -= Mathf.RoundToInt(transform.position.y);
    z -= Mathf.RoundToInt(transform.position.z);

    if ((x < 0) || (y < 0) || (z < 0) || (x >= width) || (y >= height) || (z >= width)) return;

    if (map[x,y,z] != block)
    {
      map[x,y,z] = block;
      Regenerate();
    }
  }

}
