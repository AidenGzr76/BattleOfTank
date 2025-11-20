using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

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

    public GameObject Fire;
    public GameObject Bullet;
    public int bulletSpeed;

    private AudioSource shootSound;

    // رفرنس به جوی‌استیک‌ها
    public Joystick leftJoystick;
    public Joystick rightJoystick;

    List<RaycastResult> hitObjects = new List<RaycastResult>();
    private Camera mainCamera;
    
    // متغیری برای ذخیره اینکه الان روی موبایل هستیم یا نه
    private bool isMobile;

    private void Start()
    {
        shootSound = GameObject.Find("shootSound").GetComponent<AudioSource>();
        mainCamera = Camera.main;

        // بررسی پلتفرم: آیا روی موبایل (اندروید/iOS) هستیم؟
        // نکته: این دستور در ادیتور یونیتی مقدار False برمی‌گرداند (یعنی جوی‌استیک در ادیتور مخفی می‌شود)
        isMobile = Application.isMobilePlatform;

        // اگر موبایل نیست (وب یا ویندوز)، جوی‌استیک‌ها را مخفی کن
        if (!isMobile)
        {
            leftJoystick.gameObject.SetActive(false);
            rightJoystick.gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        Quaternion camRot = Quaternion.Euler(0, 0, -transform.rotation.z);
        transform.Find("Main Camera").rotation = camRot;

        checkMovement();
        checkAiming();

        if (Input.GetMouseButtonDown(0))
        {
            if (GetObjectClicked() == null)
            {
                if (shootSound) shootSound.Play();
                CmdFire();
            }
        }
    }

    void checkMovement()
    {
        float horizontal = 0;
        float vertical = 0;

        if (isMobile)
        {
            // فقط اگر روی موبایل بودیم ورودی جوی‌استیک را بگیر
            horizontal = leftJoystick.Horizontal;
            vertical = leftJoystick.Vertical;
        }
        else
        {
            // اگر روی پی‌سی/وب بودیم ورودی کیبورد را بگیر
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
        }

        transform.position += -transform.up * vertical * speed * Time.deltaTime;
        transform.Rotate(new Vector3(0, 0, -horizontal * rotation * Time.deltaTime));
    }

    void checkAiming()
    {
        if (isMobile)
        {
            // منطق موبایل: استفاده از جوی‌استیک راست
            if (Mathf.Abs(rightJoystick.Horizontal) > 0.1f || Mathf.Abs(rightJoystick.Vertical) > 0.1f)
            {
                Vector3 moveVector = (Vector3.left * rightJoystick.Horizontal) + (-Vector3.up * rightJoystick.Vertical);
                barrelPivot.rotation = Quaternion.LookRotation(Vector3.forward, moveVector);
            }
        }
        else
        {
            // منطق پی‌سی/وب: استفاده از موس
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = transform.position.z;

            Vector3 dif = mousePosition - transform.position;
            dif.Normalize();

            float rot = Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg;
            barrelPivot.rotation = Quaternion.Euler(0, 0, rot + BARREL_PIVOT_OFFSET);
        }
    }

    public void CmdFire()
    {
        GameObject bullet = Instantiate(Bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation) as GameObject;

        if (Fire) Fire.gameObject.SetActive(true);
        Invoke("waitFire", 0.05f);

        if (bullet.GetComponent<Rigidbody2D>() != null)
        {
            bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * bulletSpeed;
        }

        Destroy(bullet, 2f);
    }

    void waitFire()
    {
        if (Fire) Fire.gameObject.SetActive(false);
    }

    private GameObject GetObjectUnderMouse()
    {
        var pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;
        EventSystem.current.RaycastAll(pointer, hitObjects);

        if (hitObjects.Count <= 0) return null;
        return hitObjects.First().gameObject;
    }

    private GameObject GetObjectClicked()
    {
        var clickedObject = GetObjectUnderMouse();
        if (clickedObject != null) return clickedObject;
        return null;
    }
}