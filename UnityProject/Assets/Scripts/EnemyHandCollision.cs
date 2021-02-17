using UnityEngine;
public class EnemyHandCollision : MonoBehaviour
{
    bool isDisabled;
    bool isDead;
    bool isReal;
    public GameObject EnemySystemObj;
    ImpactReceiver impact;
    PlayerSystem playST;
    
    // Start is called before the first frame update

    public void OnCollisionEnter(Collision collision)
    {
        isReal = EnemySystemObj.GetComponent<EnemySystem>().isReal;
        if (collision.gameObject.CompareTag("Player") && !SceneSystem.isDisabled && !PlayerSystem.isDead && !PlayerSystem.isInvincible && isReal)
        {
            CharacterSystem characterCtrl = collision.gameObject.GetComponent<CharacterSystem>();
           
            impact = collision.gameObject.GetComponent<ImpactReceiver>();
            Vector3 direction = (collision.transform.position - transform.position).normalized;

            int lifeBerry = ItemSystem.lifeBerryAmt;
            if (lifeBerry == 0)
            {
                if (PlayerSystem.playerHealth <= 1)
                    impact.AddImpact(direction, 30);
                else if (PlayerSystem.playerHealth > 1)
                    impact.AddImpact(direction, 200);
            }
            else if (lifeBerry > 0)
                impact.AddImpact(direction, 200);
            playST = collision.gameObject.GetComponent<PlayerSystem>();
            if (EnemySystemObj.GetComponent<EnemySystem>().blueStatueActive)
                playST.PlayerDamage(4, false);
            else if (EnemySystemObj.GetComponent<EnemySystem>().silverStatueActive)
                playST.PlayerDamage(8, false);
            else if (EnemySystemObj.GetComponent<EnemySystem>().goldStatueActive)
                playST.PlayerDamage(12, false);
            else if (EnemySystemObj.GetComponent<EnemySystem>().whiteSkeletonActive)
                playST.PlayerDamage(3, false);
            else if (EnemySystemObj.GetComponent<EnemySystem>().greenSkeletonActive)
                playST.PlayerDamage(6, false);
            else if (EnemySystemObj.GetComponent<EnemySystem>().redSkeletonActive)
                playST.PlayerDamage(9, false);
            else if (EnemySystemObj.GetComponent<EnemySystem>().darkSkeletonActive)
                playST.PlayerDamage(12, false);
            else if (EnemySystemObj.GetComponent<EnemySystem>().blueMimicActive)
                playST.PlayerDamage(6, false);
            else if (EnemySystemObj.GetComponent<EnemySystem>().greenMimicActive)
                playST.PlayerDamage(12, false);
            else if (EnemySystemObj.GetComponent<EnemySystem>().orangeMimicActive)
                playST.PlayerDamage(18, false);
            else if (EnemySystemObj.GetComponent<EnemySystem>().goliathToadActive)
                playST.PlayerDamage(5, false);
            else if (EnemySystemObj.GetComponent<EnemySystem>().chameleonToadActive)
                playST.PlayerDamage(15, false);
            else if (EnemySystemObj.GetComponent<EnemySystem>().devilToadActive)
                playST.PlayerDamage(30, false);

            characterCtrl.SetFalling();
        }
    }
  
}
