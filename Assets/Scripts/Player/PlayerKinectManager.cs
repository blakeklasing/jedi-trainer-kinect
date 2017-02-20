using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;


public class PlayerKinectManager : MonoBehaviour {

    KinectManager km;

    // UI manipulation variables
    public GameObject textObject;
    private Text textComponent;

    public TheForce theForce;

    // Kinect State
    private State state;
    private State prevState;
    enum State { waiting, lightsaberReady, lightsaberDrawn, lightning, heal, pushReady, pushing, pushFinal, grabReady, grab, grabbed, grabRelease, future, futureReady};
    float resetTime;

    //light saber variables
    public Lightsaber lightsaber;
    public bool lightsaberOn = false;

    // Future variables
    private Vector3 prevSpineBase;

    // joint locations
    Vector3 globalRHand;
    Vector3 globalLHand;
    Vector3 globalHead;
    Vector3 globalSpineBase;
    Vector3 globalNeck;
    Vector3 globalLShoulder;
    Vector3 globalRShoulder;
    Vector3 globalLElbow;
    Vector3 globalRElbow;
    Vector3 globalAnkleRight;
    Vector3 globalAnkleLeft;

    // Use this for initialization
    void Start () {
        km = this.gameObject.GetComponent<KinectManager>();
        textComponent = textObject.GetComponent<Text>();
	}

