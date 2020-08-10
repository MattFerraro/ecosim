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
        this.energy += Time.deltaTime * energyCollectionEfficiency;
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
            }
        }
    }

    void createOffspring() {
        Vector3 offset = Random.insideUnitCircle * 10;
        Vector3 offsetXZ = new Vector3(offset.x, 0, offset.y);

        Vector3 pos = gameObject.transform.position + offsetXZ;

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
