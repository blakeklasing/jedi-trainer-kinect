using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageImage; 
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color damageColor = new Color(1f, 0f, 0f, 0.1f);
    public Color healColor   = new Color(0f, 1f, 0f, 0.1f);

    Animator anim;
    AudioSource playerAudio;
    bool isDead;
    bool damaged;
    bool healed;

    void Awake()
    {
        // Setting up the references.
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();

        // Set the initial health of the player.
        currentHealth = startingHealth;
    }

    void Update()
    {
        // If the player has just been damaged...
        if (damaged)
        {
            damageImage.color = damageColor;
            damaged = false;
        }  
        else if (healed)
        {
            damageImage.color = damageColor;
            healed = true;
        }
        else
        {
            // flash
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
            
    }

    public void TakeDamage(int amount)
    {
        // Set the damaged flag so the screen will flash.
        damaged = true;

        // Reduce the current health by the damage amount.
        currentHealth -= amount;

        // Set the health bar's value to the current health.
        healthSlider.value = currentHealth;

        // Play the hurt sound effect.
        playerAudio.Play();

        if (currentHealth <= 0 && !isDead)
            Death();
    }

    public void heal(int amount)
    {
        // Set the damaged flag so the screen will flash.
        healed = true;

        // Reduce the current health by the damage amount.
        currentHealth += amount;

        // Set the health bar's value to the current health.
        healthSlider.value = currentHealth;

        // Play the hurt sound effect.
        playerAudio.Play();
    }

    void Death()
    {
        // Set the death flag so this function won't be called again.
        isDead = true;

        // Tell the animator that the player is dead.
        //anim.SetTrigger("Die");

        // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
        playerAudio.clip = deathClip;
        playerAudio.Play();
    }
}