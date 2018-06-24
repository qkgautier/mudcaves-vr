using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpGunSpeed : PowerUp
{

    public float speedMultiplier = 1.5f;

    protected override void SetFloatingText()
    {
        var speedText = ((int)((speedMultiplier - 1) * 100)).ToString();
        floatingStartText += " ▲" + speedText + "%";
        floatingEndText += " ▼" + speedText + "%";
    }

    protected override void StartPower()
    {
        base.StartPower();
        playerShooting.timeBetweenBullets /= speedMultiplier;
    }

    protected override void StopPower()
    {
        base.StopPower();
        playerShooting.timeBetweenBullets *= speedMultiplier;
    }
}
