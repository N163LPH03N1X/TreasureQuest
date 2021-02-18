using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class TimeSystem : MonoBehaviour 
{
   
    public bool isPermanentRain = false;
    public bool setDebug;
    public static bool isDebug;
    public Text timeOfDayDisplay;
    public AudioClip ambientDay;
    public AudioClip ambientNight;
    public AudioClip ambientStorm;
    public Light Sun;
    public Light Moon;
    public int rainIncrement = 4;
    public int currentIncrement;
    ParticleSystem rainSystem;
    public GameObject playerRain;
    GameObject clouds;

    public float sunRotation = -90;

    bool isRainActive;
    public Material[] Skyboxes;
    public float SecondsInDay = 300f;
   
    [Range(0,1)]
    public float TimeMultiplier = 1.0f;
    public static int timeInterval = 0;
    public float SunInitialIntensity;
    public float MoonInitialIntensity;

    float SunCurrentIntensity;
    float MoonCurrentIntensity;

    bool switchDayEvents = false;
    bool switchNightEvents = true;

    Quaternion orgSunRot;
    Quaternion resetSunRot = new Quaternion(-90, 90, 0, 0);
    IEnumerator timer = null;
    public static bool totalGameTimeRunning = false;

    public static float CurrentTimeOfDay = 0f;
    public static int hour = 12;
    public static int minute = 0;
    public static int day = 6;

    public static bool isNight = true;
    public static bool isShophours;
    public static bool isRain = false;
    bool isSleepTimeSet = false;
    public static string dayNightDisplay = "PM";
    public string[] weekDay;
    public Text weatherText;
    public string currentWeather = "(Clear)";

    [Header("Game Time Settings")]
    public Text gameTimeText;

    public static float totalGameTime;
   
    public int gameSeconds;
    public int gameMinutes;
    public int gameHours;
    public int gameDays;

    bool isTimeSkiped;
    bool skipInterval;

    bool isGhostShipActive;

    void Start ()
    {
        if (setDebug)
            isDebug = true;
        else
            isDebug = false;
       
        SunInitialIntensity = Sun.intensity;
        MoonInitialIntensity = Moon.intensity;
        isShophours = false;
        dayNightDisplay = "AM";
        GamePlayTimeActive(true);
        isGameTimeRunning(true);
    }
    public void isGameTimeRunning(bool active)
    {
        totalGameTimeRunning = active;
    }
    public void SetJewelInterval(bool active)
    {
        skipInterval = active;
    }
    public void GamePlayTimeActive(bool active)
    {
        if (active)
        {
            timer = PlayTimer();
            StartCoroutine(timer);
        }
        else
        {
            if(timer != null)
                StopCoroutine(timer);
            timer = null;
        }
    }
    public void SetSleepingTime(float timeofDay)
    {
        timeInterval = 360;
        CurrentTimeOfDay = timeofDay;
        minute = 0;
        hour = 6;
        day++;
           if (day > 6)
                day = 0;
        currentIncrement = Random.Range(0, rainIncrement);
        if (currentIncrement == 1)
            isRain = true;
        else
            isRain = false;
        isNight = false;
        dayNightDisplay = "AM";
        FixTimeOfDay(CurrentTimeOfDay);
        isSleepTimeSet = true;

    }
	void Update ()
    {
        if (isRain && isNight && !isGhostShipActive)
        {
            SetGhostShip(true);
            isGhostShipActive = true;
        }
        else if (!isRain && isNight && isGhostShipActive)
        {
            SetGhostShip(false);
            isGhostShipActive = false;
        }
        else if (isRain && !isNight && isGhostShipActive)
        {
            SetGhostShip(false);
            isGhostShipActive = false;
        }
        else if (!isRain && !isNight && isGhostShipActive)
        {
            SetGhostShip(false);
            isGhostShipActive = false;
        }

        if (!SceneSystem.isDisabled && !PauseGame.isPaused && !ShopSystem.isShop && !DialogueSystem.isDialogueActive && !isPermanentRain && !ShopSystem.isSleeping)
        {
            isSleepTimeSet = false;
            CurrentTimeOfDay += Time.deltaTime / SecondsInDay * TimeMultiplier;
            FixTimeOfDay(CurrentTimeOfDay);
        }
        else if (ShopSystem.isSleeping)
            if (!isSleepTimeSet)
                SetSleepingTime(0.25f);

        if (JewelSystem.yellowJewelEnabled && JewelSystem.isJewelActive)
        {
            if(isTimeSkiped)
                SecondsInDay = 30;
            
            if(hour == 6 && minute == 0 && dayNightDisplay == "AM" && skipInterval)
            {
                JewelSystem jewelSys = GameObject.FindGameObjectWithTag("Player").GetComponent<JewelSystem>();
                jewelSys.ShutOffJewelActive();
                SetJewelInterval(false);
                isTimeSkiped = false;
                SecondsInDay = 300;

            }
            else if(hour == 12 && minute == 0 && dayNightDisplay == "PM" && skipInterval)
            {
                JewelSystem jewelSys = GameObject.FindGameObjectWithTag("Player").GetComponent<JewelSystem>();
                jewelSys.ShutOffJewelActive();
                SetJewelInterval(false);
                isTimeSkiped = false;
                SecondsInDay = 300;

            }
            else if (hour == 6 && minute == 0 && dayNightDisplay == "PM" && skipInterval)
            {
                JewelSystem jewelSys = GameObject.FindGameObjectWithTag("Player").GetComponent<JewelSystem>();
                jewelSys.ShutOffJewelActive();
                SetJewelInterval(false);
                isTimeSkiped = false;
                SecondsInDay = 300;

            }
            else if (hour == 12 && minute == 0 && dayNightDisplay == "AM" && skipInterval)
            {
                JewelSystem jewelSys = GameObject.FindGameObjectWithTag("Player").GetComponent<JewelSystem>();
                jewelSys.ShutOffJewelActive();
                SetJewelInterval(false);
                isTimeSkiped = false;
                SecondsInDay = 300;
            }

        }
        if (SceneSystem.overWorldEntered || isPermanentRain || isDebug)
        {
            

            if (isRain && !isRainActive && !isPermanentRain)
                RainChecker(true);
            else if (!isRain && isRainActive && !isPermanentRain)
                RainChecker(false);
            else if (isPermanentRain && !isRainActive)
                RainChecker(true);
            if (CurrentTimeOfDay > 1.0f)
                CurrentTimeOfDay = 0.0f;
            if(SecondsInDay != 0)
                orgSunRot = Quaternion.Euler((CurrentTimeOfDay * 360) + resetSunRot.x, resetSunRot.y, resetSunRot.z);
            Sun.transform.rotation = orgSunRot;
            UpdateSun();
            //SetTimeInterval();
          
            //====================Night=12AM=====================//
            if (CurrentTimeOfDay >= 0 && CurrentTimeOfDay < 0.25f)
            {
                if (RenderSettings.skybox != Skyboxes[1])
                {
                    RenderSettings.skybox = Skyboxes[1];
                    DynamicGI.UpdateEnvironment();
                }
                else if (RenderSettings.skybox == null)
                {
                    RenderSettings.skybox = Skyboxes[1];
                    DynamicGI.UpdateEnvironment();
                }

            }
            //====================Morning=6AM====================//
            if (CurrentTimeOfDay >= 0.25f && CurrentTimeOfDay < 0.5f)
            {
                if (RenderSettings.skybox != Skyboxes[0])
                {
                    RenderSettings.skybox = Skyboxes[0];
                    DynamicGI.UpdateEnvironment();
                }
                else if (RenderSettings.skybox == null)
                {
                    RenderSettings.skybox = Skyboxes[0];
                    DynamicGI.UpdateEnvironment();
                }
                if (!switchDayEvents)
                {
                    UpdateTimeOfDay(Day.morning);
                    switchDayEvents = true;
                }
               
            }
            //====================Noon=12PM=====================//
            if (CurrentTimeOfDay >= 0.5f && CurrentTimeOfDay < 0.75f)
            {
                if (RenderSettings.skybox != Skyboxes[0])
                {
                    RenderSettings.skybox = Skyboxes[0];
                    DynamicGI.UpdateEnvironment();
                }
                else if(RenderSettings.skybox == null)
                {
                    RenderSettings.skybox = Skyboxes[0];
                    DynamicGI.UpdateEnvironment();
                }
               
            }
            //====================Evening=6PM======================//
            if (CurrentTimeOfDay >= 0.75f && CurrentTimeOfDay < 1f)
            {
                if (RenderSettings.skybox != Skyboxes[1])
                {
                    RenderSettings.skybox = Skyboxes[1];
                    DynamicGI.UpdateEnvironment();
                }
                else if (RenderSettings.skybox == null)
                {
                    RenderSettings.skybox = Skyboxes[1];
                    DynamicGI.UpdateEnvironment();
                }
                if (switchDayEvents)
                {
                    UpdateTimeOfDay(Day.evening);
                    switchDayEvents = false;
                }
               
              
            }
        }
    }
    public void SetSkybox(int num)
    {
        RenderSettings.skybox = Skyboxes[num];
        DynamicGI.UpdateEnvironment();
    }
    public void PermaRain(bool active)
    {
        isPermanentRain = active;
    }
    public IEnumerator PlayTimer()
    {

        while (totalGameTimeRunning)
        {
            yield return new WaitForSecondsRealtime(1);
            totalGameTime += 1;
            SetTime(totalGameTime);
        }
    }
    public void SetTime(float amount)
    {
        gameSeconds = Mathf.FloorToInt(amount % 60);
        gameMinutes = Mathf.FloorToInt((amount / 60) % 60);
        gameHours = Mathf.FloorToInt((amount / 3600f) % 24);
        gameDays = Mathf.FloorToInt(amount / 86400);
        gameTimeText.text = string.Format("({0:00}:{1:00}:{2:00}:{3:00})", gameDays, gameHours, gameMinutes, gameSeconds);

    }
    public void SetGhostShip(bool True)
    {
        GameObject ghostShip = GameObject.Find("OverWorld/NonStatic/GhostShip");
        if (ghostShip != null)
        {
            if (True)
                ghostShip.SetActive(true);
            else
                ghostShip.SetActive(false);
        }
    }
    public void RainChecker(bool True)
    {
        if (True)
        {
            
            rainSystem = playerRain.GetComponent<ParticleSystem>();
            rainSystem.Play();
            if (SceneSystem.overWorldEntered || isDebug)
                clouds = GameObject.Find("OverWorld/NonStatic/Clouds");
            else if (SceneSystem.isIntroScene)
                clouds = GameObject.Find("IntroScene/NonStatic/Clouds");
            if (clouds != null)
            {
                if (!clouds.activeInHierarchy)
                    clouds.SetActive(true);
                ParticleSystem ps = clouds.GetComponent<ParticleSystem>();
                ps.Play();
            }
            if (SceneSystem.isOverWorld)
            {
                AudioSystem.PlayAltAudioSource(1, ambientStorm, 0.8f, 1, true);
            }
            currentWeather = "(Rain)";
            weatherText.text = currentWeather;
            isRainActive = true;
        }
        else
        {
            if (SceneSystem.isOverWorld && !isNight)
            {
                AudioSystem.PlayAltAudioSource(1, ambientDay, 0.1f, 1, true);
            }
            else if(SceneSystem.isOverWorld && isNight)
            {
                AudioSystem.PlayAltAudioSource(1, ambientNight, 0.1f, 1, true);
            }
            rainSystem = playerRain.GetComponent<ParticleSystem>();
            rainSystem.Stop();
            if (SceneSystem.overWorldEntered)
                clouds = GameObject.Find("OverWorld/NonStatic/Clouds");
            else if (SceneSystem.isIntroScene)
                clouds = GameObject.Find("IntroScene/NonStatic/Clouds");
            if (clouds != null)
            {
                ParticleSystem ps = clouds.GetComponent<ParticleSystem>();
                ps.Stop();
            }
            currentWeather = "(Clear)";
            weatherText.text = currentWeather;
            isRainActive = false;
        }
   
    }
    public void FixTimeOfDay(float timeOfDay)
    {
        //=================================== SetMinutes ==========================//
        float minuteInterval = 0.00069444444f;
        if (timeOfDay >= minuteInterval * timeInterval && timeOfDay < minuteInterval * timeInterval + 1)
        {
            minute++;
            if (minute > 59)
            {
                minute = 0;
                hour++;
                if (hour == 6 && minute == 0 && isNight)
                    isNight = false;
                else if (hour == 6 && minute == 0 && !isNight)
                    isNight = true;

                else if (hour == 12 && minute == 0 && dayNightDisplay == "PM")
                {
                    if (isNight)
                    {
                        dayNightDisplay = "AM";
                        currentIncrement = Random.Range(0, rainIncrement);
                        if (currentIncrement == 1)
                            isRain = true;
                        else
                            isRain = false;
                        day++;
                        if (day > 6)
                            day = 0;
                    }
                }
                else if (hour == 12 && minute == 0 && dayNightDisplay == "AM")
                {

                    if (!isNight)
                    {
                        dayNightDisplay = "PM";
                        currentIncrement = Random.Range(0, rainIncrement);
                        if (currentIncrement == 1)
                            isRain = true;
                        else
                            isRain = false;
                    }
                  
                }
                if (hour > 12)
                    hour = 1;


            }
            timeInterval++;
            if (timeInterval > 1440)
                timeInterval = 0;
            
        }

        timeOfDayDisplay.text = string.Format("({0:0}:{1:00}", hour, minute) + " " + dayNightDisplay + " " + weekDay[day] + ")";
    }
    public enum Day { morning, noon, evening, night}
    public void UpdateTimeOfDay(Day period)
    {
        switch (period)
        {
            //======12AM=======//
            case Day.night:
                {
                    break;
                }
            //======6AM=======//
            case Day.morning:
                {
                    isShophours = true;
                    if (!isRain && SceneSystem.isOverWorld)
                        AudioSystem.PlayAltAudioSource(1, ambientDay, 0.1f, 1, true);
                    break;
                }
            //======12PM=======//
            case Day.noon:
                {
                    break;
                }
            //======6PM=======//
            case Day.evening:
                {
                    isShophours = false;
                    if (!isRain && SceneSystem.isOverWorld)
                        AudioSystem.PlayAltAudioSource(1, ambientNight, 0.1f, 1, true);
                    break;
                }
        }
    }
    void UpdateSun()
    {
        float SunIntensityMultiplier = 1;

        if (CurrentTimeOfDay <= 0.23f || CurrentTimeOfDay >= 0.75f)
            SunIntensityMultiplier = 0;
        else if (CurrentTimeOfDay <= 0.25f)
            SunIntensityMultiplier = Mathf.Clamp01((CurrentTimeOfDay - 0.23f) * (1 / 0.02f));
        else if (CurrentTimeOfDay >= 0.73f)
            SunIntensityMultiplier = Mathf.Clamp01(1 - ((CurrentTimeOfDay - 0.73f) * (1 / 0.02f)));

        Sun.intensity = SunInitialIntensity * SunIntensityMultiplier;
        Moon.intensity = MoonInitialIntensity - (Sun.intensity * MoonInitialIntensity);

        SunCurrentIntensity = Sun.intensity;
        MoonCurrentIntensity = Moon.intensity;
    }
    public void LoadTimeOfDay(float totalTime, float currentTimeOfDayNum, int timeIntervalNum, int dayNum, int hourNum, int minuteNum, bool night, bool rain, string dayNightDisp)
    {
        totalGameTime = totalTime;
        CurrentTimeOfDay = currentTimeOfDayNum;
        day = dayNum;
        hour = hourNum;
        minute = minuteNum;
        isNight = night;
        isRain = rain;
        dayNightDisplay = dayNightDisp;
        timeInterval = timeIntervalNum; 
        FixTimeOfDay(CurrentTimeOfDay);
    }
  
    public void SetTimeSpecifics()
    {
        isTimeSkiped = true;
    }
   
   
    public void resetTimeSystem()
    {
        RenderSettings.skybox = Skyboxes[1];
        DynamicGI.UpdateEnvironment();
        isGameTimeRunning(false);
        GamePlayTimeActive(false);
        totalGameTime = 0;
        SunInitialIntensity = Sun.intensity;
        MoonInitialIntensity = Moon.intensity;
        dayNightDisplay = "PM";
        currentWeather = "(Clear)";
        isRain = false;
        isNight = true;
        switchDayEvents = false;
        switchNightEvents = true;
        gameDays = 0;
        gameHours = 0;
        gameMinutes = 0;
        gameSeconds = 0;
        day = 6;
        hour = 12;
        minute = 0;
    }
}
