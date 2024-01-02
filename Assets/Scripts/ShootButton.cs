using UnityEngine;

public class ShootButton : MonoBehaviour
{
    private GameObject player;
    private GunHandeler bulletHandler;
    [SerializeField] private VariableJoystick joyStick;

    public void AssignValues()
    {
        player = FindObjectOfType<MyNetworkManager>().FindLocalPlayer();
        bulletHandler = player.GetComponent<GunHandeler>();
    }

    private void Update()
    {
        if (bulletHandler != null) //checks whether the local player is spawned or not
        {
            Shoot();
        }
            
    }

    private void Shoot()
    {
        if(bulletHandler.canShoot == true)
        {
            if (joyStick.Vertical > 0.75f || joyStick.Horizontal > 0.75f || joyStick.Horizontal < -0.75f || joyStick.Vertical < -0.75f)
                //Condition to check whether the shoot call is triggered from joystick
            {
                if (bulletHandler == null) return;
                //float angle = joyStick.Vertical / joyStick.Horizontal;
                Vector2 coordinates = new Vector2(joyStick.Horizontal, joyStick.Vertical);
                bulletHandler.Shoot(coordinates);
            }
        }
        
    }
    /*public void ShootCall()
    {
        if (bulletHandler == null) return;
        //bulletHandler.Shoot(1,1);  
    }*/
}
