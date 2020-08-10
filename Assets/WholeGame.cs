using System.Collections.Generic;
using UnityEngine;


public class WholeGame : MonoBehaviour
{
    public Transform bunny;
    public Transform fox;
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
}
