using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class GunHandeler : NetworkBehaviour
{
    private GunSO gunSO;
    //private Rigidbody2D bulletRigidbody;
    private GameObject bulletInstance;
    //private Bullet bullet;
    public bool canShoot = true;
    private Reload reload;
    private bool gunSwapPossible = false;
    [SerializeField] private GunSO tempGunSO;
    private GunInterface gunInstance;
    private GameObject gunObject;

    public bool GunSwapPossible
    {
        get
        {
            return gunSwapPossible;
        }
    }

    public override void OnNetworkSpawn()
    {
        AssignValues();
        if (IsOwner) { GunSwapServerRpc(); }       
        //tempGunSO = null;
    }

    private void AssignValues()
    {
        FindObjectOfType<ShootButton>().AssignValues();
        reload = GetComponent<Reload>();
    }
    //GunSwap should be synced between all clients
    [ServerRpc]
    public void GunSwapServerRpc()
    {
        //gunObject.GetComponent<NetworkObject>().Despawn();
        GunSwapClientRpc();
        gunObject?.GetComponent<NetworkObject>().Despawn();
    }

    [ClientRpc]
    public void GunSwapClientRpc()
    {        
        if (tempGunSO == null)
        {
            return;
        }
        gunSO = tempGunSO;
        //Destroy(gunObject);
        tempGunSO = null;
        AssignGunInstance();        
        reload.AssignMagSize(gunSO.magSize);
        reload.TotalBullets = gunSO.totalBulletsLeft;
        reload.ReloadMag();
    }

    private void AssignGunInstance()
    {
        switch (gunSO.gunType)
        {
            case GunSO.GunType.NormalGun:
                {
                    gunInstance = new NormalGun(gunSO, this);
                    break;
                }
            case GunSO.GunType.RocketLauncher:
                {
                    //Needs to be implemented
                    //Create guninstance of RocketLauncher
                    break;
                }
        }
    }

    public void Shoot(Vector2 coordinates)
    {
        if (!IsOwner || gunSO == null) return; //Returns when its not the owner or when the player doesn't have a gun
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

    }

    IEnumerator DelayShooting(float fireRate)
    {
        //Delays Shooting
        canShoot = false;
        yield return new WaitForSecondsRealtime(fireRate);
        canShoot = true;
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
        gunInstance.Shoot(coordinates, clientId, transform.position);
        //Shoot();
    }

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.CompareTo("Gun") == 0)
        {
            gunSwapPossible = true;
            gunObject = other.gameObject;
            tempGunSO = gunObject.GetComponent<Gun>().gunSO;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag.CompareTo("Gun") == 0)
        {
            gunSwapPossible = false;
            tempGunSO = null;
        }
    }

    //Temporary methods

    public GameObject InstantiateBullet(GameObject bulletPrefab, Vector3 position,Quaternion quaternion)
    {
        bulletInstance =  Instantiate(gunSO.bulletPrefab, transform.position, quaternion);
        return bulletInstance;
    }



    //unused functions
    /*
    private Quaternion CalculateAngle(Vector2 coordinates)
    {
        //Calculates the angle in which the bullet should be fired
        float angle = coordinates.y / coordinates.x;
        float zangle = Mathf.Atan(angle);
        Quaternion quaternion;
        quaternion = Quaternion.Euler(0f, 0f, zangle);
        return quaternion;

    }

    private void AssignBulletIDAndDamage(ulong value)
    {
        //assigns bullet id and damage to the bullet script in the bullet instance
        bullet = bulletInstance.GetComponent<Bullet>();
        bullet.Damage = gunSO.damage;
        bullet.ID = value;
    }

    private void AssignBulletForce(Vector2 coordinates, GameObject bulletInstace)
    {
        //Assigns intial force to the bullet
        Rigidbody2D bulletRigidbody = bulletInstance.GetComponent<Rigidbody2D>();
        bulletRigidbody.AddForce(coordinates * gunSO.bulletSpeed, ForceMode2D.Impulse);
    }
    

    private void Shoot()
    {
        Quaternion quaternion;
        quaternion = CalculateAngle(coordinates);
        bulletInstance = Instantiate(gunSO.bulletPrefab, transform.position,quaternion);
        AssignBulletForce(coordinates,bulletInstance); //Assigns bullet's intial force
        AssignBulletIDAndDamage(clientId); //Assigns bullet id and damage based on the current gunSO
    }*/
}
