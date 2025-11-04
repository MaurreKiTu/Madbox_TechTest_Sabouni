using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "CharacterColorData", menuName = "Character/Color Data")]
public class CharacterColorData : ScriptableObject
{
    [Header("Character Colors")]
    [SerializeField] private List<Color> availableColors = new List<Color>();
    
    [Header("Default Settings")]
    [SerializeField] private int defaultColorIndex = 0;
    
    [NonSerialized]
    // Track used colors to avoid duplicates
    private HashSet<int> usedColorIndices = new HashSet<int>();
    
    public int ColorCount => availableColors.Count;
    
    public Color GetColor(int index)
    {
        if (index >= 0 && index < availableColors.Count)
        {
            return availableColors[index];
        }
        
        Debug.LogWarning($"Color index {index} is out of range. Returning default color.");
        return GetDefaultColor();
    }
    
    public Color GetDefaultColor()
    {
        if (availableColors.Count > 0)
        {
            int index = Mathf.Clamp(defaultColorIndex, 0, availableColors.Count - 1);
            return availableColors[index];
        }
        
        return Color.white;
    }
    
    public List<Color> GetAllColors()
    {
        return new List<Color>(availableColors);
    }
    
    public int GetDefaultColorIndex()
    {
        return Mathf.Clamp(defaultColorIndex, 0, availableColors.Count - 1);
    }
    
    public Color GetRandomColor()
    {
        if (availableColors.Count == 0)
        {
            Debug.LogWarning("No colors available in CharacterColorData. Returning white.");
            return Color.white;
        }
        
        int randomIndex = Random.Range(0, availableColors.Count);
        return availableColors[randomIndex];
    }
    
    public int GetRandomColorIndex()
    {
        if (availableColors.Count == 0)
        {
            Debug.LogWarning("No colors available in CharacterColorData. Returning 0.");
            return 0;
        }
        
        return Random.Range(0, availableColors.Count);
    }
    
    public int GetUniqueRandomColorIndex()
    {
        if (availableColors.Count == 0)
        {
            Debug.LogWarning("No colors available in CharacterColorData. Returning 0.");
            return 0;
        }
        
        // If all colors have been used, reset the used colors
        if (usedColorIndices.Count >= availableColors.Count)
        {
            ResetUsedColors();
        }
        
        // Find an unused color index
        List<int> availableIndices = new List<int>();
        for (int i = 0; i < availableColors.Count; i++)
        {
            if (!usedColorIndices.Contains(i))
            {
                availableIndices.Add(i);
            }
        }
        
        if (availableIndices.Count == 0)
        {
            Debug.LogWarning("No unique colors available. Returning 0.");
            return 0;
        }
        
        // Pick a random unused index
        int randomIndex = availableIndices[Random.Range(0, availableIndices.Count)];
        usedColorIndices.Add(randomIndex);
        
        return randomIndex;
    }
    
    public Color GetUniqueRandomColor()
    {
        int index = GetUniqueRandomColorIndex();
        return availableColors[index];
    }
    
    public void MarkColorAsUsed(int colorIndex)
    {
        if (colorIndex >= 0 && colorIndex < availableColors.Count)
        {
            usedColorIndices.Add(colorIndex);
        }
    }
    
    public void MarkColorAsUnused(int colorIndex)
    {
        usedColorIndices.Remove(colorIndex);
    }
    
    public void ResetUsedColors()
    {
        usedColorIndices.Clear();
    }
    
    public bool IsColorUsed(int colorIndex)
    {
        return usedColorIndices.Contains(colorIndex);
    }
    
    public int GetAvailableUniqueColorCount()
    {
        return availableColors.Count - usedColorIndices.Count;
    }
    
    // Editor-only method to add colors
    #if UNITY_EDITOR
    public void AddColor(Color color)
    {
        if (!availableColors.Contains(color))
        {
            availableColors.Add(color);
        }
    }
    
    public void RemoveColor(int index)
    {
        if (index >= 0 && index < availableColors.Count)
        {
            availableColors.RemoveAt(index);
        }
    }
    
    public void ClearColors()
    {
        availableColors.Clear();
    }
    #endif
}
