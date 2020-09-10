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
    float hunger = 0.2f;
    static float maxAge = 100;
    float thirst = 0.2f;
    string mode = "happy";
    float recoveryTime = 0;
    float age = 0;
    float size = 1; // this is as a fraction of the size set by the genome
    static Vector3 nullVector = new Vector3(-1, -1, -1);
    Vector3 wanderLocation = nullVector;

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

        float percentGrown = age / genome["ageToReproduce"].value();
        if (percentGrown <= 1) {
            size = percentGrown * genome["size"].value();
        } else {
            size = genome["size"].value();
        }
        this.gameObject.transform.localScale = new Vector3(size, size, size);


        // die if you ded
        if (hunger >=1 || thirst >= 1 || age > maxAge){
            Die();
        }
        // mode changing logic
        if (thirst > 0.4) { // these threshold should be part of the genome!
            mode = "thirsty";
        } else if (hunger > 0.6) {
            mode = "hungry";
        } else {
            if (recoveryTime > 0) {
                recoveryTime -= Time.deltaTime;
                mode = "hungry";
            } else if (size < genome["size"].value()) {
                mode = "hungry";
            }
            else {
                mode = "frisky";
            }
        }


        // do actions, according to mode
        bool didFindSomething = false;
        switch (mode){
            case "hungry":
                didFindSomething = FixedUpdateHungry();
                break;
            case "thirsty":
                didFindSomething = FixedUpdateThirsty();
                break;
            case "frisky":
                didFindSomething = FixedUpdateFrisky();
                break;
        }
        if (didFindSomething) {
            wanderLocation = nullVector;
        } else {
            FixedUpdateWander();
        }


        // Update our status to the text overs
        TextTag myTextTag = gameObject.GetComponent<TextTag>();
        myTextTag.SetText("Hunger: " + hunger.ToString("0.00"));
    }

    void FixedUpdateWander() {
        if (wanderLocation == nullVector) {
            wanderLocation = new Vector3(0, 0, 0);
        }
        Vector3 move = Vector3.MoveTowards(rb.position, wanderLocation, genome["walkspeed"].value());
        rb.MovePosition(move);
    }

    bool FixedUpdateHungry(){
        target = closestVisiblePlant();
        if (target == null){
            return false;
            // Just pick a random spot and go!
        }
        float dx = rb.position.x - target.transform.position.x;
        float dz = rb.position.z - target.transform.position.z;
        float distXZ2 = dx * dx + dz * dz;
        if (distXZ2 > size * size){
                Vector3 move = Vector3.MoveTowards(rb.position,target.transform.position,genome["walkspeed"].value());
                rb.MovePosition(move);
        } else {
            hunger = 0;
        }
        return true;
    }
    bool FixedUpdateThirsty(){
        target = closestVisiblePond();
        if (target == null){
            return false;
        }
        if (Vector3.Distance(rb.position,target.transform.position) > 5 + size){
            Vector3 move = Vector3.MoveTowards(rb.position,target.transform.position,genome["walkspeed"].value());
            rb.MovePosition(move);
        } else {
            thirst = 0;
        }
        return true;
    }

    bool FixedUpdateFrisky(){
        GameObject target = closestVisibleMate();
        if (target == null){
            return false;
        }
        Omnivore mate = target.GetComponent<Omnivore>();
        if (Vector3.Distance(rb.position,target.transform.position) > size+0.01){
            Vector3 move = Vector3.MoveTowards(rb.position,target.transform.position,genome["walkspeed"].value());
            rb.MovePosition(move);
        } else {
            // Do the nasty
            if (mate.getMode() == "frisky" && this.getSex() == 0) {
                createOffspring(mate);
            }
        }
        return true;
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
        size = 0.1f * genome["size"].value();  // we always start out at 1/10 size
        this.gameObject.transform.localScale = new Vector3(size, size, size);
    }

    public void createOffspring(Omnivore mate) {
        recoveryTime = 10;

        Vector3 offset = new Vector3(0, size, 0);
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
