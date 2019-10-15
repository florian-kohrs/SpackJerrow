using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public abstract class MeshBuilder<T> : SaveableMonoBehaviour, IMeshInfo
{

    private float maxTriangleDistance;

    public float MaxTriangleDistance
    {
        get
        {
            if (maxTriangleDistance == 0)
            {
                maxTriangleDistance = Mathf.Sqrt(2);
            }
            return maxTriangleDistance;
        }
    }

    private MeshFilter filter;
    
    #region abstract members
    ///defining the size and form of the landscape must be defined in the 
    ///deriving class
    protected abstract Vector3 GetCurrentVertexPosition(int x, int z);

    protected abstract int XSize { get; }

    protected abstract int ZSize { get; }

    protected abstract float GetCurrentY(int x, int z);

    protected abstract T GetColorAt(float xProgress, float zProgress, float height);

    #endregion

    protected Mesh mesh;

    protected Vector3[] vertices;

    public Vector3[] Vertices => vertices;

    protected T[] colorData;
    protected int[] trianglePoints;
    
    protected int VerticesXCount
    {
        get
        {
            return XSize + 1;
        }
    }

    protected int VerticesZCount
    {
        get
        {
            return ZSize + 1;
        }
    }

    protected int VerticePoints
    {
        get
        {
            return VerticesXCount * VerticesZCount;
        }
    }

    protected int TriangleCount 
    {
        get
        {
            return ZSize * XSize * 2;
        }
    }

    private const int POINTS_PER_TRIANGLE = 3;
    
    protected virtual void Initialize()
    {
        mesh = new Mesh();
        CreateShape(out vertices);
        DrawCurrentVertices();
        BuildUvs();
        UpdateMesh();
        MeshFilter.mesh = mesh;
    }

    private MeshFilter meshFilter;

    protected MeshFilter MeshFilter
    {
        get
        {
            if(meshFilter == null)
            {
                meshFilter = GetComponent<MeshFilter>();
            }
            return meshFilter;
        }
    }

    private SMeshRenderer meshRenderer;

    protected SMeshRenderer MeshRenderer
    {
        get
        {
            if (meshRenderer == null)
            {
                meshRenderer = GetComponent<SMeshRenderer>();
            }
            return meshRenderer;
        }
    }

    protected void UpdateVertices()
    {
        mesh.vertices = vertices;
        ///may result in worse graphic
        mesh.RecalculateNormals();
    }

    protected virtual void DisplayTexture() { }
    
    protected virtual void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = trianglePoints;
        DisplayTexture();

        mesh.RecalculateNormals();
    }

    protected void CreateDefaultShape()
    {
        CreateShape(out vertices);
    }

    protected void DisplayVertexChanges()
    {
        UpdateVertices();
        mesh.RecalculateNormals();
    }

    protected void CreateShape(out Vector3[] shape)
    {
        shape = new Vector3[VerticePoints];

        int currentPointIndex = 0;
        for (int z = 0; z < VerticesZCount; z++)
        {
            for (int x = 0; x < VerticesXCount; x++)
            {
                shape[currentPointIndex] = GetCurrentVertexPosition(x,z);
                currentPointIndex++;
            }
        }
    }

    protected void ModifyShape(ref Vector3[] shape, int startXIndex, int startZIndex, int size)
    {
        for (int z = Mathf.Max(0,startZIndex); z < VerticesZCount && z < startZIndex + size; z++)
        {
            for (int x = Mathf.Max(0, startXIndex); x < VerticesXCount && x < startXIndex + size; x++)
            {
                shape[z * VerticesXCount + x].y = GetCurrentVertexPosition(x, z).y;
            }
        }
        DisplayVertexChanges();
    }
    
    protected void DrawCurrentVertices()
    {
        trianglePoints = new int[TriangleCount * POINTS_PER_TRIANGLE];

        int currentPointIndex = 0;
        int finishedRectangleCount = 0;
        for (int z = 0; z < ZSize; z++)
        {
            for (int x = 0; x < XSize; x++)
            {
                DrawRectangle(ref currentPointIndex, ref finishedRectangleCount);
            }
            ///skip one rectangle so no connection is build
            ///from the most right to the most left node
            finishedRectangleCount++;
        }
    }
    
    /// <summary>
    /// will draw the next 2 triangles to finish the rectangle
    /// </summary>
    private void DrawRectangle(ref int currentPointIndex, ref int finishedRectangle)
    {
        trianglePoints[currentPointIndex++] = finishedRectangle;
        trianglePoints[currentPointIndex++] = finishedRectangle + VerticesXCount;
        trianglePoints[currentPointIndex++] = finishedRectangle + 1;
        trianglePoints[currentPointIndex++] = finishedRectangle + 1;
        trianglePoints[currentPointIndex++] = finishedRectangle + VerticesXCount;
        trianglePoints[currentPointIndex++] = finishedRectangle + VerticesXCount + 1;
        finishedRectangle++;
    }

    protected void BuildUvs()
    {
        colorData = new T[vertices.Length];

        int index = 0;
        for (int z = 0; z < VerticesZCount; z++)
        {
            for (int x = 0; x < VerticesXCount; x++)
            {
                colorData[index] = GetColorAt((float)x / (XSize - 1), (float)z / (ZSize - 1), vertices[index].y);
                index++;
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    if (vertices != null)
    //    {
    //        for (int i = 0; i < vertices.Length; i++)
    //        {
    //            Gizmos.DrawSphere(vertices[i], 0.1f);
    //        }
    //    }
    //}

}
