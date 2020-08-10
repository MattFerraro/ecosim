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



    // Variables related to the current state of the plant:
    public float energy;
    public float size;


    // Update is called once per frame
    void Update()
    {
        // First we gotta subtract some energy
        this.energy -= 1.0f * Time.deltaTime;
        if (this.energy <= -1) {
            // Oh no we died!
            Debug.Log("OH GOD");
            GameObject.Destroy(gameObject);
        }

        // Can this plant receive any light?
        Vector3 upVec = new Vector3(0f, .866f, .5f);
        Vector3 pos = gameObject.transform.position;
        Vector3 leafLocation = new Vector3(pos.x, pos.y + this.size, pos.z);
        if (Physics.Raycast(leafLocation, upVec, 10f)) {
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
                if (this.energy > 2) {
                    createOffspring();
                    this.energy -= 2;
                    // GameObject.Destroy(gameObject);
                }
            }
        }

    }

    void createOffspring() {
        Vector3 offset = Random.insideUnitCircle * 10;
        Vector3 offsetXZ = new Vector3(offset.x, 0, offset.y);

        Vector3 pos = gameObject.transform.position + offsetXZ;

        // Check if the position we've selected is valid
        if (Mathf.Abs(pos.x) >= 10 || Mathf.Abs(pos.z) >= 10) {
            return;
        }


        // Unity doesn't let you use meaningful constructors.
        // The way to pass down the genome is explicitely with a setter:
        GameObject go = (GameObject)Instantiate(prefab, pos, Quaternion.identity);
        Plant plt = go.GetComponent<Plant>();
        plt.SetGenome(prefab, this.maxSize, this.growthSpeed, this.energyCollectionEfficiency, 2);
    }

    public void SetGenome(GameObject prefab, float maxSize, float growthSpeed, float energyCollectionEfficiency, float inputEnergy) {
        this.prefab = prefab;
        this.maxSize = maxSize;
        this.growthSpeed = growthSpeed;
        this.energyCollectionEfficiency = energyCollectionEfficiency;
        this.energy = inputEnergy;
        this.size = 0.01f;
        this.gameObject.transform.localScale = new Vector3(this.size, this.size, this.size);
    }
}
