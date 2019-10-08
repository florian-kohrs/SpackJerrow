using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BentSizeDiffCube : BentHeightDiffCube
{

    protected override float GetCurrentPlainX(int x, int z)//
    {
        float widthChange = heightMultiplier.Evaluate(Mathf.InverseLerp(1, ZSize - 1, z));

        float result = base.GetCurrentPlainX(x, z);
        if (z > 0 && z < ZSize)
        {
            switch (x)
            {
                ///cases for upper height
                case 0:
                case 3:
                case 4:
                    {
                        result += Height * ((1 - widthChange) / 2);
                        break;
                    }
                ///cases for lower height
                case 1:
                case 2:
                    {
                        result -= Height * ((1 - widthChange) / 2);
                        break;
                    }
            }
        }
        return result;
    } 

}
