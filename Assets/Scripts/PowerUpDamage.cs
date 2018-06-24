using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpDamage : PowerUp
{
    public float damageGain = 15f;

    protected override void SetFloatingText()
    {
        floatingStartText += " ▲" + ((int)damageGain).ToString();
        floatingEndText += " ▼" + ((int)damageGain).ToString();
    }

    protected override void StartPower()
    {
        base.StartPower();
        playerShooting.damagePerShot += damageGain;
    }

    protected override void StopPower()
    {
        base.StopPower();
        playerShooting.damagePerShot -= damageGain;
    }
}
