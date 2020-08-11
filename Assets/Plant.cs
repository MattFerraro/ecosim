using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    // Base prefab to use:
    public GameObject prefab; // this is purely used for reproduction!


    // Variables related to the genome:
    public float maxSize;
    public float growthSpeed;
    public float energyCollectionEfficiency;

    public float restingEnergy;
    public float energyPerSeed;

    public float h;
    public float s;
    public float v;

    // Variables related to the current state of the plant:
    public float energy;
    public float size;

    // public Dictionary<string, Gene> Fields;


    // Update is called once per frame
    void FixedUpdate()
    {
        // First we gotta subtract some energy
        this.energy -= 1.0f * Time.deltaTime;
        if (this.energy <= -1) {
            // Oh no we died!
            GameObject.Destroy(gameObject);
        }

        // Can this plant receive any light?
        Vector3 upVec = new Vector3(0f, .866f, -.5f);
        Vector3 pos = gameObject.transform.position;
        Vector3 leafLocation = new Vector3(pos.x, pos.y + this.size, pos.z);
        if (Physics.Raycast(leafLocation, upVec, 50f)) {
            // We get no energy this time step :(
        }
        else {
            // Harvest some energy!
            this.energy += Time.deltaTime * this.energyCollectionEfficiency;
            // Plants have a growth phase then a reproduction phase and that's it
            if (this.size < maxSize) {
                // In the growth phase, we spend our energy on growth
                this.size += this.energy * growthSpeed;
                // We just grew! gotta update ourselves

                this.gameObject.transform.localScale = new Vector3(this.size, this.size, this.size);
                this.energy = 0;
            }
            else {
                // In the reproduction phase, we just spend our energy on seeds
                if (energy > energyPerSeed + restingEnergy) {
                    createOffspring(energyPerSeed / 10); // there is a 10x overpayment here. Building seeds is expensive!
                    energy -= energyPerSeed;
                }
            }
        }

    }

    void createOffspring(float seedEnergy) {
        Vector3 offset = Random.insideUnitCircle * 10;
        Vector3 offsetXZ = new Vector3(offset.x, 0, offset.y);

        Vector3 pos = gameObject.transform.position + offsetXZ;

        // Check if the position we've selected is valid
        if (Mathf.Abs(pos.x) >= 30 || Mathf.Abs(pos.z) >= 30) {
            return;
        }

        // Unity doesn't let you use meaningful constructors.
        // The way to pass down the genome is explicitely with a setter:
        GameObject go = (GameObject)Instantiate(prefab, pos, Quaternion.identity);
        Plant plt = go.GetComponent<Plant>();
        plt.SetGenome(
            prefab: prefab,
            maxSize: mutate(maxSize),
            growthSpeed: growthSpeed,
            energyCollectionEfficiency: energyCollectionEfficiency,
            inputEnergy: seedEnergy,
            h: fmutate(h),
            s: fmutate(s),
            v: fmutate(v),
            restingEnergy: mutate(restingEnergy),
            energyPerSeed: mutate(energyPerSeed));
    }

    float mutate(float input) {
        float rate = 0.1f;
        return Random.value * rate - rate/2 + input;
    }

    float fmutate(float input) {
        return cap(mutate(input));
    }

    float cap(float input) {
        return Mathf.Clamp(input, 0, 1);
    }

    public void SetGenome(
            GameObject prefab,
            float maxSize,
            float growthSpeed,
            float energyCollectionEfficiency,
            float inputEnergy,
            float h,
            float s,
            float v,
            float restingEnergy,
            float energyPerSeed) {
        this.prefab = prefab;
        this.maxSize = maxSize;
        this.growthSpeed = growthSpeed;
        this.energyCollectionEfficiency = energyCollectionEfficiency;
        this.energy = inputEnergy;
        this.size = 0.01f;
        this.h = h;
        this.s = s;
        this.v = v;
        this.restingEnergy = restingEnergy;
        this.energyPerSeed = energyPerSeed;

        this.gameObject.transform.localScale = new Vector3(size, size, size);

        Transform[] children = gameObject.GetComponentsInChildren<Transform>(true);
        foreach (Transform item in children) {
            if (item.name == "Canopy") {
                //gameObject.GetComponent<Renderer>().material.color = new Color(1,1,1);
                item.gameObject.GetComponent<Renderer>().material.color = Color.HSVToRGB(h, s, v);
            }
        }
    }

    // public void SetGenomeFields(GameObject prefab, Dictionary<string, Gene> newFields) {
    //     this.prefab = prefab;
    //     Fields = newFields;
    // }

}
