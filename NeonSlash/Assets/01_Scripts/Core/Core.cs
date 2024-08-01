using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Core
{
    public static void SetBit(ref int n, int position, bool value)
    {
        if (value)
        {
            n |= (1 << position); // ��Ʈ�� 1�� ����
        }
        else
        {
            n &= ~(1 << position); // ��Ʈ�� 0���� ����
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
