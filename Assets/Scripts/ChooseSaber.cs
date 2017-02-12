using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseSaber : MonoBehaviour {


    // Sith(red) = 0
    // Jedi(blue)  = 1
    public static int lightsaber_color;

    // choose the color of your lightsaber
    public void ChooseLightsaberColor(int color)
    {
        lightsaber_color = color;
    }

    public int GetLightsaberColor()
    {
        return lightsaber_color;
    }

    
}
