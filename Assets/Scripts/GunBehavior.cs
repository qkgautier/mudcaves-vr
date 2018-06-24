using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.GrabAttachMechanics;

public class GunBehavior : MonoBehaviour {

    public GameManager gameManager;
    public PlayerShooting playerShooting;
    public bool continuousShooting = false;
    [Range(0, 1)]
    public float hapticStrength = 0.10f;
    public float hapticDuration = 0.10f;
    public float hapticInterval = 0.05f;
    public Transform viveSnapHandle;

    private VRTK_InteractableObject interactableObject;
    private GameObject interactingObject;

    private bool isShooting = false;
    private int numContinuousPowerUps = 0;


    // Use this for initialization
    void Start()
    {
        interactableObject = GetComponent<VRTK_InteractableObject>();

        interactableObject.InteractableObjectTouched += new InteractableObjectEventHandler(ObjectTouched);
        interactableObject.InteractableObjectGrabbed += new InteractableObjectEventHandler(ObjectGrabbed);
        interactableObject.InteractableObjectUngrabbed += new InteractableObjectEventHandler(ObjectReleased);
        interactableObject.InteractableObjectUsed += new InteractableObjectEventHandler(ObjectUsed);
        interactableObject.InteractableObjectUnused += new InteractableObjectEventHandler(ObjectUnused);
    }

    private void ObjectTouched(object sender, InteractableObjectEventArgs e)
    {
        if (VRTK_DeviceFinder.GetCurrentControllerType() == SDK_BaseController.ControllerType.SteamVR_ViveWand)
        {
            GetComponent<VRTK_ChildOfControllerGrabAttach>().rightSnapHandle = viveSnapHandle;
            GetComponent<VRTK_ChildOfControllerGrabAttach>().leftSnapHandle = viveSnapHandle;
        }
    }

    private void ObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        interactingObject = e.interactingObject;
        interactingObject.GetComponent<LightActivateScript>().DisableLightAndEvents();
        interactingObject.GetComponent<VRTK_Pointer>().enabled = false;
        gameManager.StartGame();
    }

    private void ObjectReleased(object sender, InteractableObjectEventArgs e)
    {
        e.interactingObject.GetComponent<LightActivateScript>().EnableLightEvents();
        e.interactingObject.GetComponent<VRTK_Pointer>().enabled = true;
        interactingObject = null;
    }

    private void ObjectUsed(object sender, InteractableObjectEventArgs e)
    {
        if (continuousShooting) { isShooting = true; }
        else { Shoot(); }
    }

    private void ObjectUnused(object sender, InteractableObjectEventArgs e)
    {
        isShooting = false;
    }

    private void Shoot()
    {
        VRTK_ControllerHaptics.TriggerHapticPulse(VRTK_ControllerReference.GetControllerReference(interactingObject), hapticStrength, hapticDuration, hapticInterval);
        //VRTK_ControllerHaptics.TriggerHapticPulse(VRTK_ControllerReference.GetControllerReference(interactingObject), playerShooting.GetComponent<AudioSource>().clip);
        playerShooting.Shoot();
    }

    public void ForceRelease()
    {
        if (interactingObject != null)
        {
            interactingObject.GetComponent<VRTK_InteractGrab>().ForceRelease();
        }
    }

    public void MakeShootContinuous(bool continuous, bool isPowerup)
    {
        if (isPowerup)
        {
            numContinuousPowerUps += continuous ? 1 : -1;
            continuous = numContinuousPowerUps > 0;
        }

        continuousShooting = continuous;
        isShooting = continuousShooting && interactableObject.IsUsing();
    }

    // Update is called once per frame
    void Update()
    {
        if (isShooting) { Shoot(); }
    }
}
