using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int tileId;
    public Material tileMaterial;
    public Vector2Int pos;
    public Renderer modelRenderer;

    public Material material
    {
        set { modelRenderer.material = value; }
        get { return modelRenderer.material; }
    }

    // Start is called before the first frame update
    void Start()
    {
        modelRenderer.material = tileMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
