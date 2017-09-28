using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShootScript : MonoBehaviour
{

    public float force;
    public float currentForce;
    public float min;
    public float max;
    public float currentPower;
    float increase = 1;
    public float multiplier = 1;
    bool mouseButtonPressed = false;
    public float increaseMultiplier = 1;
    public float GravityMultiplier = 1;
    private float gravityMultiplier;
    public Slider powerbar;
    public float powerbarTreshold;
    bool stuckOnWall = false;
    public bool canShoot = false;
    public float shootAngle = 0;
    public int numberOfShoots = 5;
    public bool infiniteShoots = false;
    public Text numberOfShootsTxt;
    public int currlvl;
    public int numberOfLevels;
    // Soundstuff -------------------------------------------

    public AudioClip wall;
    public AudioClip bounce;
    AudioSource audioSource;

    // ParticleStuff -----------------------------------------

    
    public GameObject wallP;

    // Camer animations

    public AnimationClip camAni;
    public GameObject cameraHelper;







    // Use this for initialization
    void Start()
    {
        powerbar.minValue = min;
        powerbar.maxValue = max;
        powerbarTreshold = max - min;
        powerbar.value = force;
        gravityMultiplier = GravityMultiplier;
        // Mathf.Clamp(force, min, max);
        numberOfShootsTxt.text = "Shoots: " + numberOfShoots;
        audioSource = GetComponent<AudioSource>();
        currlvl = SceneManager.GetActiveScene().buildIndex;
        numberOfLevels = SceneManager.sceneCountInBuildSettings;


    }

    void cameraActive(float length, bool active)
    {
        
        cameraHelper.SetActive(active);
     //   camAni.clip.
        
    }


    // Update is called once per frame
    bool restart = false;
    bool viewLevel = false;
    bool clipPlaying= false;
    float curTime;

    void CameraHelper()
    {
        float length = camAni.length;
        if (Input.GetKey(KeyCode.C) && !clipPlaying)
        {
            viewLevel = true;
            clipPlaying = true;
            cameraActive(length, true);
        }

        else if (viewLevel)
        {
            curTime += Time.deltaTime;
        }

        if (curTime >= length)
        {
            cameraActive(0, false);
            viewLevel = false;
            clipPlaying = false;
            curTime = 0;
        }
    }

    void Update()
    {
        CameraHelper();

        if (Input.GetKey(KeyCode.R))
        {
            restart = true;
        }
        else if (restart)
        {
            //int lvl = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currlvl);
        }



        numberOfShootsTxt.text = "Shoots: " + numberOfShoots;
       
        if (force != 0)
        {
            powerbar.value = Mathf.Abs(force);
        }

        else
        {
            powerbar.value = Mathf.Abs(currentForce);
        }

        if (force >= max)
        {
            increase = -1;
        }

        if (force <= min)
        {
            increase = 1;
        }

        if (Input.GetMouseButton(0) && canShoot)
        {
            force += increase * Time.deltaTime * increaseMultiplier;
            mouseButtonPressed = true;
        }

        else if (mouseButtonPressed)
        {
            currentForce = force;
            force = 0;
            if (numberOfShoots > 0 || infiniteShoots)
            {
                Shoot();
                if (!infiniteShoots)
                    numberOfShoots--;
                mouseButtonPressed = false;
            }

            else
            {
                SceneManager.LoadScene(currlvl);
            }

            //if (canShoot)
            //{

            //}
        }

        Gravity();
    }

    public void AddShoots(int shoots)
    {
        numberOfShoots += shoots;
    }

    void OnCollisionEnter(Collision col)
    {
        

        if (col.collider.CompareTag("Goal"))
        {
            int nextLvl = currlvl + 1;
            if (nextLvl < numberOfLevels)
            {
                SceneManager.LoadScene(nextLvl);
            }

            else
            {
                SceneManager.LoadScene(currlvl);
            }
        }

        if (col.collider.CompareTag("BouncingWall"))
        {
            transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            currentForce = -currentForce;
            Shoot(-getShootAngle(), currentForce);

            audioSource.PlayOneShot(bounce, 2.0F);
        }

        else if (col.collider.CompareTag("BouncingRoof"))
        {
            //currentForce = -currentForce;
            
            transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Shoot(-getShootAngle(), currentForce);

            audioSource.PlayOneShot(bounce, 2.0F);
        }

        else if (col.collider.CompareTag("Wall") || col.collider.CompareTag("Ground"))
        {
            audioSource.PlayOneShot(wall, 0.7F);
            

            Instantiate(wallP, transform.position, Quaternion.identity);

   
           
            canShoot = true;
        }

    }

    void OnCollisionStay(Collision col)
    {
        if (col.collider.CompareTag("Wall") || col.collider.CompareTag("Ground"))
        {
            canShoot = true;
        }

        if (col.collider.CompareTag("Wall"))
        { 
            if (!stuckOnWall && canShoot)
            {
                gravityMultiplier = 0;
                transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
                stuckOnWall = true;

            }
        }
        
    }

    void OnCollisionExit(Collision col)
    {
        
        if (col.collider.CompareTag("Wall") || col.collider.CompareTag("Ground"))
        {
            canShoot = false;
            gravityMultiplier = GravityMultiplier;    
        }

        if (col.collider.CompareTag("Wall"))
        {
            stuckOnWall = false;
        }
    }
    public void Gravity()
    {
        transform.GetComponent<Rigidbody>().AddForce(Physics.gravity * gravityMultiplier);
    }

    public float getShootAngle()
    {
        return shootAngle;
    }

    public void Shoot(float Angle = 0, float Force = 0)
    {
        float angle;
        float force;
        if (Angle == 0)
        {
            angle = transform.GetComponentInChildren<AimAssist>().Angle;
        }

        else
        {
            angle = Angle;
        }

        if (Force == 0)
        {
            force = currentForce;
        }

        else
        {
            force = Force;
        }

        shootAngle = angle;

        Vector3 dir = Quaternion.AngleAxis(shootAngle, transform.forward) * transform.right;
        transform.GetComponent<Rigidbody>().AddForce(dir * force * multiplier);
        gravityMultiplier = GravityMultiplier;
        canShoot = false;
    }
}
