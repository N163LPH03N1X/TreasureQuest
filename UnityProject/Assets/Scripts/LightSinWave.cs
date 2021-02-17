using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSinWave : MonoBehaviour
{
    // Start is called before the first frame update

    Light light;
    Color color;
    void Start()
    {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        color = new Color(Mathf.PingPong(Time.time * 0.5f, 1), Mathf.PingPong(Time.time * 0.5f, 1), Mathf.PingPong(Time.time * 0.5f, 1), 1);
        light.color = color;
    }
}
