using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public Button startButton;
    public Button exitButton;
    public GameObject firstSelected;
    

    	void Start () {
		Button btn_e = exitButton.GetComponent<Button>();
		btn_e.onClick.AddListener(OnClickExit);
        Button btn_s = startButton.GetComponent<Button>();
		btn_s.onClick.AddListener(OnClickStart);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelected);
        
	}

	void OnClickExit(){
        Application.Quit();
		Debug.Log ("exit button clicked");
    }
    

    void OnClickStart(){
        SceneManager.LoadScene("skybox_test");
		Debug.Log ("start button clicked");
    }
}
