using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeToggle : MonoBehaviour
{
    public GameObject transformModeSelectionPlate;
    private GameObject transformModeSelectionInstance;
    public BaseActionTrigger teleportModeEntryTrigger;
    public BaseActionTrigger manipulationModeEntryTrigger;

    public enum Mode{Teleport, Manipulation};
    public enum OperationMode{Translation, RotationY, RotationFree, Scale}
    // Start is called before the first frame update

    public GameObject manipulationPointer, mainCamera;
    public GameObject teleportPointer, teleportController;

    public Mode playerMode = Mode.Manipulation;
    public OperationMode operationMode = OperationMode.Translation;

    float hoveringDuration = 0f;
    
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (teleportModeEntryTrigger.TriggerActive() && playerMode != Mode.Teleport){
            Destroy(GameObject.Find("LineOriginVisual(Clone)"));

            manipulationPointer.SetActive(false);
            teleportPointer.SetActive(true);
            GetComponent<CapsuleCollider>().enabled = true;
            mainCamera.GetComponent<GvrPointerPhysicsRaycaster>().enabled = false;
            playerMode = Mode.Teleport;
        }
        
        if (manipulationModeEntryTrigger.TriggerActive() && playerMode != Mode.Manipulation){
            Destroy(GameObject.Find("TeleportTarget(Clone)"));

            teleportPointer.SetActive(false);
            manipulationPointer.SetActive(true);
            GetComponent<CapsuleCollider>().enabled = false;
            mainCamera.GetComponent<GvrPointerPhysicsRaycaster>().enabled = true;
            playerMode = Mode.Manipulation;
        }

        if (GvrControllerInput.ClickButton){
            hoveringDuration = 0f;
        }

        if (GvrControllerInput.IsTouching){
            //Just hovering
            hoveringDuration += Time.deltaTime;

            if (hoveringDuration > 1.2f && transformModeSelectionInstance == null){
                transformModeSelectionInstance = GameObject.Instantiate(transformModeSelectionPlate, mainCamera.transform);
                transformModeSelectionInstance.transform.localPosition = new Vector3(0.5f, 0.3f, 2.2f);
                transformModeSelectionInstance.transform.localRotation = Quaternion.Euler(-17.4f, 16.8f, 1.8f);
                transformModeSelectionInstance.transform.localScale = new Vector3(0.13f, 0.13f, 0.21f);
            }
        }

        if (GvrControllerInput.TouchUp || GvrControllerInput.ClickButton){
            hoveringDuration = 0f;
            if (transformModeSelectionInstance == null){
                return;
            }

            switch(transformModeSelectionInstance.GetComponent<TouchpadRotationController>().selectedQuadrant){
                case 1: 
                    operationMode = OperationMode.RotationY;
                    break;
                case 2:
                    operationMode = OperationMode.Translation;
                    break;
                case 3:
                    operationMode = OperationMode.RotationFree;
                    break;
                case 4:
                    operationMode = OperationMode.Scale;
                    break;
            }

            Destroy(transformModeSelectionInstance);
        }
    }
}
