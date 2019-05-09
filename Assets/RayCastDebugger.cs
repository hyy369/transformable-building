using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RayCastDebugger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GvrPointerPhysicsRaycaster caster = gameObject.GetComponent<GvrPointerPhysicsRaycaster>();
        List<RaycastResult> hits = new List<RaycastResult>();
        caster.Raycast(null, hits);
        Debug.Log(hits.Count);
        if (hits.Count > 1){
            Debug.Log(hits[1]);
        }
    }
}
