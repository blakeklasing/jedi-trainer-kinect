using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour {

    GameObject player;
    PlayerHealth playerHealth;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("lightning");
        if (other.gameObject == player)
             playerHealth.TakeDamage(50);
        else if (other.gameObject.tag.Equals("Enemy") && other.GetType() == typeof(CapsuleCollider))
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(50);
    }
}
