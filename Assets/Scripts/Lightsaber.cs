using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightsaber : MonoBehaviour {

    LineRenderer lineRenderer;
    public Transform start;
    public Transform end;

    private float textureOffset = 0f;
    private bool saber_on = false;
    private Vector3 endOffset;

    // default colors to blue
    public static int color_choice = 0;
    private Color color_1 = new Color(11, 58, 188);
    private Color color_2 = new Color(3, 44, 124);


	// Use this for initialization
	void Start () {
        
        lineRenderer = GetComponent<LineRenderer>();

        /*
        Gradient g;
        GradientColorKey[] gck;
        GradientAlphaKey[] gak;
        g = new Gradient();
        gck = new GradientColorKey[2];
        gck[0].color = Color.red;
        gck[0].time = 0.0F;
        gck[1].color = Color.blue;
        gck[1].time = 1.0F;

        gak = new GradientAlphaKey[2];
        gak[0].alpha = 1.0F;
        gak[0].time = 0.0F;
        gak[1].alpha = 0.0F;
        gak[1].time = 1.0F;
        g.SetKeys(gck, gak);
        Debug.Log(g.Evaluate(0.25F));
        lineRenderer.colorGradient = g;

        lineRenderer.startColor = color_1;
        lineRenderer.endColor = color_2;
        

        //lineRenderer.colorGradient = g;
        */
        endOffset = end.localPosition;
	}
	
	// Update is called once per frame
	void Update () {

        //kinect stuff!
        if(Input.GetKeyDown(KeyCode.Space))
        {
            saber_on = !saber_on;
        }

        if (saber_on)
        {
            // extend laser
            end.localPosition = Vector3.Lerp(end.localPosition, endOffset, Time.deltaTime * 10f);
            //play sound
        }
        else
        {
            // hide laser
            end.localPosition = Vector3.Lerp(end.localPosition, start.localPosition, Time.deltaTime * 10f);

            //play sound
        }


        //update rendered line
        lineRenderer.SetPosition(0, start.position);
        lineRenderer.SetPosition(1, end.position);

        //move texture
        textureOffset = textureOffset - Time.deltaTime * 4f;
                                                        //bigger numbers go faster
        if( textureOffset < -10f)
        {
            textureOffset = textureOffset + 10f;
        }
        lineRenderer.sharedMaterials[1].SetTextureOffset("_MainTex", new Vector2(textureOffset, 0f));
    }

    // change lightsaber state
    public void toggleLightsaber(bool toggle)
    {
        saber_on = toggle;
    }
    // change transform of lightsaber with kinect data
    public void updateTransform(Vector3 LHand, Vector3 RHand)
    {
        // find direction of ray created from hands
        Ray lightsaberRay = new Ray(LHand, RHand - LHand);
        // rotation our lighsaber accordingly
        this.gameObject.transform.Rotate(lightsaberRay.direction.normalized);
    }



}
