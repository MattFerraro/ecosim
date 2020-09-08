using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// a genome is just a Dictionary<string,Gene> of named genes.
// Genome is a set of static functions that work on genomes.
// there can be different sets of genes in different genomes.
// if a genome is_compatible with another, you can cross them to get a result genome
public class Genome
{
    public static float distance(Dictionary<string,Gene> a, Dictionary<string,Gene> b){
        //throw System.NotImplementedException();
        return 0;
    }
    public static Dictionary<string,Gene> cross(
            Dictionary<string,Gene> genome_a,
            Dictionary<string,Gene> genome_b
        ){
        // check speciation
        // if (!Genome.is_compatible(genome_a,genome_b)){
        //     return null;
        // }

        var output = new Dictionary<string,Gene>();
        foreach (string name in genome_a.Keys){
            output.Add(name, genome_a[name].cross(genome_b[name]));
        }
        return output;
    }
    static bool is_compatible(Dictionary<string,Gene> a, Dictionary<string,Gene> b){
        // in the future, calculate vector distance / use distance function on Genome class
        return a.Keys == b.Keys && distance(a,b) < 1;
    }

}

// // any organism has a genome, just a dictionary with specific Genes.
// public class PlantGenome {
//     public Gene maxSize;
//     public Gene growthSpeed;
//     public Gene energyCollectionEfficiency;
//     // color genes
//     public Gene h;
//     public Gene s;
//     public Gene v;

//     public PlantGenome() {

//     }
// }

// public class AnimalGenome {
//     public Gene maxSize;
//     public Gene pace;
//     public Gene jump;
//     public Gene digestionEfficiency;
//     // color genes
//     public Gene h;
//     public Gene s;
//     public Gene v;
//     public AnimalGenome() {

//     }
//     public static void cross(AnimalGenome a, AnimalGenome b) {
//         AnimalGenome result = new AnimalGenome();
//         // property checker
//         FieldInfo[] fieldInfos = typeof(AnimalGenome).GetFields(BindingFlags.Public);
//         foreach (FieldInfo fi in fieldInfos) {
//             Debug.Log("AG FieldName: " + fi.Name);
//         }

//     }
// }

// public class PredatorGenome : AnimalGenome {
//     //public diet;, list of prey?
//     //
//     public Gene preySize;
// }


/*
any gene has a value between 0 and 1,
and a scaling factor (default 1)
and crossing two genes may include a variance factor (default 0)

*/

public class Gene {
    private float _value;
    private float _scale;
    private string crossMode;
    public Gene(float value, float scale = 1, string crossMode = "float") {
        this._value = value;
        this._scale = scale;
        this.crossMode = crossMode;
    }
    public float value(){
        return this._value * this._scale;
    }

    // currently Rectangular distribution of width this.variance, prefer triangular?
    // keep the same scale for now
    public Gene mutate(Gene original, float variance){
        float result_value = original._value + variance*(Random.value-0.5f);
        result_value = Mathf.Clamp(result_value,0,1);
        return new Gene(result_value,original._scale);
    }
    public Gene cross(Gene b, float variance = 0){
        if (this.crossMode == "float") {
            return new Gene((this._value + b._value)/2, (this._scale+b._scale)/2);
        } else if (this.crossMode == "binary") {
            float val = Random.value;
            if (val >= 0.5) {
                Debug.Log("Binary crossing Returning: " + this._value);
                return new Gene(this._value, this._scale, "binary");
            } else {
                Debug.Log("Binary Crossing Returning: " + b._value);
                return new Gene(b._value, b._scale, "binary");
            }
        } else {
            throw new System.NotImplementedException();
        }

    }
    public Gene mendelian<Gene>(Gene a, Gene b){
        throw new System.NotImplementedException();
    }
}
