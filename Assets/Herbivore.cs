using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herbivore : MonoBehaviour
{
    public Rigidbody rb;
    float sightRange = 200.0f;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // find that nearest tree
        GameObject target = closestVisible("Tree");
        if (target != null){
            Debug.Log("Found a tree!");
            // move to it
            if (Vector3.Distance(rb.position,target.transform.position) > 1.1){
                Vector3 move = Vector3.MoveTowards(rb.position,target.transform.position,0.1f);
                rb.MovePosition(move);
            } else {
                //GameObject.Destroy(target);
                rb.AddForce(new Vector3(0.0f,0.0f,0.0f),ForceMode.VelocityChange);
            }
        }
    }

    GameObject closestVisible(string thing){
        // find nearby things!
        Collider[] nearbyObjects = Physics.OverlapSphere(rb.position,sightRange);
        // find closest thing
        float minDist = 10000000000f;
        GameObject nearestThing = gameObject;
        foreach (Collider c in nearbyObjects){
            // Debug.Log($"name is {c.gameObject.name}");
            if (c.gameObject.name.StartsWith(thing)){
                float dist = Vector3.Distance(rb.position,c.gameObject.transform.position);
                if (dist < minDist){
                    minDist = dist;
                    nearestThing = c.gameObject;
                }
            } else {
                Debug.Log("Nearby Object: "+c.gameObject.name);
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
