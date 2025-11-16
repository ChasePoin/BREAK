using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Start : MonoBehaviour
{

   public void StartButton(){
        SceneManager.LoadScene("CamTesting");//whatever the start scene is
		Debug.Log ("start button clicked");
   }
    
}
