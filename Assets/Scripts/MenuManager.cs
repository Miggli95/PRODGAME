using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour

 

{


    public GameObject instructionOBJ;

    public void toGame()
    {

        SceneManager.LoadScene("Level1");
    }

    public void toBlind()
    {

        SceneManager.LoadScene("BlindLevel");
    }

    public void toLow()
    {

        SceneManager.LoadScene("Level1Eye");
    }

    public void toIns()
    {
        instructionOBJ.SetActive(true);
       
    }





}
