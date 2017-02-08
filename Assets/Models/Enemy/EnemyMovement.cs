using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    Transform player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    NavMeshAgent nav;
    Animator anim;

    void Awake()
    {
        player = GameObject.Find("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
            nav.SetDestination(player.position);
        else
            nav.enabled = false;

        // Animate the player.
        Animate();
    }


    void Animate()
    {
        // Tell the animator whether or not the player is walking.
        anim.SetBool("IsWalking", true);
    }
}