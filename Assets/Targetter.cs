using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetter : MonoBehaviour
{
    public GameObject target;
    public float movePercent = .5f;
    Vector3 startPos;
    public FloorBehaviour floor;
    Vector3 offset;

    public float controllRadius = .1f;

    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var camera = Camera.main;
        var size = floor.settings.size;
        var min = -size.ToWorldPos();
        var max = size.ToWorldPos();
        var bounds = new Bounds(min, Vector3.zero);
        bounds.Encapsulate(max);

        var mouse = camera.ScreenToViewportPoint(Input.mousePosition);

        if (mouse.x < controllRadius)
            offset.x--;
        if (mouse.x > 1 - controllRadius)
            offset.x++;
        if (mouse.y < controllRadius)
            offset.z--;
        if (mouse.y > 1 - controllRadius)
            offset.z++;

        var pos = target.transform.position + offset;
        pos.x = Mathf.Clamp(pos.x, bounds.min.x, bounds.max.x);
        pos.z = Mathf.Clamp(pos.z, bounds.min.z, bounds.max.z);
        pos.y = startPos.y;

        transform.position = Vector3.Lerp(transform.position, pos, movePercent);
    }
}
