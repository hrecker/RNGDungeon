using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantVelocity : MonoBehaviour
{
    public Vector2 velocity;

    void Update()
    {
        transform.Translate(velocity * Time.deltaTime);
    }
}
