using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowsRotateBehaviour : MonoBehaviour
{
    public CubeBehaviour player;

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(0, player.cameraWrapper.transform.localEulerAngles.y, 0);
    }

    public void Rotate(bool cw)
    {
        player.cameraWrapper.GetComponent<Animator>().SetTrigger(cw ? "Right" : "Left");
    }
}
