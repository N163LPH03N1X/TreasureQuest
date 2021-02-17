using UnityEngine;
public class AmbushSystem : MonoBehaviour
{
    public int enemyCount;
    public int objectCount;
    public SectionType sectionType;
    public GameObject enemyPool;
    public GameObject objectPool;
    public GameObject[] objects;

    void Update()
    {
      SetAmbushType(sectionType);
    }
    public enum SectionType { AllEnemies, AllObjects, Sequenced}
    public void SetAmbushType(SectionType type)
    {
        switch (type)
        {
            case SectionType.AllEnemies:
                {
                    enemyCount = enemyPool.transform.childCount;
                    if (enemyCount == 0)
                        Destroy(gameObject);
                    break;
                }
            case SectionType.AllObjects:
                {
                    enemyCount = enemyPool.transform.childCount;
                    if (enemyCount == 0)
                    {
                        foreach (GameObject obj in objects)
                        {
                            Destroy(obj);
                        }
                        
                    }
                    break;
                }
            case SectionType.Sequenced:
                {
                    enemyCount = enemyPool.transform.childCount;
                    objectCount = objectPool.transform.childCount;
                    if (enemyCount == 4)
                        Destroy(objects[0].gameObject);
                    else if (enemyCount == 2)
                        Destroy(objects[1].gameObject);
                    else if (enemyCount == 0)
                    {
                        Destroy(objects[2].gameObject);
                        Destroy(objects[3].gameObject);
                    }

                    if (objectCount == 0)
                        Destroy(gameObject);
                    break;
                }
        }
    }
}