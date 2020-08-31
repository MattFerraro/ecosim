using System.Collections.Generic;
using UnityEngine;


public class WholeGame : MonoBehaviour
{
    // TODO: convert these Transforms to GameObjects like plant
    public Transform bunny;
    public Transform fox;

    public GameObject plant;
    List<Transform> bunnies = new List<Transform>();
    List<Transform> foxes = new List<Transform>();

    // List<List<Transform>> animals;

    // Start is called before the first frame update
    void Start()
    {
        // CreateStartingBunnies();
        bunnies.AddRange(CreateNBunnies(10));
        foxes.AddRange(CreateNFoxes(2));

        CreateStartingPlants();
    }

    // Update is called once per frame
    void Update()
    {

    }

    List<Transform> CreateNBunnies (int n) {
        List<Transform> result = new List<Transform>();
        for (int i = 0; i < n; i++ ) {
		    Transform t = Instantiate(bunny);
		    t.localPosition = new Vector3(Random.value * 100 - 50, 1, Random.value * 100 - 50);
            result.Add(t);
        }
        return result;
	}

    List<Transform> CreateNFoxes (int n) {
		List<Transform> result = new List<Transform>();
        for (int i = 0; i < n; i++ ) {
		    Transform t = Instantiate(fox);
		    t.localPosition = new Vector3(Random.value * 100 - 50, 1, Random.value * 100 - 50);
            result.Add(t);
        }
        return result;
	}

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
