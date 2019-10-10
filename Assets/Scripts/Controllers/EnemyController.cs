using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy")]
    public float lookradar = 10f;
    Transform Target;
    NavMeshAgent agent;
    bool Damage = false;
    public float DamageRate = 1f;

    [Header("Health")]
    public int MaxHealth = 3;
    public int health = 3;
    public GameObject DeathEffect;
    public Transform Tip;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Target = PlayerManger.instance.Player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(Target.position, transform.position);
        if (distance <= lookradar)
        {
            agent.SetDestination(Target.position);

            if (distance <= agent.stoppingDistance)
            {
                FaceTarget();
                if (!Damage)
                {
                    Target.GetComponent<PlayerController>().TakeDamage();
                    StartCoroutine(DamageRoutine());
                }
            }
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (Target.position - transform.position).normalized;
        Quaternion lookrotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookrotation, Time.deltaTime * 5f);

    }

    public void TakeHealth()
    {
        health = health - 1;
        if (health <= 0)
        {
            Instantiate(DeathEffect, Tip.position, Quaternion.identity, Tip).transform.position = Tip.position;
         
            Destroy(gameObject);
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookradar);
    }

 
    IEnumerator DamageRoutine()
    {
        Damage = true;
        yield return new WaitForSeconds(DamageRate);
        Damage = false;
    }
}
