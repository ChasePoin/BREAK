using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    public void ExitButton(){
        Application.Quit();
        Debug.Log("Exit button clicked");
    }
    
}
