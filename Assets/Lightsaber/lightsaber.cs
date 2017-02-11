using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightsaber : MonoBehaviour {

    LineRenderer lineRenderer;
    public Transform start;
    public Transform end;

    private float textureOffset = 0f;
    private bool saber_on = true;
    private Vector3 endOffset;


	// Use this for initialization
	void Start () {
        lineRenderer = GetComponent<LineRenderer>();
        endOffset = end.localPosition;
	}
	
	// Update is called once per frame
	void Update () {

        //kinect stuff!
        if(Input.GetKeyDown(KeyCode.Space))
        {
            saber_on = !saber_on;            
        }

        if(saber_on)
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
        textureOffset = textureOffset - Time.deltaTime * 2f;
                                                        //bigger numbers go faster
        if( textureOffset < -10f)
        {
            textureOffset = textureOffset + 10f;
        }
        lineRenderer.sharedMaterials[1].SetTextureOffset("_MainTex", new Vector2(textureOffset, 0f));
    }
}
