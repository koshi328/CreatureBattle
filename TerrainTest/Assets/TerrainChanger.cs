using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChanger : MonoBehaviour {

    private Terrain _terrain;
    private TerrainData _terrainData;
    float _currentHeight;
    float _maxHeight;

	// Use this for initialization
	void Start () {
        _terrain = GetComponent<Terrain>();
        _terrainData = GetComponent<Terrain>().terrainData;
        _currentHeight = 0.0f;
        _maxHeight = 0.2f;
    }
	
	// Update is called once per frame
	void Update () {

        if (_currentHeight < _maxHeight)
        {
            // Terrainのサイズを取得
            int heightmapWidth = _terrainData.heightmapWidth;
            int heightmapHeigth = _terrainData.heightmapHeight;

            Vector3 relPos = (transform.position - _terrain.transform.position);
            Vector3 coord = new Vector3(relPos.x / _terrainData.size.x, relPos.y / _terrainData.size.y, relPos.z / _terrainData.size.z);

            int terrainX = (int)(coord.x * heightmapWidth);
            int terrainY = (int)(coord.z * heightmapHeigth);

            Debug.Log(terrainY);

            int size = 10;
            int offset = size / 2;

            /*
             * This raises a square
             * 
            float[,] heights = tData.GetHeights(terrainX-offset, terrainY-offset, size, size);


            for(int i = 0; i < size; i++){
                for(int j = 0; j < size; j++){

                    heights[i,j] =  Mathf.Max(heights[i,j],  _currentHeight);

                }                
            }

            tData.SetHeights(terrainX-offset, terrainY-offset, heights);
            */

            //raise a  circle. sort of.

            float[,] heights = _terrainData.GetHeights(0, 0, heightmapWidth, heightmapHeigth);
            for (int x = terrainX - offset; x < terrainX + offset; x++)
            {
                for (int y = terrainY - offset; y < terrainY + offset; y++)
                {
                    float currentRadiusSqr = (new Vector2(terrainX, terrainY) - new Vector2(x, y)).sqrMagnitude;
                    if (currentRadiusSqr < offset * offset)
                    {
                        heights[y, x] = _currentHeight * (1 - currentRadiusSqr / (offset * offset));
                    }
                }
            }

            _terrainData.SetHeights(0, 0, heights);


            _currentHeight += 0.03f * Time.deltaTime;

        }
    }
}
