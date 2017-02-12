using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;


public class PlayerKinectManager : MonoBehaviour {

    KinectManager km;
    private bool readyToShoot = false;
    public GameObject eggPrefab;
    public FirstPersonController fpsC;

	// Use this for initialization
	void Start () {
        km = this.gameObject.GetComponent<KinectManager>();
	}

    // Update is called once per frame
    void Update() {
        lightSaber();

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

    void lightSaber()
    {
        Vector3 rightHand = km.GetJointPosition(km.GetPlayer1ID(), 11);
        Vector3 leftHand = km.GetJointPosition(km.GetPlayer1ID(), 7);

        // Make sure users hand are on top of each other
        float distance = Vector3.Distance(rightHand, leftHand);
        //Debug.Log("Right hand: " + rightHand.y + " Lefhand: " + leftHand.y);
        Debug.Log(Mathf.Abs(rightHand.y - leftHand.y));
        //if (Vector3.Distance(rightHand, leftHand) < 0.15)
        if(Mathf.Abs(rightHand.y - leftHand.y) < 0.15 && Mathf.Abs(rightHand.y - leftHand.y) > 0.03
            && Mathf.Abs(rightHand.x - leftHand.x) < 0.15 && Mathf.Abs(rightHand.z - leftHand.z) < 0.15)
        {
            Debug.Log("GREEN: " + distance);
        }
    }
}
