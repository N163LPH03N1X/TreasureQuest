using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class UnderWaterSystem : MonoBehaviour
{
    public Image underWaterUI;
    public Image airBar;
    public GameObject airBanner;
    public AudioSource gameAmbience;
    public AudioSource gameMusic;
    public AudioClip underWaterAmbience;
    public AudioClip dayAmbience;
    public AudioClip nightAmbience;
    public GameObject playerController;
    public GameObject waveEffect;
    public ParticleSystem rainParticleSystem;
    PlayerSystem playerSystem;
    CharacterSystem Char;
    float airTime = 100;
    public float drainSpeed = 5;
    public float damageSpeed = 1;
    float airTimer;
    float damageTime = 1;
    float damageTimer;

    IEnumerator fadeAudio = null;

    public static bool isSwimming = false;
    bool shutOffSwimming = true;
    public bool outOfAir;
    public bool aboveWater;

    public void Update()
    {
        if (isSwimming && !SceneSystem.inTransition)
        {
            if (aboveWater)
            {
                if (SceneSystem.isOverWorld)
                {
                    if (TimeSystem.isRain)
                        rainParticleSystem.Play();
                    if (!TimeSystem.isNight)
                        gameAmbience.clip = dayAmbience;
                    else if (TimeSystem.isNight)
                        gameAmbience.clip = nightAmbience;
                }
                else
                {
                    gameAmbience.clip = null;
                }
                SwordSystem swordSystem = playerController.GetComponent<SwordSystem>();
                swordSystem.SheathSword(false);
                waveEffect.SetActive(false);
                outOfAir = false;
                underWaterUI.enabled = false;
                airBanner.SetActive(false);
                ResetAir();
                gameMusic.volume = 1.0f;
              
                if (fadeAudio != null)
                    StopCoroutine(fadeAudio);
                gameAmbience.volume = 0.1f;
                gameAmbience.Play();
            }
            else if (shutOffSwimming && !aboveWater)
            {
                rainParticleSystem.Stop();
                Rigidbody rb = playerController.GetComponent<Rigidbody>();
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                ResetAir();
                underWaterUI.enabled = true;
                airBanner.SetActive(true);
                waveEffect.SetActive(true);
                gameAmbience.clip = underWaterAmbience;
                gameMusic.volume = 0.12f;
                if (fadeAudio != null)
                    StopCoroutine(fadeAudio);
                fadeAudio = AudioFadeIn(gameAmbience);
                StartCoroutine(fadeAudio);
                SwordSystem swordSystem = playerController.GetComponent<SwordSystem>();
                swordSystem.SheathSword(false);
                shutOffSwimming = false;
            }
            
            if (outOfAir)
            {
                if (damageTimer > 0)
                    damageTimer -= Time.deltaTime;
                else if (damageTimer < 0)
                {
                    damageTimer = damageTime;
                    playerSystem = playerController.GetComponent<PlayerSystem>();
                    playerSystem.PlayerDamage(1, false);
                    if (PlayerSystem.playerHealth <= 0)
                        ResetAir();
                }
            }
            else
            {
                damageTimer = damageTime;
                if (airTimer > 0 && !JewelSystem.blueJewelEnabled)
                    airTimer -= Time.deltaTime * drainSpeed;
                else if (airTimer < 0)
                {
                    outOfAir = true;
                    airTimer = 0;
                } 
                Handler();
            }
        }
        else
        {
            if (!shutOffSwimming)
            {
                Rigidbody rb = playerController.GetComponent<Rigidbody>();
                rb.constraints = RigidbodyConstraints.FreezeAll;
                Quaternion setRot = new Quaternion(0, playerController.transform.rotation.y, 0, playerController.transform.rotation.w);
                playerController.transform.rotation = setRot;
                Char = playerController.GetComponent<CharacterSystem>();
                Char.SelectAnimation(CharacterSystem.PlayerAnimation.Rebind, true);
                waveEffect.SetActive(false);
                outOfAir = false;
                underWaterUI.enabled = false;
                airBanner.SetActive(false);
                ResetAir();
                gameMusic.volume = 1.0f;
                if (SceneSystem.isOverWorld)
                {
                    if (TimeSystem.isRain)
                        rainParticleSystem.Play();
                    if (!TimeSystem.isNight)
                        gameAmbience.clip = dayAmbience;
                    else if (TimeSystem.isNight)
                        gameAmbience.clip = nightAmbience;
                }
                else
                {
                    gameAmbience.clip = null;
                }
                if (fadeAudio != null)
                    StopCoroutine(fadeAudio);
                gameAmbience.volume = 0.1f;
                gameAmbience.Play();
                aboveWater = false;
                shutOffSwimming = true;
            }
        }
    }
    public void ResetAir()
    {
        outOfAir = false;
        damageTimer = damageTime;
        airTimer = airTime;
        Handler();
    }
    public void Handler()
    {
        airBar.fillAmount = Map(airTimer, 0, 100, 0, 1);
    }
    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
    public void SwimmingActive(bool True)
    {
        if (True)
            isSwimming = true;
        else
            isSwimming = false;
    }
    IEnumerator AudioFadeOut(AudioSource audio)
    {
        audio.volume = 1.0f;
        float MinVol = 0;
        for (float f = 1f; f > MinVol; f -= 0.05f)
        {
            audio.volume = f;
            yield return new WaitForSecondsRealtime(.01f);
            if (f <= 0.1)
            {
                audio.Stop();
                audio.volume = MinVol;
            }
        }
    }
    IEnumerator AudioFadeIn(AudioSource audio)
    {
        audio.volume = 0.0f;
        float MaxVol = 1;
        audio.Play();
        for (float f = 0f; f < MaxVol; f += 0.05f)
        {
            audio.volume = f;
            yield return new WaitForSecondsRealtime(.01f);
            if (f >= 0.9)
            {
                audio.volume = MaxVol;
            }
        }
    }
    public void StartSwimming(bool True)
    {
        if (True)
        {
            if (PlayerSystem.stormBootEnabled)
            {
                aboveWater = false;
                isSwimming = true;
            }
            else if (!PlayerSystem.stormBootEnabled && !SceneSystem.inTransition && !SceneSystem.isDisabled)
            {
                playerSystem = playerController.GetComponent<PlayerSystem>();
                playerSystem.PlayerDamage(500, false);
                Char = playerController.GetComponent<CharacterSystem>();
                Char.SetPosition();
                playerSystem.LoadPosition();
                isSwimming = false;
                aboveWater = false;
            }

        }
        else 
        { 
            isSwimming = false; 
            aboveWater = false;
            if (DebugSystem.isDebugMode)
            {
                DebugSystem debugSystem = playerController.GetComponent<DebugSystem>();
                debugSystem.EnableCode(DebugSystem.Code.SwimmingOff);
            }
        }
    }
    public void DebugSwimming(bool active)
    {
        if (active)
        {
            aboveWater = false;
            isSwimming = true;
        }
        else
        {
            isSwimming = false;
            aboveWater = false;
        }
    }
    public void AboveWater(bool active)
    {
        if (active)
        {
            if (PlayerSystem.stormBootEnabled)
            {
                isSwimming = true;
                aboveWater = true;
            }
            else if (!PlayerSystem.stormBootEnabled && !SceneSystem.inTransition && !SceneSystem.isDisabled)
            {
                playerSystem = playerController.GetComponent<PlayerSystem>();
                playerSystem.PlayerDamage(100, false);
                Char = playerController.GetComponent<CharacterSystem>();
                Char.SetPosition();
                playerSystem.LoadPosition();
                isSwimming = false;
            }
        }
        else
        {
            aboveWater = false;
            Quaternion setRot = new Quaternion(0, playerController.transform.rotation.y, 0, playerController.transform.rotation.w);
            playerController.transform.rotation = setRot;
        }
    }
    public void AirBannerActive(bool True)
    {
        if (True)
            airBanner.SetActive(true);
        else
            airBanner.SetActive(false);
    }
}
