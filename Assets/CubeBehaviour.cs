using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehaviour : MonoBehaviour
{
    public Transform cameraWrapper;

    public float cubeAngle = 15f;   // ここを変えると回転速度が変わる

    float cubeSize = 1f;           // キューブの大きさ
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

        for (int i = 0; i  < sides.Length; i++)
        {
            var side = sides[i];
            float dist = Vector3.Dot(Vector3.down, rotation * side);
            if (!min.HasValue || dist < min.Value.Item2)
                min = (i, dist);
        }

        return min?.Item1 ?? 0;
    }

    void Start()
    {
    }

    void Update()
    {
        //回転中は入力を受け付けない
        if (isRotate)
            return;

        var currentPoint = transform.localPosition;
        var nextPoint = Vector3.zero;
        var rotatePoint = Vector3.zero;
        var rotateAxis = Vector3.zero;

        Quaternion rot = Quaternion.Euler(0, Mathf.FloorToInt(cameraWrapper.localEulerAngles.y / 90) * 90, 0);
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            nextPoint = currentPoint + rot * new Vector3(cubeSize, 0f, 0f);
            rotatePoint = currentPoint + rot * new Vector3(cubeSizeHalf, -cubeSizeHalf, 0f);
            rotateAxis = rot * new Vector3(0, 0, -1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            nextPoint = currentPoint + rot * new Vector3(-cubeSize, 0f, 0f);
            rotatePoint = currentPoint + rot * new Vector3(-cubeSizeHalf, -cubeSizeHalf, 0f);
            rotateAxis = rot * new Vector3(0, 0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            nextPoint = currentPoint + rot * new Vector3(0f, 0f, -cubeSize);
            rotatePoint = currentPoint + rot * new Vector3(0f, -cubeSizeHalf, -cubeSizeHalf);
            rotateAxis = rot * new Vector3(-1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            nextPoint = currentPoint + rot * new Vector3(0f, 0f, cubeSize);
            rotatePoint = currentPoint + rot * new Vector3(0f, -cubeSizeHalf, cubeSizeHalf);
            rotateAxis = rot * new Vector3(1, 0, 0);
        }

        // 入力がない時はコルーチンを呼び出さないようにする
        if (rotatePoint == Vector3.zero)
            return;

        var nextRotation = Quaternion.AngleAxis(90, rotateAxis) * transform.rotation;
        var nextPos = nextPoint.ToMazePos();

        // いけるかどうか
        var tile = FloorBehaviour.GetInstance().Get(nextPos.x, nextPos.y);
        if (tile == null)
            return;
        if (tile.tileId != GetSideId(nextRotation))
        {
            //var currentPoint = currentPoint;
            //var currentPos = currentPoint.ToMazePos();
            //Debug.LogFormat("Cannot Move\n  Current(pos={0}, id={1}, rot={2})\n  Next(pos={3}, id={4}, rot={5})\n  CurrentTile(pos={6}, id={7})\n  NextTile(pos={8}, id={9})",
            //    currentPoint, GetSideId(transform.rotation), transform.rotation.eulerAngles,
            //    nextPoint, GetSideId(nextRotation), nextRotation.eulerAngles,
            //    currentPos, FloorBehaviour.GetInstance().Get(currentPos.x, currentPos.y).tileId,
            //    nextPos, tile.tileId);

            return;
        }

        StartCoroutine(MoveCube(rotatePoint, rotateAxis));
    }

    IEnumerator MoveCube(Vector3 rotatePoint, Vector3 rotateAxis)
    {
        //回転中のフラグを立てる
        isRotate = true;

        //回転処理
        float sumAngle = 0f; //angleの合計を保存
        while (sumAngle < 90f)
        {
            sumAngle += cubeAngle;

            // 90度以上回転しないように値を制限
            if (sumAngle > 90f)
                cubeAngle -= sumAngle - 90f;

            transform.RotateAround(transform.parent.TransformPoint(rotatePoint), transform.parent.TransformDirection(rotateAxis), cubeAngle);

            yield return null;
        }

        //回転中のフラグを倒す
        isRotate = false;

        yield break;
    }
}
