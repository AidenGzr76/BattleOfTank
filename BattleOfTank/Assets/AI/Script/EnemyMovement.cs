using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
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

    public GameObject Bullet;

    public int bulletSpeed;

    // Update is called once per frame
    void FixedUpdate()
    {

        checkMovement();
        checkAiming();

        if (Input.GetMouseButtonDown(0))
        {
            CmdFire();
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

        barrelPivot.rotation = Quaternion.Euler(0, 0, rot + BARREL_PIVOT_OFFSET);
    }
    

    public void CmdFire()
    {
        GameObject bullet = Instantiate(Bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation) as GameObject;

        //Bullet b = bullet.GetComponent<Bullet>();
        //b.playerFrom = this.gameObject;

        //bullet.GetComponent<Rigidbody>().isKinematic = false;

        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * bulletSpeed;

        Destroy(bullet, 2f);
    }
}
