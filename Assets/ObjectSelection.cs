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

    // For Translation
    Vector3 startDragPosition, startDraggingObjectPosition;
    public GameObject planeIndicatorPrefab;
    GameObject planeIndicator;
    readonly float indicatorOffset;

    // For RotateY & Scale
    public GameObject roundMeterPrefab, roundMeterCursorPrefab;
    GameObject roundMeter, roundMeterCursor;
    readonly float planeRadius = 0.7f;
    GameObject selectingObject;
    Plane roundMeterPlane;
    Vector3  roundMeterPlaneCenter;
    Vector3 lastPlaneHitPoint;

    // For RotationFree
    Quaternion lastPointerRotation;
    public GameObject freeRotationIndicatorPrefab;
    GameObject freeRotationIndicator;

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

                        planeIndicator = Instantiate(planeIndicatorPrefab, startDragPosition + Vector3.up * indicatorOffset,
                        selectingObject.transform.rotation, selectingObject.transform);
                        Vector3 originalScale = planeIndicator.transform.localScale;
                        planeIndicator.transform.localScale = new Vector3(
                            originalScale.x / selectingObject.transform.localScale.x,
                            originalScale.y / selectingObject.transform.localScale.y,
                            originalScale.z / selectingObject.transform.localScale.z);
                    }
                    break;
                case ModeToggle.OperationMode.Scale: case ModeToggle.OperationMode.RotationY:
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
                case ModeToggle.OperationMode.RotationFree:
                    lastPointerRotation = transform.rotation;
                    freeRotationIndicator = Instantiate(freeRotationIndicatorPrefab, rayCaster.GetComponent<RayCastDebugger>().hitPoint, 
                    selectingObject.transform.rotation, selectingObject.transform);
                        Vector3 originalScale1 = freeRotationIndicator.transform.localScale;
                        freeRotationIndicator.transform.localScale = new Vector3(
                            originalScale1.x / selectingObject.transform.localScale.x,
                            originalScale1.y / selectingObject.transform.localScale.y,
                            originalScale1.z / selectingObject.transform.localScale.z);
                    break;
            }
        }

        if (selecting){
            // Moving
            selectingObject.GetComponent<Rigidbody>().isKinematic = true;

            switch(modeManager.operationMode){
                case ModeToggle.OperationMode.Translation: 
                    Vector3 currentDragPosition = rayCaster.GetComponent<RayCastDebugger>().floorHitResult.worldPosition;
                    selectingObject.transform.position = currentDragPosition - startDragPosition + startDraggingObjectPosition;
                    break;
                case ModeToggle.OperationMode.Scale: case ModeToggle.OperationMode.RotationY:
                    RaycastResult result = rayCaster.GetComponent<RayCastDebugger>().UIHitResult;
                    Vector3 hitPoint = result.worldPosition;
                    Vector3 meterCurser = roundMeterPlaneCenter + (hitPoint - roundMeterPlaneCenter).normalized * planeRadius;
                    if (roundMeterCursor == null){
                        roundMeterCursor = Instantiate(roundMeterCursorPrefab);
                        roundMeterCursor.transform.localScale = Vector3.one * 0.05f;
                    }
                    roundMeterCursor.transform.position = meterCurser;
                    roundMeterCursor.transform.rotation = Quaternion.LookRotation(roundMeterPlane.normal, hitPoint - roundMeterPlaneCenter);
                    float deltaAngle = Vector3.Angle(
                        (lastPlaneHitPoint - roundMeterPlaneCenter).normalized,
                        (hitPoint - roundMeterPlaneCenter).normalized
                    ) * 
                    Mathf.Sign(Vector3.Dot(
                        transform.position - roundMeterPlaneCenter,
                        Vector3.Cross(
                        lastPlaneHitPoint - roundMeterPlaneCenter,
                        hitPoint - roundMeterPlaneCenter)));
                    
                    if (modeManager.operationMode == ModeToggle.OperationMode.Scale){
                        selectingObject.transform.localScale *= (1 + deltaAngle / 360.0f);
                    }
                    else{
                        selectingObject.transform.Rotate(0f, deltaAngle, 0f);
                    }

                    lastPlaneHitPoint = hitPoint;
                    break;
                case ModeToggle.OperationMode.RotationFree:
                    Quaternion currentRotation = transform.rotation;
                    selectingObject.transform.rotation *= currentRotation * Quaternion.Inverse(lastPointerRotation);

                    lastPointerRotation = currentRotation;
                    break;
            }
        }

        if (GvrControllerInput.ClickButtonUp && selecting){
            // Clicking up
            selectingObject.GetComponent<Rigidbody>().isKinematic = false;

            selecting = false;
            switch(modeManager.operationMode){
                case ModeToggle.OperationMode.Translation:
                    Destroy(planeIndicator);
                    break;
                case ModeToggle.OperationMode.Scale: case ModeToggle.OperationMode.RotationY:
                    Destroy(roundMeter);
                    Destroy(roundMeterCursor);
                    break;
                case ModeToggle.OperationMode.RotationFree:
                    Destroy(freeRotationIndicator);
                    break;
            }
        }

    }
}
