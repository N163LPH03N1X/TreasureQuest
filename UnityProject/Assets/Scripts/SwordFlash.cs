using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordFlash : MonoBehaviour
{
    MeshRenderer meshRend;
    public bool Terra;
    public bool Frost;
    public bool Lightning;
    public bool Flare;


    // Start is called before the first frame update
    void Start()
    {
        meshRend = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Terra)
        {
            Color color = new Color(Mathf.PerlinNoise(Time.time, 1), 1, Mathf.PerlinNoise(Time.time, 1), Mathf.PerlinNoise(Time.time, 1));
            meshRend.material.SetColor("_EmissionColor", color);
        }
        else if (Frost)
        {
            Color color = new Color(Mathf.PerlinNoise(Time.time, 1), Mathf.PerlinNoise(Time.time, 1), 1, Mathf.PerlinNoise(Time.time, 1));
            meshRend.material.SetColor("_EmissionColor", color);
        }
        else if (Lightning)
        {
            Color color = new Color(Mathf.PerlinNoise(Time.time, 1), Mathf.PerlinNoise(Time.time, 1), Mathf.PerlinNoise(Time.time, 1), Mathf.PerlinNoise(Time.time, 1));
            meshRend.material.SetColor("_EmissionColor", color);
        }
        else if (Flare)
        {
            Color color = new Color(1, Mathf.PerlinNoise(Time.time, 1), Mathf.PerlinNoise(Time.time, 1), Mathf.PerlinNoise(Time.time, 1));
            meshRend.material.SetColor("_EmissionColor", color);
        }
      
    }
}
