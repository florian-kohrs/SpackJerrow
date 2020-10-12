using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DefaultWater : TerrainGenerator, ITerrainHeightEvaluator, IFloatingController
{

    public Transform center;

    public const float WATER_DENSITY = 1000;

    [Range(0.1f, 10)]
    [Save]
    public float waterSurfaceChangeSpeed = 1f;

    [Range(0.05f, 20)]
    [Save]
    public float waveTravelSpeed = 1f;

    [Range(0, 50)]
    [Save]
    public float waveAmplitude;

    [Tooltip("The amount of waves per 100 Units")]
    [Range(0.1f, 10)]
    [Save]
    public float waveFrequenzy = 4;

    [Range(1,300)]
    [Save]
    public int calculatedWaterSize = 100;

    [Tooltip("All sinking objects will sink until they reached the seaFloor")]
    public Transform seaFloor;

    public IWaterCallbackReceiver CallbackReceiver { get; set; }

    private new void Start()
    {
        base.Start();
        if(center == null)
        {
            center = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private const int repeatSize = 100;

    private float WaveTravelSpeed
    {
        get
        {
            return waveTravelSpeed / 50;
        }
    }

    [Save]
    private float waterDistance;

    protected override ITerrainHeightEvaluator HeightEvaluater
    {
        get { return this; }
    }

    protected void Update()
    {
        if (GameManager.GameIsNotFrozen)
        {
            MoveWater();
        }
    }

    private new void OnValidate()
    {
        ShowTerrain(false);
    }

    private Transform Center => GameManager.Player.transform;

    protected virtual void MoveWater()
    {
        noiseSeed += waterSurfaceChangeSpeed * Time.deltaTime;
        waterDistance += WaveTravelSpeed * Time.deltaTime;

        Vector2 progress = ToLocalProgress(Center.position);

        Vector2 startPosition = ProgressToXAndZIndex(progress);

        int calculatedWaterSize = PersistentGameDataController.Settings.waterCalculationSize;
        
        ModifyShape(ref BaseBuilder.vertices, Mathf.RoundToInt(startPosition.x - (calculatedWaterSize / 2)), Mathf.RoundToInt(startPosition.y - (calculatedWaterSize / 2)), calculatedWaterSize);

        //ReshapeY(ref vertices);

        if (CallbackReceiver != null)
        {
            CallbackReceiver.OnWaterChange();
        }
    }

    private float Frequenzy(float x)
    {
        return (x + waterDistance) * waveFrequenzy * Mathf.PI * (terrainLength / (float)repeatSize) * 2;
    }

    public WaterFloatInfo GetObjectFloatInfo(WaterRigidBody t)
    {
        
        WaterFloatInfo result = new WaterFloatInfo();
        try
        {
            Vector3 targetPosition = t.CurrentPosition;

            Vector3 localPosition = TransformToAnkorPosition(targetPosition);

            Vector3 objectForward = t.transform.forward;

            Vector3 objectRight = t.transform.right;

            float stackedHeight = 0;

            int xLength = (int)(t.maxLength * t.floatAccuracy + 1);

            ///move object in rotate direction
            Vector3 startPosition = localPosition + (objectForward * -1 * (t.maxLength / 2) + objectRight * -1 * (t.maxWidth / 2));

            float[][] heightGraph = new float[(int)(t.maxWidth * t.floatAccuracy + 1)][];

            Vector3 currentPosition = startPosition;

            Vector3 current2DPosition;

            int waveHeightCounter = 0;

            int totalSize = heightGraph.Length * XSize;
            float pointPercentage = 1f / totalSize;

            float objectStackedHeight = 0;
            float pointsInWater = 0;
            float maxPointsInWater = 0;

            for (int x = 0; x < heightGraph.Length; x++)
            {
                heightGraph[x] = new float[xLength];
                currentPosition = startPosition + objectRight * x;
                for (int z = 0; z < heightGraph[x].Length; z++)
                {
                    current2DPosition = new Vector2(currentPosition.x, currentPosition.z);
                    Vector2 progress = ToProgress(current2DPosition);
                    if (progress.x > 0 || progress.x < 1 || progress.y > 0 || progress.y < 1)
                    {
                        float waveHeight;
                        switch (t.floatPrecision)
                        {
                            case (WaterRigidBody.FloatHeightPrecision.Approximately):
                                {
                                    int index = GetNearestIndex(progress);
                                    if (index >= 0)
                                    {
                                        waveHeight = Vertices[index].y + transform.position.y;
                                    }
                                    else
                                    {
                                        waveHeight = 0;
                                        //abseits von wasser
                                    }
                                    break;
                                }
                            case (WaterRigidBody.FloatHeightPrecision.Exact):
                                {
                                    waveHeight = GetAccurateLocalHeightOnProgress(progress);
                                    break;
                                }
                            default:
                                {
                                    waveHeight = 0;
                                    Debug.LogWarning("Unkown float precision: " + t.floatPrecision);
                                    break;
                                }
                        }
                        if (waveHeight/* + transform.position.y*/ > currentPosition.y || t.floatBehaviour == WaterRigidBody.FloatBehaviour.OnPoint)
                        {
                            stackedHeight += waveHeight;
                            objectStackedHeight += currentPosition.y;
                            heightGraph[x][z] = waveHeight;
                            waveHeightCounter++;
                            pointsInWater++;
                        }
                        maxPointsInWater++;
                    }

                    currentPosition += objectForward;

                }
            }
            result.sizePercentageInWater = pointsInWater / maxPointsInWater;
            if (result.sizePercentageInWater > 0)
            {
                stackedHeight /= waveHeightCounter;

                objectStackedHeight /= waveHeightCounter;

                float densityDifference = (t.Mass / t.volume) / WATER_DENSITY;
                result.densityDifferenceFactor = densityDifference;

                float currentHeightPercentageInWater = Mathf.Clamp((stackedHeight - objectStackedHeight) / t.maxHeight, 0, 1);
                if (currentHeightPercentageInWater == 0)
                {
                    result.densityDifferenceFactor = 0;
                }
                else if (currentHeightPercentageInWater == 1)
                {
                    result.densityDifferenceFactor = densityDifference;
                }
                else
                {
                    float currentVolumeInWater = t.volumeDistribution.Integrate(0, currentHeightPercentageInWater, t.integrationPrecision);
                    result.densityDifferenceFactor = (t.Mass / (currentVolumeInWater * t.volume)) / WATER_DENSITY;
                }

                float heightPercentageInWater = t.volumeDistribution.IntegrateUntil(0, densityDifference);

                result.heightPercentageInWater = heightPercentageInWater;

                result.height = stackedHeight + (((t.maxHeight / 2) - t.maxHeight * heightPercentageInWater) + t.offset.y);

                float frequenzy = Frequenzy(ToZProgress(localPosition.z));

                float degreeRotation = Mathf.Cos(frequenzy) * waveAmplitude * (waveFrequenzy / (1 / 3f));

                float objectArea = t.maxWidth * t.maxLength;

                objectForward.y = 0;
                objectForward.Normalize();

                ///calculate the rotation for x and z axis. Use z direction axis as x rotation axis (both ways)
                result.newEulerAngle = new Vector3(-degreeRotation * objectForward.z, t.transform.eulerAngles.y, -degreeRotation * objectForward.x);
            }
        }
        catch
        {

        }
        return result;
    }

    public virtual float EvaluatePlainHeight(Vector2 progress)
    {
        return Mathf.Sin(Frequenzy(progress.y)) * waveAmplitude;
    }
}
