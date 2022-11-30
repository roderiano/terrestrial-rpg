using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{

    public static float[,] GenerateNoiseMap(int _width, int _height, float _scale, int _octaves, float _redistribuition, Vector2 chunck)
    {
        Debug.Log(chunck);
        float[,] noiseMap = new float[_width, _height];

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                float sampleX = (float)x / _width * _scale;
                float sampleY = (float)y / _height * _scale;
                
                float noise = 0f;
                float frequency = 0f;
                for (int oct = 1; oct < _octaves; oct *= 2)
                {
                    frequency += 1f / oct;
                    noise += (1f / oct) * Mathf.PerlinNoise(oct * (sampleX + (chunck.x * _width)), oct * (sampleY + (chunck.y * _height)));
                }
                    
                noise = noise / frequency;
                noiseMap[x, y] = Mathf.Pow(noise, _redistribuition) / 10;
            }
        }

        return noiseMap;
    }
}