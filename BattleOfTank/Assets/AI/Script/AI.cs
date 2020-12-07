using System.Linq;
using UnityEngine;
using Pathfinding;
using System.Collections;

public class AI : MonoBehaviour
{
    public Team Team => _team;
    [SerializeField] private Team _team;
    [SerializeField] private LayerMask _layerMask;

    private float _attackRange = 10.3f;

    private Vector3 _direction;
    //public static Player _target;
    public Player Target;
    private DroneState _currentState;

    //EnemyAI
    public Transform nextPos;
    public float speed = 200f;
    public float nextWayPointDistance = 3f;

    public Transform enemyGFX;

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

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        tempSpeed = transform.GetComponent<AIPath>().maxSpeed;

        InvokeRepeating("CmdFire", 0f, 1f);
        InvokeRepeating("NextRandomPos", 0f, 2f);
    }

    void NextRandomPos()
    {
        if (allowFindRandomPos)
        {
            allowFindRandomPos = false;
            nextPos.position = new Vector3(Random.Range(-9, 9), Random.Range(-5, 5), 0);
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
                        Debug.Log("Path Blocked");
                        //NextRandomPos();
                        allowFindRandomPos = true;
                    }

                    if (PlayerFind.playerFinded)
                    {
                        _currentState = DroneState.Attack;
                    }

                    break;
                }
            //case DroneState.Chase:
            //    {
            //        if (Target == null)
            //        {
            //            _currentState = DroneState.Saerch;
            //            return;
            //        }

            //        AIDestinationSetter.wantSearch = false;

            //        if (Vector3.Distance(transform.position, Target.transform.position) < _attackRange)
            //        {
            //            _currentState = DroneState.Attack;
            //        }

            //        checkEscape();

            //        break;
            //    }
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
                    Debug.Log("ACC");
                    if (Target != null)
                    {
                        allowFire = false;
                        Debug.Log("ADD");
                        AIDestinationSetter.wantSearch = true;

                        if (AIHealth.isEscape && Vector3.Distance(Target.transform.position, nextPos.position) > 10f)
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
        if (AIHealth.isEscape)
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

        if (collision.gameObject.GetComponent<SpriteRenderer>().sortingLayerName == "Block")
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

        if (collision.gameObject.GetComponent<SpriteRenderer>().sortingLayerName == "Block")
        {
            isHitBlock = false;
        }
    }

}

public enum Team
{
    Red,
    Blue
}

public enum DroneState
{
    Saerch,
    Chase,
    Attack,
    Escape
}