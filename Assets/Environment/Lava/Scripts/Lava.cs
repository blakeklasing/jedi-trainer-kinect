using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour {

    GameObject player;
    PlayerHealth playerHealth;

	void Start () {
        player = GameObject.Find("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
            playerHealth.TakeDamage(100);
        else if (other.gameObject.tag.Equals("Enemy") && other.GetType() == typeof(CapsuleCollider))
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(100);
    }
}
