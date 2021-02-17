using UnityEngine;

public class TorchSystem : MonoBehaviour
{
    public bool fixedTorch;
    Light pointLight;
    ParticleSystem ps;
    bool disableTorch;
   
    void Update()
    {
        if (!TimeSystem.isNight && !disableTorch && !fixedTorch)
        {
            ActivateTorch(false);
            disableTorch = true;
        }
        else if(TimeSystem.isNight && disableTorch && !fixedTorch)
        {
            ActivateTorch(true);
            disableTorch = false;
        }
    
    }
    public void ActivateTorch(bool active)
    {
        if (!active)
        {
            GameObject particleObj = transform.GetChild(0).gameObject;
            GameObject lightObj = particleObj.transform.GetChild(0).gameObject;
            pointLight = lightObj.GetComponent<Light>();
            ps = particleObj.GetComponent<ParticleSystem>();
            ps.Stop();
            pointLight.enabled = false;
        }
        else
        {
            GameObject particleObj = transform.GetChild(0).gameObject;
            GameObject lightObj = particleObj.transform.GetChild(0).gameObject;
            pointLight = lightObj.GetComponent<Light>();
            ps = particleObj.GetComponent<ParticleSystem>();
            ps.Play();
            pointLight.enabled = true;
        }
    }
   
}
