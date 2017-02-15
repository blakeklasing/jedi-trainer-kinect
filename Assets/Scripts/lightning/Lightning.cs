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
        Debug.Log("lightning");
        if (other.gameObject == player)
             playerHealth.TakeDamage(20);
        else if (other.gameObject.tag.Equals("Enemy") && other.GetType() == typeof(CapsuleCollider))
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(50);
    }
}
