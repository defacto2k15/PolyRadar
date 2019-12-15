using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sinewavescript : MonoBehaviour
{
    Transform[] cubes;
    float t = 0;
    public float increment = 0.01f;
    public float amplitude = 1f;
    public float frequency = 1f;
    public float phase = 0f;
    public float scale = 5f;
    // Start is called before the first frame update
    void Start()
    {
        cubes = GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        t += increment;
        foreach (Transform T in cubes)
        {
            Vector3 newPosition = T.position;
            newPosition.y = amplitude * Mathf.Sin(frequency * t + phase + T.position.x / scale);
            T.position = newPosition;
        }
    }
}
