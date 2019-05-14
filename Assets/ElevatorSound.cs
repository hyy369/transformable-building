using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorSound : MonoBehaviour
{
    [SerializeField]
    AudioClip floorReachSound;
    public void PlaySound(){
        GetComponent<AudioSource>().PlayOneShot(floorReachSound);
    }
}
