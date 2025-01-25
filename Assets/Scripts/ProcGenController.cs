using System;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class ProcGenController : MonoBehaviour
{
    // we want this to spawn the bubbles automatically.
    // each one will be spawned in the scene and will have to be correctly spawned in the lines 
    
    // game object to copy
    [SerializeField] private GameObject prefab;
    
    // how many times to copy
    [SerializeField] private int iterations;
    [SerializeField] private float randomYDistanceOffset;
    [SerializeField] private float minXValue;
    [SerializeField] private float maxXValue;
    [SerializeField] private float minRadius;
    [SerializeField] private float maxRadius;
    
    private void Start()
    {
        float previousXValue = -999;
        
        for (var i = 0; i < iterations; i++)
        {
            var xValue = Random.Range(minXValue, maxXValue);
            var radius = Random.Range(minRadius, maxRadius);

            while (i != 0 && (Math.Abs(previousXValue - xValue) > 3.5f || Math.Abs(previousXValue - xValue) < 1f))
            {
                xValue = Random.Range(minXValue, maxXValue);
            }
            
            Debug.Log($"[{i}]: The delta between the two offsets is: {Math.Abs(previousXValue - xValue)}.");
            
            var prefab = Instantiate(this.prefab, new Vector3(xValue, 6 * i * randomYDistanceOffset, 0), Quaternion.identity);
            prefab.name = $"Bubble {i}";
            prefab.transform.localScale = new Vector3(radius, radius, 0);
            
            previousXValue = xValue;
        }
    }

}
