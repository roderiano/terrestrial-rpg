using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public static class TerrainGenerator
{

    public static GameObject GenerateTerrain(float[,] _noiseMap, Material[] terrainMaterials, Vector2 chunk, float sizeMultiplier)
    {   
        int _width = _noiseMap.GetLength(0);
        int _height = _noiseMap.GetLength(1);

        Vector3[] vertices = GenerateTerrainVertices(_noiseMap, sizeMultiplier);
        int[] triangles = GenerateTerrainTriangles(_width, _height);
        Vector2[] uv = GenerateTerrainUV(vertices, _width, _height);
        

        Mesh mesh = new Mesh();
        GameObject terrain = new GameObject("Terrain");
        MeshRenderer meshRenderer = terrain.AddComponent<MeshRenderer>();
        MeshFilter meshFilter = terrain.AddComponent<MeshFilter>();
        MeshCollider meshCollider = terrain.AddComponent<MeshCollider>();
        
        meshFilter.mesh = mesh;
        meshRenderer.materials = terrainMaterials;
        meshRenderer.materials[0].SetTexture("_MainTex", TextureGenerator.NoiseTextureFromHeightMap(_noiseMap));

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();
        
        meshCollider.sharedMesh = meshFilter.mesh;
        terrain.transform.position = new Vector3(chunk.x * _width, 0f, chunk.y * _height) * sizeMultiplier;

        return terrain;
    }
    
    public static Vector3[] GenerateTerrainVertices(float[,] _noiseMap, float sizeMultiplier)
    {
        int _width = _noiseMap.GetLength(0);
        int _height = _noiseMap.GetLength(1);

        Vector3[] vertices = new Vector3[(_width + 1) * (_height + 1)];
		for (int i = 0, z = 0; z <= _height; z++) 
        {
			for (int x = 0; x <= _width; x++, i++) 
            {
                float noise = _noiseMap[x == _width ? x - 1 : x, z == _height ? z - 1 : z];  
				vertices[i] = new Vector3(x, noise * 100f, z) * sizeMultiplier;
			}
		}

        return vertices;
    }

    public static int[] GenerateTerrainTriangles(int _width, int _height)
    {
        int[] triangles = new int[_width * _height * 6];
		for (int ti = 0, vi = 0, z = 0; z < _height; z++, vi++) {
			for (int x = 0; x < _width; x++, ti += 6, vi++) {
				triangles[ti] = vi;
				triangles[ti + 3] = triangles[ti + 2] = vi + 1;
				triangles[ti + 4] = triangles[ti + 1] = vi + _width + 1;
				triangles[ti + 5] = vi + _width + 2;
			}
		}

        return triangles;
    }

    public static Vector2[] GenerateTerrainUV(Vector3[] vertices, int _width, int _height)
    {
        Vector2[] uv = new Vector2[vertices.Length];

        for (int i = 0, z = 0; z <= _height; z++) 
        {
			for (int x = 0; x <= _width; x++, i++) 
            {
                uv[i] = new Vector2((float)x / (float)_width, (float)z / (float)_height);
			}
		}

        return uv;
    }

    public static void GenerateTrees(GameObject terrain, GameObject tree, Vector2 areaChunk, float sizeMultiplier, float size) 
    {
        const int TREE_COUNT_PER_ISLAND = 10;
        
        MeshFilter mesh = terrain.GetComponent<MeshFilter>();
        Vector3[] meshVertices = mesh.sharedMesh.vertices;

        List<Vector3> elegibleGroundPositions = new List<Vector3>();
        elegibleGroundPositions.AddRange(meshVertices);
        elegibleGroundPositions = elegibleGroundPositions.FindAll(vert => vert.y > 5f);

        int treeCount = 0;
        while(treeCount < TREE_COUNT_PER_ISLAND)
        {
            Vector3 randomPosition = elegibleGroundPositions[Random.Range(0, elegibleGroundPositions.Count - 1)];
            randomPosition = randomPosition + new Vector3(areaChunk.x * size * sizeMultiplier, 0, areaChunk.y * size * sizeMultiplier);

            Quaternion randomRotation = Quaternion.Euler(new Vector3(0, Random.Range(0f, 360f), 0));
            GameObject newTree = GameObject.Instantiate(tree, randomPosition, randomRotation);
            newTree.transform.localScale = Vector3.one * Random.Range(1.5f, 3.5f);

            treeCount++;
        }
    }
}