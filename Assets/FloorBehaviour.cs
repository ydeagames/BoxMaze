using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FloorBehaviour : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject flagStartPrefab;
    public GameObject flagEndPrefab;
    public Vector2Int size = new Vector2Int(10, 10);
    public Faces faces;
    public Dictionary<(int x, int y), Tile> tiles = new Dictionary<(int x, int y), Tile>();
    public CubeBehaviour player;

    // Start is called before the first frame update
    void Start()
    {
        transform.parent.localPosition += new Vector3(-size.x / 2 + .5f, 0, -(-size.y / 2 + .5f));
        StartCoroutine(Generate());
    }

    IEnumerator Generate()
    {
        var start = new Maze.Cell() { IX = Random.Range(1, size.x), IY = Random.Range(1, size.y) };
        var playerpos = new Vector2Int(start.IX, start.IY).ToWorldPos(); ;
        player.transform.localPosition = playerpos + new Vector3(0, .5f, 0);
        var flagStart = Instantiate(flagStartPrefab, transform.parent);
        flagStart.transform.localPosition = playerpos;

        var maze = new Maze(new Maze.Cell() { IX = size.x, IY = size.y }, start);
        maze.DebugPrint();
        var routes = maze.GetRoutes();

        Maze.RouteNode? longest = null;
        foreach (var route in routes)
            if (route.Count > 0)
            {
                var last = route.Last();
                if (!longest.HasValue || last.pos.Count > longest.Value.pos.Count)
                    longest = last;
            }
        if (longest.HasValue)
        {
            Debug.LogFormat("Longest is ({0},{1}), Count={2}", longest.Value.pos.IX, longest.Value.pos.IY, longest.Value.pos.Count);

            var goalpos = new Vector2Int(longest.Value.pos.IX, longest.Value.pos.IY).ToWorldPos();
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
                    before = new Vector2Int(node.before.IX, node.before.IY);
                    if (rotMap.ContainsKey(before.Value))
                        rotation = rotMap[before.Value];
                }
                var pos = new Vector2Int(node.pos.IX, node.pos.IY);

                var move = before.HasValue ? pos - before.Value : Vector2Int.zero;
                rotation = CubeBehaviour.GetMoveRotation(move.ToDirection(), rotation);
                //else
                //    Debug.LogFormat("Not Move Route move={0}, pos={1}", move, pos);
                int id = CubeBehaviour.GetSideId(rotation);

                rotMap.Add(pos, rotation);
                Create(pos.x, pos.y, id);

                //Create(pos.x, pos.y, Random.Range(0, tileMaterials.Length));
                yield return new WaitForSeconds(.01f);
            }
        yield break;
    }

    public Tile Create(int x, int y, int id)
    {
        var tile = Get(x, y);
        if (tile == null)
        {
            var gobj = Instantiate(tilePrefab, transform);
            tile = gobj.AddComponent<Tile>();
        }
        tile.transform.localPosition = new Vector3(x, 0, -y);
        tile.tileId = id;
        tile.tileMaterial = faces.GetFace(tile.tileId).material;
        if (tiles.ContainsKey((x, y)))
            Debug.LogFormat("Duplicate Pos ({0}, {1})", x, y);
        tiles[(x, y)] = tile;
        return tile;
    }

    public Tile Get(int x, int y)
    {
        if (tiles.ContainsKey((x, y)))
            return tiles[(x, y)];
        return null;
    }

    public static FloorBehaviour GetInstance()
    {
        return GameObject.Find("Floor").GetComponent<FloorBehaviour>();
    }
}
