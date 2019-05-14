using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MinimapTexture : MonoBehaviour
{
    public GameObject obj;
    public Texture floor1, floor2, floor3;
    int current_floor = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (obj.transform.position.y < 1)
        {
            if (current_floor != 1)
                GetComponent<Renderer>().material.SetTexture("_MainTex", floor1);
            current_floor = 1;
        }
        else if ((obj.transform.position.y >= 1) && (obj.transform.position.y < 5))
        {
            if (current_floor != 2)
                GetComponent<Renderer>().material.SetTexture("_MainTex", floor2);
            current_floor = 2;
        }
        else if (obj.transform.position.y >= 5)
        {
            if (current_floor != 3)
                GetComponent<Renderer>().material.SetTexture("_MainTex", floor3);
            current_floor = 3;
        }
    }
}
