using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAssist : MonoBehaviour {
    public float Angle;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerPos = Camera.main.WorldToScreenPoint(this.transform.position);//kanske vara parent transform
        mousePos.x = mousePos.x - playerPos.x;
        mousePos.y = mousePos.y - playerPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        Angle = angle;
        this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

    }
}
