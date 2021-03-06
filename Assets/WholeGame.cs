﻿using System.Collections.Generic;
using UnityEngine;


public class WholeGame : MonoBehaviour
{
    public GameObject bunny;
    public GameObject fox;
    public GameObject omnivore;

    public GameObject plant;
    List<GameObject> bunnies = new List<GameObject>();
    List<GameObject> foxes = new List<GameObject>();

    List<GameObject> omnivores = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // create starting animals
        omnivores.AddRange(CreateNOmnivores(10));
        // foxes.AddRange(CreateNFoxes(2));
        // test cross methods here

        CreateStartingPlants();
    }

    // Update is called once per frame
    void Update()
    {

    }

    List<GameObject> CreateNOmnivores (int n) {
		List<GameObject> result = new List<GameObject>();
        for (int i = 0; i < n; i++ ) {
		    GameObject c = (GameObject)Instantiate(
                omnivore,
                new Vector3(Random.value * 100 - 50, 1, Random.value * 100 - 50),
                Quaternion.identity
            );
            Omnivore omni = c.GetComponent<Omnivore>();
            // gene dictionary initialize!
            Dictionary<string, Gene> genome = new Dictionary<string, Gene>(){
                {"h", new Gene(i % 2 == 0 ? 0: 0.8f)},
                {"s", new Gene(1)},
                {"v", new Gene(1)},
                {"walkspeed", new Gene(0.03f)},
                {"size", new Gene(0.05f, 10)},
                {"ageToReproduce", new Gene(0.1f, 100)},
                {"sex", new Gene(i % 2 == 0 ? 0: 1, 1, "binary")},
                {"id", new Gene(i / 65000.0f)},
                {"sightRange", new Gene(1.0f,100f)}
            };
            omni.SetGenome(omnivore, genome);
            result.Add(c);
        }
        return result;
	}

    // List<GameObject> CreateNFoxes (int n) {
	// 	List<GameObject> result = new List<GameObject>();
    //     for (int i = 0; i < n; i++ ) {
	// 	    GameObject c = (GameObject)Instantiate(
    //             fox,
    //             new Vector3(Random.value * 100 - 50, 1, Random.value * 100 - 50),
    //             Quaternion.identity
    //         );
    //         //Carnivore carn = c.GetComponent<Carnivore>();
    //         Omnivore omni = c.GetComponent<Omnivore>();
    //         result.Add(c);
    //     }
    //     return result;
	// }

    void CreateStartingPlants() {
        // Let's make a big tree in the center of the place Also it is red!
        GameObject go = (GameObject)Instantiate(plant, new Vector3(0f, 0, 0f), Quaternion.identity);
        Plant plt = go.GetComponent<Plant>();
        plt.SetGenome(
            prefab: plant,
            maxSize: 7,
            growthSpeed: .3f,
            energyCollectionEfficiency: 2f,
            inputEnergy: 2,
            h: .05f,
            s: 1,
            v: 1,
            restingEnergy: 5,
            energyPerSeed: 20);


        go = (GameObject)Instantiate(plant, new Vector3(25f, 0, 0f), Quaternion.identity);
        plt = go.GetComponent<Plant>();
        plt.SetGenome(
            prefab: plant,
            maxSize: 6.35f,
            growthSpeed: .3021f,
            energyCollectionEfficiency: 2.3032f,
            inputEnergy: 2,
            h: .05f,
            s: 1,
            v: 1,
            restingEnergy: 5,
            energyPerSeed: 20);

        go = (GameObject)Instantiate(plant, new Vector3(0, 0, 25), Quaternion.identity);
        plt = go.GetComponent<Plant>();
        plt.SetGenome(
            prefab: plant,
            maxSize: 7.2f,
            growthSpeed: .312f,
            energyCollectionEfficiency: 2.012f,
            inputEnergy: 2,
            h: .05f,
            s: 1,
            v: 1,
            restingEnergy: 5,
            energyPerSeed: 20);

        // // Medium sized tree
        // go = (GameObject)Instantiate(plant, new Vector3(4f, 0, -4f), Quaternion.identity);
        // plt = go.GetComponent<Plant>();
        // plt.SetGenome(
        //     prefab: plant,
        //     maxSize: 3,
        //     growthSpeed: .3f,
        //     energyCollectionEfficiency: 2f,
        //     inputEnergy: 1,
        //     h: .27f,
        //     s: .9f,
        //     v: .5f,
        //     restingEnergy: 5,
        //     energyPerSeed: 20); // offspring starts at size 20/10 = 2


        // Let's make some small shrubs
        // GameObject go2 = (GameObject)Instantiate(plant, new Vector3(-4f, 0, -4f), Quaternion.identity);
        // Plant plt2 = go2.GetComponent<Plant>();
        // plt2.SetGenome(
        //     prefab: plant,
        //     maxSize: .5f,
        //     growthSpeed: .1f,
        //     energyCollectionEfficiency: 2.0f,
        //     inputEnergy: 0.1f,
        //     h: .3f,
        //     s: .9f,
        //     v: .9f,
        //     restingEnergy: 1,
        //     energyPerSeed: 2);

    }
}
