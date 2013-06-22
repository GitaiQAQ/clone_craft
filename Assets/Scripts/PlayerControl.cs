using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Terrain))]

public class PlayerControl : MonoBehaviour {

  RaycastHit hit;
  public byte activeBlockType = 1;
  public Transform retAdd, retDel;
  public Terrain terrain;

  void Start () {
    Screen.lockCursor = true;
    terrain = GetComponent<Terrain>();
  }

  void Update () {
    Ray ray = new Ray(transform.position + transform.forward/2, transform.forward);
    if (Physics.Raycast(ray, out hit, 8f))
    {
      Vector3 p = hit.point - hit.normal / 2;
      retDel.position = new Vector3(Mathf.Floor(p.x), Mathf.Floor(p.y), Mathf.Ceil(p.z));

      p = hit.point + hit.normal / 2;
      retAdd.position = new Vector3(Mathf.Floor(p.x), Mathf.Floor(p.y), Mathf.Ceil(p.z));

      Control();
    } else {
      retAdd.position = new Vector3(0, -100, 0);
      retDel.position = new Vector3(0, -100, 0);
    }
  }

  /* Add a block
  ------------------------------------------------------------------------------*/
  void addBlock()
  {
    int x = Mathf.RoundToInt(retAdd.position.x);
    int y = Mathf.RoundToInt(retAdd.position.y);
    int z = Mathf.RoundToInt(retAdd.position.z);
    terrain.GetChunk(x,y,z).SetBrick(x,y,z,activeBlockType);
    Debug.Log("Add Block @ " + x + "," + y + "," + z);
  }

  /* Remove a block
  ------------------------------------------------------------------------------*/
  void delBlock()
  {
    if ( retDel.position.y == 0 ) return;

    int x = Mathf.RoundToInt(retDel.position.x);
    int y = Mathf.RoundToInt(retDel.position.y);
    int z = Mathf.RoundToInt(retDel.position.z);
    terrain.GetChunk(x,y,z).SetBrick(x,y,z,0);
    Debug.Log("Del Block @ " + x + "," + y + "," + z);
  }

  /* Listen for user input
  ------------------------------------------------------------------------------*/
  void Control()
  {
    if (Input.GetMouseButtonDown(0))
      addBlock();
    if (Input.GetMouseButtonDown(1))
      delBlock();
  }

}
