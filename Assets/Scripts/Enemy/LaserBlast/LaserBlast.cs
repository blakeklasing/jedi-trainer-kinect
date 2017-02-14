using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBlast : MonoBehaviour {

    public int attackDamage = 10;

    GameObject player;
    PlayerHealth playerHealth;

    void Start () {
        player = GameObject.Find("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        gameObject.transform.position += 25.0f * Time.smoothDeltaTime * gameObject.transform.forward;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            playerHealth.TakeDamage(attackDamage);
            Destroy(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }
}