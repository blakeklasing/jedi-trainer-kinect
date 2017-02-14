using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		
	}

    // perform lightning things
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("yesss");
        // check if this collider is an enemy
        if (other.gameObject.tag == "Enemy")
        {
            //deal damage!
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(50);
            Debug.Log("lightning dmg");
        }
    }

}
