using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
