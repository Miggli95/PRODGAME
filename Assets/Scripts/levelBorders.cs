using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelBorders : MonoBehaviour {


    void OnTriggerExit(Collider col)
    {
        int lvl = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(lvl);
    }
}
