using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinTerrainGenerator : MonoBehaviour
{
    // Dimensions of the terrain
    public int terrainWidth = 500;
    public int terrainHeight = 500;

    // Height multiplier
    public int heightScale;

    // Dimensions of the perlin noise
    public int perlinWidth = 256;
    public int perlinHeight = 256;

    // Smoothness of the perlin noise (0 - smooth, 1 - rough)
    public float smoothness;

    // Tilling dimension for perlin noise
    public float offsetX;
    public float offsetY;

    // Frequency of the perlin noise
    public float xFrequency;
    public float yFrequency;

    // 2D array to store the height of each coord
    float[,] heights;

    void Start()
    {
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }


    // This function takes in the current terrainData and process it 
    // to return a new terrainData with perlin noise applied
    TerrainData GenerateTerrain(TerrainData terrainData){
        // Heightmap resolution is always a value of power of 2 plus 1
        terrainData.heightmapResolution = perlinWidth + 1;
        terrainData.size = new Vector3(terrainWidth, heightScale, terrainHeight);
        terrainData.SetHeights(0, 0, GenerateHeights());

        return terrainData;
    }

    private float[,] GenerateHeights(){
        heights = new float[perlinWidth, perlinHeight];

        for(int x = 0; x < perlinWidth; x++){
            for(int y = 0; y < perlinHeight; y++){
                // ADD CODE HERE: Calculate the height for a specific perlin coord/pixel
                heights[x, y] = CalculateHeight(x, y);
            }
        }

        return heights;
    }

    private float CalculateHeight(int x, int y){
        float height = 0f;
        float xPerlin = (float)x * xFrequency + offsetX;
        float yPerlin = (float)y * yFrequency + offsetY;

        // ADD CODE HERE: Calculate the height for a coord using perlin noise
        height = Mathf.PerlinNoise(xPerlin, yPerlin) * smoothness;

        return height;
    }
}

