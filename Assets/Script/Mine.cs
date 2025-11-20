using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public Animator Explosion;

    private AudioSource explosionSound;

    void Start()
    {
        explosionSound = GameObject.Find("explosionSound").GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            var hit = collider.gameObject;
            var health = hit.GetComponent<Health>();

            if (health != null)
            {
                health.TakeDamage(collider.gameObject, 50);
            }

            explosionSound.Play();
            Explosion.SetBool("isExplore", true);
            Destroy(gameObject, Explosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        }
    }
}
