using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public enum Tactics { Random, Stealth, Grunt, Sniper, Melee, Kamikazi };

    public Tactics tactic;
    public NavMeshAgent nav;
    public bool forceAffected = false;

    Transform player;
    Transform waypoint;
    PlayerHealth playerHealth;
    EnemyAttack enemyAttack;
    EnemyHealth enemyHealth;
    Animator anim;

    Vector3 targetPosition = new Vector3(257.09f, 5.23f, 223.79f);
    float timer = 0f;
    float timeBetweenMovements;
    bool reachedWaypoint = false;

    void Awake()
    {
        player = GameObject.Find("Player").transform;
        if (GameObject.Find("MovementWayPoint"))
            waypoint = GameObject.Find("MovementWayPoint").transform;

        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        enemyAttack = GetComponent<EnemyAttack>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        timeBetweenMovements = Random.Range(1, 3);
        updateMovementInstant();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
        {
            if (forceAffected)
            {
                anim.SetFloat("Speed", 0.0f);
                anim.SetTrigger("Force");
                nav.enabled = false;
                return;
            }
            else
            {
                anim.SetBool("Force", false);
                nav.enabled = true;
            }

            if (waypoint != null && Vector3.Distance(waypoint.position, gameObject.transform.position) < 2)
                reachedWaypoint = true;

            if (tactic == Tactics.Random)
            {
                gameObject.transform.LookAt(new Vector3(player.position.x, gameObject.transform.position.y, player.position.z));
                if (timer >= timeBetweenMovements)
                {
                    int x = Random.Range(-4, 4);
                    int z = Random.Range(-4, 4);
                    targetPosition = new Vector3(gameObject.transform.position.x - x, gameObject.transform.position.y, gameObject.transform.position.z - z);

                    timer = 0f;
                    timeBetweenMovements = Random.Range(1, 3);
                }
                else if (targetPosition != null)
                {
                    // Low-pass filter the deltaMove
                    float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
                    float fracJourney = smooth / Vector3.Distance(gameObject.transform.position, targetPosition);
                    transform.position = Vector3.Lerp(gameObject.transform.position, targetPosition, smooth);
                }
            }
            else if (tactic == Tactics.Grunt)
            {
                // Hold position
                if (enemyAttack.inMeleeRange)
                {
                    anim.SetFloat("Speed", 0.0f);
                    gameObject.transform.LookAt(new Vector3(player.position.x, gameObject.transform.position.y, player.position.z));
                    nav.speed = 0.0f;
                    nav.Stop();
                }
                // Approach slowly if in shoot range
                else if (enemyAttack.inShootRange)
                {
                    anim.SetFloat("Speed", 0.3f);
                    nav.speed = 2.0f;
                    gameObject.transform.LookAt(new Vector3(player.position.x, gameObject.transform.position.y, player.position.z));
                    nav.SetDestination(player.position);
                }
                // If player is not in range, approach quickly
                else
                {
                    anim.SetFloat("Speed", 0.8f);
                    nav.speed = 5f;
                    if (reachedWaypoint)
                    {
                        gameObject.transform.LookAt(new Vector3(player.position.x, gameObject.transform.position.y, player.position.z));
                        nav.SetDestination(player.position);
                    }
                    else
                    {
                        gameObject.transform.LookAt(new Vector3(waypoint.position.x, gameObject.transform.position.y, waypoint.position.z));
                        nav.SetDestination(waypoint.position);
                    }
                }
            }
            else if (tactic == Tactics.Sniper)
            {
                // Hold position
                if (enemyAttack.inMeleeRange)
                {
                    gameObject.transform.LookAt(new Vector3(player.position.x, gameObject.transform.position.y, player.position.z));
                    anim.SetFloat("Speed", 0.0f);
                    nav.speed = 0.0f;
                    nav.Stop();
                }
                // Hold position if in shoot range
                else if (enemyAttack.inShootRange)
                {
                    gameObject.transform.LookAt(new Vector3(player.position.x, gameObject.transform.position.y, player.position.z));
                    anim.SetFloat("Speed", 0.0f);
                    nav.speed = 0.0f;
                    nav.Stop();
                }
                // If player is not in range, approach slowly
                else
                {
                    anim.SetFloat("Speed", 0.3f);
                    nav.speed = 2.0f;
                    if (reachedWaypoint)
                    {
                        gameObject.transform.LookAt(new Vector3(player.position.x, gameObject.transform.position.y, player.position.z));
                        nav.SetDestination(player.position);
                    }
                    else
                    {
                        gameObject.transform.LookAt(new Vector3(waypoint.position.x, gameObject.transform.position.y, waypoint.position.z));
                        nav.SetDestination(waypoint.position);
                    }
                }
            }
            else if (tactic == Tactics.Melee)
            {
                // Hold position
                if (enemyAttack.inMeleeRange)
                {
                    gameObject.transform.LookAt(new Vector3(player.position.x, gameObject.transform.position.y, player.position.z));
                    anim.SetFloat("Speed", 0.0f);
                    nav.speed = 0.0f;
                    nav.Stop();
                }
                else if (reachedWaypoint)
                {
                    gameObject.transform.LookAt(new Vector3(player.position.x, gameObject.transform.position.y, player.position.z));
                    anim.SetFloat("Speed", 0.8f);
                    nav.speed = 5f;
                    nav.SetDestination(player.position);
                }
                else
                {
                    // Sprint towards the waypoint
                    gameObject.transform.LookAt(new Vector3(waypoint.position.x, gameObject.transform.position.y, waypoint.position.z));
                    anim.SetFloat("Speed", 0.8f);
                    nav.speed = 5f;
                    nav.SetDestination(waypoint.position);
                }
            }
            else if (tactic == Tactics.Kamikazi)
            {
                if (reachedWaypoint)
                {
                    gameObject.transform.LookAt(new Vector3(player.position.x, gameObject.transform.position.y, player.position.z));
                    anim.SetFloat("Speed", 0.8f);
                    nav.speed = 5f;
                    nav.SetDestination(player.position);
                }
                else
                {
                    // Sprint towards the waypoint
                    gameObject.transform.LookAt(new Vector3(waypoint.position.x, gameObject.transform.position.y, waypoint.position.z));
                    anim.SetFloat("Speed", 0.8f);
                    nav.speed = 5f;
                    nav.SetDestination(waypoint.position);
                }
            }
        }
        else
            nav.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10 && forceAffected)
        {
            forceAffected = false;
            anim.SetTrigger("Fell");
        }
    }

    public void updateMovementInstant()
    {
        timer += Time.deltaTime;

        if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
        {

            if (forceAffected)
            {
                anim.SetFloat("Speed", 0.0f);
                anim.SetTrigger("Force");
                anim.Play("Fall");
                nav.enabled = false;
                return;
            }
            else
            {
                anim.SetBool("Force", false);
                nav.enabled = true;
            }

            if (waypoint != null && Vector3.Distance(waypoint.position, gameObject.transform.position) < 2)
                reachedWaypoint = true;

            if (tactic == Tactics.Random)
            {
                gameObject.transform.LookAt(new Vector3(player.position.x, gameObject.transform.position.y, player.position.z));
                if (timer >= timeBetweenMovements)
                {
                    int x = Random.Range(-4, 4);
                    int z = Random.Range(-4, 4);
                    targetPosition = new Vector3(gameObject.transform.position.x - x, gameObject.transform.position.y, gameObject.transform.position.z - z);

                    timer = 0f;
                    timeBetweenMovements = Random.Range(1, 3);
                }
                else if (targetPosition != null)
                {
                    // Low-pass filter the deltaMove
                    float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
                    float fracJourney = smooth / Vector3.Distance(gameObject.transform.position, targetPosition);
                    transform.position = Vector3.Lerp(gameObject.transform.position, targetPosition, smooth);
                }
            }
            else if (tactic == Tactics.Grunt)
            {
                // Hold position
                if (enemyAttack.inMeleeRange)
                {
                    anim.SetFloat("Speed", 0.0f);
                    anim.Play("Idle");
                    gameObject.transform.LookAt(new Vector3(player.position.x, gameObject.transform.position.y, player.position.z));
                    nav.speed = 0.0f;
                    nav.Stop();
                }
                // Approach slowly if in shoot range
                else if (enemyAttack.inShootRange)
                {
                    anim.SetFloat("Speed", 0.3f);
                    anim.Play("Walk");
                    nav.speed = 2.0f;
                    gameObject.transform.LookAt(new Vector3(player.position.x, gameObject.transform.position.y, player.position.z));
                    nav.SetDestination(player.position);
                }
                // If player is not in range, approach quickly
                else
                {
                    anim.SetFloat("Speed", 0.8f);
                    anim.Play("Run");
                    nav.speed = 5f;
                    if (reachedWaypoint)
                    {
                        gameObject.transform.LookAt(new Vector3(player.position.x, gameObject.transform.position.y, player.position.z));
                        nav.SetDestination(player.position);
                    }
                    else
                    {
                        gameObject.transform.LookAt(new Vector3(waypoint.position.x, gameObject.transform.position.y, waypoint.position.z));
                        nav.SetDestination(waypoint.position);
                    }
                }
            }
            else if (tactic == Tactics.Sniper)
            {
                // Hold position
                if (enemyAttack.inMeleeRange)
                {
                    gameObject.transform.LookAt(new Vector3(player.position.x, gameObject.transform.position.y, player.position.z));
                    anim.SetFloat("Speed", 0.0f);
                    anim.Play("Idle");
                    nav.speed = 0.0f;
                    nav.Stop();
                }
                // Hold position if in shoot range
                else if (enemyAttack.inShootRange)
                {
                    gameObject.transform.LookAt(new Vector3(player.position.x, gameObject.transform.position.y, player.position.z));
                    anim.SetFloat("Speed", 0.0f);
                    anim.Play("Idle");
                    nav.speed = 0.0f;
                    nav.Stop();
                }
                // If player is not in range, approach slowly
                else
                {
                    anim.SetFloat("Speed", 0.3f);
                    anim.Play("Walk");
                    nav.speed = 2.0f;
                    if (reachedWaypoint)
                    {
                        gameObject.transform.LookAt(new Vector3(player.position.x, gameObject.transform.position.y, player.position.z));
                        nav.SetDestination(player.position);
                    }
                    else
                    {
                        gameObject.transform.LookAt(new Vector3(waypoint.position.x, gameObject.transform.position.y, waypoint.position.z));
                        nav.SetDestination(waypoint.position);
                    }
                }
            }
            else if (tactic == Tactics.Melee)
            {
                // Hold position
                if (enemyAttack.inMeleeRange)
                {
                    gameObject.transform.LookAt(new Vector3(player.position.x, gameObject.transform.position.y, player.position.z));
                    anim.SetFloat("Speed", 0.0f);
                    anim.Play("Idle");
                    nav.speed = 0.0f;
                    nav.Stop();
                }
                else if (reachedWaypoint)
                {
                    gameObject.transform.LookAt(new Vector3(player.position.x, gameObject.transform.position.y, player.position.z));
                    anim.SetFloat("Speed", 0.8f);
                    anim.Play("Run");
                    nav.speed = 5f;
                    nav.SetDestination(player.position);
                }
                else
                {
                    // Sprint towards the waypoint
                    gameObject.transform.LookAt(new Vector3(waypoint.position.x, gameObject.transform.position.y, waypoint.position.z));
                    anim.SetFloat("Speed", 0.8f);
                    anim.Play("Run");
                    nav.speed = 5f;
                    nav.SetDestination(waypoint.position);
                }
            }
            else if (tactic == Tactics.Kamikazi)
            {
                if (reachedWaypoint)
                {
                    gameObject.transform.LookAt(new Vector3(player.position.x, gameObject.transform.position.y, player.position.z));
                    anim.SetFloat("Speed", 0.8f);
                    anim.Play("Run");
                    nav.speed = 5f;
                    nav.SetDestination(player.position);
                }
                else
                {
                    // Sprint towards the waypoint
                    gameObject.transform.LookAt(new Vector3(waypoint.position.x, gameObject.transform.position.y, waypoint.position.z));
                    anim.SetFloat("Speed", 0.8f);
                    anim.Play("Run");
                    nav.speed = 5f;
                    nav.SetDestination(waypoint.position);
                }
            }
        }
        else
            nav.enabled = false;
    }
}