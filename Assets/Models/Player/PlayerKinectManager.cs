using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;


public class PlayerKinectManager : MonoBehaviour {

    KinectManager km;
    private bool readyToShoot = false;
    public GameObject eggPrefab;
    public FirstPersonController fpsC;
    
    //
    public TheForce the_force;

    //light saber variables
    public lightsaber light_saber;
    public bool lightsaber_on = false;

    // joint locations
    Vector3 localRHand;
    Vector3 localLHand;
    Vector3 localHead;
    Vector3 globalRHand;
    Vector3 globalLHand;


    // Use this for initialization
    void Start () {
        km = this.gameObject.GetComponent<KinectManager>();
	}

    // Update is called once per frame
    void Update() {

        localRHand = km.GetJointLocalPosition(km.GetPlayer1ID(), 11);
        localLHand = km.GetJointLocalPosition(km.GetPlayer1ID(), 7);
        localHead = km.GetJointLocalPosition(km.GetPlayer1ID(), 3);
        globalRHand = km.GetJointPosition(km.GetPlayer1ID(), 11);
        globalLHand = km.GetJointPosition(km.GetPlayer1ID(), 7);
        
        //check lightsaber
        checkLightsaber();
        if (!lightsaber_on)
        {
            if( checkLightning() )
            {
                // do lightning
                the_force.ForceLightning();
            }
            else if(checkheal())
            {
                the_force.ForceHeal();
            }
            
        }





        //Debug.Log("X distance: " + Mathf.Abs(globalRHand.x - globalLHand.x) + " Y: " + Mathf.Abs(globalRHand.y - globalLHand.y));
        Debug.Log(Vector3.Distance(globalRHand, globalLHand));




        //lightSaber();

        /*
        Vector3 leftElbow = km.GetJointPosition(km.GetPlayer1ID(), 5);
        Vector3 rightElbow = km.GetJointPosition(km.GetPlayer1ID(), 9);
        Vector3 neck = km.GetJointPosition(km.GetPlayer1ID(), 2);

        if (leftElbow.y > neck.y && rightElbow.y > neck.y && readyToShoot == false)
        {
            readyToShoot = true;
            print("Ready to shoot an egg!");
        }

        if(readyToShoot && leftElbow.y < neck.y && rightElbow.y < neck.y)
        {
            readyToShoot = false;
            print("Shot an egg!");

            GameObject.Instantiate(eggPrefab, transform.position, transform.rotation);
        }

        if (leftElbow.z > rightElbow.z && Mathf.Abs(leftElbow.z - rightElbow.z) >= .5)
            fpsC.TurnLeft();
        if (rightElbow.z > leftElbow.z && Mathf.Abs(leftElbow.z - rightElbow.z) >= .5)
            fpsC.TurnRight();
            */

        


	}

    void checkLightsaber()
    {
        // Make sure users hand are on top of each other
        float distance = Vector3.Distance(globalRHand, globalLHand);
        //Debug.Log("Right hand: " + globalRHand.y + " Lefhand: " + globalLHand.y);
        //Debug.Log(Mathf.Abs(globalRHand.y - globalLHand.y));
        //if (Vector3.Distance(globalRHand, globalLHand) < 0.15)
        //Debug.Log( "Local R: " + localRHand.ToString() + " L: " + localLHand.ToString());
        
        //check less strict rules for having lightsaber out
        if (lightsaber_on)
        {
            if (distance < 0.2f)
                return;

            if (!( Mathf.Abs(globalRHand.y - globalLHand.y) < 0.3f && Mathf.Abs(globalRHand.y - globalLHand.y) > 0.03f
                && Mathf.Abs(globalRHand.x - globalLHand.x) < 0.3f && Mathf.Abs(globalRHand.z - globalLHand.z) < 0.8f ))
            {
                // turn lightsaber off
                lightsaber_on = false;
                light_saber.toggleLightsaber(lightsaber_on);
            }
        }
        else
        {

            if (Mathf.Abs(globalRHand.y - globalLHand.y) < 0.15f && Mathf.Abs(globalRHand.y - globalLHand.y) > 0.03f
                && Mathf.Abs(globalRHand.x - globalLHand.x) < 0.15f && Mathf.Abs(globalRHand.z - globalLHand.z) < 0.15f)
            {
                Debug.Log("GREEN: " + distance);
                // turn lightsaber on
                lightsaber_on = true;
                light_saber.toggleLightsaber(lightsaber_on);
            }
        }



    }

    bool checkLightning()
    {
        

        if (    ((localRHand.y < 0.04f) && (localRHand.y > 0.0f) && (localLHand.y < 0.00f) && (localRHand.x < 0.01f && localRHand.x > -0.01f) )
            || ((localLHand.y < 0.04f) && (localLHand.y > 0.0f) && (localRHand.y < 0.00f)) )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool checkheal()
    {

        // check that both hands are above head
        if ( (localRHand.y - localHead.y) > 0.0 && (localRHand.y - localHead.y) > 0.0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}
