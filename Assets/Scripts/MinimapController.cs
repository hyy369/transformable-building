using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    public GameObject FPS;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = new Vector3(FPS.transform.position.x , -99.9f, FPS.transform.position.z);
        transform.forward = FPS.transform.forward;
    }
}
