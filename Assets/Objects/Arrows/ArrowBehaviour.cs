using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArrowBehaviour : MonoBehaviour
{
    public ArrowsBehaviour controller;
    public Renderer arrowColor;
    public Maze.Direction direction;

    private Collider col;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponentInChildren<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        var tileId = CubeBehaviour.GetSideId(CubeBehaviour.GetMoveRotation(direction, controller.player.transform.localRotation));
        arrowColor.material.color = FloorBehaviour.GetInstance().faces.GetFace(tileId).color;

        if (!EventSystem.current.IsPointerOverGameObject())
            if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
                if (hit.collider == col)
                    controller.Move(direction);
    }
}
