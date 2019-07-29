using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehaviour : MonoBehaviour
{
    public Transform cameraWrapper;

    public float CubeAngle = 15f;   // ここを変えると回転速度が変わる
    public float CubeAcc = 1f;      // ここを変えると回転速度が変わる
    public float UnableCubeAngle = 15f;   // ここを変えると回転速度が変わる
    public float UnableCubeAcc = 1f;   // ここを変えると回転速度が変わる

    float cubeSize = 1f;            // キューブの大きさ
    float cubeSizeHalf = .5f;       // キューブの大きさの半分
    bool isRotate = false;          // 回転中に立つフラグ。回転中は入力を受け付けない

    static Vector3[] sides =
    {
        Vector3.forward,
        Vector3.back,
        Vector3.up,
        Vector3.right,
        Vector3.down,
        Vector3.left,
    };
    public static int GetSideId(Quaternion rotation)
    {
        (int, float)? min = null;

        for (int i = 0; i < sides.Length; i++)
        {
            var side = sides[i];
            float dist = Vector3.Dot(Vector3.down, rotation * side);
            if (!min.HasValue || dist < min.Value.Item2)
                min = (i, dist);
        }

        return min?.Item1 ?? 0;
    }

    public static Quaternion GetMoveRotation(Maze.Direction? direction, Quaternion rotation)
    {
        if (direction.HasValue)
            switch (direction.Value)
            {
                case Maze.Direction.Right:
                    {
                        var axis = new Vector3(0, 0, -1);
                        return Quaternion.AngleAxis(90, axis) * rotation;
                    }

                case Maze.Direction.Left:
                    {
                        var axis = new Vector3(0, 0, 1);
                        return Quaternion.AngleAxis(90, axis) * rotation;
                    }

                case Maze.Direction.Down:
                    {
                        var axis = new Vector3(-1, 0, 0);
                        return Quaternion.AngleAxis(90, axis) * rotation;
                    }

                case Maze.Direction.Up:
                    {
                        var axis = new Vector3(1, 0, 0);
                        return Quaternion.AngleAxis(90, axis) * rotation;
                    }
            }
        return rotation;
    }

    void Start()
    {
    }

    public void Move(Maze.Direction direction, Quaternion rot)
    {
        //回転中は入力を受け付けない
        if (isRotate)
            return;

        var currentPoint = transform.localPosition;
        var nextPoint = Vector3.zero;
        var rotatePoint = Vector3.zero;
        var rotateAxis = Vector3.zero;

        switch (direction)
        {
            case Maze.Direction.Right:
                {
                    nextPoint = currentPoint + rot * new Vector3(cubeSize, 0f, 0f);
                    rotatePoint = currentPoint + rot * new Vector3(cubeSizeHalf, -cubeSizeHalf, 0f);
                    rotateAxis = rot * new Vector3(0, 0, -1);
                }
                break;

            case Maze.Direction.Left:
                {
                    nextPoint = currentPoint + rot * new Vector3(-cubeSize, 0f, 0f);
                    rotatePoint = currentPoint + rot * new Vector3(-cubeSizeHalf, -cubeSizeHalf, 0f);
                    rotateAxis = rot * new Vector3(0, 0, 1);
                }
                break;

            case Maze.Direction.Down:
                {
                    nextPoint = currentPoint + rot * new Vector3(0f, 0f, -cubeSize);
                    rotatePoint = currentPoint + rot * new Vector3(0f, -cubeSizeHalf, -cubeSizeHalf);
                    rotateAxis = rot * new Vector3(-1, 0, 0);
                }
                break;

            case Maze.Direction.Up:
                {
                    nextPoint = currentPoint + rot * new Vector3(0f, 0f, cubeSize);
                    rotatePoint = currentPoint + rot * new Vector3(0f, -cubeSizeHalf, cubeSizeHalf);
                    rotateAxis = rot * new Vector3(1, 0, 0);
                }
                break;
        }

        // 入力がない時はコルーチンを呼び出さないようにする
        if (rotatePoint == Vector3.zero)
            return;

        var nextRotation = Quaternion.AngleAxis(90, rotateAxis) * transform.localRotation;
        var nextPos = nextPoint.ToMazePos();

        // いけるかどうか
        var tile = FloorBehaviour.GetInstance().Get(nextPos.x, nextPos.y);
        if (tile == null || tile.tileId != GetSideId(nextRotation))
        {
            //var currentPoint = currentPoint;
            //var currentPos = currentPoint.ToMazePos();
            //Debug.LogFormat("Cannot Move\n  Current(pos={0}, id={1}, rot={2})\n  Next(pos={3}, id={4}, rot={5})\n  CurrentTile(pos={6}, id={7})\n  NextTile(pos={8}, id={9})",
            //    currentPoint, GetSideId(transform.localRotation), transform.localRotation.eulerAngles,
            //    nextPoint, GetSideId(nextRotation), nextRotation.eulerAngles,
            //    currentPos, FloorBehaviour.GetInstance().Get(currentPos.x, currentPos.y).tileId,
            //    nextPos, tile.tileId);

            var modelRenderer = transform.Find("Model").GetComponent<Renderer>();
            var tileId = CubeBehaviour.GetSideId(CubeBehaviour.GetMoveRotation(direction, transform.localRotation));
            StartCoroutine(UnableMove(rotatePoint, rotateAxis, new ChangeColor[]
            {
                (Color diffuse, Color emission)=> { if (tile != null) { tile.material.color = diffuse; tile.material.SetColor("_EmissionColor", emission); } },
                (Color diffuse, Color emission)=> { modelRenderer.materials[tileId].color = diffuse; modelRenderer.materials[tileId].SetColor("_EmissionColor", emission); },
            }));
        }
        else
        {
            StartCoroutine(MoveCube(rotatePoint, rotateAxis));
        }
    }

    public Vector2Int GetCurrentPos()
    {
        return transform.localPosition.ToMazePos();
    }

    void Update()
    {
        {
            Quaternion rot = Quaternion.Euler(0, Mathf.CeilToInt(cameraWrapper.localEulerAngles.y / 90) * 90, 0);
            Maze.Direction? direction = null;
            if (Input.GetKeyDown(KeyCode.RightArrow))
                direction = Maze.Direction.Right;
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                direction = Maze.Direction.Left;
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                direction = Maze.Direction.Down;
            else if (Input.GetKeyDown(KeyCode.UpArrow))
                direction = Maze.Direction.Up;
            if (direction.HasValue)
                Move(direction.Value, rot);
        }
        {
            if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                var tile = hit.collider.GetComponent<Tile>();
                Maze.Direction? direction = null;
                if (tile != null)
                {
                    var pos = tile.pos - transform.localPosition.ToMazePos();
                    if (pos.x == 1 && pos.y == 0)
                        direction = Maze.Direction.Right;
                    else if (pos.x == -1 && pos.y == 0)
                        direction = Maze.Direction.Left;
                    else if (pos.x == 0 && pos.y == 1)
                        direction = Maze.Direction.Down;
                    else if (pos.x == 0 && pos.y == -1)
                        direction = Maze.Direction.Up;
                    if (direction.HasValue)
                        Move(direction.Value, Quaternion.identity);
                }
            }
        }
    }

    delegate void ChangeColor(Color diffuse, Color emission);

    void SnapCube()
    {
        Vector3 euler = transform.localEulerAngles;
        euler.x = Mathf.RoundToInt(euler.x / 90f) * 90f;
        euler.y = Mathf.RoundToInt(euler.y / 90f) * 90f;
        euler.z = Mathf.RoundToInt(euler.z / 90f) * 90f;
        transform.localEulerAngles = euler;

        Vector3 pos = transform.localPosition;
        pos.x = Mathf.RoundToInt(pos.x);
        pos.y = Mathf.FloorToInt(pos.y) + .5f;
        pos.z = Mathf.RoundToInt(pos.z);
        transform.localPosition = pos;
    }

    IEnumerator UnableMove(Vector3 rotatePoint, Vector3 rotateAxis, ChangeColor[] colorApplyees)
    {
        //回転中のフラグを立てる
        isRotate = true;

        //回転処理
        float prog = 0f;
        float vel = UnableCubeAngle;
        float pos = 0f;
        for (; vel > 0 || pos > 0f; vel -= UnableCubeAcc, pos += vel, prog += .5f)
        {
            transform.RotateAround(transform.parent.TransformPoint(rotatePoint), transform.parent.TransformDirection(rotateAxis), vel * 60 * Time.deltaTime);

            //var p = (Mathf.Sin(prog) + 1) / 2;
            //var diffuse = new Color(1, p, p);
            //var emission = new Color(1 - p, 1 - p, 1 - p);
            //foreach (var applyee in colorApplyees)
            //    applyee.Invoke(diffuse, emission);

            yield return null;
        }

        transform.RotateAround(transform.parent.TransformPoint(rotatePoint), transform.parent.TransformDirection(rotateAxis), pos);

        SnapCube();
        foreach (var applyee in colorApplyees)
            applyee.Invoke(Color.white, Color.black);

        //回転中のフラグを倒す
        isRotate = false;

        yield break;
    }

    IEnumerator MoveCube(Vector3 rotatePoint, Vector3 rotateAxis)
    {
        //回転中のフラグを立てる
        isRotate = true;

        //回転処理
        float vel = CubeAngle;
        float pos = 0f; //angleの合計を保存
        while (pos < 90f)
        {
            vel += CubeAcc;
            pos += vel * 60 * Time.deltaTime;

            // 90度以上回転しないように値を制限
            if (pos > 90f)
                vel -= pos - 90f;

            transform.RotateAround(transform.parent.TransformPoint(rotatePoint), transform.parent.TransformDirection(rotateAxis), vel * 60 * Time.deltaTime);

            yield return null;
        }

        SnapCube();

        //回転中のフラグを倒す
        isRotate = false;

        yield break;
    }
}
