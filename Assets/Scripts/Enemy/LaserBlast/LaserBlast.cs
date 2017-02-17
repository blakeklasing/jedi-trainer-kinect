using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBlast : MonoBehaviour {

    public int attackDamage;

    GameObject player;
    GameObject lightsaber;
    PlayerHealth playerHealth;

    void Start () {
        player = GameObject.Find("Player");
        player.GetComponentInChildren<Collider>();
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        gameObject.transform.position += 25.0f * Time.smoothDeltaTime * gameObject.transform.forward;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("collided" + other.gameObject.name);
        if (other.gameObject == player)
        {
            playerHealth.TakeDamage(attackDamage);
            Destroy(this.gameObject);
        }
        else if (other.gameObject.name == "lightsaber2" )
        {
            
        }
    }
}