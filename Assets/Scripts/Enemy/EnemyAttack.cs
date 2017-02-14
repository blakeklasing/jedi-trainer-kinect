using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject weaponBlast;
    public int shootRange;
    public int shootDamage;
    public float shootCooldown;
    public int meleeRange;
    public int meleeDamage;
    public float meleeCooldown;

    public bool inShootRange;
    public bool inMeleeRange;

    Animator anim;
    GameObject player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    EnemyMovement enemyMovement;
    SphereCollider shootCollider;
    SphereCollider meleeCollider;
    float timeUntilNextAttack;
    float attackCooldownTimer;

    int colliderCounter;


    void Awake()
    {
        player = GameObject.Find("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        enemyMovement = GetComponent<EnemyMovement>();
        anim = GetComponent<Animator>();
        SphereCollider[] colliders = GetComponents<SphereCollider>();

        // Create shooting and melee colliders
        shootCollider = GetComponents<SphereCollider>()[0];
        meleeCollider = GetComponents<SphereCollider>()[1];
        shootCollider.radius = shootRange;
        meleeCollider.radius = meleeRange;

        colliderCounter = 0;
        attackCooldownTimer = 0;
        timeUntilNextAttack = 0;
    }


    void Update()
    {
        attackCooldownTimer += Time.deltaTime;

        // If we are able to attack
        if (CanAttack())
        {
            gameObject.transform.LookAt(gameObject.transform.position, gameObject.transform.up);
            Attack();
        }

        // If the player is dead
        if (playerHealth.currentHealth <= 0)
            anim.SetTrigger("PlayerDead");
    }

    bool CanAttack()
    {
        // Check if the player is still alive
        if (enemyHealth.currentHealth < 0)
            return false;

        // If attack is in cooldown
        if (attackCooldownTimer <= timeUntilNextAttack)
            return false;

        // Check if the player is within attack range
        if (!inShootRange && !inMeleeRange)
            return false;

        return true;
    }

    void Attack()
    {
        if (inMeleeRange)
        {
            anim.SetTrigger("Melee");
            playerHealth.TakeDamage(meleeDamage);
            timeUntilNextAttack = Random.Range(meleeCooldown, meleeCooldown + 1.0f);
        }
        else if (inShootRange)
        {
            enemyMovement.nav.Stop();
            anim.SetTrigger("Shoot");
            GameObject blast = GameObject.Instantiate(weaponBlast, meleeCollider.ClosestPointOnBounds(player.transform.position), gameObject.transform.rotation);
            Destroy(blast, 4);
            timeUntilNextAttack = Random.Range(shootCooldown, shootCooldown + 1.0f);
        }

        // Reset attack cooldown timer
        attackCooldownTimer = 0f;
    }

    void OnTriggerEnter(Collider other)
    {
        colliderCounter++;

        if (other.gameObject == player) {
            // Check shoot collider
            if (colliderCounter == 1)
                inShootRange = true;
            // Check melee collider
            else if (colliderCounter == 2)
                inMeleeRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            // Check shoot collider
            if (colliderCounter == 1)
                inShootRange = false;
            // Check melee collider
            else if (colliderCounter == 2)
                inMeleeRange = false;
        }
    }
}