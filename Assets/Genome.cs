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
    // color genes
    public Gene h;
    public Gene s;
    public Gene v;

    public PlantGenome() {

    }
}

public class AnimalGenome {
    public Gene maxSize;
    public Gene pace;
    public Gene jump;
    public Gene digestionEfficiency;
    // color genes
    public Gene h;
    public Gene s;
    public Gene v;
    public AnimalGenome() {

    }
    public static void cross(AnimalGenome a, AnimalGenome b) {
        AnimalGenome result = new AnimalGenome();
        // property checker
        FieldInfo[] fieldInfos = typeof(AnimalGenome).GetFields(BindingFlags.Public);
        foreach (FieldInfo fi in fieldInfos) {
            Debug.Log("AG FieldName: " + fi.Name);
        }
        
    }
}

public class PredatorGenome : AnimalGenome {
    //public diet;, list of prey?
    //
    public Gene preySize;
}


/* 
any gene has a value between 0 and 1,
and a scaling factor (default 1)
and crossing two genes may include a variance factor (default 0)

*/

public class Gene {
    public float value;
    public float scale;
    public Gene(float value, float scale = 1) {
        this.value = value;
        this.scale = scale;
    }

    // currently Rectangular distribution of width this.variance, prefer triangular?
    // keep the same scale for now
    public Gene mutate(Gene original, float variance){
        float result_value = original.value + variance*(Random.value-0.5f);
        result_value = Mathf.Clamp(result_value,0,1);
        return new Gene(result_value,original.scale);
    }
    public Gene cross(Gene a, Gene b, float variance = 0){
        return new Gene((a.value + b.value)/2, (a.scale+b.scale)/2);

    }
    public Gene mendelian<Gene>(Gene a, Gene b){
        throw new System.NotImplementedException();
    }
}
