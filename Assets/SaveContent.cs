using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveContent : MonoBehaviour {
    GameObject textObject;
	// Use this for initialization
	void Start ()
    {
        textObject = gameObject.transform.GetChild(0).gameObject;
        string text = textObject.GetComponent<Text>().text;
        text  = PlayerPrefs.GetString("HelpText","");
        textObject.SetActive(text.Length>0);
        textObject.GetComponent<Text>().text = text;
        //PlayerPrefs.SetString("HelpText", text);
        //PlayerPrefs.Save();
	}

    public void UpdateText(string inputText)
    {
        string text = PlayerPrefs.GetString("HelpText","");
        text += inputText + "\n";
        PlayerPrefs.SetString("HelpText", text);
        
    }

	// Update is called once per frame
	void Update () {
		
	}
}
