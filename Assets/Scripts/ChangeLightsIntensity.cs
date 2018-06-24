using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VRTK;

public class ChangeLightsIntensity : MonoBehaviour
{

    public List<Light> lights;
    public VRTK_ControllerEvents.ButtonAlias lightIncreaseButton = VRTK_ControllerEvents.ButtonAlias.ButtonTwoPress;
    public VRTK_ControllerEvents.ButtonAlias lightDecreaseButton = VRTK_ControllerEvents.ButtonAlias.ButtonOnePress;
    public bool wrapIntensity = true;
    public float maxIntensity = 2.0f;
    public float intensityStep = 0.2f;

    protected VRTK_ControllerEvents controller;

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<VRTK_ControllerEvents>();

        controller.SubscribeToButtonAliasEvent(lightIncreaseButton, true, DoLightIncreaseButtonPressed);
        controller.SubscribeToButtonAliasEvent(lightDecreaseButton, true, DoLightDecreaseButtonPressed);
    }
    

    protected virtual void DoLightIncreaseButtonPressed(object sender, ControllerInteractionEventArgs e)
    {
        foreach (var l in lights)
        {
            l.intensity += intensityStep;
            if (l.intensity > maxIntensity)
            {
                if (wrapIntensity) { l.intensity = 0.0f; }
                else { l.intensity = maxIntensity; }
            }
        }
    }

    protected virtual void DoLightDecreaseButtonPressed(object sender, ControllerInteractionEventArgs e)
    {
        foreach (var l in lights)
        {
            l.intensity -= intensityStep;
            if(l.intensity < 0.0f)
            {
                if (wrapIntensity) { l.intensity = maxIntensity; }
                else { l.intensity = 0.0f; }
            }
        }
    }
}
