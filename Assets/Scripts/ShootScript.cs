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
    public bool blindMode = false;
    public BlindScript blindScript;
    public float blindAimSpeed = 10;
    AimAssist aim;
    public bool paused = false;
    // Soundstuff -------------------------------------------
    GameObject pauseScreen;
    public AudioClip wall;
    public AudioClip bounce;
    AudioSource audioSource;

    // ParticleStuff -----------------------------------------

    
    public GameObject wallP;

    // Camera animations

    public AnimationClip camAni;
    public GameObject cameraHelper;

    // LastShot text

    public GameObject lastShot;


    public float velAngle = 0;
    public Vector3 velocity;
    public bool OnGround = false;
    public Button button;

    // Use this for initialization
    public bool mouseActive = true;


    public void MouseSetActive(bool value)
    {
        mouseActive = value;
    }
    void Start()
    {
        //Input.mousePosition;
        powerbar.minValue = min;
        powerbar.maxValue = max;
        powerbarTreshold = max - min;
        powerbar.value = force;
        powerbar.interactable = false;
        gravityMultiplier = GravityMultiplier;
        // Mathf.Clamp(force, min, max);
        numberOfShootsTxt.text = "Shoots: " + numberOfShoots;
        audioSource = GetComponent<AudioSource>();
        currlvl = SceneManager.GetActiveScene().buildIndex;
        numberOfLevels = SceneManager.sceneCountInBuildSettings;
        blindScript = transform.GetComponent<BlindScript>();
        blindMode = blindScript.enabled;
        aim = transform.GetComponentInChildren<AimAssist>();
        pauseScreen = GameObject.FindGameObjectWithTag("PauseScreen").transform.GetChild(0).gameObject;


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
    bool bounced = false;
    Vector3 lastBouncePos;

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
  
    void BlindMode()
    {
        if (Input.GetKey(KeyCode.V))
        {
            switchMode = true;
        }

        else if (switchMode)
        {
            blindMode = !blindMode;
            
            updateMode = true;
            switchMode = false;
            
            //if (canShoot)
            //{

            //}
        }

        if (updateMode)
        {
            blindScript.enabled = blindMode;
            updateMode = false;
        }

        
    }

    void Update()
    {
        paused = pauseScreen.activeSelf;

        canShoot = !paused && OnGround;
            aim.blindMode = blindMode;
            shootAngle = transform.GetComponentInChildren<AimAssist>().Angle;
            CameraHelper();
            BlindMode();
            if (transform.GetComponent<Rigidbody>().velocity != Vector3.zero)
            {
                velocity = transform.GetComponent<Rigidbody>().velocity;
                velAngle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            }

            if (numberOfShoots <= 0 && transform.GetComponent<Rigidbody>().velocity == Vector3.zero && canShoot)
            {
                SceneManager.LoadScene(currlvl);
            }

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

            if (numberOfShoots <= 1)
            {
                lastShot.SetActive(true);
            }

            else
            {
                lastShot.SetActive(false);
            }

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

            if (Input.GetMouseButton(0) && canShoot && mouseActive)
            {
                if (!blindMode)
                {
                    force += increase * Time.deltaTime * increaseMultiplier;
                }

                else
                {
                    force = max;
                }
                mouseButtonPressed = true;
            }

            else if (mouseButtonPressed)
            {
                currentForce = force;
                force = 0;
                if (numberOfShoots > 0 || infiniteShoots)
                {
                    shooting = true;
                    if (!infiniteShoots)
                        numberOfShoots--;
                    mouseButtonPressed = false;
                }


                //if (canShoot)
                //{

                //}
            }
        

      
    }

    void FixedUpdate()
    {
        if (shooting)
        {
            Shoot();
            shooting = false;
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
            if (Mathf.Abs(lastBouncePos.y - transform.position.y)> 0.15f)
            {
                velAngle = Mathf.Atan2(velocity.y, -velocity.x) * Mathf.Rad2Deg;

                transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
                Shoot(velAngle, currentForce);
                lastBouncePos = transform.position;
                audioSource.PlayOneShot(bounce, 2.0F);
                lastBouncePos = transform.position;
            }
        }

        else if (col.collider.CompareTag("BouncingRoof"))
        {
            //currentForce = -currentForce;

            if (Mathf.Abs(lastBouncePos.x - transform.position.x)> 0.15f)
            {
                velAngle = Mathf.Atan2(-velocity.y, velocity.x) * Mathf.Rad2Deg;

                transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
                Shoot(velAngle, currentForce);
                audioSource.PlayOneShot(bounce, 2.0F);
                lastBouncePos = transform.position;
            }
        }

        else if (col.collider.CompareTag("BouncingDiagonal"))
        {
            //currentForce = -currentForce;

            if (Mathf.Abs(lastBouncePos.x - transform.position.x) > 0.15f || Mathf.Abs(lastBouncePos.y - transform.position.y) > 0.15f)
            {
                velAngle = Mathf.Atan2(-velocity.y, -velocity.x) * Mathf.Rad2Deg;

                transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
                Shoot(velAngle, currentForce);
                audioSource.PlayOneShot(bounce, 2.0F);
                lastBouncePos = transform.position;
            }
        }

        else if (col.collider.CompareTag("Wall") || col.collider.CompareTag("Ground"))
        {
            audioSource.PlayOneShot(wall, 0.7F);
            

            Instantiate(wallP, transform.position, Quaternion.identity);

   
           
            //canShoot = true;
        }

    }
    float timer = 0;
    private bool switchMode;
    private bool updateMode;
    private bool shooting;

    void OnCollisionStay(Collision col)
    {
        if (!col.collider.CompareTag("Slippery") && !col.collider.CompareTag("BouncingWall") && !col.collider.CompareTag("BouncingRoof"))
        {
            OnGround = true;
        }

        if (col.collider.CompareTag("BouncingWall") || col.collider.CompareTag("BouncingRoof"))
        {
            timer += Time.deltaTime;
            if (timer > 0.2f)
            {
                OnGround = true;
                timer = 0;
            }
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
            OnGround = false;
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
            if (blindMode)
            {
                angle = blindScript.getAngle();
            }

            else
            {
                angle = transform.GetComponentInChildren<AimAssist>().Angle;
            }
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
        OnGround = false;
    }
}
