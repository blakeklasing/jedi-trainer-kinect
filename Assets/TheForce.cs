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
    GameObject grabObject = null;
    Vector3 screenPoint;
    Vector3 offset;


    // push variables
    float push_timer;
    const float PUSH_DURATION = 0.7f;
    ArrayList pushedList;
    Vector3 pushDirection;

    // Use this for initialization
    void Start () {
        pushedList = new ArrayList();
	}
	
	// Update is called once per frame
	void Update () {
        if (grabObject != null)
        {
            ForceMove(Input.mousePosition);
        }
        if (Input.GetMouseButtonDown(0))
        {
            ForceGrab(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0))
        {
            ForceRelease();
        }

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
    public void ForceGrab(Vector3 position)
    {
        RaycastHit hit;
        Vector3 jedi_pos = gameObject.transform.position;

        // get ray from camera to cast
        Ray camera_ray = camera.ScreenPointToRay(position);

        Debug.DrawRay(camera_ray.origin, camera_ray.direction*100, Color.blue);

        if (Physics.Raycast(camera_ray, out hit))
        {
            // No object select, grab one if available
            if (grabObject == null)
            {
                if (hit.collider.gameObject && hit.collider.gameObject.layer == LayerMask.NameToLayer("Pushable layer"))
                {
                    grabObject = hit.collider.gameObject;
                    screenPoint = Camera.main.WorldToScreenPoint(grabObject.transform.position);
                    offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, screenPoint.z));
                    if (grabObject.tag == "Enemy")
                    {
                        grabObject.GetComponent<EnemyMovement>().forceAffected = true;
                    }
                }
            }
        }
    }

    void ForceMove(Vector3 position)
    {
        if(grabObject != null)
        {
            Vector3 curScreenPoint = new Vector3(position.x, position.y, screenPoint.z);

            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
            grabObject.transform.position = curPosition;
            Debug.Log(position);
        }
    }

    void ForceRelease()
    {
        if (grabObject.tag == "Enemy")
        {
            grabObject.GetComponent<EnemyMovement>().forceAffected = false;
        }

        grabObject = null;
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
                    if (hit_object.tag == "Enemy")
                    {
                        hit_object.GetComponent<EnemyMovement>().forceAffected = true;
                    }

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
