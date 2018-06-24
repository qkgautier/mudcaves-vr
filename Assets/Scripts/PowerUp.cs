using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour {

    public float objectLife = 10f;
    public float objectSpeed = 0.5f;
    public float powerDuration = 10f;

    public EZObjectPools.EZObjectPool floatingTextPool;
    public string floatingStartText = "";
    public Color floatingEndTextColor;
    public string floatingEndText = "";
    public Color floatingStartTextColor;


    protected Collider gunCollider;
    protected GunBehavior gun;
    protected PlayerShooting playerShooting;

    protected bool startPower = false;
    protected float powerTimer = 0.0f;

    protected bool objectIsDying = true;
    protected float objectLifeTimer = 0.0f;
    protected bool isDead = false;

    protected ParticleSystem particles;


    
    void Start ()
    {
        objectLifeTimer = Time.time;

        particles = GetComponent<ParticleSystem>();
    }


    void Update ()
    {
        // Continuously move the object up if it is dying
        if (objectIsDying)
        {
            transform.position += new Vector3(0, objectSpeed * Time.deltaTime, 0);
        }

        // Check if power timer is up
        if (startPower && Time.time >= powerTimer + powerDuration)
        {
            StopPower();
            isDead = true;
        }

        // Check if object is at end of life and nobody picked it up :(
        if (objectIsDying && Time.time >= objectLifeTimer + objectLife)
        {
            isDead = true;
        }

        // Destroy object after particles have finished emitting
        if (isDead)
        {
            if (particles != null)
            {
                if (!particles.isPlaying) { Death(); }
            }
            else { Death(); }
        }
    }

    protected void Death()
    {
        Destroy(gameObject);
    }

    protected virtual void SetFloatingText() {}

    protected virtual void StartPower()
    {
        // Create a floating text
        GameObject popupCanvas;
        var gunBounds = gunCollider.bounds;
        var textPosition = new Vector3(gunBounds.max.x, gunBounds.center.y, gunBounds.max.z);
        floatingTextPool.TryGetNextObject(textPosition, VRTK.VRTK_DeviceFinder.HeadsetTransform().rotation, out popupCanvas);
        popupCanvas.GetComponent<FloatingTextHandler>().ShowText(floatingStartText, floatingStartTextColor, true, 22, 0.5f);
    }

    protected virtual void StopPower()
    {
        // Create a floating text
        GameObject popupCanvas;
        var gunBounds = gunCollider.bounds;
        var textPosition = new Vector3(gunBounds.max.x, gunBounds.center.y, gunBounds.max.z);
        floatingTextPool.TryGetNextObject(textPosition, VRTK.VRTK_DeviceFinder.HeadsetTransform().rotation, out popupCanvas);
        popupCanvas.GetComponent<FloatingTextHandler>().ShowText(floatingEndText, floatingEndTextColor, false, 22, 0.5f);
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (!isDead && other.tag == "gun")
        {
            GetComponent<Collider>().enabled = false;
            GetComponent<Renderer>().enabled = false;
            
            gunCollider = other.GetComponent<Collider>();
            gun = other.GetComponent<GunBehavior>();
            playerShooting = gun.playerShooting;

            SetFloatingText();
            StartPower();

            if(particles != null)
            {
                particles.Play();
            }

            powerTimer = Time.time;
            startPower = true;
            objectIsDying = false;
        }
    }
}
