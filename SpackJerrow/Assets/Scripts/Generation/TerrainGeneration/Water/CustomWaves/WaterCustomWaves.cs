using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCustomWaves : DefaultWater
{

    public List<CustomWave> currentWaves = new List<CustomWave>();

    [Tooltip("The sea depth determines how waves will behave/ break in water")]
    public AnimationCurve seaDepthOverLength;

    [Tooltip("The sea depth determines how waves will behave/ break in water")]
    public AnimationCurve seaDepthOverWidth;

    [Tooltip("Multiplied with the seaDeapthOverLengthCurve to get the current seadepth")]
    [Range(0.01f, 100f)]
    public float depthFactor;

    private float[][] seaDepth;

    float[] newHeights;
    
    protected override void MoveWater()
    {
        CalculateWaterSurface();
        base.MoveWater();
    }

    protected virtual void CalculateWaterSurface()
    {
        newHeights = new float[VerticePoints];
        foreach (CustomWave w in currentWaves)
        {
            w.ApplyWave(newHeights, this, seaDepth, Time.fixedUnscaledDeltaTime);
        }
    }

    protected override void ShowTerrain(bool updateMesh)
    {
        CalculateChanges();
        base.ShowTerrain(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            currentWaves.Add(new LinearWave(new Vector2(1,0)) { startPosition  = new Vector2(0,50), amplitude = 7, currentPosition = new Vector2(0,0), speed = 13});
        }
    }

    protected override float GetCurrentY(int x, int z)
    {
        return base.GetCurrentY(x, z);
    }

    public override float EvaluatePlainHeight(Vector2 progress)
    {
        return newHeights[GetNearestIndex(progress)];
    }

    private void CalculateChanges()
    {
        seaDepth = new float[XSize][];
        for (int x = 0; x < XSize; x++)
        {
            seaDepth[x] = new float[ZSize];
            for (int z = 0; z < ZSize; z++)
            {
                seaDepth[x][z] = seaDepthOverLength.Evaluate(z) * depthFactor * seaDepthOverWidth.Evaluate(x);
            }
        }
        CalculateWaterSurface();
    }

    private void Awake()
    {
        CalculateChanges();
    }

}
