using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class CustomWave
{

    protected const float AMPLITUDE_WIDTH = 10;
    
    protected abstract void ApplyWave(float[] newSeaVertices, TerrainGenerator form, float[][] seaLevel);

    public void ApplyWave(float[] newSeaVertices, TerrainGenerator form, float[][] seaLevel, float deltaTimeSinceLastCall)
    {
        ApplyWave(newSeaVertices, form, seaLevel);
        timeAlive += deltaTimeSinceLastCall;
    }
    
    protected float timeAlive;

    public Vector2 startPosition;
    
    public float speed;

    [Range(0.1f,50)]
    public float amplitude = 1;
    
}
