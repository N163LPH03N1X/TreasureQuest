using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    public EnemySystem enemySys;
    int randomGen;

    public void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("Player"))  
        {
            randomGen = Random.Range(0,3);
            if (randomGen == 1)
            {
                enemySys.EffectPlayer();
            }
         
        }
    }
}
