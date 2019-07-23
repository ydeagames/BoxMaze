using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBehaviour : MonoBehaviour
{
    public GameObject tilePrefab;
    public Material[] tileMaterials;
    public Dictionary<(int x, int y), Tile> tiles = new Dictionary<(int x, int y), Tile>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Generate());
    }

    IEnumerator Generate()
    {
        var maze = new Maze(10, 10);
        maze.DebugPrint();
        var routes = maze.GetRoutes();
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
                if (move.x == 1)
                {
                    var axis = new Vector3(0, 0, -1);
                    rotation = Quaternion.AngleAxis(90, axis) * rotation;
                }
                else if (move.x == -1)
                {
                    var axis = new Vector3(0, 0, 1);
                    rotation = Quaternion.AngleAxis(90, axis) * rotation;
                }
                else if (move.y == 1)
                {
                    var axis = new Vector3(-1, 0, 0);
                    rotation = Quaternion.AngleAxis(90, axis) * rotation;
                }
                else if (move.y == -1)
                {
                    var axis = new Vector3(1, 0, 0);
                    rotation = Quaternion.AngleAxis(90, axis) * rotation;
                }
                //else
                //    Debug.LogFormat("Not Move Route move={0}, pos={1}", move, pos);
                int id = CubeBehaviour.GetSideId(rotation);

                rotMap.Add(pos, rotation);
                Create(pos.x, pos.y, id);

                //Create(pos.x, pos.y, Random.Range(0, tileMaterials.Length));
                yield return new WaitForSeconds(.1f);
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
        tile.tileMaterial = tileMaterials[tile.tileId];
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
