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

    public GameObject roundMeterPrefab;
    GameObject roundMeter, roundMeterIndicator;
    readonly float planeRadius = 0.7f;
    GameObject selectingObject;
    Plane roundMeterPlane;
    Vector3  roundMeterPlaneCenter;

    Vector3 lastPlaneHitPoint;

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
            selecting = true;
            selectingObject = rayCaster.GetComponent<RayCastDebugger>().closestHit;

            switch(modeManager.operationMode){
                case ModeToggle.OperationMode.Translation:
                    RaycastResult result = rayCaster.GetComponent<RayCastDebugger>().floorHitResult;
                    if (result.gameObject != null){
                        startDragPosition = result.worldPosition;
                        startDraggingObjectPosition = selectingObject.transform.position;
                    }
                    break;
                case ModeToggle.OperationMode.Scale:
                    if (roundMeter != null){
                        break;
                    }

                    Vector3 planeBottom = rayCaster.GetComponent<RayCastDebugger>().hitPoint;
                    Vector3 planeNorm = transform.position - planeBottom;
                    Vector3 planeCenter = planeBottom + Vector3.Cross(transform.right, planeNorm).normalized * planeRadius;

                    Quaternion spriteRotation = Quaternion.LookRotation(planeNorm, Vector3.up);
                    roundMeter = Instantiate(roundMeterPrefab, planeCenter, spriteRotation);
                    // TODO: scale by distance from camera
                    roundMeter.transform.localScale = Vector3.one * 0.4f;
                    roundMeter.tag = "UI";

                    roundMeterPlane = new Plane(planeNorm.normalized, planeCenter);
                    roundMeterPlaneCenter = planeCenter;

                    lastPlaneHitPoint = planeBottom;

                    break;
            }
        }

        if (selecting){
            // Moving
            switch(modeManager.operationMode){
                case ModeToggle.OperationMode.Translation:
                    Vector3 currentDragPosition = rayCaster.GetComponent<RayCastDebugger>().floorHitResult.worldPosition;
                    selectingObject.transform.position = currentDragPosition - startDragPosition + startDraggingObjectPosition;
                    break;
                case ModeToggle.OperationMode.Scale:
                    RaycastResult result = rayCaster.GetComponent<RayCastDebugger>().UIHitResult;
                    Vector3 hitPoint = result.worldPosition;
                    Vector3 meterCurser = roundMeterPlaneCenter + (hitPoint - roundMeterPlaneCenter).normalized * planeRadius;
                    if (roundMeterIndicator == null){
                        roundMeterIndicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        roundMeterIndicator.transform.localScale = Vector3.one * 0.1f;
                    }
                    roundMeterIndicator.transform.position = meterCurser;
                    float deltaAngle = Vector3.Angle(
                        (lastPlaneHitPoint - roundMeterPlaneCenter).normalized,
                        (hitPoint - roundMeterPlaneCenter).normalized
                    ) * 
                    Mathf.Sign(Vector3.Dot(
                        transform.position - roundMeterPlaneCenter,
                        Vector3.Cross(
                        lastPlaneHitPoint - roundMeterPlaneCenter,
                        hitPoint - roundMeterPlaneCenter)));

                    Debug.Log("DeltaAngle = " + deltaAngle);
                    
                    selectingObject.transform.localScale *= (1 + deltaAngle / 360.0f);

                    lastPlaneHitPoint = hitPoint;
                    break;
            }
        }

        if (GvrControllerInput.ClickButtonUp && selecting){
            // Clicking up
            selecting = false;
            switch(modeManager.operationMode){
                case ModeToggle.OperationMode.Scale:
                    Destroy(roundMeter);
                    Destroy(roundMeterIndicator);
                    break;
            }
        }

    }

    /*
    public void OnDrawGizmos(){
        Debug.Log("Giz");
        Gizmos.DrawSphere(dbg_center, 0.1f);
        Gizmos.DrawSphere(dbg_bottom, 0.05f);
    }
    */
}
