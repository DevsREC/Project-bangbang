using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class GunHandeler : NetworkBehaviour
{
    public GunSO gunSO;
    private Rigidbody2D bulletRigidbody;
    private GameObject bulletInstance;
    private Bullet bullet;
    public bool canShoot = true;
    private Reload reload;

    public override void OnNetworkSpawn()
    {
        AssignValues();
        GunSwap();
    }

    private void AssignValues()
    {
        FindObjectOfType<ShootButton>().AssignValues();
        reload = GetComponent<Reload>();
    }

    private void GunSwap()
    {
        reload.AssignMagSize(gunSO.magSize);
        reload.TotalBullets = gunSO.totalBulletsLeft;
        reload.ReloadMag();
    }
    public void Shoot(Vector2 coordinates)
    {
        if (!IsOwner) return; //Returns when its not the owner
        if (reload.BulletCounter()) //checks whether there is any bullet left
        {
            StartCoroutine(DelayShooting(gunSO.fireRate)); //waits for firerate time
            ShootBulletServerRpc(coordinates); //shoot server rpc
        }
        else
        {
            StartCoroutine(DelayShooting(gunSO.reloadTime)); //waits for reload time
            reload.ReloadMag(); // reloads the mag
        }
        
        //bulletRigidbody.AddForce(Vector2.up * UpwardForce, ForceMode2D.Impulse);

    }

    IEnumerator DelayShooting(float fireRate)
    {
        //Delays Shooting
        canShoot = false;
        yield return new WaitForSecondsRealtime(fireRate);
        canShoot = true;
    }

    private void AssignBulletIDAndDamage(ulong value)
    {
        //assigns bullet id and damage to the bullet script in the bullet instance
        bullet = bulletInstance.GetComponent<Bullet>();
        bullet.Damage = gunSO.damage;
        bullet.ID = value;
    }

    private void AssignBulletForce(Vector2 coordinates , GameObject bulletInstace)
    {
        //Assigns intial force to the bullet
        bulletRigidbody = bulletInstance.GetComponent<Rigidbody2D>();
        bulletRigidbody.AddForce(coordinates * gunSO.bulletSpeed, ForceMode2D.Impulse);
    }
    [ServerRpc]
    public void ShootBulletServerRpc(Vector2 coordinates,ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        ShootBulletClientRpc(coordinates,clientId);
    }

    [ClientRpc]
    private void ShootBulletClientRpc(Vector2 coordinates,ulong clientId)
    {
        Quaternion quaternion;
        quaternion = CalculateAngle(coordinates);
        bulletInstance = Instantiate(gunSO.bulletPrefab, transform.position,quaternion);
        AssignBulletForce(coordinates,bulletInstance); //Assigns bullet's intial force
        AssignBulletIDAndDamage(clientId); //Assigns bullet id and damage based on the current gunSO
    }

    private Quaternion CalculateAngle(Vector2 coordinates)
    {
        //Calculates the angle in which the bullet should be fired
        float angle = coordinates.y / coordinates.x;
        float zangle = Mathf.Atan(angle);
        Quaternion quaternion;
        quaternion = Quaternion.Euler(0f, 0f, zangle);
        return quaternion;

    }
}
