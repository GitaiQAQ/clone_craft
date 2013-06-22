using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Terrain : MonoBehaviour {

  public int chunkSize = 16;
  public int viewSize = 32;
  public Chunk chunkFab;
  public List<Chunk> chunks = new List<Chunk>();

	// Use this for initialization
	void Start () {
	 chunks.Add((Chunk)Instantiate(chunkFab));
	}

	// Update is called once per frame
	void Update () {
    CreateChunks();
 	}

  /* Create Chunks
  ------------------------------------------------------------------------------*/
  public void CreateChunks()
  {
    for (float x = transform.position.x - viewSize; x < transform.position.x + viewSize; x += chunkSize)
    {
      for (float z = transform.position.z - viewSize; z < transform.position.z + viewSize; z += chunkSize)
      {
        if (ChunkExists(x,z)) continue;
        int chunkX = Mathf.FloorToInt(x/chunkSize) * chunkSize;
        int chunkZ = Mathf.FloorToInt(z/chunkSize) * chunkSize;
        chunks.Add((Chunk)Instantiate(chunkFab, new Vector3(chunkX, 0, chunkZ), Quaternion.identity));
      }
    }
  }

  /* Check if a chunk exists at x / y
  ------------------------------------------------------------------------------*/
  public bool ChunkExists(float x, float z)
  {
    for (int i = 0; i < chunks.Count; i++)
    {
      if ( (x < chunks[i].transform.position.x) ||
           (z < chunks[i].transform.position.z) ||
           (x >= chunks[i].transform.position.x + chunkSize) ||
           (z >= chunks[i].transform.position.z + chunkSize))
        continue;
      return true;
    }
    return false;
  }

  /* Query a chunk
  ------------------------------------------------------------------------------*/
  public Chunk GetChunk(int x, int y, int z)
  {
    for (int i = 0; i < chunks.Count; i++)
    {
      if ( (x < chunks[i].transform.position.x) ||
           (z < chunks[i].transform.position.z) ||
           (x >= chunks[i].transform.position.x + chunkSize) ||
           (z >= chunks[i].transform.position.z + chunkSize))
        continue;
      return chunks[i];
    }
    return null;
  }

}
