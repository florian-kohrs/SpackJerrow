using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Water doesn`t go through surface, waves are rising at shore
/// </summary>
public class WaveWater : DefaultWater
{

    /// <summary>
    /// stores for each vertice of the water the depth at that point.
    /// </summary>
    public float[][] seaDepth;

    public override void OnAwake()
    {
        seaDepth = new float[VerticesXCount][];
        for (int x = 0; x < VerticesXCount; x++)
        {
            seaDepth[x] = new float[VerticesZCount];
            for (int z = 0; z < VerticesZCount; z++)
            {
                seaDepth[x][z] = -100;
            }
        }
    }
    
    public void RegisterWaterCollider(Vector3 globalPosition)
    {
        Vector2 progress;
        if (TryGlobalPosToProgress(globalPosition, out progress))
        {
            Vector2Int indicies = ProgressToXAndZIndex(progress);
            seaDepth[indicies.x][indicies.y] = globalPosition.y;
        }
    }
    
    protected override float GetCurrentY(int x, int z)
    {
        float plainHeight = BuildPlainHeightOnIndex(x, z);
        float heightFactor = GetWaveHeightMultiplier(x, z, plainHeight);
        float noiseHeight = GetPerlinNoise(x, z);
        plainHeight *= heightFactor;
        if(heightFactor != 0)
        {
            noiseHeight /= heightFactor;
        }
        else
        {
            noiseHeight = 0;
        }
        float result = plainHeight + noiseHeight;

        return plainHeight + noiseHeight;
    }

    public float GetWaveHeightMultiplier(int x, int z, float height)
    {
        float result = 1;
        if (seaDepth != null && height > 0)
        {
            float seaDepthAtPosition = seaDepth[x][z] - SeaLevel;
            if (seaDepthAtPosition > waveAmplitude)
            {
                result = 0f;
            }
            else
            {
                float diff = (height + seaDepthAtPosition);
                if (diff > 0)
                {
                    if (diff < waveAmplitude / 2)
                    {
                        result = Mathf.Min(1 + diff, waveAmplitude * 2);
                    }
                    else
                    {
                        ///wave breaks
                        //temp
                        result = 0;
                    }
                }
            }
        }
        return result;
    }
    

}
