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
            ObjectSystem objectSystem = GameObject.Find("Core").GetComponent<ObjectSystem>();
            isOn = true;
            if (!enemiesActive)
            {
                TurnOnEnemies();
                objectSystem.SetActiveObject(currentSection, ObjectSystem.gameEnemySection);
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
        for(int e = 0; e <  sections.prefabs.Length; e++)
        {
            enemySection = Instantiate(sections.prefabs[e], transform.position, transform.rotation);
            enemySection.transform.parent = transform;
            numOfEnemies = enemySection.transform.childCount;
            enemiesActive = true;
        }
    }
    public void CheckSwitchSystem(int section)
    {
        if (!isChecked)
        {
            for (int s = 0; s < ObjectSystem.gameEnemySection.Length; s++)
            {
                if (s == section)
                {
                    if (ObjectSystem.gameEnemySection[s])
                    {
                        TurnOnEnemies();
                        isChecked = true;
                        isOn = true;
                        break;
                    }
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