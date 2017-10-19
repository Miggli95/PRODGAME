using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindScript : MonoBehaviour {

    public AudioSource goal;
    public AudioSource sensor;
    public AudioClip goalSound;
    public AudioClip sensorSoundRight;
    public AudioClip sensorSoundLeft;
    RaycastHit rayHit;
    AimAssist aim;
    public bool hit;
    Ray ray;
    public float rayLength;
    public float multiplier = 2;
    public float originalPitch = 1;
    public List<SoundSignal> blindPath; 
	// Use this for initialization
	void Start ()
    {
        aim = transform.GetComponentInChildren<AimAssist>();
        if(sensor == null)
            sensor = transform.GetComponent<AudioSource>();
        sensor.clip = sensorSoundRight;
        goal = GameObject.FindGameObjectWithTag("Goal").GetComponent<AudioSource>();
        goal.maxDistance = (goal.transform.position - transform.position).magnitude+10; 
        goal.clip = goalSound;

    }
    float time;
    float timer;
    private bool mouseButtonPressed;
    AudioClip currentSensorSound;
    // Update is called once per frame
    void Update ()
    {
        if (aim.Angle > 0 || aim.Angle < -180)
        {
            sensor.pitch = 1;
        }

        else
        {
            sensor.pitch = 2;
        }

       if (aim.Angle > 90 || aim.Angle < -90)
        {
            if (sensor.clip != goalSound)
            {
                sensor.clip = sensorSoundLeft;
            }
            
            Debug.Log("Aiming Left");
        }

        else
        {
            if (sensor.clip != goalSound)
            {
                sensor.clip = sensorSoundRight;
            }
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
            if (sensor.clip != goalSound || currentSensorSound == null)
            {
                currentSensorSound = sensor.clip;
            }

            sensor.clip = goalSound;
        }

        else
        {
            if(sensor.clip == goalSound)
            sensor.clip = currentSensorSound;
        }

        if (timer > time && !sensor.isPlaying || sensor.clip != goalSound && timer > time)
        {
            sensor.PlayOneShot(sensor.clip);
            timer = 0;
        }

       
        if (blindPath[0] == null)
        {
            if (blindPath.Count > 1)
            {
                blindPath[0] = blindPath[1];
                blindPath.Remove(blindPath[1]);
            }
        }

        sensor.mute = blindPath[0].source.isPlaying;
        if (Input.GetMouseButton(1) && !blindPath[0].playingSound)
        {
            //Debug.Log("goalSound");

            blindPath[0].PlaySound(goalSound);
        }
    }
}
