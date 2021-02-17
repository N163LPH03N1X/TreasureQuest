
using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{
    public bool Terra;
    public bool Frost;
    public bool Lightning;
    public bool Flare;
    ParticleSystem ps;

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        //if (ps.)
        //    Destroy(gameObject, 5);
    }

    private void OnParticleCollision(GameObject other)
    {

        if (other.gameObject.CompareTag("Enemy") && Terra)
        {
            if (other.gameObject.GetComponent<EnemySystem>() != null)
            {
                EnemySystem enemy = other.gameObject.GetComponent<EnemySystem>();
                enemy.DamageEnemy(1);
            }
            else if (other.gameObject.GetComponent<GraveYardGhostSystem>() != null)
            {
                GraveYardGhostSystem enemy = other.gameObject.GetComponent<GraveYardGhostSystem>();
                enemy.DamageEnemy(1);
            }

        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && Frost)
        {
            if (collision.gameObject.GetComponent<EnemySystem>() != null)
            {
                EnemySystem enemy = collision.gameObject.GetComponent<EnemySystem>();
                enemy.ElementDamage(EnemySystem.Element.ice, true);
                if (PlayerSystem.playerVitality >= 5 && PlayerSystem.playerVitality < 13)
                    enemy.DamageEnemy(1);
                else if (PlayerSystem.playerVitality >= 13 && PlayerSystem.playerVitality < 18)
                    enemy.DamageEnemy(2);
                else if (PlayerSystem.playerVitality >= 18 && PlayerSystem.playerVitality < 23)
                    enemy.DamageEnemy(3);
                else if (PlayerSystem.playerVitality >= 23 && PlayerSystem.playerVitality < 28)
                    enemy.DamageEnemy(4);
                else if (PlayerSystem.playerVitality >= 28 && PlayerSystem.playerVitality < 33)
                    enemy.DamageEnemy(5);
                else if (PlayerSystem.playerVitality >= 33 && PlayerSystem.playerVitality < 38)
                    enemy.DamageEnemy(6);
                else if (PlayerSystem.playerVitality >= 38 && PlayerSystem.playerVitality < 43)
                    enemy.DamageEnemy(7);
                else if (PlayerSystem.playerVitality >= 43 && PlayerSystem.playerVitality < 48)
                    enemy.DamageEnemy(8);
                else if (PlayerSystem.playerVitality >= 48 && PlayerSystem.playerVitality < 53)
                    enemy.DamageEnemy(9);
                else if (PlayerSystem.playerVitality >= 53 && PlayerSystem.playerVitality < 58)
                    enemy.DamageEnemy(10);
                else if (PlayerSystem.playerVitality >= 58 && PlayerSystem.playerVitality < 63)
                    enemy.DamageEnemy(11);
                else if (PlayerSystem.playerVitality >= 63 && PlayerSystem.playerVitality < 68)
                    enemy.DamageEnemy(12);
                else if (PlayerSystem.playerVitality >= 68 && PlayerSystem.playerVitality < 73)
                    enemy.DamageEnemy(13);
                else if (PlayerSystem.playerVitality >= 73 && PlayerSystem.playerVitality < 78)
                    enemy.DamageEnemy(14);
                else if (PlayerSystem.playerVitality >= 78 && PlayerSystem.playerVitality < 83)
                    enemy.DamageEnemy(15);
                else if (PlayerSystem.playerVitality >= 83 && PlayerSystem.playerVitality < 88)
                    enemy.DamageEnemy(16);
                else if (PlayerSystem.playerVitality >= 88 && PlayerSystem.playerVitality < 93)
                    enemy.DamageEnemy(17);
                else if (PlayerSystem.playerVitality >= 93 && PlayerSystem.playerVitality < 96)
                    enemy.DamageEnemy(18);
                else if (PlayerSystem.playerVitality >= 96 && PlayerSystem.playerVitality < 100)
                    enemy.DamageEnemy(19);
                else if (PlayerSystem.playerVitality >= 100)
                    enemy.DamageEnemy(20);
                Destroy(gameObject);
            }
            else if (collision.gameObject.GetComponent<GraveYardGhostSystem>() != null)
            {
                GraveYardGhostSystem enemy = collision.gameObject.GetComponent<GraveYardGhostSystem>();
                if (PlayerSystem.playerVitality >= 5 && PlayerSystem.playerVitality < 13)
                    enemy.DamageEnemy(1);
                else if (PlayerSystem.playerVitality >= 13 && PlayerSystem.playerVitality < 18)
                    enemy.DamageEnemy(2);
                else if (PlayerSystem.playerVitality >= 18 && PlayerSystem.playerVitality < 23)
                    enemy.DamageEnemy(3);
                else if (PlayerSystem.playerVitality >= 23 && PlayerSystem.playerVitality < 28)
                    enemy.DamageEnemy(4);
                else if (PlayerSystem.playerVitality >= 28 && PlayerSystem.playerVitality < 33)
                    enemy.DamageEnemy(5);
                else if (PlayerSystem.playerVitality >= 33 && PlayerSystem.playerVitality < 38)
                    enemy.DamageEnemy(6);
                else if (PlayerSystem.playerVitality >= 38 && PlayerSystem.playerVitality < 43)
                    enemy.DamageEnemy(7);
                else if (PlayerSystem.playerVitality >= 43 && PlayerSystem.playerVitality < 48)
                    enemy.DamageEnemy(8);
                else if (PlayerSystem.playerVitality >= 48 && PlayerSystem.playerVitality < 53)
                    enemy.DamageEnemy(9);
                else if (PlayerSystem.playerVitality >= 53 && PlayerSystem.playerVitality < 58)
                    enemy.DamageEnemy(10);
                else if (PlayerSystem.playerVitality >= 58 && PlayerSystem.playerVitality < 63)
                    enemy.DamageEnemy(11);
                else if (PlayerSystem.playerVitality >= 63 && PlayerSystem.playerVitality < 68)
                    enemy.DamageEnemy(12);
                else if (PlayerSystem.playerVitality >= 68 && PlayerSystem.playerVitality < 73)
                    enemy.DamageEnemy(13);
                else if (PlayerSystem.playerVitality >= 73 && PlayerSystem.playerVitality < 78)
                    enemy.DamageEnemy(14);
                else if (PlayerSystem.playerVitality >= 78 && PlayerSystem.playerVitality < 83)
                    enemy.DamageEnemy(15);
                else if (PlayerSystem.playerVitality >= 83 && PlayerSystem.playerVitality < 88)
                    enemy.DamageEnemy(16);
                else if (PlayerSystem.playerVitality >= 88 && PlayerSystem.playerVitality < 93)
                    enemy.DamageEnemy(17);
                else if (PlayerSystem.playerVitality >= 93 && PlayerSystem.playerVitality < 96)
                    enemy.DamageEnemy(18);
                else if (PlayerSystem.playerVitality >= 96 && PlayerSystem.playerVitality < 100)
                    enemy.DamageEnemy(19);
                else if (PlayerSystem.playerVitality >= 100)
                    enemy.DamageEnemy(20);
                Destroy(gameObject);
            }
        }

    }
}
