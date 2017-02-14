using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public enum Tactics { Random, Stealth, Grunt, Sniper, Melee, Kamikazi };

    public Tactics tactic;
    public NavMeshAgent nav;

    Transform player;
    Transform fpc;
    PlayerHealth playerHealth;
    EnemyAttack enemyAttack;
    EnemyHealth enemyHealth;
    Animator anim;

    float timer;
    float timeBetweenMovements;

    void Awake()
    {
        player = GameObject.Find("Player").transform;
        fpc = GameObject.Find("FirstPersonCharacter").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        enemyAttack = GetComponent<EnemyAttack>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        timeBetweenMovements = Random.Range(1, 2);
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Always look at the player
        gameObject.transform.LookAt(new Vector3(player.position.x, gameObject.transform.position.y, player.position.z));

        if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
            if (tactic == Tactics.Random)
            {
                anim.SetFloat("Speed", 0.1f);

                if (timer >= timeBetweenMovements)
                {
                    int x = Random.Range(-4, 4);
                    int z = Random.Range(-4, 4);
                    nav.SetDestination(new Vector3(gameObject.transform.position.x - x, gameObject.transform.position.y, gameObject.transform.position.z - z));

                    timer = 0f;
                    timeBetweenMovements = Random.Range(1, 2);
                }
            }
            else if (tactic == Tactics.Grunt)
            {
                // Hold position
                if (enemyAttack.inMeleeRange)
                {
                    anim.SetFloat("Speed", 0.0f);
                    nav.Stop();
                }
                // Approach slowly if in shoot range
                else if (enemyAttack.inShootRange)
                {
                    anim.SetFloat("Speed", 0.3f);
                    nav.SetDestination(player.position);
                }
                // If player is not in range, approach quickly
                else
                {
                    anim.SetFloat("Speed", 0.8f);
                    nav.SetDestination(player.position);
                }
            }
            else if (tactic == Tactics.Sniper)
            {
                // Hold position
                if (enemyAttack.inMeleeRange)
                {
                    anim.SetFloat("Speed", 0.0f);
                    nav.Stop();
                }
                // Hold position if in shoot range
                else if (enemyAttack.inShootRange)
                {
                    anim.SetFloat("Speed", 0.0f);
                    nav.Stop();
                }
                // If player is not in range, approach slowly
                else
                {
                    anim.SetFloat("Speed", 0.3f);
                    nav.SetDestination(player.position);
                }
            }
            else if (tactic == Tactics.Melee)
            {
                // Sprint towards player
                anim.SetFloat("Speed", 0.8f);
                nav.SetDestination(player.position);
            }
            else if (tactic == Tactics.Kamikazi)
            {
                // Sprint towards player
                anim.SetFloat("Speed", 0.8f);
                nav.SetDestination(player.position);
            }
            else
                nav.enabled = false;
    }
}