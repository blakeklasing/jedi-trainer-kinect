using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;


public class PlayerKinectManager : MonoBehaviour {

    KinectManager km;
    
    public FirstPersonController fpsC;

    // UI manipulation variables
    public GameObject textObject;
    private Text textComponent;
    
    public TheForce theForce;

    // Kinect State
    private State state;
    private State prevState;
    enum State { waiting, lightsaberReady, lightsaberDrawn, lightning, heal, pushReady, push, grabReady, grab, future};

    //light saber variables
    public Lightsaber lightsaber;
    public bool lightsaberOn = false;

    // joint locations
    Vector3 localRHand;
    Vector3 localLHand;
    Vector3 localHead;
    Vector3 globalRHand;
    Vector3 globalLHand;
    Vector3 globalHead;
    Vector3 globalSpineBase;
    Vector3 globalNeck;

    // Use this for initialization
    void Start () {
        km = this.gameObject.GetComponent<KinectManager>();
        //textComponent = textObject.GetComponent<Text>();
	}

    // Update is called once per frame
    void Update() {

        // get kinect data
        updateJointData();
        
        // update state based on previous state
        state = currentState(state);
        //textComponent.text = state.ToString() + " ~ ";
        switch(state)
        {
            case State.lightsaberDrawn:
                lightsaber.updateTransform(globalLHand, globalRHand);
                break;
            case State.heal:
                theForce.ForceHeal();
                state = State.waiting;
                break;
            case State.lightning:
                theForce.ForceLightning();
                state = State.waiting;
                break;
            case State.grab:
                break;
            case State.push:
                break;
            case State.future:
                break;
            default:
                break;
        }
        
        //DEBUG
        //textComponent.text = textComponent.text + " ::   L: " + globalLHand.y + " | S: " + globalSpineShoulder.y;
        //textComponent.text = "neck: " + km.GetJointPosition(km.GetPlayer1ID(), 2).y + "\nhead: " + globalHead.y;




        //check lightsaber
        /*
        checkLightsaber();
        if (!lightsaberOn)
        {
            if( checkLightning() )
            {
                // do lightning
                theForce.ForceLightning();
            }
            else if(checkheal())
            {
                theForce.ForceHeal();
            }
            
        }
        */


        
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

    void updateJointData()
    {
        //hands
        globalRHand = km.GetJointPosition(km.GetPlayer1ID(), 11);
        globalLHand = km.GetJointPosition(km.GetPlayer1ID(), 7);
        //head
        globalHead = km.GetJointPosition(km.GetPlayer1ID(), 3);
        //spine base
        globalSpineBase = km.GetJointPosition(km.GetPlayer1ID(), 0);
        //neck
        globalNeck = km.GetJointPosition(km.GetPlayer1ID(), 2);
    }

    /* ----------- Current State----------- */
    State currentState(State prevState)
    {
        if ( prevState == State.waiting )
        {   
            // check first level states
            if ( isLightsaberReady() )
            {
                return State.lightsaberReady;
            }
            else if ( isLightningReady() )
            {
                return State.lightning;
            }
            else if ( isHealReady() )
            {
                return State.heal;
            }
            else if ( isGrabReady() )
            {
                
            }
            else if ( isPushReady() )
            {
                return State.pushReady;
            }
            else if (isFutureReady() )
            {

            }
            // ....
            else
            {
                prevState = State.waiting;
            }

        }
        else
        {
            // check second level states
            if (prevState == State.lightsaberReady)
            {
                if (isLightsaberDrawn())
                {
                    return State.lightsaberDrawn;
                }
                else
                    return State.lightsaberReady;
            }
            else if (prevState == State.pushReady)
            {
                if( isPush() )
                {

                }
            }
            else if (prevState == State.lightning)
            {
                return State.lightning;
            }

        }
        
        return State.waiting;
    }



    /* ----------- State Functions ----------- */
    bool isLightsaberReady()
    {
        // check if hand is in lightsaber ready position

        return false;
    }
    bool isLightsaberDrawn()
    {
        // check if hands are in lightsaber drawn position

        return false;
    }
    bool isLightningReady()
    {
        float spineLength = Vector3.Distance(globalNeck, globalSpineBase);
        float handDistance_Y = Mathf.Abs(globalRHand.y - globalLHand.y);

        bool leftRaised  = (globalLHand.y - globalNeck.y < 0.3 && globalLHand.y > (globalSpineBase.y + spineLength / 2.0f) && globalRHand.y < (globalSpineBase.y + spineLength / 2.0f));
        bool rightRaised = (globalRHand.y - globalNeck.y < 0.3 && globalRHand.y > (globalSpineBase.y + spineLength / 2.0f) && globalLHand.y < (globalSpineBase.y + spineLength / 2.0f));
        // check left hand.y close to neck.y
        if ( (leftRaised || rightRaised) && handDistance_Y > 0.4)
            return true;
        else
            return false;
    }
    bool isHealReady()
    {
        float handDistance_Y = Mathf.Abs(globalRHand.y - globalLHand.y);

        bool handsOverHead = (globalLHand.y > (globalHead.y + 0.1f)) && (globalRHand.y > (globalHead.y + 0.1f));
        // check if hands are above head
        if ( handsOverHead && handDistance_Y < 0.3 )
        {
            return true;
        }
        return false;
    }
    bool isPushReady()
    {

        return false;
    }
    bool isGrabReady()
    {

        return false;
    }
    bool isFutureReady()
    {

        return false;
    }

    bool isPush()
    {
        return false;
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
        if (lightsaberOn)
        {
            if (distance < 0.2f)
                return;

            if (!( Mathf.Abs(globalRHand.y - globalLHand.y) < 0.3f && Mathf.Abs(globalRHand.y - globalLHand.y) > 0.03f
                && Mathf.Abs(globalRHand.x - globalLHand.x) < 0.3f && Mathf.Abs(globalRHand.z - globalLHand.z) < 0.8f ))
            {
                // turn lightsaber off
                lightsaberOn = false;
                lightsaber.toggleLightsaber(lightsaberOn);
            }
        }
        else
        {

            if (Mathf.Abs(globalRHand.y - globalLHand.y) < 0.15f && Mathf.Abs(globalRHand.y - globalLHand.y) > 0.03f
                && Mathf.Abs(globalRHand.x - globalLHand.x) < 0.15f && Mathf.Abs(globalRHand.z - globalLHand.z) < 0.15f)
            {
                Debug.Log("GREEN: " + distance);
                // turn lightsaber on
                lightsaberOn = true;
                lightsaber.toggleLightsaber(lightsaberOn);
            }
        }
    }  


}
