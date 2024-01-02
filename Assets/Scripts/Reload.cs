using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Reload : MonoBehaviour
{
    private int magSize = 0;
    private int bulletsLeftInLoad = 0;
    private int totalBulletsLeft = 0;

    public void AssignMagSize(int value)
    {
        magSize = value;
    }
    
    public void AssignBulletDetails(TextMeshProUGUI bulletLeft , TextMeshProUGUI totalBulletLeft)
    {
        bulletLeft.text = bulletsLeftInLoad.ToString() + "/";
        totalBulletLeft.text = totalBulletsLeft.ToString();
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
