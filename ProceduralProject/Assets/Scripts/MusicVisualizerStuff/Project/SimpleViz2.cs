using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(LineRenderer))]
public class SimpleViz2 : MonoBehaviour
{

    static public SimpleViz2 viz2 { get; private set;}
    public float ringHieght = 4;
    public float ringRadius = 50;
    public int numBands = 512;
    public float avgAmp = 0;
    private AudioSource player;
    private LineRenderer line;
    public PostProcessing ppShader;
    void Start()
    {

        viz2 = this;
        player = GetComponent<AudioSource>();
        line =  GetComponent<LineRenderer>();

    }

    void OnDestroy(){
        if(viz2 == this) viz2 = null;
    }
    
    // Update is called once per frame
    void Update()
    {
        UpdateWaveform();
        UpdateFreqBands();
    }
    private void UpdateFreqBands(){

        float[] bands = new float[numBands];
        player.GetSpectrumData(bands, 0, FFTWindow.BlackmanHarris);
    }

    private void UpdateWaveform(){
        int samples = 1024;
        float[] data = new float[samples];
        player.GetOutputData(data, 0);

        Vector3[] points = new Vector3[samples];

        avgAmp =0;

        for (int i = 0; i < data.Length; i++)
        {
            float sample = data[i];

            avgAmp += data[i];

            float rads = Mathf.PI * 2 * i / samples;

            float x = Mathf.Cos(rads) * ringRadius;
            float z = Mathf.Sin(rads) * ringRadius;

            float y = sample * ringHieght;

            points[i] = new Vector3(x,y,z);

            line.positionCount = points.Length;
            line.SetPositions(points);
        }

        avgAmp /= samples;

        //ppShader.UpdateAmp();

        line.SetPositions(points);
    }
}
