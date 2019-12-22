using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetter : MonoBehaviour
{
    public GameObject target;
    public float movePercent = .5f;
    Vector3 startPos;
    public FloorBehaviour floor;

    public float controllRadius = .1f;
    public float bias = .2f;

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

        var mouse = camera.ScreenToViewportPoint(Input.mousePosition);
        var uv0 = new Vector2(Mathf.Clamp01(mouse.x), Mathf.Clamp01(mouse.y)); // 0～1

        var uv = uv0;
        var offset = Camera.main.WorldToScreenPoint(transform.position);
        if (uv.x < controllRadius)
            offset.x--;
        if (uv.x > 1 - controllRadius)
            offset.x++;
        if (uv.y < controllRadius)
            offset.y--;
        if (uv.y > 1 - controllRadius)
            offset.y++;

        offset.x = Mathf.Clamp(offset.x, min.x, max.x);
        offset.z = Mathf.Clamp(offset.z, min.z, max.z);
        offset.y = startPos.y;

        //Debug.Log($"UV0:{uv0} UV:{uv} Mouse:{mouse}");
        var plane = new Plane(Vector3.up, Vector3.zero);
        var ray = Camera.main.ScreenPointToRay(offset);
        if (plane.Raycast(ray, out float enter))
        {
            var pos = ray.GetPoint(enter);
            transform.position = Vector3.Lerp(transform.position, pos, movePercent);
        }
    }
}
