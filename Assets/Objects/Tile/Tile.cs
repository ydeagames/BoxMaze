using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
