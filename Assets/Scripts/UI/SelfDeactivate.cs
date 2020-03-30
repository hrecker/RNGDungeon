using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDeactivate : MonoBehaviour
{
    public float lifetime;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            gameObject.SetActive(false);
            ResetTimer();
        }
    }

    public void ResetTimer()
    {
        timer = 0.0f;
    }
}
