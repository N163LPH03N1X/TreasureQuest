using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(Rigidbody))]

public class EnemyKnockBack : MonoBehaviour, ISwordHittable
{
    Rigidbody rb;
    NavMeshAgent nav;

    //public void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Sword"))
    //    {
    //        rb = GetComponent<Rigidbody>();
    //        nav = GetComponent<NavMeshAgent>();
    //        Vector3 direction = (transform.position - other.transform.position).normalized;
    //        rb.AddForce(direction * 5f, ForceMode.Impulse);
    //        nav.velocity = rb.velocity;
    //        Debug.Log("Knocked Back!");
    //    }
    //}
    public void OnGetHitBySword(int force)
    {
        rb = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        Vector3 direction = (transform.position - GameObject.FindGameObjectWithTag("Player").transform.position).normalized;
        rb.AddForce(direction * 5f, ForceMode.Impulse);
        nav.velocity = rb.velocity;
        Debug.Log("Knocked Back!");
    }
}
