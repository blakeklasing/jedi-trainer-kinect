using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheForce : MonoBehaviour {

    
    float global_timer;


    //lightning prefab
    public GameObject lightning;
    // lightning instance
    private GameObject lightningObject;

    // heal prefab
    public GameObject heal;
    // heal instance
    private GameObject healObject;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //DEBUG
        if (Input.GetKeyDown(KeyCode.A))
        {
            ForceLightning();
            ForceHeal();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            ForceGrab();
        }


    }

    // lightning
    public void ForceLightning()
    {
        lightningObject = GameObject.Instantiate(lightning, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(lightningObject, 0.3f);
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
        this.GetComponent<PlayerHealth>().heal(25);
        healObject = GameObject.Instantiate(heal, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(healObject, 1.0f);
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
