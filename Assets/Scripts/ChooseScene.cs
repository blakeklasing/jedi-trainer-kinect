using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseScene : MonoBehaviour {

    int sceneIndex;

    public void LoadByIndex(int sceneIndex)
    {
        //set the current scene
        SceneManager.LoadScene(sceneIndex);
    }

}
