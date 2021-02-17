using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoneySystem : MonoBehaviour
{
    public int gold;
    PlayerSystem playST;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playST = other.gameObject.GetComponent<PlayerSystem>();
            playST.AddGold(gold);
            Destroy(gameObject);
        }
    }

}
