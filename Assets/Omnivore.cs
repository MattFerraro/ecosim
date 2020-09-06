using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Omnivore : MonoBehaviour
{
    public Rigidbody rb;
    // genome is set at birth and immutable
    public Dictionary<string,Gene> genome;
    // non-heritable state:
    float energy = 30.0f;
    Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        target = rb.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // navigate to a target
        if (Vector3.Distance(rb.position,target)<1.0){
            // do stuff with target
            // then set new target
            target = new Vector3(
                Random.value * 100 - 50, 1, Random.value * 100 - 50
            );
        }


        // find that nearest tree
        // GameObject target = closestVisible("Tree");
        // GameObject danger = closestVisible("Fox");
        // GameObject suitor = closestVisible("Bunny");
        if (target != null){
            // Debug.Log("Found a tree!");
            // move to it
            if (Vector3.Distance(rb.position,target) >.1){
                Vector3 move = Vector3.MoveTowards(rb.position,target,0.07f);
                rb.MovePosition(move);
            }// else {
        //         //GameObject.Destroy(target);
        //         rb.AddForce(new Vector3(0.0f,0.0f,0.0f),ForceMode.VelocityChange);
        //     }
        }
    }
// SetGenome will set this animal's genome, then set properties based on it
    public void SetGenome(Dictionary<string,Gene> genome){
        this.genome = genome;
        gameObject.GetComponent<Renderer>().material.color = 
            Color.HSVToRGB(genome["h"].value(), genome["s"].value(), genome["v"].value());
        // set energy and other state
    }

    public void createOffspring(AnimalGenome mate) {

    }
    GameObject closestVisible(string thing){
        // find nearby things!
        Collider[] nearbyObjects = Physics.OverlapSphere(rb.position,genome["sightRange"].value());
        // find closest thing
        float minDist = 10000000000f;
        GameObject nearestThing = null;
        foreach (Collider c in nearbyObjects){
            // Debug.Log($"name is {c.gameObject.name}");
            if (c.gameObject.name.StartsWith(thing)){
                float dist = Vector3.Distance(rb.position,c.gameObject.transform.position);
                if (dist < minDist & c.gameObject != gameObject){
                    minDist = dist;
                    nearestThing = c.gameObject;
                }
            }
        }
        return nearestThing;
    }
}
