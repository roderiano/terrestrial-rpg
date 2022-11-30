using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class TerrainController : MonoBehaviour
{
    [Range(0f, 10f)]
    public float redistribuition;
    [Range(10f, 250f)]
    public int size;
    public int octaves;
    public float scale;
    public float sizeMultiplier;
    public Material[] terrainMaterials;
    public GameObject oceanChunk;
    public GameObject tree;


    private float[,] noiseMap;
    
    private Texture2D texture;
    public Dictionary<Vector2, float[,]> instantiatedChunks = new Dictionary<Vector2, float[,]>();

    private Thread t1;
    private GameObject[] players;
 
    void Start ()
    {
        t1 = new Thread(HandleTerrainChuncks) {Name = "Thread 1"};
        t1.Start();
    }


    void FixedUpdate() {
        players = GameObject.FindGameObjectsWithTag("Player");
    }
    
    public void HandleTerrainChuncks() 
    {
        
        while(true) {
            if(players == null)
                continue;

            foreach (GameObject player in players)
            {
                Vector2 rootChunk = new Vector2();

                UnityMainThread.wkr.AddJob(() => {
                    rootChunk = new Vector2((int)(player.transform.position.x / size), (int)(player.transform.position.z / size));
                });  
                Thread.Sleep(50);  
                
                for(int x = (int)rootChunk.x; x <= (int)rootChunk.x; x++)
                {
                    for(int z = (int)rootChunk.y; z <= (int)rootChunk.y; z++)
                    {
                        Vector2 areaChunk = new Vector2(rootChunk.x + x, rootChunk.y + z);
                        
                        if(!instantiatedChunks.ContainsKey(areaChunk))
                        {
                            instantiatedChunks.Add(areaChunk, null);
                            noiseMap = Noise.GenerateNoiseMap(size, size, scale, octaves, redistribuition, areaChunk);
                            instantiatedChunks[areaChunk] = noiseMap;
                            noiseMap = FallOffGenerator.ApplyFallOffMap(noiseMap, size);

                            UnityMainThread.wkr.AddJob(() => {
                                GameObject terrain = TerrainGenerator.GenerateTerrain(noiseMap, terrainMaterials, areaChunk, sizeMultiplier);
                                TerrainGenerator.GenerateTrees(terrain, tree);

                                GameObject oceanTerrainChunk = GameObject.Instantiate(oceanChunk, terrain.transform.position, terrain.transform.rotation, terrain.transform);
                                oceanTerrainChunk.transform.position = new Vector3(oceanTerrainChunk.transform.position.x + 175f, 4f, oceanTerrainChunk.transform.position.z + 175f);
                            });                 
                        }
                    }
                }
            }
        }
    }
}
 