using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public bool random;

    Transform player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    NavMeshAgent nav;
    Animator anim;
    float timer;
    float timeBetweenMovements;

    void Awake()
    {
        player = GameObject.Find("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        timeBetweenMovements = Random.Range(1, 2);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
            if (random)
            {
                if (timer >= timeBetweenMovements)
                {
                    int x = Random.Range(-4, 4);
                    int z = Random.Range(-4, 4);
                    nav.SetDestination(new Vector3(gameObject.transform.position.x - x, gameObject.transform.position.y, gameObject.transform.position.z - z));

                    timer = 0f;
                    timeBetweenMovements = Random.Range(1, 2);
                }
            }
            else
            {
                nav.SetDestination(player.position);
            }
        else
            nav.enabled = false;
        Animate();
    }

    void Animate()
    {
        anim.SetBool("IsWalking", true);
    }
}