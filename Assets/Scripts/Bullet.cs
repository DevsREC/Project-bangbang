using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Added to all bullets
public class Bullet : MonoBehaviour
{
    private int damage;
    private ulong id;
    private ushort lifeSpan = 10;

    public ulong ID //Id of the player who instantiated this bullet  
    {
        get
        {
            return id;
        }
        set
        {
            id = value;
        }
    }

    public int Damage //Damage derived 
    {
        get
        {
            return damage;
        }

        set
        {
            damage = value;
        }
    }
    private void Start()
    {
        StartCoroutine(DeactivateBullet());
    }

    IEnumerator DeactivateBullet()
    {
        yield return new WaitForSecondsRealtime(lifeSpan);
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        DestroyOnImpact(other.tag);
        DamagePlayer(other.gameObject);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DestroyOnImpact(collision.gameObject.tag);
        DamagePlayer(collision.gameObject);
    }

    private void DestroyOnImpact(string tag) //Destroys the bullet when it hits the ground or wall
    {
        if (tag == "Ground" || tag == "Wall")
        {
            gameObject.SetActive(false);
        }
    }

    private void DamagePlayer(GameObject player) //calls the function that reduces the players health
    {
        player.TryGetComponent<Health>(out Health health); //assigns health variable the health component if there is a health component
        if (health != null) health.UpdateHealth(damage,id);
    }

}
