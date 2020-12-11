using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFind : MonoBehaviour
{
    public static bool playerFinded = false;
    public static string enemyName;

    private void OnTriggerEnter2D(Collider2D collider)
    {   
        if (collider.gameObject.tag == "Player")
        {
            playerFinded = true;
            //AI._target = collider.GetComponent<Player>();
            transform.parent.GetComponent<AIPath>().maxSpeed = 0;
            enemyName = transform.parent.name;
        }
    }
}
