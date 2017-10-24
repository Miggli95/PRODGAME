using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackScript : MonoBehaviour 
{

    public GameObject pauseScreen;
    public bool paused = false;
    public GameObject player;
    public ShootScript shootScript;
    // Use this for initialization
    void Start()
    {
        pauseScreen = GameObject.FindGameObjectWithTag("PauseScreen").transform.GetChild(0).gameObject;

        player = GameObject.FindGameObjectWithTag("Player");
        shootScript = player.GetComponent<ShootScript>();
    }

  
    // Update is called once per frame
    void Update()
    {

    }

    public void Back()
    {
        pauseScreen.SetActive(true);
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}
