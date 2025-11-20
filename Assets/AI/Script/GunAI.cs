using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAI : MonoBehaviour
{
    public Player Target;


    private Transform Gun;

    public GameObject Fire;

    public GameObject Bullet;

    public int bulletSpeed;

    [SerializeField]
    private Transform bulletSpawn;

    float tempSpeed;

    bool isHitBlock;

    public static bool allowFire = false;
    bool allowFindRandomPos = false;

    private AudioSource shootSound;

    private void Start()
    {
        shootSound = GameObject.Find("shootSound").GetComponent<AudioSource>();

        Target = GameObject.Find("AIPlayerTank").GetComponent<Player>();

        Gun = this.transform;

        InvokeRepeating("CmdFire", 0f, 0.35f);
    }

    void FixedUpdate()
    {
        if ((GunPlayerFind.GunFindedplayer && GunPlayerFind.GunName == transform.name) ||
            (PlayerBullet.playerShotToGun && PlayerBullet.playerShotNameToGun == transform.name))
        {
            //Debug.Log(GunPlayerFind.GunName);
            //Debug.Log(transform.name);

            Vector3 difference = Target.transform.position - Gun.transform.position;
            float rotationZ = (Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg) + 90.0f;
            Quaternion goalRot = Quaternion.Euler(Quaternion.identity.x,
                Quaternion.identity.y,
                rotationZ);

            //Gun.transform.rotation = Quaternion.Lerp(Gun.transform.rotation, goalRot, Time.deltaTime * 0.5f);

            Gun.Find("Pivot").transform.rotation = Quaternion.Lerp(
                Gun.Find("Pivot").transform.rotation,
                goalRot, Time.deltaTime * 7f);


            allowFire = true;
        }
    }


    //IEnumerator wait()
    //{
    //    yield return new WaitForSeconds(0.5f);
    //    CmdFire();
    //}

    public void CmdFire()
    {
        if (!GunHealth.isDestroy)
        {
            if (allowFire &&
                ((GunPlayerFind.GunFindedplayer && GunPlayerFind.GunName == transform.name) ||
                (PlayerBullet.playerShotToGun && PlayerBullet.playerShotNameToGun == transform.name)))
            {
                shootSound.Play();

                GameObject bullet = Instantiate(Bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation) as GameObject;

                Fire.gameObject.SetActive(true);
                Invoke("waitFire", 0.1f);

                bullet.GetComponent<Rigidbody2D>().linearVelocity = bullet.transform.up * bulletSpeed;

                Destroy(bullet, 2f);
            }
        }
    }

    void waitFire()
    {
        Fire.gameObject.SetActive(false);
    }
}
