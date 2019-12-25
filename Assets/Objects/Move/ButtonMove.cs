using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonMove : MonoBehaviour
{
    public UnityEvent onPressed;
    public bool Pressed { get; set; }

    void Update()
    {
        if (Pressed)
            onPressed.Invoke();
    }
}
