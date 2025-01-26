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

    [Tooltip("Player's current position. Used for spawning the balls correctly.")]
    [SerializeField] private Transform playerPos;

    [Tooltip("Margin to spawn the infinite bubbles in the pool")] 
    [SerializeField] private float infiniteMargin = 2f;
    
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

    [SerializeField] 
    private float MaxSpawnRadiusProximity = 5f;

    private PoolManager _poolManager;
    private float _maxY;
    private bool _isGenerating = false;

    private readonly List<string> _poolNames = new List<string>() { "BubbleManager", "JailBubblePool", "DeathBubblePool", "YarnBubblePool" };

    private void Start()
    {
        CheckForInvalidValues();

        _poolManager = FindObjectsByType<PoolManager>(FindObjectsSortMode.None).First();
        InitializeBubbles();
    }

    private void Update() 
    {
        if (Math.Abs(playerPos.position.y - _maxY) <= MaxSpawnRadiusProximity && !_isGenerating)
        {
            Debug.Log("GOT TO THE TOP! Reloading bubbles");

            var initialIteration = (int) Math.Ceiling(maxYOffset + _maxY + infiniteMargin);
            Debug.Log($"NEW ITERATIONS WILL START FROM: {initialIteration}");
            Debug.Log($"Which means that it will start from {Random.Range(minYOffset, maxYOffset) * initialIteration}");
            
            InitializeBubbles(initialIteration: initialIteration);
        }
    }

    private void InitializeBubbles(int initialIteration = 0)
    {
        float previousXValue = -999;
        _isGenerating = true;

        for (var i = initialIteration; i < iterations + initialIteration; i++)
        {
            var xValue = Random.Range(minXValue, maxXValue);
            var yOffset = Random.Range(minYOffset, maxYOffset);
            var yValue = _maxY + yOffset; // Add to the maximum Y of the last bubble

            var radius = Random.Range(minRadius, maxRadius);

            while (i != initialIteration && (Math.Abs(previousXValue - xValue) > 3.5f || Math.Abs(previousXValue - xValue) < 1f))
            {
                xValue = Random.Range(minXValue, maxXValue);
            }

            string poolName;

            if (i % 5 == 0)
            {
                // After 20 iterations, choose "BubbleManager" with 50% chance, otherwise randomize among other types
                bool isNormalBubble = Random.Range(0f, 1f) < 0.5f;
                if (isNormalBubble)
                {
                    poolName = "BubbleManager";
                }
                else
                {
                    // Choose one of the other three pool names randomly
                    var specialPools = _poolNames.Where(name => name != "BubbleManager").ToList();
                    poolName = specialPools[Random.Range(0, specialPools.Count)];
                }
            }
            else
            {
                // For the first 15 iterations, always choose "BubbleManager" (normal bubbles)
                poolName = "BubbleManager";
            }

            Vector2 position = new Vector2(xValue, yValue);
            var obj = _poolManager.RetrieveFromPool(poolName, position);

            switch (poolName) // scaling
            {
                case "JailBubblePool": obj.transform.localScale = new Vector3(radius * 2.2f, radius * 2.2f, 0); break;
                case "DeathBubblePool": obj.transform.localScale = new Vector3(radius * 1.5f, radius * 1.5f, 0); break;
                case "BubbleManager": case "YarnBubblePool":  obj.transform.localScale = new Vector3(radius * 1f, radius * 1f, 0); break;
            }

            previousXValue = xValue;

            if (_maxY <= yValue)
                _maxY = yValue; // Update _maxY to the current bubble's Y-value
        }

        _isGenerating = false;
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
