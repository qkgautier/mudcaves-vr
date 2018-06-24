using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpGunAuto : PowerUp
{

    protected override void StartPower()
    {
        base.StartPower();
        gun.MakeShootContinuous(true, isPowerup: true);
    }

    protected override void StopPower()
    {
        gun.MakeShootContinuous(false, isPowerup: true);
        var endText = floatingEndText;

        if (gun.continuousShooting) { floatingEndText = ""; }
        base.StopPower();
        floatingEndText = endText;
    }

 //   // Use this for initialization
 //   void Start () {
		
	//}
	
	//// Update is called once per frame
	//void Update () {
		
	//}
}
