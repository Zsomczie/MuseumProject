using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class SceneSwitcher : MonoBehaviour
{
    ARSession session;
    public void SwitchToFlowerScene()
    {
        session = FindObjectOfType<ARSession>();
        session.Reset();
        SceneManager.LoadScene("FlowerField");
    }
    public void SwitchToPaintingScene() 
    {
       session = FindObjectOfType<ARSession>();
        session.Reset();
        SceneManager.LoadScene("Museum");

    }
    private void OnApplicationQuit()
    {
        session = FindObjectOfType<ARSession>();
        session.Reset();
    }
}
