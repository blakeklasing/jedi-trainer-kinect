using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheForce : MonoBehaviour {

    
    float global_timer;

    //lightning variables
    public GameObject lightning;
    private GameObject lightning_instance;
    bool lightning_on = false;
    float lightning_timer = 0.0f;



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        /*
        global_timer += Time.deltaTime;
        if(lightning_on && (Time.time - lightning_timer) > 1.0f )
        {
            Destroy(lightning);
        }
        */
        //kinect stuff!
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ForceLightning();
        }
    }

    // lightning
    void ForceLightning()
    {
        lightning_on = true;
        lightning_timer = Time.time;
        lightning_instance = GameObject.Instantiate(lightning, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(lightning_instance, 0.1f);
    }

    // grab
    void ForceGrab()
    {

    }

    // push
    void ForcePush()
    {

    }

    // heal
    void ForceHeal()
    {


    }

    // future
    void ForceFuture()
    {

    }

    // Choke
    void ForceChoke()
    {

    }

    // crouch
    void crouch()
    {


    }




}
