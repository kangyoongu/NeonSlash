using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Core
{
    public static void SetBit(ref int n, int position, bool value)
    {
        if (value)
        {
            n |= (1 << position); // 비트를 1로 설정
        }
        else
        {
            n &= ~(1 << position); // 비트를 0으로 설정
        }
    }

    public static bool IsBitSet(int n, int position)
    {
        return (n & (1 << position)) != 0;
    }

    public static bool RandomPercent(float percent)
    {
        return Random.value < percent;
    }
    public static bool RandomBool()
    {
        return Random.value < 0.5f;
    }
}
