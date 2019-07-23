using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PositionExtensions
{
    public static Vector2Int ToMazePos(this Vector3 pos)
    {
        return new Vector2Int(Mathf.RoundToInt(pos.x), -Mathf.RoundToInt(pos.z));
    }

    public static Vector3 ToWorldPos(this Vector2Int pos)
    {
        return new Vector3(pos.x, 0, -pos.y);
    }
}

public class Tile : MonoBehaviour
{
    public int tileId;
    public Material tileMaterial;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material = tileMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
