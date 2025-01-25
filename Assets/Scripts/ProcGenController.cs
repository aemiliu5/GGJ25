using System;
using System.Linq;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;
using System.Collections.Generic;

public class ProcGenController : MonoBehaviour
{
    // we want this to spawn the bubbles automatically.
    // each one will be spawned in the scene and will have to be correctly spawned in the lines 
    
    [Tooltip("How many times to spawn the prefab.")] 
    [Range(1, 300)] 
    [SerializeField]
    private int iterations = 50;
    
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

    private PoolManager _poolManager;

    private List<string> _poolNames = new List<string>() { "BubbleManager", "JailBubblePool", "DeathBubblePool" };
    
    private void Start()
    {
        CheckForInvalidValues();

        _poolManager = FindObjectsByType<PoolManager>(FindObjectsSortMode.None).First();
        float previousXValue = -999;
        
        for (var i = 0; i < iterations; i++)
        {
            var xValue = Random.Range(minXValue, maxXValue);
            var yValue = Random.Range(minYOffset, maxYOffset);
            var radius = Random.Range(minRadius, maxRadius);

            while (i != 0 && (Math.Abs(previousXValue - xValue) > 3.5f || Math.Abs(previousXValue - xValue) < 1f))
            {
                xValue = Random.Range(minXValue, maxXValue);
            }
            
            Debug.Log($"[{i}]: The delta between the two offsets is: {Math.Abs(previousXValue - xValue)}.");

            int maxRange = 3;
            int rndGenIndex = Random.Range(0, maxRange);
            Vector2 position = new Vector2(xValue, yValue * i);

            var obj = _poolManager.RetrieveFromPool(_poolNames[rndGenIndex], position);

            int multiplier = rndGenIndex == 1 ? 3 : 1;

            obj.transform.localScale = new Vector3(radius * multiplier, radius * multiplier, 0);
            previousXValue = xValue;
        }
    }

    private void CheckForInvalidValues()
    {
        var errors = "";
        
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
