using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetter : MonoBehaviour
{
    public GameObject target;
    public float movePercent = .5f;
    Vector3 startPos;
    public FloorBehaviour floor;

    public float controllRadius = .3f;
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

        var p1 = camera.WorldToScreenPoint(new Vector2Int(-size.x, +size.y).ToWorldPos());
        var p2 = camera.WorldToScreenPoint(new Vector2Int(+size.x, +size.y).ToWorldPos());
        var p3 = camera.WorldToScreenPoint(new Vector2Int(+size.x, -size.y).ToWorldPos());
        var p4 = camera.WorldToScreenPoint(new Vector2Int(-size.x, -size.y).ToWorldPos());

        var bound = new Bounds(p1, Vector3.zero);
        bound.Encapsulate(p2);
        bound.Encapsulate(p3);
        bound.Encapsulate(p4);

        var mouse = camera.ScreenToViewportPoint(Input.mousePosition);
        var uv0 = new Vector2(Mathf.Clamp01(mouse.x), Mathf.Clamp01(mouse.y)); // 0～1

        var uv = uv0; // 0～1
        uv = uv * 2 - Vector2.one; // -1～1
        {
            var trace = (1 < Mathf.Abs(uv.y / uv.x)) ? uv * Mathf.Abs(1 / uv.y) : uv * Mathf.Abs(1 / uv.x);
            var lengthmax = trace.magnitude; // 0～1.4
            var length = uv.magnitude; // 0～1.4
            var t = length / lengthmax; // 0～1
            t = Mathf.Pow(t, 7); // 0～1
            length = Mathf.Lerp(0, lengthmax, t); // 0～1.4
            uv = uv.normalized * length; // -1～1
        }
        uv = (uv + Vector2.one) / 2; // 0～1

        Debug.Log($"UV0:{uv0} UV:{uv} Bound:{bound}");

        var boundpoint = new Vector3(Mathf.Lerp(bound.min.x, bound.max.x, uv.x), Mathf.Lerp(bound.min.y, bound.max.y, uv.y));
        var offset = Vector3.zero;
        var plane = new Plane(Vector3.up, Vector3.zero);
        var ray = Camera.main.ScreenPointToRay(boundpoint);
        if (plane.Raycast(ray, out float enter))
        {
            offset = ray.GetPoint(enter);
        }
        var pos = target.transform.position + offset;
        pos.y = startPos.y;
        transform.position = Vector3.Lerp(transform.position, pos, movePercent);
    }
}
