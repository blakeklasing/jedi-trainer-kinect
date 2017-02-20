using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBlast : MonoBehaviour {

    public int attackDamage;

    GameObject player;
    GameObject lightsaber;
    PlayerHealth playerHealth;

    bool deflected = false;

    void Start () {
        player = GameObject.Find("Player");
        player.GetComponentInChildren<Collider>();
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (deflected)
            gameObject.transform.position += 25.0f * Time.smoothDeltaTime * gameObject.transform.forward * -1;
        else
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
        else if (deflected && other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(100);
            Destroy(this.gameObject);
        }
        else if (other.gameObject.name == "Lightsaber" )
        {
            deflected = true;
        }
    }
}