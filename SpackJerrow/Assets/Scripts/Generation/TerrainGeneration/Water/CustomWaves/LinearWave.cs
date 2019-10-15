using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearWave : CustomWave
{

    public LinearWave(Vector2 direction)
    {
        this.direction = direction.normalized;
    }
    

    protected override void ApplyWave(float[] newSeaVertices, TerrainGenerator form, float[][] seaLevel)
    {
        int ZSize = form.GetZSize();
        int XSize = form.GetXSize();
        Vector2 waveLine = (currentPosition - startPosition);
        int waveSourceLength = (int)waveLine.magnitude;
        waveLine.Normalize();

        Vector2 waveStartPos = startPosition + direction * timeAlive * speed;
        
        float waveSize = amplitude * AMPLITUDE_WIDTH;

        for (int x = 0; x < waveSourceLength; x++)
        {
            Vector2 currentPosition = waveStartPos + waveLine * x;
            for (int i = 0; i < waveSize; i++)
            {

                IndexInfo[] points = form.GetNearestIndicies(form.LocalPosToProgress(currentPosition), false);

                foreach(IndexInfo index in points)
                {
                    if(index != null && index.index >= 0 && index.index < newSeaVertices.Length)
                    {
                        float waveProgress = (i + Vector2.Scale(direction, index.diffToOriginal).magnitude) / waveSize;
                        newSeaVertices[index.index] += Mathf.Sin(Mathf.PI /** 2*/ * waveProgress) * amplitude * (1-index.DistanceToOriginal);
                    }
                }
                currentPosition += direction;
            }
        }
    }
    
    public virtual float TimeAliveForPosition(Vector2 pos)
    {
        return timeAlive;
    }

    public Vector2 direction;

    public Vector2 currentPosition;
    
}
