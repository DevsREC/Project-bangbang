using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reload : MonoBehaviour
{
    private int magSize = 30;
    private int bulletsLeftInLoad = 30;
    private int totalBulletsLeft = 200;

    public void AssignMagSize(int value)
    {
        magSize = value;
    }

    public int TotalBullets
    {
        set
        {
            totalBulletsLeft = value;
        }
    }

    public bool BulletCounter()
    {
        //Returns true if there are bullets left in load
        if (bulletsLeftInLoad > 0)
        {
            bulletsLeftInLoad--;
            return true;
        }
        else
        {
            return false;
        }

    }

    public void ReloadMag()
    {
        //Reloads the gun by adjusting totalbulletsleft and bulletleftinhand
        if(totalBulletsLeft > 0)
        {
            if (totalBulletsLeft >= magSize)
            {
                bulletsLeftInLoad = magSize;
                totalBulletsLeft -= magSize;
            }
            else
            {
                bulletsLeftInLoad = totalBulletsLeft;
                totalBulletsLeft = 0;
            }

        }
        else
        {
            Debug.Log("No bullets left");
        }
        
    }

}
