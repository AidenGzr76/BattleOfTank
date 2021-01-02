using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPlayerFind : MonoBehaviour
{
    public static bool GunFindedplayer = false;
    public static string GunName;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            //Debug.Log("Player In");
            GunFindedplayer = true;
            GunName = transform.parent.name;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            //Debug.Log("Player Out");
            GunFindedplayer = false;
            GunAI.allowFire = false;
        }
    }
}
