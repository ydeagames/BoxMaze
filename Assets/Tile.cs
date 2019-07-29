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

    public static Vector2Int ToVec(this Maze.Direction direction)
    {
        switch (direction)
        {
            case Maze.Direction.Right:
                return new Vector2Int(1, 0);
            case Maze.Direction.Left:
                return new Vector2Int(-1, 0);
            case Maze.Direction.Down:
                return new Vector2Int(0, 1);
            case Maze.Direction.Up:
                return new Vector2Int(0, -1);
        }
        return Vector2Int.zero;
    }

    public static Maze.Direction? ToDirection(this Vector2Int move)
    {
        if (move.x == 1)
            return Maze.Direction.Right;
        else if (move.x == -1)
            return Maze.Direction.Left;
        else if (move.y == 1)
            return Maze.Direction.Down;
        else if (move.y == -1)
            return Maze.Direction.Up;
        return null;
    }
}

public class Tile : MonoBehaviour
{
    public int tileId;
    public Material tileMaterial;
    public Vector2Int pos;
    public Material material
    {
        set { GetComponent<Renderer>().material = value; }
        get { return GetComponent<Renderer>().material; }
    }

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
