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
        for (int i = 0; i < 10; i++ ) {

            bunnies.Add(CreateBunny());
        }
        foxes.Add(CreateFox());

        CreatePlant();
    }

    // Update is called once per frame
    void Update()
    {

    }

    Transform CreateBunny () {
		Transform t = Instantiate(bunny);
		t.localPosition = new Vector3(Random.value * 100 - 50, 1, Random.value * 100 - 50);
        return t;
	}

    Transform CreateFox () {
		Transform t = Instantiate(fox);
		t.localPosition = new Vector3(0f,3f,0f);// Random.value * 100 - 50, 3, Random.value * 100 - 50);
        return t;
	}

    GameObject CreatePlant() {
        GameObject go = (GameObject)Instantiate(plant, new Vector3(-2f, 0, -2f), Quaternion.identity);
        Plant plt = go.GetComponent<Plant>();
        plt.SetGenome(plant, 3, .3f, 1, 1);
        return go;
    }
}
