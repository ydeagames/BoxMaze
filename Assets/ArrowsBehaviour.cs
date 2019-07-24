using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowsBehaviour : MonoBehaviour
{
    public CubeBehaviour player;

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = player.transform.localPosition;
    }

    public void Move(Maze.Direction direction)
    {
        player.Move(direction, Quaternion.identity);
    }
}
