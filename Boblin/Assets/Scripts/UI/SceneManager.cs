using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagement : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene(1);
        Debug.Log("Scene Loaded");
    }

    public void LoadInstructionsScene()
    {
        SceneManager.LoadScene(2);
        Debug.Log("Scene Loaded");
    }
}
