using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpHealth : PowerUp
{
    public float healthGain = 15f;

    protected override void SetFloatingText()
    {
        floatingStartText += " ▲" + ((int)healthGain).ToString();
    }

    protected override void StartPower()
    {
        base.StartPower();
        PlayerScore.PlayerHealth += healthGain;
    }

    protected override void StopPower()
    {
    }
}
