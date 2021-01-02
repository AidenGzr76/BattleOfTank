using System.Linq;
using UnityEngine;
using Pathfinding;
using System.Collections;

public class AI : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;

    private float _attackRange = 10.3f;

    private Vector3 _direction;
    //public static Player _target;
    public Player Target;
    private DroneState _currentState;

    //EnemyAI
    private Transform nextPos;
    public float speed = 200f;
    public float nextWayPointDistance = 3f;

    private Transform enemyGFX;

    Path path;
    int currentWayPoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    public GameObject Fire;

    public GameObject Bullet;

    public int bulletSpeed;

    [SerializeField]
    private Transform bulletSpawn;

    float tempSpeed;

    bool isHitBlock;

    bool allowFire = false;
    bool allowFindRandomPos = false;

    //private int thisEnemyNumber;

    private AudioSource shootSound;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        shootSound = GameObject.Find("shootSound").GetComponent<AudioSource>();

        Target = GameObject.Find("AIPlayerTank").GetComponent<Player>();

        enemyGFX = transform.Find("EnemyGFX");

        //string[] split = gameObject.name.Split('-');

        //thisEnemyNumber = int.Parse(split[1]);

        //_currentState = new DroneState[SingleGameController.globalEnemyNumber];

        tempSpeed = transform.GetComponent<AIPath>().maxSpeed;

        InvokeRepeating("CmdFire", 0f, 1f);
        InvokeRepeating("NextRandomPos", 0f, 2f);
    }

    void NextRandomPos()
    {
        if (allowFindRandomPos)
        {
            try
            {
                allowFindRandomPos = false;

                string[] split = gameObject.name.Split('-');

                int num = int.Parse(split[1]);

                nextPos = GameObject.Find("nextPos" + "-" + num).transform;

                nextPos.position = new Vector3(Random.Range(-24, 24), Random.Range(-18, 10), 0);
            }
            catch (System.Exception)
            {
                // Nope
                //throw;
            }
        }
    }

    void FixedUpdate()
    {
        switch (_currentState)
        {
            case DroneState.Saerch:
                {
                    transform.GetComponent<AIPath>().maxSpeed = tempSpeed;

                    AIDestinationSetter.wantSearch = true;

                    if (isHitBlock)
                    {
                        //Debug.Log("Path Blocked");
                        //NextRandomPos();
                        allowFindRandomPos = true;
                    }
                    
                    if ((PlayerFind.playerFinded && PlayerFind.enemyName == transform.name) ||
                        (PlayerBullet.playerShot && PlayerBullet.playerShotName == transform.name))
                    {
                        _currentState = DroneState.Attack;
                    }

                    break;
                }
            case DroneState.Attack:
                {
                    if (Target != null)
                    {
                        Vector3 difference = Target.transform.position - enemyGFX.parent.transform.position;
                        float rotationZ = (Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg) - 90.0f;
                        Quaternion goalRot = Quaternion.Euler(Quaternion.identity.x,
                            Quaternion.identity.y,
                            rotationZ);

                        enemyGFX.transform.rotation = Quaternion.Lerp(enemyGFX.transform.rotation, goalRot, Time.deltaTime * 0.5f);

                        enemyGFX.Find("BarrelPivot").transform.rotation = Quaternion.Lerp(
                            enemyGFX.Find("BarrelPivot").transform.rotation,
                            goalRot, Time.deltaTime * 4f);

                        allowFire = true;

                        AIDestinationSetter.wantSearch = false;

                        checkEscape();
                    }

                    break;
                }
            case DroneState.Escape:
                {
                    if (Target != null)
                    {
                        try
                        {
                            allowFire = false;
                            AIDestinationSetter.wantSearch = true;

                            string[] split = gameObject.name.Split('-');

                            int num = int.Parse(split[1]);

                            nextPos = GameObject.Find("nextPos" + "-" + num).transform;

                            if (AIHealth.isEscape &&
                                (transform.name == AIHealth.healthEnemyName) &&
                                (Vector3.Distance(Target.transform.position, nextPos.position) > 10f))
                            {
                                AIHealth.isEscape = false;
                                _currentState = DroneState.Saerch;
                            }
                            else
                            {
                                allowFindRandomPos = true;
                                //NextRandomPos();
                            }
                        }
                        catch (System.Exception)
                        {
                            // Nope !
                            //throw;
                        }
                    }
                    break;
                }
        }
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.5f);
        CmdFire();
    }

    public void checkEscape()
    {
        if (AIHealth.isEscape && AIHealth.escapeEnemyName == this.transform.name)
        {
            PlayerFind.playerFinded = false;
            _currentState = DroneState.Escape;
            //_currentState = DroneState.Saerch;
        }
    }

    public void CmdFire()
    {
        if (allowFire)
        {
            shootSound.Play();

            GameObject bullet = Instantiate(Bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation) as GameObject;

            Fire.gameObject.SetActive(true);
            Invoke("waitFire", 0.1f);

            //Bullet b = bullet.GetComponent<Bullet>();
            //b.playerFrom = this.gameObject;

            //bullet.GetComponent<Rigidbody>().isKinematic = false;

            bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * bulletSpeed;

            Destroy(bullet, 2f);
        }

    }

    void waitFire()
    {
        Fire.gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.gameObject.name);
        
        //if (collision.gameObject.tag == "Block")
        //{
        //    isHitBlock = true;
        //}

        if (collision.gameObject.GetComponent<SpriteRenderer>().sortingLayerName == "Block" ||
            collision.transform.tag == "Enemy")
        {
            isHitBlock = true;
        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        //Debug.Log(collision.gameObject.name);

        //if (collision.gameObject.tag == "Block")
        //{
        //    isHitBlock = false;
        //}

        if (collision.gameObject.GetComponent<SpriteRenderer>().sortingLayerName == "Block" ||
            collision.transform.tag == "Enemy")
        {
            isHitBlock = false;
        }
    }

}

public enum DroneState
{
    Saerch,
    Attack,
    Escape
}