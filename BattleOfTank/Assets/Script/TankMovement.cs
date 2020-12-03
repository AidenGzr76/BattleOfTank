using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class TankMovement : MonoBehaviour
{

    const float BARREL_PIVOT_OFFSET = 90.0f;

    [SerializeField]
    private float speed = 2;
    [SerializeField]
    private float rotation = 60;

    [SerializeField]
    private Transform barrelPivot;

    [SerializeField]
    private Transform bulletSpawn;


    private float lastRotation;

    public bool isLocalPlayer = false;

    public GameObject Bullet;

    public int bulletSpeed;
    //public GameObject playercamera;
    private GameObject playercamera;

    Vector3 oldPosition;
    Vector3 currentPosition;
    Quaternion oldRotation;
    Quaternion currentRotation;
    Quaternion oldBarrelRotation;
    Quaternion currentBarrelRotation;

    public GameObject Fire;

    // Start is called before the first frame update
    void Start()
    {
        oldPosition = transform.position;
        currentPosition = oldPosition;
        oldRotation = transform.rotation;
        currentRotation = oldRotation;
        oldBarrelRotation = transform.Find("BarrelPivot").rotation;
        currentBarrelRotation = oldBarrelRotation;

        playercamera = transform.Find("Main Camera").gameObject;
        //playercamera = GameObject.FindGameObjectWithTag("MainCamera") as GameObject;

        if (isLocalPlayer == true)
            playercamera.SetActive(true);
        else
            playercamera.SetActive(false);

        //if (isLocalPlayer == true)
        //{
        //    Vector3 zero = new Vector3(0, 0, 0);
        //    GameObject camera = Instantiate(playercamera, zero, Quaternion.identity) as GameObject;
        //    transform.SetParent(camera.transform);
        //}
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        Quaternion camRot = Quaternion.Euler(0, 0, -transform.rotation.z);
        transform.Find("Main Camera").rotation = camRot;

        checkMovement();
        checkAiming();

        currentPosition = transform.position;
        currentRotation = transform.rotation;
        currentBarrelRotation = transform.Find("BarrelPivot").rotation;

        if (currentPosition != oldPosition)
        {
            Network.instance.GetComponent<Network>().CommandMove(transform.position);
            oldPosition = currentPosition;
        }

        if (currentRotation != oldRotation)
        {
            Network.instance.GetComponent<Network>().CommandTurn(transform.rotation);
            oldRotation = currentRotation;
        }

        if (currentBarrelRotation != oldBarrelRotation)
        {
            Network.instance.GetComponent<Network>().CommandBarrel(transform.Find("BarrelPivot").rotation);
            oldBarrelRotation = currentBarrelRotation;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Network n = Network.instance.GetComponent<Network>();
            //CmdFire();
            n.CommandShoot();
        }
    }

    void checkMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        transform.position += -transform.up * vertical * speed * Time.deltaTime;
        transform.Rotate(new Vector3(0, 0, -horizontal * rotation * Time.deltaTime));
    }

    void checkAiming()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dif = mousePosition - transform.position;
        dif.Normalize();
        float rot = Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg;

        lastRotation = rot;

        barrelPivot.rotation = Quaternion.Euler(0, 0, rot + BARREL_PIVOT_OFFSET);
    }

    public void CmdFire()
    {
        GameObject bullet = Instantiate(Bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation) as GameObject;

        Fire.gameObject.SetActive(true);
        Invoke("waitFire", 0.1f);

        Bullet b = bullet.GetComponent<Bullet>();
        b.playerFrom = this.gameObject;

        //bullet.GetComponent<Rigidbody>().isKinematic = false;

        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * bulletSpeed;

        Destroy(bullet, 2f);

    }

    void waitFire()
    {
        Fire.gameObject.SetActive(false);
    }
}
