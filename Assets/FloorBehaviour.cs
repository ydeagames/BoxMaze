using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBehaviour : MonoBehaviour
{
    public GameObject tilePrefab;
    public Material[] tileMaterials;

    // Start is called before the first frame update
    void Start()
    {
        for (int iy = 0; iy < 10; iy++) for (int ix = 0; ix < 10; ix++)
            {
                var obj = Instantiate(tilePrefab, transform);
                obj.transform.position = new Vector3(ix, 0, iy);
                obj.GetComponent<Renderer>().material = tileMaterials[Random.Range(0, tileMaterials.Length)];
            }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
