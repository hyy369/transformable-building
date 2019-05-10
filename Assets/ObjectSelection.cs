using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ObjectSelection : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject rayCaster;

    bool selecting = false;
    Vector3 startDragPosition, startDraggingObjectPosition;
    GameObject draggingObject;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!selecting && GvrControllerInput.ClickButtonDown && 
         rayCaster.GetComponent<RayCastDebugger>().hitStatus == RayCastDebugger.RayHitStatus.Selectable){
            RaycastResult result = rayCaster.GetComponent<RayCastDebugger>().floorHitResult;
            if (result.gameObject != null){
                selecting = true;
                startDragPosition = result.worldPosition;
                draggingObject = rayCaster.GetComponent<RayCastDebugger>().closestHit;
                startDraggingObjectPosition = draggingObject.transform.position;
            }
        }

        if (selecting){
            Vector3 currentDragPosition = rayCaster.GetComponent<RayCastDebugger>().floorHitResult.worldPosition;
            draggingObject.transform.position = currentDragPosition - startDragPosition + startDraggingObjectPosition;
        }

        if (GvrControllerInput.ClickButtonUp && selecting){
            selecting = false;
        }
    }
}
