using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehaviour : MonoBehaviour
{
    bool moving;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!moving)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                StartCoroutine(Move(transform.position, transform.rotation, transform.position + Vector3.left, Quaternion.Euler(0, 0, 90) * transform.rotation));
            if (Input.GetKeyDown(KeyCode.RightArrow))
                StartCoroutine(Move(transform.position, transform.rotation, transform.position + Vector3.right, Quaternion.Euler(0, 0, -90) * transform.rotation));
            if (Input.GetKeyDown(KeyCode.UpArrow))
                StartCoroutine(Move(transform.position, transform.rotation, transform.position + Vector3.forward, Quaternion.Euler(90, 0, 0) * transform.rotation));
            if (Input.GetKeyDown(KeyCode.DownArrow))
                StartCoroutine(Move(transform.position, transform.rotation, transform.position + Vector3.back, Quaternion.Euler(-90, 0, 0) * transform.rotation));
        }
    }

    IEnumerator Move(Vector3 startPos, Quaternion startRot, Vector3 endPos, Quaternion endRot)
    {
        moving = true;
        for (float t = 0; t < 1; t += .02f)
        {
            transform.position = Vector3.Lerp(startPos, endPos, t);
            transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }
        moving = false;
    }
}
