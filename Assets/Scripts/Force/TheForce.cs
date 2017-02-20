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

    // ForceFuture
    public ForceFuture forceFuture;
    float force_timer;

    // grab variables
    public Material highlightMaterial;
    Material prevMaterial;
    GameObject grabObject = null;
    Vector3 screenPoint;
    Vector3 offset;
    float startTime;

    float objectDistance;

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
        /*
        if (grabObject != null) {
            ForceMove(Input.mousePosition);
        }
        if (Input.GetMouseButton(0)) {
            ForceGrab(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0)) {
            ForceRelease();
        }
         */

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
        if (Input.GetKeyDown(KeyCode.T))
        {
            ForceFuture();
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

    public void ForceHighlight(Vector3 position, Vector3 direction)
    {
        //Ray ray = camera.ScreenPointToRay(position);
        direction.x = -1 * direction.x;
        Ray ray = new Ray(gameObject.GetComponentInChildren<Camera>().transform.position, direction.normalized*-100);
        Debug.Log(ray.origin);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Hit: " + hit.collider.gameObject.name);
            if (grabObject != null)
            {
                grabObject.GetComponent<Renderer>().material = prevMaterial;
                Debug.Log("UnHighlight: " + grabObject.name);
            }

            if (hit.collider.gameObject.layer == 9)
            {
                grabObject = hit.collider.gameObject;
                prevMaterial = grabObject.GetComponent<Renderer>().material;
                grabObject.GetComponent<Renderer>().material = highlightMaterial;
                Debug.Log("Highlight: " + grabObject.name);
            }
            /*
            else
            {
                grabObject.GetComponent<Renderer>().material = prevMaterial;
                Debug.Log("UnHighlight: " + grabObject.name);
            }
            */


            //Debug.DrawLine(ray.origin, ray.direction * 20, Color.red, 20);
            //Debug.DrawRay(ray.origin, ray.direction * 20, Color.red, 20);



            /*
            Debug.Log("Hit: " + hit.collider.gameObject.name);
            if (grabObject != null && hit.collider.gameObject != grabObject)
            {
                Debug.Log("Material: " + prevMaterial.name);
                grabObject.GetComponent<Renderer>().material = prevMaterial;
                grabObject = null;
                prevMaterial = null;
            }

            if (hit.collider.gameObject && hit.collider.gameObject.layer == 9)
            {
                Debug.Log("Highlight: " + hit.collider.gameObject.name);
                grabObject = hit.collider.gameObject;
                prevMaterial = new Material(grabObject.GetComponent<Renderer>().material);
                grabObject.GetComponent<Renderer>().material = highlightMaterial;
            }
            */
        }
    }

    // grab
    public void ForceGrab()
    {
        objectDistance = Vector3.Distance(gameObject.GetComponentInChildren<Camera>().transform.position, grabObject.transform.position);

        if (grabObject.tag == "Enemy")
            grabObject.GetComponent<EnemyMovement>().forceAffected = true;

        startTime = Time.time;
        Debug.Log("Init force grab: " + grabObject.transform.position);
    }

    public void ForceMove(Vector3 direction)
    {
        if (grabObject != null) {

            //Vector3 currentScreenPoint = new Vector3(position.x, position.y, screenPoint.z);
            //Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenPoint);
            Vector3 tempPosition = gameObject.GetComponentInChildren<Camera>().transform.position + direction * objectDistance;
            tempPosition.z = grabObject.transform.position.z;
            float distCovered = (Time.time - startTime) * 1.0f;
            float fracJourney = distCovered / Vector3.Distance(grabObject.transform.position, tempPosition);
            grabObject.transform.position = Vector3.Lerp(grabObject.transform.position, tempPosition, fracJourney);

            Debug.Log("forcemove: " + grabObject.transform.position);
            Debug.Log("distance: " + objectDistance);
        }
        
    }

    public void ForceRelease()
    {
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
        pushDirection = new Vector3(cameraRay.direction.x, cameraRay.direction.y + 0.1f, cameraRay.direction.z);

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
                    if (hit_object.tag == "Enemy") {
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
        if (forceFuture != null) {
            StartCoroutine(forceFuture.activate());
        }
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
