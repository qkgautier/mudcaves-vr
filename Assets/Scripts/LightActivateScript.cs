using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VRTK;

public class LightActivateScript : MonoBehaviour
{

    private VRTK_ControllerEvents controller;

    public VRTK_ControllerEvents.ButtonAlias activationButton = VRTK_ControllerEvents.ButtonAlias.GripPress;

    public Light LightToActivate;

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<VRTK_ControllerEvents>();

        EnableLightEvents();
    }

    public void DisableLightAndEvents()
    {
        LightToActivate.enabled = false;
        controller.UnsubscribeToButtonAliasEvent(activationButton, true, DoActivationButtonPressed);
    }

    public void EnableLightEvents()
    {
        controller.SubscribeToButtonAliasEvent(activationButton, true, DoActivationButtonPressed);
    }

    protected virtual void DoActivationButtonPressed(object sender, ControllerInteractionEventArgs e)
    {
        LightToActivate.enabled = !LightToActivate.enabled;
    }
}
