using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Operables : MonoBehaviour
{
    public Component[] operables;
    // Start is called before the first frame update
    void Start()
    {
        operables = this.gameObject.GetComponentsInChildren<Animator>();
        for (int i=0 ; i<operables.Length; i++)
        {
            Renderer[] rend = operables[i].GetComponentsInChildren<Renderer>();
            for (int j = 0; j < rend.Length; j++)
            {
                Renderer childrend = rend[j];
                childrend.material.color = new Color(.71f, .73f, .45f);
                //childrend.material.SetColor("_Color", Color.green);
                //childrend.material.SetColor("_Color", Color.green);
            }

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
