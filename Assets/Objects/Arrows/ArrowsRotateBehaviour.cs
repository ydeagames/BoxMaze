using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowsRotateBehaviour : MonoBehaviour
{
    public CubeBehaviour player;

    // Update is called once per frame
    void Update()
    {
        var wheel = Input.mouseScrollDelta.y;
        if (wheel < -.1f || Input.GetKeyDown(KeyCode.X))
            player.cameraWrapper.GetComponent<Animator>().SetTrigger("Left");
        else if (wheel > .1f || Input.GetKeyDown(KeyCode.Z))
            player.cameraWrapper.GetComponent<Animator>().SetTrigger("Right");

        transform.localRotation = Quaternion.Euler(0, player.cameraWrapper.transform.localEulerAngles.y, 0);
    }

    public void Rotate(bool cw)
    {
        player.cameraWrapper.GetComponent<Animator>().SetTrigger(cw ? "Right" : "Left");
    }
}
