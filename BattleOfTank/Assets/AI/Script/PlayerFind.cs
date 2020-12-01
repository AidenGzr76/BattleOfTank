using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFind : MonoBehaviour
{
    public static bool playerFinded = false;

    private void OnTriggerEnter2D(Collider2D collider)
    {   
        if (collider.gameObject.tag == "Player")
        {
            playerFinded = true;
            //AI._target = collider.GetComponent<Player>();
            transform.parent.GetComponent<AIPath>().maxSpeed = 0;
        }
    }
}
