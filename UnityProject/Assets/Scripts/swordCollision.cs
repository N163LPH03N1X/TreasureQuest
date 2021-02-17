using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordCollision : MonoBehaviour
{
    AudioSource audioSrc;
    public AudioClip[] collideSfx;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.GetComponent<EnemySystem>() != null)
            {
                if (other.gameObject.GetComponent<EnemySystem>().isIndestructible)
                    SwordCollision(Collision.objects);
                else if (!other.gameObject.GetComponent<EnemySystem>().isReal)
                    SwordCollision(Collision.objects);
                else
                    SwordCollision(Collision.enemy);
            }
            else if(other.gameObject.GetComponent<GraveYardGhostSystem>() != null)
                SwordCollision(Collision.enemy);

        }
        else if (other.gameObject.CompareTag("EnemyTop") || other.gameObject.CompareTag("Player"))
            SwordCollision(Collision.none);
        else if (!other.gameObject.CompareTag("Enemy"))
            SwordCollision(Collision.objects);

    }
    public enum Collision {objects, enemy, none}
    public void SwordCollision(Collision type)
    {
        audioSrc = GetComponent<AudioSource>();
        switch (type)
        {
            case Collision.objects:
                {
                    int randomnum = Random.Range(1, 2);
                    audioSrc.pitch = Random.Range(0.8f, 1f);
                    if (randomnum == 1)
                        audioSrc.PlayOneShot(collideSfx[0]);
                    else if (randomnum == 2)
                        audioSrc.PlayOneShot(collideSfx[1]);
                    break;
                }
            case Collision.enemy:
                {
                    audioSrc.PlayOneShot(collideSfx[2]);
                    break;
                }
            case Collision.none:
                {
                    break;
                }
        }
    }
}
