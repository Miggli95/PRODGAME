using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public GameObject pauseScreen;
    public GameObject helpScreen;
    public bool paused = false;
    public GameObject player;
    public ShootScript shootScript;
    public  GameObject instructionScreen;
    public GameObject pauseButton;
    // Use this for initialization
    void Start ()
    {
        Transform parent = GameObject.FindGameObjectWithTag("PauseScreen").transform;
        pauseButton = parent.GetChild(0).gameObject;
        pauseScreen = parent.GetChild(1).gameObject;
        helpScreen = parent.GetChild(2).gameObject;
        instructionScreen = parent.GetChild(3).gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        shootScript = player.GetComponent<ShootScript>();
       
    
    }

    public void Pause()
    {
        paused = !paused;
        pauseScreen.SetActive(paused);

        if (paused)
        {
            //player.GetComponent<ShootScript>().enabled = false;
            pauseButton.SetActive(false);
            shootScript.enabled = false;
            player.GetComponentInChildren<AimAssist>().enabled = false;
            Time.timeScale = 0;
            
        }

        else
        {
            pauseButton.SetActive(true);
            //player.GetComponent<ShootScript>().enabled = true;
            helpScreen.SetActive(false);
            shootScript.enabled = true;
            player.GetComponentInChildren<AimAssist>().enabled = true;
            Time.timeScale = 1;
            shootScript.MouseSetActive(true);
        }
    }
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Help()
    {
        helpScreen.SetActive(true);
        pauseScreen.SetActive(false);
    }

    public void Instructions()
    {
        instructionScreen.SetActive(true);
        pauseScreen.SetActive(false);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OnMouseOver()
    {
        shootScript.MouseSetActive(false);
    }

    public void OnMouseExit()
    {
        shootScript.MouseSetActive(true);
    }
}
