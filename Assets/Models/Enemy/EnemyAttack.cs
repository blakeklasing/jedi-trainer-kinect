using UnityEngine;
using System.Collections;


public class EnemyAttack : MonoBehaviour
{
    public GameObject laserBlast;

    Animator anim;
    GameObject player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    SphereCollider collider;
    float timer;
    float timeBetweenAttacks;


    void Awake()
    {
        player = GameObject.Find("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent<Animator>();
        collider = GetComponent<SphereCollider>();

        timeBetweenAttacks = Random.Range(0.2f, 2);
    }


    void Update()
    {
        timer += Time.deltaTime;
        transform.LookAt(player.transform);

        if (timer >= timeBetweenAttacks && enemyHealth.currentHealth > 0)
        {
            Attack();
        }

        if (playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger("PlayerDead");
        }
    }


    void Attack()
    {
        timer = 0f;
        timeBetweenAttacks = Random.Range(0.2f, 2);

        if (playerHealth.currentHealth > 0)
        {
            GameObject.Instantiate(laserBlast, collider.ClosestPointOnBounds(player.transform.position), gameObject.transform.rotation);    
        }
    }

}