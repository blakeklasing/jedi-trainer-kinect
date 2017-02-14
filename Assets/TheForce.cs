﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheForce : MonoBehaviour {

    
    float global_timer;

    //lightning variables
    public GameObject lightning;
    private GameObject lightning_instance;



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ForceLightning();
        }
        ForceGrab();
    }

    // lightning
    public void ForceLightning()
    {
        lightning_instance = GameObject.Instantiate(lightning, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(lightning_instance, 0.1f);
        // find enemies in range infront of you
        
        // dmg each enemy


    }

    // grab
    public void ForceGrab()
    {
        RaycastHit hit;
        bool grabbed = false;
        GameObject hit_object = null;
        Vector3 jedi_pos = gameObject.transform.position;
        //jedi_pos.y = jedi_pos.y;

        if (Physics.Raycast(jedi_pos, gameObject.transform.forward*100, out hit, 10.0f))
        {


            Debug.DrawRay(jedi_pos, gameObject.transform.forward * 1000);//gameObject.transform.rotation);
            if(hit.collider.gameObject && Input.GetMouseButtonDown(0) && !grabbed)
            {
                hit_object = hit.collider.gameObject;
                grabbed = true;
                Debug.DrawRay(jedi_pos, gameObject.transform.forward * 1000, Color.blue);
                Debug.Log("hello");
            }
            else if(Input.GetMouseButtonDown(0) && grabbed)
            {
                grabbed = false;
            }
        }

        if(grabbed && hit_object != null)
        {
            hit_object.transform.position.Set(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        }


    }

    // push
    public void ForcePush()
    {

        // find enemies in range infront of you

        // push them
    }

    // heal
    public void ForceHeal()
    {
        lightning_instance = GameObject.Instantiate(lightning, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(lightning_instance, 0.1f);

    }

    // future
    public void ForceFuture()
    {

    }

    // Choke
    public void ForceChoke()
    {

    }

    // crouch
    public void crouch()
    {


    }




}
