using System.Collections;
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
    public void ForceLightning()
    {
        lightning_instance = GameObject.Instantiate(lightning, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(lightning_instance, 0.1f);
    }

    // grab
    public void ForceGrab()
    {

    }

    // push
    public void ForcePush()
    {

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
