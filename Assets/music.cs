using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class music : MonoBehaviour
{
    public GameObject obj;
    public AudioSource floor1;
    public AudioSource floor2;
    public AudioSource floor3;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (obj.transform.position.y < 1){
            if (!floor1.isPlaying)
            {
                floor1.Play();
            }
            if (floor2.isPlaying){
                floor2.Stop();
            }if (floor3.isPlaying){
                floor3.Stop();
            }
        }else if ((obj.transform.position.y >= 1) && (obj.transform.position.y < 5)) {
            if (!floor2.isPlaying)
            {
                floor2.Play();
            }
            if (floor3.isPlaying){
                floor3.Stop();
            }if (floor1.isPlaying){
                floor1.Stop();
            }
        }else if (obj.transform.position.y >= 5){
            if (!floor3.isPlaying)
            {
                floor3.Play();
            }
            if (floor1.isPlaying){
                floor1.Stop();
            }else if (floor2.isPlaying){
                floor2.Stop();
            }
        }
    }
}
