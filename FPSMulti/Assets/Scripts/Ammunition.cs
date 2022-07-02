using FPSMulti;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammunition : MonoBehaviour
{
    public int count;
    private bool gathered;
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if(player != null && !gathered)
        {
            player.GatherAmmo(count);

            StartCoroutine(SpawAgain(2));
        }
    }


    private IEnumerator SpawAgain(float pWait)
    {
        gathered = true;
        Debug.Log("zebrane");
        gameObject.SetActive(false);
        yield return new WaitForSeconds(pWait);
        gameObject.SetActive(true);
        Debug.Log("spawn amunicji");
        gathered = false;
    }
}
