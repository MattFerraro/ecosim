using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxBehavior : MonoBehaviour
{
    public Rigidbody rb;
    float sightRange = 200.0f;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        // Debug.Log("we're here!");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // find that nearest bunny
        GameObject target = closestVisible("Bunny");
        if (target != null){
            // move to it
            if (Vector3.Distance(rb.position,target.transform.position) > 1.1){
                Vector3 move = Vector3.MoveTowards(rb.position,target.transform.position,0.1f);
                rb.MovePosition(move);
            } else {
                GameObject.Destroy(target);
            }
        }
        
        // else {
        //     rb.AddForce(new Vector3(0.0f,0.0f,0.0f),ForceMode.VelocityChange);
        // }
        // homf!
        
        //rb.AddForce(0, 0, 1000 * Time.deltaTime);
    }

    GameObject closestVisible(string thing){
        // find nearby things!
        Collider[] nearbyObjects = Physics.OverlapSphere(rb.position,sightRange);
        // find closest bunny
        float minDist = 10000000000f;
        GameObject nearestThing = gameObject;
        foreach (Collider c in nearbyObjects){
            Debug.Log($"name is {c.gameObject.name}");
            if (c.gameObject.name.StartsWith(thing)){
                float dist = Vector3.Distance(rb.position,c.gameObject.transform.position);
                if (dist < minDist){
                    minDist = dist;
                    nearestThing = c.gameObject;
                }
            }
        }
        if (nearestThing == gameObject){
            return null;
        }
        else {
            return nearestThing;
        }
    }
}