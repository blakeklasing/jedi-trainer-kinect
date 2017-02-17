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

    public bool inShootRange = false;
    public bool inMeleeRange = false;

    Animator anim;
    GameObject player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    EnemyMovement enemyMovement;
    float timeUntilNextAttack;
    float attackCooldownTimer;
    float rangedAttackSpeed = 1;
    float meleeAttackSpeed = 1;

    void Awake()
    {
        player = GameObject.Find("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        enemyMovement = GetComponent<EnemyMovement>();
        anim = GetComponent<Animator>();

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
        float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        // Check if the player is still alive
        if (enemyHealth.currentHealth <= 0)
            return false;

        // If attack is in cooldown
        if (attackCooldownTimer <= timeUntilNextAttack)
            return false;

        // Check if player is within melee range
        if (distance < meleeRange)
        {
            inMeleeRange = true;
            return true;
        }
        // Check if player is within shoot range
        if (distance < shootRange)
        {
            inShootRange = true;
            return true;
        }

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
    // Called by event animation event
    public void meleeAttack()
    {
        playerHealth.TakeDamage(meleeDamage);
        timeUntilNextAttack = Random.Range(meleeCooldown, meleeCooldown + 1.0f);
    }

    // Called by animation event
    public void rangedAttack()
    {
        Transform shootingHand = FindTransform(gameObject.transform, "hand.R");
        LaserBlast blast = GameObject.Instantiate(weaponBlast, shootingHand.position, gameObject.transform.rotation) as LaserBlast;
        //Physics.IgnoreCollision(blast.GetComponent<CapsuleCollider>(), gameObject.GetComponent<CapsuleCollider>());
        blast.attackDamage = shootDamage;
        Destroy(blast.gameObject, 4.0f);
        timeUntilNextAttack = Random.Range(shootCooldown, shootCooldown + 1.0f);
    }
}
