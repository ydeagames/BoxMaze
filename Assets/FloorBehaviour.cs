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
        for (int iy = 0; iy < 10; iy++) for (int ix = 0; ix < 10; ix++)
                Create(ix, iy);
    }

    public Tile Create(int x, int y)
    {
        var tile = Get(x, y);
        if (tile == null)
        {
            var gobj = Instantiate(tilePrefab, transform);
            tile = gobj.AddComponent<Tile>();
        }
        tile.transform.position = new Vector3(x, 0, y);
        tile.tileId = Random.Range(0, tileMaterials.Length);
        tile.tileMaterial = tileMaterials[tile.tileId];
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