    // Update is called once per frame
    void Update() {
        //Ray ray = new Ray(globalRShoulder, globalRHand - globalRShoulder);
        //Debug.DrawRay(ray.origin, ray.direction*100, Color.blue);

        // get kinect data
        updateJointData();
        
        // reset timer
        /*if (resetTime)
        {

        }*/

        // update state based on previous state
        state = currentState(state);
        //DEBUG
        textComponent.text = state.ToString() + " ~ ";
        // perform an action
        switch (state)
        {
	    case State.lightsaberReady:
                lightsaber.setHandPositions(globalRHand, globalLHand);
                lightsaber.toggleLightsaber(true);
                break;
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
                theForce.ForceGrab();
                Debug.Log("forceGrab");
                break;
            case State.grabbed:
                Ray ray1 = new Ray(globalRShoulder, globalRHand - globalRShoulder);
                Ray ray2 = new Ray(globalRShoulder, globalRElbow - globalRShoulder);
                Ray ray3 = new Ray(globalRElbow, globalRHand - globalRElbow);
                theForce.ForceMove((ray1.direction + ray2.direction + ray3.direction) / 3);
                Debug.Log("forceMove");
                break;
            case State.grabRelease:
                theForce.ForceRelease();
                state = State.waiting;
                Debug.Log("forceRelease");
                break;
            case State.pushFinal:
                theForce.ForcePush();
                Debug.Log("forcePush");
                state = State.waiting;
                break;
            case State.future:
                theForce.ForceFuture();
                state = State.waiting;
                Debug.Log("forceFuture");
                break;
            default:
                break;
        }
        
        //DEBUG
        //textComponent.text = textComponent.text + " ::   L: " + globalLHand.y + " | S: " + globalSpineShoulder.y;
        //textComponent.text = textComponent.text + "\nshoulderLeft: " + km.GetJointPosition(km.GetPlayer1ID(), 4).z + ", " + km.GetJointPosition(km.GetPlayer1ID(), 4).y + "\n" + "handLeft: " + 
        //                        globalLHand.z + ", " + globalLHand.y;
        ///textComponent.text = textComponent.text + "\nSpine base: " + globalSpineBase;
        //textComponent.text = textComponent.text + "\nHeight: " + Mathf.Abs(globalHead.y - (globalAnkleRight.y + globalAnkleLeft.y) / 2.0f);


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
        //shoulders
        globalLShoulder = km.GetJointPosition(km.GetPlayer1ID(), 4);
        globalRShoulder = km.GetJointPosition(km.GetPlayer1ID(), 8);
        //elbow
        globalLElbow = km.GetJointPosition(km.GetPlayer1ID(), 5);
        globalRElbow = km.GetJointPosition(km.GetPlayer1ID(), 9);
        // Ankles
        globalAnkleLeft = km.GetJointPosition(km.GetPlayer1ID(), 14);
        globalAnkleRight = km.GetJointPosition(km.GetPlayer1ID(), 18);
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
                resetTime = Time.time;
                return State.grabReady;
            }
            else if ( isPushReady() )
            {
                resetTime = Time.time;
                return State.pushReady;
            }
            else if (isFutureReady() )
            {
                resetTime = Time.time;
                return State.futureReady;
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
            if (prevState == State.lightsaberReady || prevState == State.lightsaberDrawn)
            {
                if (isLightsaberDrawn())
                    return State.lightsaberDrawn;
                else
                {
                    lightsaber.toggleLightsaber(false);
                    return State.waiting;
                }
                    
            }
            else if (prevState == State.pushReady)
            {
                //if we have been in pushReady for more than 2.0 seconds, get out
                if (resetTime + 2.0 < Time.time)
                    return State.waiting;
                else if ( isPushReady() )
                    return State.pushReady;
                else if( isPush() )
                {
                    return State.pushFinal;
                }
                else
                {
                    return State.pushReady;
                }
            } else if (prevState == State.grabReady) {
                theForce.ForceHighlight(globalRElbow, globalRHand - globalRElbow);

                //if we have been in pushReady for more than 2.0 seconds, get out
                if (resetTime + 2.0 < Time.time)
                    return State.waiting;
                else if (isGrab())
                    return State.grab;
                else if (isGrabReady())
                    return State.grabReady;
            } else if (prevState == State.grab || prevState == State.grabbed) {
                if (isGrabReleased())
                    return State.grabRelease;
                else
                    return State.grabbed;
            }
            else if (prevState == State.lightning)
            {
                return State.lightning;
            }
            else if (prevState == State.futureReady)
            {
                if (resetTime + 2.0 < Time.time)
                    return State.waiting;
                else if (isFuture())
                    return State.future;
                else
                    return State.futureReady;
            }

        }
        return State.waiting;
    }

    /* ----------- State Functions ----------- */
    bool isLightsaberReady()
    {
        // check if hand is in lightsaber ready position
	
	float spineLength       = Vector3.Distance(globalNeck, globalSpineBase);
        bool handsLow           = globalLHand.y < (globalSpineBase.y + spineLength) && globalRHand.y < (globalSpineBase.y + spineLength);
        float handDistance_Y    = Mathf.Abs(globalRHand.y - globalLHand.y);
        float handDistance_X    = Mathf.Abs(globalRHand.x - globalLHand.x);
        float handDistance      = Vector3.Distance(globalRHand, globalLHand);
        
        if (Mathf.Abs(globalRHand.y - globalLHand.y) < 0.15f && Mathf.Abs(globalRHand.y - globalLHand.y) > 0.03f
            && Mathf.Abs(globalRHand.x - globalLHand.x) < 0.15f && Mathf.Abs(globalRHand.z - globalLHand.z) < 0.15f
            && handsLow)
        {
            return true;
        }
	
        return false;
    }
    bool isLightsaberDrawn()
    {
        // check if hands are in lightsaber drawn position
	/*/ check if hands are in lightsaber drawn position
        if (!(Mathf.Abs(globalRHand.y - globalLHand.y) < 0.3f && Mathf.Abs(globalRHand.y - globalLHand.y) > 0.03f
            && Mathf.Abs(globalRHand.x - globalLHand.x) < 0.3f && Mathf.Abs(globalRHand.z - globalLHand.z) < 0.8f))
        {
            // turn lightsaber off
            lightsaberOn = false;
            
        }
        */

        return true;
    }
    bool isLightningReady()
    {
        float spineLength = Vector3.Distance(globalNeck, globalSpineBase);
        float handDistance_Y = Mathf.Abs(globalRHand.y - globalLHand.y);

        bool leftRaised  = (globalLHand.y - globalNeck.y < 0.3 && globalLHand.y > (globalSpineBase.y + spineLength / 2.0f) && globalRHand.y < (globalSpineBase.y + spineLength / 2.0f));
        bool leftExtended = (globalLShoulder.z - globalLHand.z) > 0.4f;

        if (leftExtended && leftRaised && handDistance_Y > 0.4)
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
        float handDistance_Y = Mathf.Abs(globalRHand.y - globalLHand.y);

        bool handsAtShoulderHeight = ( (Mathf.Abs(globalLHand.y - globalLShoulder.y) < 0.2f)  && (Mathf.Abs(globalRHand.y - globalRShoulder.y) < 0.2f) );
        bool handsAtShoulderDepth = ((Mathf.Abs(globalLHand.z - globalLShoulder.z) < 0.4f) && (Mathf.Abs(globalRHand.z - globalRShoulder.z) < 0.4f));
        // check if hands are above head
        if (handsAtShoulderHeight && handsAtShoulderDepth && handDistance_Y < 0.3)
        {
            return true;
        }
        return false;
    }
    /*
    bool isPushing()
    {
        float handDistance_Y = Mathf.Abs(globalRHand.y - globalLHand.y);

        bool handsAtShoulderHeight = ((Mathf.Abs(globalLHand.y - globalLShoulder.y) < 0.2f) && (Mathf.Abs(globalRHand.y - globalRShoulder.y) < 0.2f));
        bool handsAtShoulderDepth  = ((Mathf.Abs(globalLHand.z - globalLShoulder.z) < 0.4f) && (Mathf.Abs(globalRHand.z - globalRShoulder.z) < 0.4f));
        // check if hands are above head
        if (handsAtShoulderHeight && handsAtShoulderDepth && handDistance_Y < 0.3)
        {
            return true;
        }
        return false;
    }
     */
    bool isPush()
    {
        float handDistance_Y = Mathf.Abs(globalRHand.y - globalLHand.y);

        bool handsAtShoulderHeight  = ((Mathf.Abs(globalLHand.y - globalLShoulder.y) < 0.2f) && (Mathf.Abs(globalRHand.y - globalRShoulder.y) < 0.2f));
        bool handsExtended        = ((globalRShoulder.z - globalRHand.z) > 0.45f) && ((globalLShoulder.z - globalLHand.z) > 0.45f);
        bool handsElbows = (Mathf.Abs(globalRElbow.z - globalRHand.z) < 0.3f) && (Mathf.Abs(globalLElbow.z - globalLHand.z) < 0.3f);
        // check if hands are extended
        if (handsAtShoulderHeight && handsExtended && handsElbows && handDistance_Y < 0.3)
        {
            return true;
        }
        return false;
    }

    bool isGrabReady()
    {
        float spineLength = Vector3.Distance(globalNeck, globalSpineBase);
        float handDistance_Y = Mathf.Abs(globalRHand.y - globalLHand.y);

        bool rightRaised = (globalRHand.y - globalNeck.y < 0.3 && globalRHand.y > (globalSpineBase.y + spineLength / 2.0f) && globalLHand.y < (globalSpineBase.y + spineLength / 2.0f));
        bool rightExtended = (globalRShoulder.z - globalRHand.z) > 0.4f;
        // check left hand.y close to neck.y
        if (rightRaised && rightExtended && handDistance_Y > 0.4)
            return true;
        else
            return false;
    }

    bool isGrabReleased()
    {
        float spineLength = Vector3.Distance(globalNeck, globalSpineBase);

        bool rightLowered = (globalRHand.y < (globalSpineBase.y + spineLength / 2.0f));
        // check left hand.y close to neck.y
        if (rightLowered)
            return true;
        else
            return false;
    }

    bool isGrab()
    {
        bool headNod = Mathf.Abs(globalHead.x - globalNeck.x) > 0.1;
        if (headNod) {
            Debug.Log("Nod");
            return true;
        }   
        else
            return false;
        
        /*
        float spineLength = Vector3.Distance(globalNeck, globalSpineBase);
        float handDistance_Y = Mathf.Abs(globalRHand.y - globalLHand.y);

        bool leftRaised  = (globalLHand.y - globalNeck.y < 0.3 && globalLHand.y > (globalSpineBase.y + spineLength / 2.0f));
        bool leftExtended = (globalLShoulder.z - globalLHand.z) > 0.4f;
        bool rightRaised = (globalRHand.y - globalNeck.y < 0.3 && globalRHand.y > (globalSpineBase.y + spineLength / 2.0f));
        bool rightExtended = (globalRShoulder.z - globalRHand.z) > 0.4f;
        // check left hand.y close to neck.y
        if (leftRaised && leftExtended && rightRaised && rightExtended && handDistance_Y < 0.2)
            return true;
        else
            return false;
         */
    }

    bool isFutureReady()
    {
        if ((globalSpineBase.y > 0.5f && globalSpineBase.y < 0.9f))
        {
            prevSpineBase = globalSpineBase;
            return true;
        }
        return false;
    }

    bool isFuture()
    {
        bool jumped = (globalSpineBase.y - prevSpineBase.y) > 0.1f && Mathf.Abs(globalSpineBase.x - prevSpineBase.x) < 0.2f;
        if (jumped)
            return true;
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

    Vector3 KinectToScreenPoints(Vector3 position)
    {
        Debug.Log(Screen.width);
        Debug.Log(Screen.height);
        int x = (int) (Mathf.Floor((position.x * 0.5f) + 0.5f) * Screen.width);
        int y = (int) (Mathf.Floor((position.y * -0.5f) + 0.5f) * Screen.height);
        return new Vector3(x, y, 0);
    }
	

    State getState()
    {
        return this.state;
    }
	
}
