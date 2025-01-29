using UnityEngine;
using System.Collections.Generic;

public class BackgroundRepeater : MonoBehaviour
{
    [Tooltip("The initial background in the scene.")]
    [SerializeField] private GameObject initialBackground;

    [Tooltip("The player transform. Used to track vertical position.")]
    [SerializeField] private Transform playerTransform;

    [Tooltip("Number of backgrounds to preload initially.")]
    [SerializeField] private int preloadCount = 3;

    [Tooltip("The normal background sprite.")]
    [SerializeField] private Sprite normalBackground;

    [Tooltip("The boost background sprite.")]
    [SerializeField] private Sprite boostBackground;

    [Tooltip("The flames GameObject to toggle during Boost mode.")]
    [SerializeField] private GameObject flames;

    private float _backgroundHeight;
    private float _lastSpawnY;
    private readonly Queue<GameObject> _activeBackgrounds = new Queue<GameObject>();
    private bool isBoostMode = false;

    private void Start()
    {
        if (initialBackground == null || playerTransform == null)
        {
            Debug.LogError("Initial background or player transform is not assigned.");
            return;
        }

        // Calculate the height of the background using the initial background's scale
        _backgroundHeight = initialBackground.GetComponent<SpriteRenderer>().bounds.size.y;

        // Initialize the first background
        _activeBackgrounds.Enqueue(initialBackground);
        _lastSpawnY = initialBackground.transform.position.y;

        // Preload additional backgrounds
        for (int i = 1; i <= preloadCount; i++)
        {
            SpawnBackground(_lastSpawnY + _backgroundHeight);
        }
    }

    private void Update()
    {
        if (playerTransform == null) return;
        
        // Check if the player is approaching the topmost background
        if (playerTransform.position.y + _backgroundHeight > _lastSpawnY)
        {
            SpawnBackground(_lastSpawnY + _backgroundHeight);
            RemoveOldBackground(); // Clean up old backgrounds if needed
        }

        // Check for Boost mode
        if (PlayerController.instance.currentState == PlayerController.PlayerState.Boosting)
        {
            if (!isBoostMode)
            {
                ToggleBoostMode(true);
            }
        }
        else
        {
            if (isBoostMode)
            {
                ToggleBoostMode(false);
            }
        }
    }

    private void SpawnBackground(float yPos)
    {
        // Create a copy of the initial background
        var newBackground = Instantiate(initialBackground, new Vector3(0, yPos, 0), Quaternion.identity);
        initialBackground = newBackground;
        _activeBackgrounds.Enqueue(newBackground);
        _lastSpawnY = yPos;
    }

    private void RemoveOldBackground()
    {
        // Optional: Remove the bottom-most background to optimize memory
        if (_activeBackgrounds.Count > preloadCount + 1)
        {
            var oldBackground = _activeBackgrounds.Dequeue();
            Destroy(oldBackground);
        }
    }

    private void ToggleBoostMode(bool activate)
    {
        isBoostMode = activate;
        flames.SetActive(activate);

        foreach (var spriteRenderer in FindObjectsOfType<SpriteRenderer>())
        {
            if (spriteRenderer.gameObject.name.Contains("Background"))
            {
                spriteRenderer.sprite = activate ? boostBackground : normalBackground;
            }
        }
    }
}
