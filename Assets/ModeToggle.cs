using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeToggle : MonoBehaviour
{
    public BaseActionTrigger teleportModeEntryTrigger;
    public BaseActionTrigger manipulationModeEntryTrigger;

    public enum Mode{Teleport, Manipulation};
    // Start is called before the first frame update

    public GameObject manipulationPointer, mainCamera;
    public GameObject teleportPointer, teleportController;

    public Mode playerMode = Mode.Manipulation;
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

    }
}
