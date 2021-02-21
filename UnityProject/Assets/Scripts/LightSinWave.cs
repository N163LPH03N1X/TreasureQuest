using UnityEngine;

public class LightSinWave : MonoBehaviour
{
    // Start is called before the first frame update

    Light ThisLight = new Light();
    Color color;
    void Start()
    {
        ThisLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        color = new Color(Mathf.PingPong(Time.time * 0.5f, 1), Mathf.PingPong(Time.time * 0.5f, 1), Mathf.PingPong(Time.time * 0.5f, 1), 1);
        ThisLight.color = color;
    }
}
