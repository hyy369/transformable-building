using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformableTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    Animator anim = GetFirstAnimatorInAncestor(hit.transform);
                    if (anim != null){
                        anim.SetTrigger("Toggle");
                    }
                }
            }
        }
    }

    private Animator GetFirstAnimatorInAncestor(Transform leaf){
        while (leaf != null){
            if (leaf.GetComponent<Animator>() != null){
                return leaf.GetComponent<Animator>();
            }
            leaf = leaf.parent;
        }
        return null;
    }
}
