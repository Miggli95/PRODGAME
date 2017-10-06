using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindScript : MonoBehaviour {

    public AudioSource goal;
    public AudioSource sensor;
    public AudioClip goalSound;
    public AudioClip sensorSound;
    RaycastHit rayHit;
    AimAssist aim;
    public bool hit;
    Ray ray;
    public float rayLength;
    public float multiplier = 2;
    public float originalPitch = 1;
	// Use this for initialization
	void Start ()
    {
        aim = transform.GetComponentInChildren<AimAssist>();
        if(sensor == null)
            sensor = transform.GetComponent<AudioSource>();
        sensor.clip = sensorSound;
        goal = GameObject.FindGameObjectWithTag("Goal").GetComponent<AudioSource>();
        goal.maxDistance = (goal.transform.position - transform.position).magnitude+10; 
        goal.clip = goalSound;

    }
    float time;
    float timer;
    private bool mouseButtonPressed;

    // Update is called once per frame
    void Update ()
    {
       if (aim.Angle > 90 || aim.Angle < -90)
        {
            sensor.pitch = 2;
            Debug.Log("Aiming Left");
        }

        else
        {
            sensor.pitch = 1;
            Debug.Log("AimingRight");
        }
        
        Vector3 dir = Quaternion.AngleAxis(aim.Angle, transform.forward) * transform.right;
        ray = new Ray(transform.position, dir);
        hit = Physics.Raycast(ray,out rayHit);
        rayLength = rayHit.distance;

        timer += Time.deltaTime;
        time = rayLength / multiplier;

        if (rayHit.collider.CompareTag("Goal") || rayHit.collider.CompareTag("ExtraShoot") )
        {
            sensor.clip = goalSound;
        }

        else
        {
            sensor.clip = sensorSound;
        }

        if (timer > time && !sensor.isPlaying || sensor.clip != goalSound && timer > time)
        {
            sensor.PlayOneShot(sensor.clip);
            timer = 0;
        }

        sensor.mute = goal.isPlaying;

        if (Input.GetMouseButton(1) && !goal.isPlaying)
        {
            //Debug.Log("goalSound");
            goal.PlayOneShot(goalSound);
        }
    }
}
