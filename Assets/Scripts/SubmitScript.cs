using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubmitScript : MonoBehaviour
{
    
    InputField inputField;
    Button button;
    public GameObject content;
    Text submitText;
    int playerNumber;
	// Use this for initialization
	void Start ()
    {
        playerNumber = PlayerPrefs.GetInt("playerNumber", 0);
        inputField = gameObject.transform.parent.GetComponentInChildren<InputField>();
	}

    public void Submit()
    {
        if (inputField.text.Length > 0)
        {
            playerNumber++;
            GameObject submitObject = new GameObject();
            Transform parent;
            //submitText.text = inputField.text;
            submitObject.transform.SetParent(content.transform);
            parent = submitObject.transform.parent;
            submitObject.AddComponent<Text>();
            submitObject.GetComponent<Text>().text = "Player " + playerNumber + ": " + inputField.text;
            submitObject.GetComponent<Text>().font = Font.CreateDynamicFontFromOSFont("Arial", 10);
            submitObject.GetComponent<Text>().color = new Color(0, 0, 0);
            submitObject.name = "player " + (parent.childCount - 1);
            submitObject.transform.SetAsLastSibling();
            inputField.text = "";
            content.GetComponent<SaveContent>().UpdateText(submitObject.GetComponent<Text>().text);
            PlayerPrefs.SetInt("playerNumber", playerNumber);
           // submitObject.transform.position = submitObject.transform.parent.GetChild(parent.childCount - 2).transform.position;

        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
