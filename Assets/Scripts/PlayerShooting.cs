using UnityEngine;
using VRTK;

public class PlayerShooting : MonoBehaviour
{
    public float damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;

    [Range(0, 1)]
    public float hapticStrength = 0.10f;
    public float hapticDuration = 0.10f;
    public float hapticInterval = 0.05f;


    float timer;
    Ray shootRay = new Ray();
    RaycastHit shootHit;
    int shootableMask;
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;


    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable");
        gunParticles = GetComponent<ParticleSystem> ();
        gunLine = GetComponent <LineRenderer> ();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light> ();
    }


    void Update ()
    {
        timer += Time.deltaTime;

        if(timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects ();
        }

        gunLight.intensity -= 1.0f * Time.deltaTime;
    }


    public void DisableEffects ()
    {
        gunLine.enabled = false;
        //gunLight.enabled = false;
        //gunLight.intensity = 0.0f;
    }


    public void Shoot (GameObject interactingObject = null)
    {
        if (timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            timer = 0f;

            gunAudio.Play();

            VRTK_ControllerHaptics.TriggerHapticPulse(VRTK_ControllerReference.GetControllerReference(interactingObject), hapticStrength, hapticDuration, hapticInterval);

            gunLight.intensity = 0.4f;
            //gunLight.enabled = true;

            gunParticles.Stop();
            gunParticles.Play();

            gunLine.enabled = true;
            gunLine.SetPosition(0, transform.position);

            shootRay.origin = transform.position;
            shootRay.direction = transform.forward;

            if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
            {
                EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damagePerShot, shootHit.point);
                }
                gunLine.SetPosition(1, shootHit.point);
            }
            else
            {
                gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
            }
        }
    }
}
