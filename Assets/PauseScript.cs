using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    public GameObject pauseScreen;
    public bool paused = false;
    public GameObject player;
    public ShootScript shootScript;
	// Use this for initialization
	void Start ()
    {
        pauseScreen = GameObject.FindGameObjectWithTag("PauseScreen").transform.GetChild(0).gameObject;
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
            shootScript.enabled = false;
            player.GetComponentInChildren<AimAssist>().enabled = false;
            Time.timeScale = 0;
            
        }

        else
        {
            //player.GetComponent<ShootScript>().enabled = true;
            shootScript.enabled = true;
            player.GetComponentInChildren<AimAssist>().enabled = true;
            Time.timeScale = 1;
        }
    }
	// Update is called once per frame
	void Update ()
    {
		
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
