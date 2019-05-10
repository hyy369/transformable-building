using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RayCastDebugger : MonoBehaviour
{
    public GameObject closestHit;

    /*Operable: including free translate and operable hinge 
    Selectable: can perform (traditional) rotation, translation and scale*/
    public enum RayHitStatus{None, Selectable, Operable};
    
    /* TODO - Should only return the nearest floor that is hit, excluding the ceiling*/
    public RaycastResult floorHitResult{
        get{
            GvrPointerPhysicsRaycaster caster = gameObject.GetComponent<GvrPointerPhysicsRaycaster>();
            List<RaycastResult> hits = new List<RaycastResult>();
            caster.Raycast(null, hits);

            foreach (RaycastResult hit in hits){
                if (hit.gameObject.tag == "Floor" 
                    && hit.worldPosition.y < transform.position.y){
                    return hit;
                }
            }

            var invalid = new RaycastResult();
            invalid.gameObject =  null;
            return invalid;
        }
    }

    /*Call this function when ButtonUp or ButtonDown */
    public RayHitStatus hitStatus{
        get{
            UpdateStatus();

            if (closestHit == null){
                return RayHitStatus.None;
            }

            if (closestHit.layer == LayerMask.NameToLayer("Selectable")){
                return RayHitStatus.Selectable;
            }
            return RayHitStatus.Operable;
        }
    }

    // Update is called once per frame
    void UpdateStatus()
    {
        GvrPointerPhysicsRaycaster caster = gameObject.GetComponent<GvrPointerPhysicsRaycaster>();
        List<RaycastResult> hits = new List<RaycastResult>();
        caster.Raycast(null, hits);

        if (hits.Count >= 1){
            float minDistance = float.MaxValue;
            foreach (RaycastResult r in hits){
                if (r.distance < minDistance){
                    minDistance = r.distance;
                    closestHit = r.gameObject;
                }
            }
        }
        else{
            closestHit = null;
        }
        
        //Debug.Log("Update hit status = " + hitStatus + "Floorstatus = " + floorHitResult.isValid);
    }

}
