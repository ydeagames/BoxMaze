using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Faces : ScriptableObject
{
    public Face face00;
    public Face face01;
    public Face face10;
    public Face face11;
    public Face face20;
    public Face face21;

    public int[] faceIndex = { 0, 1, 2, 3, 4, 5 };
    public Face[] faces
    {
        get
        {
            return new Face[] { face00, face01, face10, face11, face20, face21 };
        }
    }
    public Face GetFace(int id)
    {
        return faces[faceIndex[id]];
    }
}
