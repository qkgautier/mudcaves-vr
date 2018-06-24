using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float startingHealth = 100;
    public float currentHealth;
    public EZObjectPools.EZObjectPool floatingTextPool;
    public Color popupDamageColor;
    public Color popupScoreColor;
    public float sinkSpeed = 2.5f;
    public float scoreValue = 10;
    public float attackValue = 5;
    public Color damageColor = Color.red;
    //public AudioClip deathClip;
    public GameObject[] powerUpsToSpawn;
    public float[] powerUpProbabilities;

    Animation legacyAnim;
    //Animator anim;
    //AudioSource enemyAudio;
    //ParticleSystem hitParticles;
    //CapsuleCollider capsuleCollider;
    Material material;
    Color materialColor;
    float colorTimer = 100.0f;

    bool isDead;
    bool isSinking;


    void Awake ()
    {
        legacyAnim = GetComponent<Animation>();
        //anim = GetComponent<Animator>();
        //enemyAudio = GetComponent <AudioSource> ();
        //hitParticles = GetComponentInChildren <ParticleSystem> ();
        //capsuleCollider = GetComponent <CapsuleCollider> ();
        material = transform.Find("PC_Spider").GetComponent<Renderer>().material;
        materialColor = material.color;

        currentHealth = startingHealth;
    }


    void Update ()
    {
        colorTimer += Time.deltaTime;

        if(colorTimer >= 0.2f)
        {
            material.SetColor("_Color", materialColor);
        }

        if (isSinking)
        {
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
        }
        //TakeDamage(20 * Time.deltaTime, transform.position); // debug
    }


    public void TakeDamage (float amount, Vector3 hitPoint)
    {
        if(isDead)
            return;

        //enemyAudio.Play ();

        currentHealth -= amount;
            
        //hitParticles.transform.position = hitPoint;
        //hitParticles.Play();

        if(currentHealth <= 0)
        {
            Death ();
        }
        else
        {
            material.SetColor("_Color", damageColor);
            colorTimer = 0.0f;
        }

        // Create a floating text with damage
        GameObject floatingCanvas;
        var halfscale = transform.localScale / 2f;
        floatingTextPool.TryGetNextObject(transform.position + new Vector3(0, halfscale.y, 0), VRTK.VRTK_DeviceFinder.HeadsetTransform().rotation, out floatingCanvas);
        floatingCanvas.GetComponent<FloatingTextHandler>().ShowText("-" + ((int)amount).ToString(), popupDamageColor, up: false);
    }

    void Death ()
    {
        isDead = true;

        //GetComponent<Animation>().enabled = false;

        EnemyMovement patrol = GetComponent<EnemyMovement>();
        patrol.enabled = false;
        //patrol.StartPhysics();
        legacyAnim.CrossFade("death");
        StartCoroutine(DelayedSinking());

        //capsuleCollider.isTrigger = true;

        //anim.SetTrigger("Dead");

        //enemyAudio.clip = deathClip;
        //enemyAudio.Play ();

        PlayerScore.Score += scoreValue;

        if (powerUpsToSpawn.Length > 0)
        {
            List<int> objIdxToSpawn = new List<int>(powerUpsToSpawn.Length);

            for (int i = 0; i < powerUpProbabilities.Length; i++)
            {
                if (Random.value < powerUpProbabilities[i])
                {
                    objIdxToSpawn.Add(i);
                }
            }

            if (objIdxToSpawn.Count > 0)
            {
                var obj = Instantiate(powerUpsToSpawn[objIdxToSpawn[Random.Range(0, objIdxToSpawn.Count)]], transform.position, transform.rotation);
                obj.GetComponent<PowerUp>().floatingTextPool = floatingTextPool;
            }
        }

        // Create a floating text with score
        GameObject popupCanvas;
        var halfscale = transform.localScale / 2f;
        floatingTextPool.TryGetNextObject(transform.position + new Vector3(0.2f, halfscale.y, 0.1f), VRTK.VRTK_DeviceFinder.HeadsetTransform().rotation, out popupCanvas);
        popupCanvas.GetComponent<FloatingTextHandler>().ShowText("+" + ((int)scoreValue).ToString(), popupScoreColor);
    }

    IEnumerator DelayedSinking()
    {
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        yield return new WaitForSeconds(0.5f);
        StartSinking();
    }

    public void StartSinking ()
    {
        isSinking = true;
        //ScoreManager.score += scoreValue;
        Destroy (gameObject, 2f);
    }
}
