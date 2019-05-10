using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ObjectSelection : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject rayCaster;
    public GameObject player;

    ModeToggle modeManager;

    bool selecting = false;
    Vector3 startDragPosition, startDraggingObjectPosition;
    GameObject draggingObject;

    void Start()
    {
        modeManager = player.GetComponent<ModeToggle>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!selecting && GvrControllerInput.ClickButtonDown && 
         rayCaster.GetComponent<RayCastDebugger>().hitStatus == RayCastDebugger.RayHitStatus.Selectable){
            //Click down

            switch(modeManager.operationMode){
                case ModeToggle.OperationMode.Translation:
                    RaycastResult result = rayCaster.GetComponent<RayCastDebugger>().floorHitResult;
                    if (result.gameObject != null){
                        selecting = true;
                        startDragPosition = result.worldPosition;
                        draggingObject = rayCaster.GetComponent<RayCastDebugger>().closestHit;
                        startDraggingObjectPosition = draggingObject.transform.position;
                    }
                    break;
            }
        }

        if (selecting){
            // Moving
            switch(modeManager.operationMode){
                case ModeToggle.OperationMode.Translation:
                    Vector3 currentDragPosition = rayCaster.GetComponent<RayCastDebugger>().floorHitResult.worldPosition;
                    draggingObject.transform.position = currentDragPosition - startDragPosition + startDraggingObjectPosition;
                break;
            }
        }

        if (GvrControllerInput.ClickButtonUp && selecting){
            // Clicking up
            selecting = false;
        }

    }
}
