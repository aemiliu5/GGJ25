using System;
using System.Numerics;
using NUnit.Framework;
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
    
    [Tooltip("A prefab to spawn inside the scene.")]
    [SerializeField] private GameObject prefab;

    [Tooltip("The that will follow the player throughout the game")]
    [SerializeField] private GameObject background;
    
    [Tooltip("How many times to spawn the prefab.")]
    [UnityEngine.Range(1, 60)][SerializeField] private int iterations = 60;
    
    [Tooltip("Minimum height difference that will be added from one bubble to another.")]
    [SerializeField] private float minYOffset = 2.7f;
    
    [Tooltip("Maximum height difference that will be added from one bubble to another.")]
    [SerializeField] private float maxYOffset = 3.3f;
    
    [Tooltip("Maximum height difference that will be added from one bubble to another.")]
    [SerializeField] private float minXValue = -5f;
    
    [Tooltip("Maximum height difference that will be added from one bubble to another.")]
    [SerializeField] private float maxXValue = 5f;
    
    [Tooltip("Maximum height difference that will be added from one bubble to another.")]
    [SerializeField] private float minRadius = 0.2f;
    
    [Tooltip("Maximum height difference that will be added from one bubble to another.")]
    [SerializeField] private float maxRadius = 0.35f;
    
    private void Start()
    {
        CheckForInvalidValues();
        
        float previousXValue = -999;
        
        for (var i = 0; i < iterations; i++)
        {
            var xValue = Random.Range(minXValue, maxXValue);
            var yValue = Random.Range(minYOffset, minYOffset);
            var radius = Random.Range(minRadius, maxRadius);

            while (i != 0 && (Math.Abs(previousXValue - xValue) > 3.5f || Math.Abs(previousXValue - xValue) < 1f))
            {
                xValue = Random.Range(minXValue, maxXValue);
            }
            
            Debug.Log($"[{i}]: The delta between the two offsets is: {Math.Abs(previousXValue - xValue)}.");
            
            var prefab = Instantiate(this.prefab, new Vector3(xValue, i * yValue, 0), Quaternion.identity);
            prefab.name = $"Bubble {i}";
            prefab.transform.localScale = new Vector3(radius, radius, 0);
            
            previousXValue = xValue;
        }
    }

    private void Update()
    {
        var backgroundPosition = background.transform.position;
        var prefabPosition = prefab.transform.position;
        
        background.transform.position = new Vector3(backgroundPosition.x, prefabPosition.y);
    }

    private void CheckForInvalidValues()
    {
        var errors = "";

        if (prefab == null)
            errors += "You must specify a prefab\n";
        
        if (iterations < 1)
            errors += "Iterations must happen at least one time.\n";
        
        if (minXValue > maxXValue)
            errors += "The minimum x value must be less than the max\n";
        
        if (minYOffset > maxYOffset)
            errors += "The minimum y value must be less than the max\n";
        
        if (minRadius > maxRadius)
            errors += "The minimum radius value must be less than the max\n";

        if (errors != "")
            throw new Exception(errors);
    }

}
