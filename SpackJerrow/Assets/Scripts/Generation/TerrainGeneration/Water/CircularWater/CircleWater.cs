using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleWater : DefaultWater, ITerrainHeightEvaluator
{
    protected override ITerrainHeightEvaluator HeightEvaluater
    {
        get { return this; }
    }


    //protected override Vector3 GetCenteredMesh()
    //{
    //    return new Vector3(terrainLength / 2, 0, terrainLength / 2);
    //}


    [Range(1, 50)]
    public float radius;
    
    public override float EvaluatePlainHeight(Vector2 progress)
    {
        float result = base.EvaluatePlainHeight(progress);

        result += Mathf.Sin(Mathf.PI * progress.x) * radius + Mathf.Sin(Mathf.PI * progress.y) * radius;

        return result;
    }

    protected override Vector3 TransformToAnkorPosition(Vector3 pos)
    {
        Vector3 result = Vector3.zero;
        float xLength = vertices[vertices.Length - 1].x - vertices[0].x;
        float zLength = vertices[vertices.Length - 1].z - vertices[0].z;

        result.x = pos.x - (transform.position.x - (Offset * (xLength / TerrainWidth)).x);
        result.z = pos.z - (transform.position.z - (Offset * (zLength / terrainLength)).z);

        result.y = pos.y - (EvaluatePlainHeight(new Vector2(0.5f, 0.5f)) - EvaluatePlainHeight(new Vector2(0, 0)));
        return result;
    }

}