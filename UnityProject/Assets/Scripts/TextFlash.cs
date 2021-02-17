using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFlash : MonoBehaviour
{
    Text gameText;
    // Start is called before the first frame update
    void Start()
    {
        gameText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        gameText.color = new Color(gameText.color.r, gameText.color.g, gameText.color.b, Mathf.PingPong(Time.unscaledTime, 1));
    }
}
