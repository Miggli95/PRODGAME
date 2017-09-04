using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootScript : MonoBehaviour
{
    public float force;
    public float currentForce;
    public float min;
    public float max;
    public float currentPower;
    int increase = 1;
    public float multiplier = 1;
    bool mouseButtonPressed = false;
    public int increaseMultiplier = 1;
    public float GravityMultiplier = 1;
    private float gravityMultiplier;
    public Slider powerbar;
    public float powerbarTreshold;
    // Use this for initialization
    void Start()
    {
        powerbar.minValue = min;
        powerbar.maxValue = max;
        powerbarTreshold = max - min;
        powerbar.value = force;
        gravityMultiplier = GravityMultiplier;
        // Mathf.Clamp(force, min, max);
    }

    // Update is called once per frame
    void Update()
    {
        if (force != 0)
        {
            powerbar.value = force;
        }

        else
        {
            powerbar.value = currentForce;
        }

        if (force >= max)
        {
            increase = -1;
        }

        if (force <= min)
        {
            increase = 1;
        }

        if (Input.GetMouseButton(0))
        {
            force += increase * Time.deltaTime * increaseMultiplier;
            mouseButtonPressed = true;
        }

        else if (mouseButtonPressed)
        {
            currentForce = force;
            force = 0;
            mouseButtonPressed = false;
            Shoot();
        }

        Gravity();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.CompareTag("Wall"))
        {
            gravityMultiplier = 0;
        }
    }

    public void Gravity()
    {
        transform.GetComponent<Rigidbody>().AddForce(Physics.gravity * gravityMultiplier);
    }

    public void Shoot()
    {
        float angle = transform.GetComponentInChildren<AimAssist>().Angle;
        Vector3 dir = Quaternion.AngleAxis(angle, transform.forward) * transform.right;
        transform.GetComponent<Rigidbody>().AddForce(dir * currentForce * multiplier);
        gravityMultiplier = GravityMultiplier;
    }
}
