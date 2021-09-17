using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediatorsRandomGenerator : MonoBehaviour
{
    public GameObject MediatorPrefab;

    /// <summary>
    /// If not 0 then this count of meds will appear on the next Update()
    /// </summary>
    private int MediatorsCountToInstantiate { get; set; }

    private float MinX { get; set; }
    private float MaxX { get; set; }
    private float MinY { get; set; }
    private float MaxY { get; set; }
    private float MinZ { get; set; }
    private float MaxZ { get; set; }

    /// <summary>
    /// Meds will be generated on the next Update()
    /// </summary>
    /// <param name="count"></param>
    public void Generate(int count, float minX = -9.0f, float maxX = 9.0f, float minY = 0.5f, float maxY = 0.5f, float minZ = -9.0f, float maxZ = 9.0f)
    {
        MediatorsCountToInstantiate = count;
        MinX = minX;
        MaxX = maxX;
        MinY = minY;
        MaxY = maxY;
        MinZ = minZ;
        MaxZ = maxZ;
}

    public void CancelGenerate()
    {
        MediatorsCountToInstantiate = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (MediatorsCountToInstantiate > 0)
        {
            for (int i = 0; i < MediatorsCountToInstantiate; i++)
            {
                Vector3 position = new Vector3(MinX == MaxX ? MinX : UnityEngine.Random.Range(MinX, MaxX),
                    MinY == MaxY ? MinY : UnityEngine.Random.Range(MinY, MaxY),
                    MinZ == MaxZ ? MinZ : UnityEngine.Random.Range(MinZ, MaxZ));
                Instantiate(MediatorPrefab, position, Quaternion.identity, this.transform);
            }
            MediatorsCountToInstantiate = 0;
        }
    }
}
