using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class FallOffGenerator
{
    public static float[,] GenerateFallOffMap(int _size)
    {
        float[,] map = new float[_size, _size];
        for (int i = 0; i < _size; i++)
        {
            for (int j = 0; j < _size; j++)
            {
                float x = i / (float)_size * 2 - 1;
                float y = j / (float)_size * 2 - 1;

                // find which value is closer to the edge of the map
                float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
                map[i, j] = Evaluate(value, 1f, 50f);
            }
        }
        return map;
    }

    public static float[,] ApplyFallOffMap(float[,] map, int size)
    {
        float[,] fallOffMap = FallOffGenerator.GenerateFallOffMap(size);
        
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                map[x, y] = Mathf.Clamp01(map[x, y] - fallOffMap[x, y]);
            }
        }

        return map;
    }

    /// <summary>
    /// Plots the values generated by the falloff map to a curve
    /// </summary>
    private static float Evaluate(float x, float a, float b)
    {
        return (float)Math.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(b - b * x, a));
    }
}