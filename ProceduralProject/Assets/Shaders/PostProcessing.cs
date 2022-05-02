using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessing : MonoBehaviour
{

    public Shader shader;
    private Material mat;

    public Texture noiseTexture;
    // Start is called before the first frame update
    void Start()
    {
        mat = new Material(shader);

        mat.SetTexture("_NoiseTex", noiseTexture);
    }
    public void UpdateAmp(){

    }

    void Update(){
        mat.SetFloat("_Amp", 12312);
    }
    // Update is called once per frame
    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, mat);
    }
}
