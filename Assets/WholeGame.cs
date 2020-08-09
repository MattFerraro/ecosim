using UnityEngine;

public class WholeGame : MonoBehaviour
{
    public Transform bunny;
    public Transform fox;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++ ) {

            CreateBunny();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreateBunny () {
		Transform t = Instantiate(bunny);
		t.localPosition = new Vector3(Random.value * 100 - 50, 3, Random.value * 100 - 50);

	}
}
