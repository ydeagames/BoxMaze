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
    public float controllSpeed = .1f;
    private Vector3 move;

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

        move = Quaternion.AngleAxis(camera.transform.eulerAngles.y, Vector3.up) * move;
        move *= controllSpeed;
        offset += move;
        move = Vector3.zero;

        var pos = offset + target.transform.position;
        pos.x = Mathf.Clamp(pos.x, bounds.min.x, bounds.max.x);
        pos.z = Mathf.Clamp(pos.z, bounds.min.z, bounds.max.z);
        pos.y = startPos.y;
        offset = pos - target.transform.position;

        transform.position = Vector3.Lerp(transform.position, pos, movePercent);
    }

    public void Move(Vector2 delta)
    {
        move += new Vector3(delta.x, 0, delta.y);
    }

    public void Move(float angle)
    {
        move += Quaternion.AngleAxis(angle, Vector3.down) * Vector3.forward;
    }
}
