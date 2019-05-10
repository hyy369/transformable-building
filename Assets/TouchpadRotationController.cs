using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchpadRotationController : MonoBehaviour
{
    // Start is called before the first frame update
    public int selectedQuadrant; // range: [1,4]

    public GameObject rotationPivot;
    void Start()
    {

        selectedQuadrant = 2;
    }

    // Update is called once per frame
    void Update()
    {
        float x = GvrControllerInput.TouchPos.x - 0.5f;
        float y = 1f - GvrControllerInput.TouchPos.y - 0.5f;
        if (Mathf.Abs(x - 0f) > 0.01f && Mathf.Abs(y - 0f) > 0.01f){
            float angle = Mathf.Atan2(y, x);
            if (angle < 0){
                angle += 2 * Mathf.PI;
            }
            selectedQuadrant = Mathf.FloorToInt(angle / (Mathf.PI / 2)) + 1;
        }

        rotationPivot.transform.localRotation = Quaternion.Euler(0f, 0f, (float)(selectedQuadrant - 2 ) * 90f);
    }
}
