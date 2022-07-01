using FPSMulti;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammunition : MonoBehaviour
{
    public int count;
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if(player != null)
        {
            player.GatherAmmo(count);
            gameObject.SetActive(false);
        }
    }
}
