using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genome
{

}

public class PlantGenome {
    public Gene maxSize;
    public Gene growthSpeed;
    public Gene energyCollectionEfficiency;

    public Gene h;
    public Gene s;
    public Gene v;

    PlantGenome() {

    }
}

public class Gene {
    float min;
    float max;
    Gene(float min, float max) {
        this.min = min;
        this.max = max;
    }
}
