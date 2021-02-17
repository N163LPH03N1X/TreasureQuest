using UnityEngine;
using System;
[RequireComponent(typeof(BoxCollider))]
public class EnemyTerritoryActivator : MonoBehaviour
{
    public int currentSection;
    public EnemySections sections;
    GameObject enemySection;
    BoxCollider boxCollider;
    bool enemiesActive = false;
    int numOfEnemies;
    public bool dontRespawn;
    bool isChecked;
    bool isOn;
    public bool exitEnabled;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        isOn = false;
    }
    public void Update()
    {
        if(!isOn)
            CheckSwitchSystem(currentSection);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isOn = true;
            if (!enemiesActive)
            {
                    foreach (GameObject section in sections.prefabs)
                    {
                        TurnOnEnemies();
                        ObjectSystem objectSystem = GameObject.Find("Core").GetComponent<ObjectSystem>();
                        objectSystem.SetEnemySectionActive(currentSection);
                    }
            }

            else if (enemiesActive)
            {
                if (enemySection != null && enemySection.transform.childCount < numOfEnemies)
                {
                    Destroy(enemySection);
                    GameObject enemyUIObjs = GameObject.Find("Core/Player/PopUpCanvas/EnemyUIStorage");
                    foreach (Transform child in enemyUIObjs.transform)
                        Destroy(child.gameObject);
                    if (!dontRespawn)
                        enemiesActive = false;
                }
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && exitEnabled)
        {
            if (enemySection != null && enemySection.transform.childCount <= numOfEnemies)
            {
                Destroy(enemySection);
                GameObject enemyUIObjs = GameObject.Find("Core/Player/PopUpCanvas/EnemyUIStorage");
                foreach (Transform child in enemyUIObjs.transform)
                    Destroy(child.gameObject);
                if (!dontRespawn)
                    enemiesActive = false;
            }
        }
    }

    public void TurnOnEnemies()
    {
        foreach (GameObject section in sections.prefabs)
        {
            enemySection = Instantiate(section, transform.position, transform.rotation) as GameObject;
            enemySection.transform.parent = transform;
            numOfEnemies = enemySection.transform.childCount;
            enemiesActive = true;
        }
    }
    public void CheckSwitchSystem(int section)
    {
        if (!isChecked)
        {
            if (section == 1)
            {
                if (ObjectSystem.enemySection1)
                {
                    TurnOnEnemies();
                    isChecked = true;
                    isOn = true;
                }
            }
            else if (section == 2)
            {
                if (ObjectSystem.enemySection2)
                {
                    TurnOnEnemies();
                    isChecked = true;
                    isOn = true;
                }
            }
            else if (section == 3)
            {
                if (ObjectSystem.enemySection3)
                {
                    TurnOnEnemies();
                    isChecked = true;
                    isOn = true;
                }
            }
            else if (section == 4)
            {
                if (ObjectSystem.enemySection4)
                {
                    TurnOnEnemies();
                    isChecked = true;
                    isOn = true;
                }
            }
            else if (section == 5)
            {
                if (ObjectSystem.enemySection5)
                {
                    TurnOnEnemies();
                    isChecked = true;
                    isOn = true;
                }
            }
            else if (section == 6)
            {
                if (ObjectSystem.enemySection6)
                {
                    TurnOnEnemies();
                    isChecked = true;
                    isOn = true;
                }
            }
            else if (section == 7)
            {
                if (ObjectSystem.enemySection7)
                {
                    TurnOnEnemies();
                    isChecked = true;
                    isOn = true;
                }
            }
            else if (section == 8)
            {
                if (ObjectSystem.enemySection8)
                {
                    TurnOnEnemies();
                    isChecked = true;
                    isOn = true;
                }
            }
            else if (section == 9)
            {
                if (ObjectSystem.enemySection9)
                {
                    TurnOnEnemies();
                    isChecked = true;
                    isOn = true;
                }
            }
            else if (section == 10)
            {
                if (ObjectSystem.enemySection10)
                {
                    TurnOnEnemies();
                    isChecked = true;
                    isOn = true;
                }
            }
            else if (section == 11)
            {
                if (ObjectSystem.enemySection11)
                {
                    TurnOnEnemies();
                    isChecked = true;
                    isOn = true;
                }
            }
            else if (section == 12)
            {
                if (ObjectSystem.enemySection12)
                {
                    TurnOnEnemies();
                    isChecked = true;
                    isOn = true;
                }
            }
            else if (section == 13)
            {
                if (ObjectSystem.enemySection13)
                {
                    TurnOnEnemies();
                    isChecked = true;
                    isOn = true;
                }
            }
            else if (section == 14)
            {
                if (ObjectSystem.enemySection14)
                {
                    TurnOnEnemies();
                    isChecked = true;
                    isOn = true;
                }
            }
            else if (section == 15)
            {
                if (ObjectSystem.enemySection15)
                {
                    TurnOnEnemies();
                    isChecked = true;
                    isOn = true;
                }
            }
            else if (section == 16)
            {
                if (ObjectSystem.enemySection16)
                {
                    TurnOnEnemies();
                    isChecked = true;
                    isOn = true;
                }
            }
            else if (section == 17)
            {
                if (ObjectSystem.enemySection17)
                {
                    TurnOnEnemies();
                    isChecked = true;
                    isOn = true;
                }
            }
            else if (section == 18)
            {
                if (ObjectSystem.enemySection18)
                {
                    TurnOnEnemies();
                    isChecked = true;
                    isOn = true;
                }
            }
        }
    }
}
[Serializable]
public struct EnemySections
{
    public GameObject[] prefabs;
}