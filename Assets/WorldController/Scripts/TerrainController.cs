using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class TerrainController : MonoBehaviour
{
    [Range(0f, 10f)]
    public float redistribuition;
    [Range(100f, 500f)]
    public int size;
    public int octaves;
    public float scale;
    public Material terrainMaterial;
    public GameObject oceanChunck;

    private float[,] noiseMap;
    
    private Texture2D texture;
    public Dictionary<Vector2, float[,]> instantiatedChuncks = new Dictionary<Vector2, float[,]>();

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
                Vector2 rootChunck = new Vector2();

                UnityMainThread.wkr.AddJob(() => {
                    rootChunck = new Vector2((int)(player.transform.position.x / size), (int)(player.transform.position.z / size));
                });  
                Thread.Sleep(50);  
                
                for(int x = (int)rootChunck.x - 2; x <= (int)rootChunck.x + 2; x++)
                {
                    for(int z = (int)rootChunck.y - 2; z <= (int)rootChunck.y + 2; z++)
                    {
                        Vector2 areaChunck = new Vector2(rootChunck.x + x, rootChunck.y + z);
                        
                        if(!instantiatedChuncks.ContainsKey(areaChunck))
                        {
                            instantiatedChuncks.Add(areaChunck, null);
                            noiseMap = Noise.GenerateNoiseMap(size, size, scale, octaves, redistribuition, areaChunck);
                            instantiatedChuncks[areaChunck] = noiseMap;
                            noiseMap = FallOffGenerator.ApplyFallOffMap(noiseMap, size);

                            UnityMainThread.wkr.AddJob(() => {
                                GameObject terrain = TerrainGenerator.GenerateTerrain(noiseMap, terrainMaterial, areaChunck);
                                GameObject oceanTerrainChunck = GameObject.Instantiate(oceanChunck, terrain.transform.position, terrain.transform.rotation, terrain.transform);
                                oceanTerrainChunck.transform.position = new Vector3(oceanTerrainChunck.transform.position.x + 125f, 4f, oceanTerrainChunck.transform.position.z + 125f);
                            });      


                            // for (int _y = 0; _y < size; _y++)
                            // {
                                // for (int _x = 0; _x < size; _x++)
                                // {
                                    // if(noiseMap[_x, _y] > 0.05f)
                                    // {
                                        // Vector3 propPos = new Vector3((areaChunck.x * size) + _x, noiseMap[_x, _y] * 100, (areaChunck.y * size) + _y);

                                        // UnityMainThread.wkr.AddJob(() => {
                                            // if(Random.Range(0, 100) < 5)
                                                // PhotonNetwork.Instantiate("Terrain/Trees/Tree1", propPos, transform.rotation);
                                        // });  
 
                                        // Thread.Sleep(10);
                                    // }
                                // }
                            // }                
                        }
                    }
                }
            }
        }
    }
}
 