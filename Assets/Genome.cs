using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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

public class AnimalGenome {
    public Gene maxSize;
    public Gene pace;
    public Gene digestionEfficiency;
    AnimalGenome() {

    }
    void breed(AnimalGenome a, AnimalGenome b) {
        AnimalGenome result = new AnimalGenome();
        // property checker
        FieldInfo[] fieldInfos = typeof(AnimalGenome).GetFields(BindingFlags.Public);
        foreach (FieldInfo fi in fieldInfos) {
            Debug.Log("AG FieldName: " + fi.Name);
        }
        
    }
}

public class PredatorGenome {
    public AnimalGenome animal;
    //public diet;, list of prey?
    public Gene preySize;
}


public class Gene {
    float min;
    float max;
    Gene(float min, float max) {
        this.min = min;
        this.max = max;
    }
}
