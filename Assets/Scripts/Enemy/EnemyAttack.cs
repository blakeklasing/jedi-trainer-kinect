using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public LaserBlast weaponBlast;
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
    float rangedAttackSpeed = 1;
    float meleeAttackSpeed = 1;
    bool isAttacking;

    int colliderCounter;


    void Awake()
    {
        player = GameObject.Find("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        enemyMovement = GetComponent<EnemyMovement>();
        anim = GetComponent<Animator>();
        SphereCollider[] colliders = GetComponents<SphereCollider>();

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
        if (enemyHealth.currentHealth <= 0)
            return false;

        // If attack is in cooldown
        if (attackCooldownTimer <= timeUntilNextAttack)
            return false;

        // Check if the player is within attack range
        if (!inShootRange && !inMeleeRange)
            return false;

        if (isAttacking)
            return false;

        return true;
    }

    void Attack()
    {
        if (inMeleeRange)
        {
            enemyMovement.nav.Stop();
            anim.SetTrigger("Melee");
            // Event animation will execute meleeAttack()
        }
        else if (inShootRange)
        {
            enemyMovement.nav.Stop();
            anim.SetTrigger("Shoot");
            // Event animation will execute rangedAttack()
        }

        // Reset attack cooldown timer
        attackCooldownTimer = 0f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player) {
            colliderCounter++;

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

            colliderCounter--;
        }
    }

    Transform FindTransform(Transform parent, string name)
    {
        if (parent.name.Equals(name)) return parent;
        foreach (Transform child in parent)
        {
            Transform result = FindTransform(child, name);
            if (result != null) return result;
        }
        return null;
    }


    public void meleeAttack()
    {
        playerHealth.TakeDamage(meleeDamage);
        timeUntilNextAttack = Random.Range(meleeCooldown, meleeCooldown + 1.0f);
    }

    public void rangedAttack()
    {
        Transform shootingHand = FindTransform(gameObject.transform, "hand.R");
        LaserBlast blast = GameObject.Instantiate(weaponBlast, shootingHand.position, gameObject.transform.rotation) as LaserBlast;
        blast.attackDamage = shootDamage;
        Destroy(blast.gameObject, 0.1f);
        timeUntilNextAttack = Random.Range(shootCooldown, shootCooldown + 1.0f);
    }
}