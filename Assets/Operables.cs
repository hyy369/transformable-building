using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Operables : MonoBehaviour
{
    Component[] operables;

    readonly float changeSpeed = 1.0f;
    // Start is called before the first frame update
    Dictionary<Renderer, Color> originalColor;
    bool changeDirection = true;
    float weight = 0.5f;
    readonly Color highlightColor = new Color(.71f, .73f, .45f);

    void Start()
    {
        originalColor = new Dictionary<Renderer, Color>();
        operables = this.gameObject.GetComponentsInChildren<Animator>();
        for (int i=0 ; i<operables.Length; i++)
        {
            Renderer[] rend = operables[i].GetComponentsInChildren<Renderer>();
            for (int j = 0; j < rend.Length; j++)
            {
                Renderer childrend = rend[j];
                originalColor[childrend] = childrend.material.color;
                // childrend.material.color = new Color(.71f, .73f, .45f);
                //childrend.material.SetColor("_Color", Color.green);
                //childrend.material.SetColor("_Color", Color.green);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        weight = changeDirection ? weight + Time.deltaTime * changeSpeed : weight - Time.deltaTime * changeSpeed;
        if (weight > 1.0f || weight < 0f){
            changeDirection = !changeDirection;
        }
        weight = Mathf.Clamp01(weight);

        foreach(var renderer in originalColor.Keys){
            if (changeDirection){
                renderer.material.color = Color.Lerp(originalColor[renderer], highlightColor, weight);
            }
        }
    }
}
