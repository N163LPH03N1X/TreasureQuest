using System.Collections;
using UnityEngine;
public class ItemDisappear : MonoBehaviour
{
    bool On = false;
    public float timer;
    public MeshRenderer[] meshRend;

	void Update ()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if (timer < 0 )
        {
            StartCoroutine(Fading());
            timer = 0;
        }
	}
    IEnumerator Fading()
    {
        for(float i = 0.25f; i > 0; i -= 0.01f)
        {
            if (i < 0.1f && meshRend != null)
            {
                Destroy(gameObject);
            }
            else
            {
                if (On && meshRend != null)
                {
                    MeshRendTurnOn(true);
                    yield return new WaitForSeconds(i);
                    On = false;
                }

                else if(meshRend != null)
                {
                    MeshRendTurnOn(false);
                    yield return new WaitForSeconds(i);
                    On = true;
                }
            }
        }
    }
    void MeshRendTurnOn(bool On)
    {
        if (On && meshRend != null)
        {
            for (int i = 0; i < meshRend.Length; i++)
            {
                meshRend[i].enabled = true;
            }
        }
        else if(meshRend != null)
        {
            for (int i = 0; i < meshRend.Length; i++)
            {
                meshRend[i].enabled = false;
            }
        }
    }
}
