using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class FloorSettings
{
    public int id;
    public Vector2Int size;
    public int seed;
    public Vector2Int spawn;

    public FloorSettings(int id, int seed, Vector2Int size)
    {
        this.id = id;
        var rnd = new System.Random(seed);
        this.seed = rnd.Next();
        this.size = size;
        this.spawn = new Vector2Int(rnd.Next(0, size.x), rnd.Next(0, size.y));
    }

    public FloorSettings(int id, Vector2Int size)
        : this(id, Random.Range(0, int.MaxValue), size)
    {
    }
}

public class FloorBehaviour : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject wallPrefab;
    public GameObject flagStartPrefab;
    public GameObject flagEndPrefab;
    public GameObject coinPrefab;
    public Faces faces;
    public Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();
    public CubeBehaviour player;
    public Dictionary<(Vector2Int, Vector2Int), Wall> walls = new Dictionary<(Vector2Int, Vector2Int), Wall>();
    public Maze maze;
    public Vector2Int goal;
    public class Coin
    {
        public Vector2Int pos;
        public GameObject obj;

        public Coin(Vector2Int pos, GameObject obj)
        {
            this.pos = pos;
            this.obj = obj;
        }
    }
    public List<Coin> coins;
    public static FloorSettings nextSettings;
    public static FloorSettings currentSettings;
    public FloorSettings settings;

    // Start is called before the first frame update
    void Start()
    {
        GameStats.Reset();
        if (nextSettings != null)
        {
            settings = nextSettings;
            currentSettings = nextSettings;
            nextSettings = null;
        }
        else
        {
            if (currentSettings != null)
                settings = currentSettings;
            else
                settings = new FloorSettings(-1, new Vector2Int(10, 10));
        }
        transform.parent.localPosition += new Vector3(-settings.size.x / 2 + .5f, 0, -(-settings.size.y / 2 + .5f));
        var rnd = new System.Random(settings.seed);
        coins = new List<Coin>();
        for (int i = 0; i < 3; i++)
        {
            coins.Add(new Coin(new Vector2Int(rnd.Next(0, settings.size.x), rnd.Next(0, settings.size.y)), null));
        }
        StartCoroutine(Generate());
    }

    IEnumerator Generate()
    {
        var start = settings.spawn.ToCellPos();
        var playerpos = start.ToVecPos().ToWorldPos();
        player.transform.localPosition = playerpos + new Vector3(0, .5f, 0);
        var flagStart = Instantiate(flagStartPrefab, transform.parent);
        flagStart.transform.localPosition = playerpos;

        maze = new Maze(settings.size.ToCellPos(), start, settings.seed);
        maze.DebugPrint();
        var routes = maze.GetRoutes();

        Maze.RouteNode? longest = maze.GetGoal();
        if (longest.HasValue)
        {
            Debug.LogFormat("Longest is ({0},{1}), Count={2}", longest.Value.pos.IX, longest.Value.pos.IY, longest.Value.pos.Count);

            goal = longest.Value.pos.ToVecPos();
            var goalpos = goal.ToWorldPos();
            var flagEnd = Instantiate(flagEndPrefab, transform.parent);
            flagEnd.transform.localPosition = goalpos;
        }

        foreach (var coin in coins)
        {
            var coinpos = coin.pos.ToWorldPos();
            var coinObj = coin.obj = Instantiate(coinPrefab, transform.parent);
            coinObj.transform.localPosition = coinpos;
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
                //yield return new WaitForSeconds(.01f);
            }
        yield break;
    }

    public Tile Create(Vector2Int pos, int id)
    {
        var tile = Get(pos);
        if (tile == null)
        {
            var gobj = Instantiate(tilePrefab, transform);
            tile = gobj.GetComponent<Tile>();
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
