using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchController : MonoBehaviour
{
    public Material material;
    public float Setting1;
    public float Setting2;
    public float Setting3;
    // Start is called before the first frame update
    public void StartGlitch(){
        material.SetFloat("_Test1", 10);
        material.SetFloat("_GrapeAmount", 10);
        material.SetFloat("_GapeAmount2", 10);
    }
    public void StopGlitch(){
        material.SetFloat("_Test1", 0);
        material.SetFloat("_GrapeAmount", 0);
        material.SetFloat("_GapeAmount2", 0);
    }
}
