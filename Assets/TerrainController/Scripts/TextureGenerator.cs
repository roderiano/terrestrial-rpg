using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Creates a texture from a 1-D colour map
/// </summary>
public static class TextureGenerator
{
    public static Texture2D TerrainTextureFromHeightMap(float[,] _heightMap, Color sandColor, Color dirtColor, Color grassColor)
    {
        int _width = _heightMap.GetLength(0);
        int _height = _heightMap.GetLength(1);

        Color[] colourMap = new Color[_width * _height];
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                Color _color;

                if(_heightMap[x, y] < 0.03f)
                    _color = sandColor;   
                else if(_heightMap[x, y] < 0.038f)
                    _color = dirtColor; 
                else
                _color = grassColor; 

                colourMap[(y * _width) + x] = _color;
            }
        }

        Texture2D texture = new Texture2D(_width, _height);
        texture.filterMode = FilterMode.Bilinear;
        texture.wrapMode = TextureWrapMode.Repeat;
        texture.SetPixels(colourMap);
        texture.Apply();
        return texture;
    }

    public static Texture2D NoiseTextureFromHeightMap(float[,] _heightMap)
    {
        int _width = _heightMap.GetLength(0);
        int _height = _heightMap.GetLength(1);

        Color[] colourMap = new Color[_width * _height];
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                colourMap[(y * _width) + x] = Color.Lerp(Color.black, Color.white, _heightMap[x, y]);
            }
        }

        Texture2D texture = new Texture2D(_width, _height);
        texture.filterMode = FilterMode.Bilinear;
        texture.wrapMode = TextureWrapMode.Repeat;
        texture.SetPixels(colourMap);
        texture.Apply();
        return texture;
    }
}