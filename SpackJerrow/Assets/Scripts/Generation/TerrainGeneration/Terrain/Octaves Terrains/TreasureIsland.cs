using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureIsland : Island
{

    [Save]
    private List<ChestInfo> chests = new List<ChestInfo>();

    //[Save]
    //[HideInInspector]
    //public List<Serializable2DVector> treasurePositions;

    //[Save]
    //private List<Serializable2DVector> treasureProgressLocations = new List<Serializable2DVector>();

    //public List<GameObject> fixedChestsTreasure;

    public List<ChestContainer> fixedTreasures;

    public GameObject[] treasures;

    public int treasureCount;

    private const float MIN_DISTANCE_TO_BORDER = 5;

    public Color treasureColor;

    public Color colorblindTreasureColor;

    public int treasureMarkerSize = 5;

    public int treasureMarkerFontWidth = 2;

    private bool wasJustConstructed;

    private bool wasJustValidated;

    //protected override void OnValidate()
    //{
    //    chests.Clear();
    //    SetTreasure();
    //    wasJustValidated = true;
    //    base.OnValidate();
    //}

    protected override void Start()
    {
        if (wasJustValidated || wasJustConstructed)
        {
            SetOffset();
            InitializeTreasure();
        }
        base.Start();
        if (wasJustConstructed)
        {
            wasJustConstructed = false;
            PlaceTreasure();
        }
    }

    protected override void OnFirstTimeBehaviourAwakend()
    {
        wasJustConstructed = true;
    }


    protected override void BehaviourLoaded()
    {
        wasJustValidated = false;
        base.BehaviourLoaded();
    }

    private void InitializeTreasure()
    {
        chests.Clear();
        SetTreasure();
        foreach (GameObject o in treasures)
        {
            o.GetComponent<BurriedChestInteraction>().surface = this;
        }
        foreach (GameObject o in treasures)
        {
            o.GetComponent<BurriedChestInteraction>().surface = this;
        }
    }

    private void SetTreasure()
    {
        Vector2 islandCenter = new Vector2(offset.x, offset.z);
        Vector2 treasureMaxDistance = islandCenter - new Vector2(MIN_DISTANCE_TO_BORDER, MIN_DISTANCE_TO_BORDER);

        for (int i = 0; i < treasureCount; i++)
        {
            ChestInfo chestInfo = new ChestInfo();
            bool randomPosition = ChestAt(i, out chestInfo.chest);
            chestInfo.moveChest = randomPosition;
            chestInfo.markChest = chestInfo.chest == null || chestInfo.chest.isMarked;
            Vector2 treasurePosition = Vector2.zero;
            Vector2 locationProgress = Random.insideUnitCircle;
            if (randomPosition)
            {
                treasurePosition = locationProgress * treasureMaxDistance;

                treasurePosition = new Vector2(Mathf.Round(treasurePosition.x), Mathf.Round(treasurePosition.y));
                locationProgress = ToProgress(treasurePosition + islandCenter);
            }
            else
            {
                treasurePosition = TransformToLocal2DPosition(chestInfo.chest.transform.position);
                locationProgress = ToProgress(treasurePosition + islandCenter);
            }
            
            chestInfo.localPosition = treasurePosition;
            chestInfo.locationProgress = locationProgress;
            //treasureProgressLocations.Add();
            //treasurePositions.Add(treasurePosition);
            chests.Add(chestInfo);
        }
    }
    //-22.52298,-8.834782,-8.834782
    private void PlaceTreasure()
    {
        foreach (ChestInfo v in chests)
        {
            BurriedChestInteraction treasure;
            bool randomPosition = TakeNextChest(out treasure);
            treasure.transform.parent = transform;
            treasure.surface = this;
            if (v.moveChest)
            {
                treasure.transform.localPosition = new Vector3(
                    v.localPosition.x,
                    GetAccurateLocalHeightOnProgress(v.locationProgress) - Random.Range(1, 3),
                    v.localPosition.y);
                treasure.transform.eulerAngles = new Vector3(Random.Range(-20, 20), Random.Range(0, 359), Random.Range(-20, 20));
            }
        }
    }

    private bool ChestAt(int index, out BurriedChestInteraction treasure)
    {
        bool result = true;
        treasure = null;
        if (fixedTreasures.Count > index)
        {
            result = fixedTreasures[index].randomPosition;
            treasure = fixedTreasures[index].chest.GetComponent<BurriedChestInteraction>();
        }
        return result;
    }

    private bool TakeNextChest(out BurriedChestInteraction treasure)
    {
        bool result = true;
        treasure = null;
        if (fixedTreasures.Count > 0)
        {
            result = fixedTreasures[0].randomPosition;
            treasure = fixedTreasures[0].chest.GetComponent<BurriedChestInteraction>();
            fixedTreasures.RemoveAt(0);
        }
        else
        {
            treasure = Instantiate(Rand.PickOne(treasures)).GetComponent<BurriedChestInteraction>();
        }
        return result;
    }

    protected override Color GetColorAt(float xProgress, float zProgress, float height)
    {
        Color result = base.GetColorAt(xProgress, zProgress, height);
        foreach (ChestInfo c in chests)
        {
            if (c.markChest)
            {
                float xDelta = Mathf.Abs(c.locationProgress.x - xProgress);
                float zDelta = Mathf.Abs(c.locationProgress.y - zProgress);
                float totalXDiff = xDelta * XSize;
                float totalZDiff = zDelta * ZSize;
                float pDiff = Mathf.Abs(totalXDiff - totalZDiff);
                bool isXInRange = totalXDiff < treasureMarkerFontWidth && totalZDiff < treasureMarkerSize;
                bool isZInRange = totalZDiff < treasureMarkerFontWidth && totalXDiff < treasureMarkerSize;
                if ((isXInRange && isZInRange && pDiff < treasureMarkerFontWidth) || isXInRange ^ isZInRange)
                {
                    if (PersistentGameDataController.Settings.isColorBlindModeOn)
                    {
                        result += colorblindTreasureColor;
                        result /= 2;
                    }
                    else
                    {
                        result += result + treasureColor;
                        result /= 3;
                    }
                }
            }
        }
        return result;
    }

}
