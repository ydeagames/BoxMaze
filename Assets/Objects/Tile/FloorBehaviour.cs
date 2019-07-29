using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FloorBehaviour : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject wallPrefab;
    public GameObject flagStartPrefab;
    public GameObject flagEndPrefab;
    public Vector2Int size = new Vector2Int(10, 10);
    public Faces faces;
    public Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();
    public CubeBehaviour player;
    public Dictionary<(Vector2Int, Vector2Int), Wall> walls = new Dictionary<(Vector2Int, Vector2Int), Wall>();
    public Maze maze;

    // Start is called before the first frame update
    void Start()
    {
        transform.parent.localPosition += new Vector3(-size.x / 2 + .5f, 0, -(-size.y / 2 + .5f));
        StartCoroutine(Generate());
    }

    IEnumerator Generate()
    {
        var start = new Vector2Int(Random.Range(0, size.x), Random.Range(0, size.y)).ToCellPos();
        var playerpos = start.ToVecPos().ToWorldPos(); ;
        player.transform.localPosition = playerpos + new Vector3(0, .5f, 0);
        var flagStart = Instantiate(flagStartPrefab, transform.parent);
        flagStart.transform.localPosition = playerpos;

        maze = new Maze(size.ToCellPos(), start);
        maze.DebugPrint();
        var routes = maze.GetRoutes();

        Maze.RouteNode? longest = maze.GetGoal();
        if (longest.HasValue)
        {
            Debug.LogFormat("Longest is ({0},{1}), Count={2}", longest.Value.pos.IX, longest.Value.pos.IY, longest.Value.pos.Count);

            var goalpos = longest.Value.pos.ToVecPos().ToWorldPos();
            var flagEnd = Instantiate(flagEndPrefab, transform.parent);
            flagEnd.transform.localPosition = goalpos;
        }

        Dictionary<Vector2Int, Quaternion> rotMap = new Dictionary<Vector2Int, Quaternion>();
        foreach (var route in routes)
            foreach (var node in route)
            {
                var rotation = Quaternion.identity;
                Vector2Int? before = null;
                if (node.before != null)
                {
                    before = node.before.ToVecPos();
                    if (rotMap.ContainsKey(before.Value))
                        rotation = rotMap[before.Value];
                }
                var pos = node.pos.ToVecPos();

                var move = before.HasValue ? pos - before.Value : Vector2Int.zero;
                rotation = CubeBehaviour.GetMoveRotation(move.ToDirection(), rotation);
                //else
                //    Debug.LogFormat("Not Move Route move={0}, pos={1}", move, pos);
                int id = CubeBehaviour.GetSideId(rotation);

                rotMap.Add(pos, rotation);
                Create(pos, id);

                //Create(pos.x, pos.y, Random.Range(0, tileMaterials.Length));
                yield return new WaitForSeconds(.01f);
            }
        yield break;
    }

    public Tile Create(Vector2Int pos, int id)
    {
        var tile = Get(pos);
        if (tile == null)
        {
            var gobj = Instantiate(tilePrefab, transform);
            tile = gobj.AddComponent<Tile>();
        }
        else
            Debug.LogFormat("Duplicate Pos ({0}, {1})", pos.x, pos.y);
        tile.pos = pos;
        tile.transform.localPosition = tile.pos.ToWorldPos();
        tile.tileId = id;
        tile.tileMaterial = faces.GetFace(tile.tileId).material;
        tiles[pos] = tile;
        return tile;
    }

    public static (Vector2Int, Vector2Int)? GetWallPos(Vector2Int pos1, Vector2Int pos2)
    {
        var diff = pos2 - pos1;
        var dir = diff.ToDirection();
        if (dir == null)
            return null;
        else if (dir == Maze.Direction.Left || dir == Maze.Direction.Up)
        {
            var tmp = pos1;
            pos1 = pos2;
            pos2 = tmp;
        }
        return (pos1, pos2);
    }

    public Wall CreateWall(Vector2Int pos1, Vector2Int pos2)
    {
        var pos = GetWallPos(pos1, pos2);
        if (pos.HasValue)
        {
            var wall = Get(pos.Value);
            if (wall == null)
            {
                var gobj = Instantiate(wallPrefab, transform);
                wall = gobj.AddComponent<Wall>();
            }
            wall.pos = pos.Value;
            wall.transform.localPosition = (pos.Value.Item1.ToWorldPos() + pos.Value.Item2.ToWorldPos()) / 2;
            {
                var euler = wall.transform.localEulerAngles;
                var diff = pos2 - pos1;
                var dir = diff.ToDirection();
                if (dir == Maze.Direction.Left || dir == Maze.Direction.Right)
                    euler.y = 90;
                else
                    euler.y = 0;
                wall.transform.localEulerAngles = euler;
            }
            walls[pos.Value] = wall;
            return wall;
        }
        return null;
    }

    public Tile Get(Vector2Int pos)
    {
        if (tiles.ContainsKey(pos))
            return tiles[pos];
        return null;
    }

    public Wall Get((Vector2Int, Vector2Int) pos)
    {
        if (walls.ContainsKey(pos))
            return walls[pos];
        return null;
    }

    public static FloorBehaviour GetInstance()
    {
        return GameObject.Find("Floor").GetComponent<FloorBehaviour>();
    }
}
