using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheForce : MonoBehaviour {

    
    
    public Camera camera;

    //lightning prefab
    public GameObject lightning;
    // lightning instance
    private GameObject lightningObject;

    // heal prefab
    public GameObject heal;
    // heal instance
    private GameObject healObject;

    // grab variables

    // push variables
    float push_timer;
    const float PUSH_DURATION = 0.7f;
    ArrayList pushedList;
    Vector3 pushDirection;

    // Use this for initialization
    void Start () {
        pushedList = new ArrayList();
        camera = this.GetComponentInChildren<Camera>();
	}
	
	// Update is called once per frame
	void Update () {

        //DEBUG
        if (Input.GetKeyDown(KeyCode.F))
        {
            ForceLightning();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            ForceHeal();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {   
            
            ForcePush();
            push_timer = Time.time;
        }
        if (pushedList.Count > 0 && (Time.time - push_timer) < PUSH_DURATION)
        {
            PushAll();
            Debug.Log((Time.time - push_timer));
        }
            

        if (Input.GetMouseButtonDown(0))
        {
            
        }
        //ForceGrab(true);
        if (Input.GetMouseButtonUp(0))
        {
            ForceGrab(false);
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
    public void ForceGrab(bool grabbing)
    {
        RaycastHit hit;
        bool grabbed = false;
        GameObject hit_object = null;
        Vector3 jedi_pos = gameObject.transform.position;

        // get ray from camera to cast
        Ray camera_ray = camera.ScreenPointToRay(Input.mousePosition);

        //Debug.DrawRay(jedi_pos, gameObject.transform.forward * 1000);
        Debug.DrawRay(camera_ray.origin, camera_ray.direction*100, Color.blue);
        /*
         * 
         * if (Physics.Raycast(transform.position, fwd, 10)) 
            print("There is something in front of the object!");
         * */


        if ( Physics.Raycast(camera_ray, out hit) )        //Physics.Raycast(jedi_pos, gameObject.transform.forward*100, out hit, 10.0f))
        {
            Debug.Log("ray: " + hit.collider.gameObject.ToString());
            
            if(hit.collider.gameObject && ((hit.collider.GetType() == typeof(CapsuleCollider) && hit.collider.gameObject.tag == "Enemy") || hit.collider.GetType() != typeof(CapsuleCollider)) && grabbing && !grabbed)
            {
                hit_object = hit.collider.gameObject;
                grabbed = true;
                Debug.Log("grabbed");
            }
            else if(!grabbing && grabbed)
            {
                grabbed = false;
            }
        }

        if(grabbed && hit_object != null)
        {
            hit_object.transform.position.Set(camera_ray.origin.x + camera_ray.direction.x, camera_ray.origin.y + camera_ray.direction.y, camera_ray.origin.z + camera_ray.direction.z);// gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        }


    }

    // push
    public void ForcePush()
    {

        // find enemies in range infront of you
        RaycastHit hit;
        GameObject hit_object = null;
        Ray findRay;

        // reset list
        pushedList.Clear();

        // get ray from camera to cast
        Ray cameraRay = camera.ScreenPointToRay(Input.mousePosition);
        pushDirection = new Vector3(cameraRay.direction.x, cameraRay.direction.y + 1, cameraRay.direction.z);

        // cast 30 rays infront of player to look for enemies to push
        for (int i = 0; i < 1; i++)
        {
            //Debug.Log("force push");
            findRay = new Ray(cameraRay.origin, cameraRay.direction);
            if (Physics.Raycast(findRay, out hit))
            {
                hit_object = hit.collider.gameObject;

                // add objects to collection
                if(hit_object.layer == 9)
                {
                    pushedList.Add(hit_object);
                    Debug.Log("added to list");
                }
                    
                
            }
        }
        
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



    // helper functions
    void PushAll()
    {
        // push all objects in list
        foreach( GameObject pushedObject in pushedList)
        {
            pushedObject.transform.position += 10.0f * Time.smoothDeltaTime * pushDirection;
            Debug.Log("pushed");
        }

    }



}
