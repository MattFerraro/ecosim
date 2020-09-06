using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Omnivore : MonoBehaviour
{
    public Rigidbody rb;
    // genome is set at birth and immutable
    public Dictionary<string,Gene> genome;
    // non-heritable state:
    float hunger = 0.4f;
    float thirst = 0.2f;
    string mode = "happy";
    GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        target = gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // get more hungry/thirsty
        hunger += Time.deltaTime * 0.001f;
        thirst += Time.deltaTime * 0.001f;
        // die if you ded
        if (hunger >=1 || thirst >= 1){
            GameObject.Destroy(gameObject);
        }
        // mode changing logic
        if (hunger > thirst){
            mode = "hungry";
        } else {
            mode = "thirsty";
        }
        // do actions, according to mode
        switch (mode){
            case "hungry":
                FixedUpdateHungry();
                break;
            case "thirsty":
                FixedUpdateThirsty();
                break;
            default:
                break;
        }
        // find that nearest tree
        // GameObject target = closestVisible("Tree");
        // GameObject danger = closestVisible("Fox");
        // GameObject suitor = closestVisible("Bunny");
        // Debug.Log("Found a tree!");
        // move to it
        // else {
        //         //GameObject.Destroy(target);
        //         rb.AddForce(new Vector3(0.0f,0.0f,0.0f),ForceMode.VelocityChange);
        //     }
        //}
    }
    void FixedUpdateHungry(){
        target = closestVisiblePlant();
        if (target == null){
            return;
        }
        if (Vector3.Distance(rb.position,target.transform.position) >5){
                Vector3 move = Vector3.MoveTowards(rb.position,target.transform.position,0.07f);
                rb.MovePosition(move);
        } else {
            hunger = 0;
        }
    }
    void FixedUpdateThirsty(){
        Debug.Log("I'm Thirsty!");
        target = closestVisiblePond();
        if (target == null){
            Debug.Log("but not a drop to drink");
            return;
        }
        if (Vector3.Distance(rb.position,target.transform.position) >6){
            Vector3 move = Vector3.MoveTowards(rb.position,target.transform.position,0.07f);
            rb.MovePosition(move);
        } else {
            thirst = 0;
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
    GameObject closestVisiblePlant(){
        // find nearby things!
        Collider[] nearbyObjects = Physics.OverlapSphere(rb.position,genome["sightRange"].value());
        // find closest thing
        float minDist = 10000000000f;
        GameObject nearestThing = null;
        foreach (Collider c in nearbyObjects){
            // Debug.Log($"name is {c.gameObject.name}");
            Plant plt = c.gameObject.GetComponent<Plant>();
            if (plt != null){
                float dist = Vector3.Distance(rb.position,c.gameObject.transform.position);
                if (dist < minDist & c.gameObject != gameObject){
                    minDist = dist;
                    nearestThing = c.gameObject;
                }
            }
        }
        return nearestThing;
    }
    GameObject closestVisiblePond(){
        // find nearby things!
        Collider[] nearbyObjects = Physics.OverlapSphere(rb.position,genome["sightRange"].value());
        // find closest thing
        float minDist = 10000000000f;
        GameObject nearestThing = null;
        foreach (Collider c in nearbyObjects){
            // Debug.Log($"name is {c.gameObject.name}");
            if (c.gameObject.name.StartsWith("Pond")){
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
