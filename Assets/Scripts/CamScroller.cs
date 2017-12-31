using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamScroller : MonoBehaviour {

    public float zoomValue;
    public float min;
    public float max;


    void Start () {
        zoomValue = GetComponent<Camera>().orthographicSize;
		
	}
	
	
	void Update () {

       

        float temp;
        temp = Input.GetAxis("Mouse ScrollWheel"); 

        if (temp > 0)
        {
            zoomValue--;

        }else if (temp < 0)
        {
            zoomValue++;
        }

        GetComponent<Camera>().orthographicSize = zoomValue;

        zoomValue = Mathf.Clamp(zoomValue, min, max);


    }
}
