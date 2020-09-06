using System.Collections.Generic;
using UnityEngine;


public class WholeGame : MonoBehaviour
{
    public GameObject bunny;
    public GameObject fox;

    public GameObject plant;
    List<GameObject> bunnies = new List<GameObject>();
    List<GameObject> foxes = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // create starting animals
        bunnies.AddRange(CreateNBunnies(10));
        // foxes.AddRange(CreateNFoxes(2));
        // test cross methods here

        // CreateStartingPlants();
    }

    // Update is called once per frame
    void Update()
    {

    }

    List<GameObject> CreateNBunnies (int n) {
		List<GameObject> result = new List<GameObject>();
        for (int i = 0; i < n; i++ ) {
		    GameObject c = (GameObject)Instantiate(
                bunny, 
                new Vector3(Random.value * 100 - 50, 1, Random.value * 100 - 50),
                Quaternion.identity
            );
            // Herbivore herb = c.GetComponent<Herbivore>();
            Omnivore omni = c.GetComponent<Omnivore>();
            // gene dictionary initialize!
            Dictionary<string, Gene> genome = new Dictionary<string, Gene>(){
                {"h", new Gene(Random.value)},
                {"s", new Gene(1)},
                {"v", new Gene(1)},
            };
            omni.SetGenome(genome);
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
        GameObject go = (GameObject)Instantiate(plant, new Vector3(4f, 0, 4f), Quaternion.identity);
        Plant plt = go.GetComponent<Plant>();
        plt.SetGenome(
            prefab: plant,
            maxSize: 7,
            growthSpeed: .2f,
            energyCollectionEfficiency: 3f,
            inputEnergy: 2,
            h: .05f,
            s: .9f,
            v: .8f,
            restingEnergy: 5,
            energyPerSeed: 20);

        // Medium sized tree
        go = (GameObject)Instantiate(plant, new Vector3(4f, 0, -4f), Quaternion.identity);
        plt = go.GetComponent<Plant>();
        plt.SetGenome(
            prefab: plant,
            maxSize: 3,
            growthSpeed: .2f,
            energyCollectionEfficiency: 3f,
            inputEnergy: 1,
            h: .27f,
            s: .9f,
            v: .5f,
            restingEnergy: 5,
            energyPerSeed: 20); // offspring starts at size 20/10 = 2


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
