using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
public class Gun : ScriptableObject
{
    public string name;
    public int damage;
    public int ammo;
    public int clipsize;
    public float firerate;
    public float distance;
    public float aimSpeed; //wiecej to szybciej
    public float bloom; //wiecej to wiêkszy rozstrza³. 20 to idealny
    public float aimbloom; //jak wyzej, przy celowaniu.
    public float recoil; //odrzut pionowy broni
    public float kicback;
    public float reloadTime;
    public int burst; //0 is semi, 1 is auto, 2 is burstfire

    public GameObject prefab;

    private int stash; //current ammo
    private int clip; //now in clip

    public void Initialize()
    {
        stash = ammo;
        clip = clipsize;
    }

    public bool CanFire()
    {
        Debug.Log(clip);
        if (clip > 0)
        {
            clip -= 1;
            return true;
        }
        else return false;
    }

    public bool Reload()
    {
        if (stash > 0)
        {
            Debug.Log("Reload");
            //throws away remaining bullets
            Debug.Log("Stash: " + stash);
            Debug.Log("Clip: " + clipsize);
            stash -= clipsize;
            clip = clipsize;
            Debug.Log("Later Stash: " + stash);
            Debug.Log("Later Clip: " + clipsize);
            return true;

            //adds bullet to existing ones
            /*
             * stash += clip;
             * clip = Matchf.Min(clipsize,stash);
             * stash -= clip;
             */
        }
        else return false;
    }

    public int GetStash()
    { return stash; }

    public int GetClip()
    { return clip; }

    public void AddToStash(int count) 
    {
        stash += count;
    }
}