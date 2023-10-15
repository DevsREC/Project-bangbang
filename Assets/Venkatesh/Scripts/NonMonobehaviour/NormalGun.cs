using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGun : GunInterface
{
    private GameObject bulletInstance;
    private GunSO gunSO;
    private GunHandeler gunhandeler;

    public NormalGun(GunSO gunSO , GunHandeler gunhandeler) //constructor 
    {
        this.gunhandeler = gunhandeler;
        this.gunSO = gunSO;
    }
    public void Shoot(Vector2 coordinates, ulong clientId , Vector3 origin)
    {
        Quaternion quaternion;
        quaternion = CalculateAngle(coordinates);
        //bulletInstance = Instantiate(gunSO.bulletPrefab, origin, quaternion);
        bulletInstance = gunhandeler.InstantiateBullet(gunSO.bulletPrefab, origin, quaternion); 
        // cannnot call instantiate method so we are calling the temporary method instantiatebullet method from gunhandeler 
        AssignBulletForce(coordinates, bulletInstance , gunSO.bulletSpeed); //Assigns bullet's intial force
        AssignBulletIDAndDamage(clientId , gunSO.damage);
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

    private void AssignBulletForce(Vector2 coordinates, GameObject bulletInstace , float bulletSpeed)
    {
        //Assigns intial force to the bullet
        Rigidbody2D bulletRigidbody = bulletInstance.GetComponent<Rigidbody2D>();
        bulletRigidbody.AddForce(coordinates * bulletSpeed, ForceMode2D.Impulse);
    }

    private void AssignBulletIDAndDamage(ulong value , int damage)
    {
        //assigns bullet id and damage to the bullet script in the bullet instance
        Bullet bullet = bulletInstance.GetComponent<Bullet>();
        bullet.Damage = damage;
        bullet.ID = value;
    }
}
