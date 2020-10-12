using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class Island : OctavesColourGenerator
{

    public enum IslandShape {Simple, Complex };

    [Save]
    public IslandShape shape;

    [Save]
    private Dictionary<Serializable2DVector, float> modifiedHeights = new Dictionary<Serializable2DVector, float>();

    protected override void GenerateHeightMap()
    {
        base.GenerateHeightMap();
        foreach(KeyValuePair<Serializable2DVector, float> pair in modifiedHeights)
        {
            noiseMap[(int)pair.Key.v.x][(int)pair.Key.v.y] = pair.Value;
        }
    }

    protected override float GetHeightMultiplierForProgress(float x, float z)
    {
        float result = 0;
        if (shape == IslandShape.Simple)
        {
            result = 1 - Mathf.Clamp(2f - (Mathf.Sin(x * Mathf.PI) + Mathf.Sin(z * Mathf.PI)), 0, 1);
        }
        else if(shape == IslandShape.Complex)
        {
            float xFactor = Mathf.Sin(seed);
            float zFactor = Mathf.Cos(seed);
            result = 1 - Mathf.Clamp01(2f - (Mathf.Sin(x * Mathf.PI) + Mathf.Sin(z * Mathf.PI)));
            result = Mathf.Clamp01(result * 2);

        }
        return result;
    }

    protected override float GetPerlinNoiseAt(float x, float z)
    {
        return Mathf.PerlinNoise(x, z)/* * 2 -1 */;
    }
    
    public bool Dig(Vector3 position, int digSize, float digPower, float spreadFactor, float throwPower, float heapSize, Vector3 digDirection)
    {
        Vector2 progress = ToLocalRotatedProgress(position);
        Color diggedTerrainColor = default;
        float newHeight;
        IndexInfo[] digIndices = GetCircleIndiciesAroundPosition(ShapeVec(position), digSize);
        for (int x = 0; x < digSize * 2; x++)
        {
            for (int y = 0; y < digSize * 2; y++)
            {
                IndexInfo i = digIndices[x * digSize * 2 + y];
                if (i != null) {
                    float distance = Mathf.Sqrt(Mathf.Pow(digSize - x, 2) + Mathf.Pow(digSize - y, 2));
                    diggedTerrainColor = ColorData[i.index];
                    newHeight = Vertices[i.index].y - digPower * (Mathf.Cos((i.DistanceToOriginal / digSize) * Mathf.PI / 2));
                    Vertices[i.index].y = newHeight;
                    ColorData[i.index] = GetColorAt(0, 0, Vertices[i.index].y);
                    modifiedHeights[new Serializable2DVector(i.xIndex, i.zIndex / VerticesXCount)] = newHeight;
                }
            }
        }

        //foreach (IndexInfo i in /* GetNearestIndicies(progress, false)*/)
        //{
        //    if (i != null)
        //    {
        //        diggedTerrainColor = colorData[i.index];
        //        newHeight = vertices[i.index].y - digPower * (1 - Mathf.Min(1, i.DistanceToOriginal / spreadFactor));
        //        vertices[i.index].y = newHeight;
        //        colorData[i.index] = GetColorAt(0, 0, vertices[i.index].y);
        //        modifiedHeights[new Serializable2DVector(i.xIndex,i.zIndex / VerticesXCount)] = newHeight;
        //    }
        //}

        //Vector2 heapProgress = ToLocalRotatedProgress(position - digDirection * Random.Range(throwPower, throwPower + 4));
        //foreach (IndexInfo i in GetNearestIndicies(heapProgress, false))
        //{
        //    if (i != null)
        //    {
        //        newHeight = vertices[i.index].y + digPower / 5 * heapSize;
        //        vertices[i.index].y = newHeight;
        //        colorData[i.index] = diggedTerrainColor;
        //        modifiedHeights[new Serializable2DVector(i.xIndex, i.zIndex / VerticesXCount)] = newHeight;
        //        //colorData[i.index] = GetColorAt(0, 0, vertices[i.index].y);
        //    }
        //}

        UpdateMeshWithCollider();
        return true;
    }

}
