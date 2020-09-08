using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Omnivore : MonoBehaviour
{
    public GameObject prefab; // this is purely used for reproduction!
    public Rigidbody rb;
    // genome is set at birth and immutable
    public Dictionary<string,Gene> genome;
    // non-heritable state:
    float hunger = 0.4f;
    float thirst = 0.51f;
    string mode = "happy";
    float recoveryTime = 1;
    float age = 0;

    GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        target = gameObject;
    }

    void Die() {
        GameObject.Destroy(gameObject);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        age += Time.deltaTime;
        // get more hungry/thirsty
        hunger += Time.deltaTime * 0.01f;
        thirst += Time.deltaTime * 0.01f;
        // die if you ded
        if (hunger >=1 || thirst >= 1 || age > 25){
            Die();
        }
        // mode changing logic
        if (thirst > 0.5) { // these threshold should be part of the genome!
            mode = "thirsty";
        } else if (hunger > 0.7) {
            mode = "hungry";
        } else {
            if (recoveryTime > 0) {
                recoveryTime -= Time.deltaTime;
                mode = "hungry";
            } else if (age < 3) {
                mode = "hungry";
            }
            else {
                mode = "frisky";
            }
        }


        // do actions, according to mode
        switch (mode){
            case "hungry":
                FixedUpdateHungry();
                break;
            case "thirsty":
                FixedUpdateThirsty();
                break;
            case "frisky":
                FixedUpdateFrisky();
                break;
        }
    }
    void FixedUpdateHungry(){
        target = closestVisiblePlant();
        if (target == null){
            return;
        }
        if (Vector3.Distance(rb.position,target.transform.position) > 5){
                Vector3 move = Vector3.MoveTowards(rb.position,target.transform.position,0.07f);
                rb.MovePosition(move);
        } else {
            hunger = 0;
        }
    }
    void FixedUpdateThirsty(){
        target = closestVisiblePond();
        if (target == null){
            return;
        }
        if (Vector3.Distance(rb.position,target.transform.position) > 6){
            Vector3 move = Vector3.MoveTowards(rb.position,target.transform.position,0.07f);
            rb.MovePosition(move);
        } else {
            thirst = 0;
        }
    }

    void FixedUpdateFrisky(){
        GameObject target = closestVisibleMate();
        if (target == null){
            return;
        }
        Omnivore mate = target.GetComponent<Omnivore>();
        if (Vector3.Distance(rb.position,target.transform.position) > 1){
            Vector3 move = Vector3.MoveTowards(rb.position,target.transform.position,0.07f);
            rb.MovePosition(move);
        } else {
            // Do the nasty
            if (mate.getMode() == "frisky" && this.getSex() == 0) {
                createOffspring(mate);
            }
        }
    }

    // SetGenome will set this animal's genome, then set properties based on it
    public void SetGenome(GameObject prefab, Dictionary<string,Gene> genome){
        this.genome = genome;
        this.prefab = prefab;
        Renderer renderer = gameObject.GetComponent<Renderer>();
        Material material = GetComponent<Renderer>().material;
        Color color = material.color;
        Color newColor;
        if (genome["sex"].value() == 0) {

            newColor = Color.HSVToRGB(0, 1, 1);
        } else {
            newColor = Color.HSVToRGB(0.8f, 1, 1);
        }
        material.color = newColor;
        // set energy and other state
    }

    public void createOffspring(Omnivore mate) {
        recoveryTime = 10;

        Vector3 offset = new Vector3(0, 5, 0);
        Vector3 pos = gameObject.transform.position + offset;

        // Unity doesn't let you use meaningful constructors.
        // The way to pass down the genome is explicitely with a setter:
        GameObject go = (GameObject)Instantiate(prefab, pos, Quaternion.identity);
        Omnivore omni = go.GetComponent<Omnivore>();
        Dictionary<string, Gene> newGenome = Genome.cross(this.genome, mate.genome);
        omni.SetGenome(prefab, newGenome);
    }

    public float getSex() {
        if (this.genome["sex"] != null) {
            return genome["sex"].value();
        } else {
            return -1;
        }
    }

    public string getMode() {
        return this.mode;
    }

    GameObject closestVisiblePlant(){
        // find nearby things!
        Collider[] nearbyObjects = Physics.OverlapSphere(rb.position,genome["sightRange"].value());
        // find closest thing
        float minDist = 10000000000f;
        GameObject nearestThing = null;
        foreach (Collider c in nearbyObjects){
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

    GameObject closestVisibleMate(){
        // find nearby things!
        Collider[] nearbyObjects = Physics.OverlapSphere(rb.position,genome["sightRange"].value());
        // find closest thing
        float minDist = 10000000000f;
        GameObject nearestThing = null;
        foreach (Collider c in nearbyObjects){
            Omnivore mate = c.gameObject.GetComponent<Omnivore>();
            // Not all gameObjects are even Omnivores
            if (mate != null) {

                // Not all omnivores are the right sex, and not all of them are frisky
                if (mate.getSex() == -1 || mate.getSex() == this.getSex() || mate.getMode() != "frisky") {
                    mate = null;
                }
            }
            if (mate != null){
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
            if (c.gameObject.name.StartsWith("Pond")){
                float dist = Vector3.Distance(rb.position, c.gameObject.transform.position);
                if (dist < minDist & c.gameObject != gameObject){
                    minDist = dist;
                    nearestThing = c.gameObject;
                }
            }
        }
        return nearestThing;
    }
}
