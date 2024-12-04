using System.Collections.Generic;
using UnityEngine;

public abstract class Carousel : MonoBehaviour
{
    private int currentIndex;
    
    public void Select(int index)
    {
        if (GetItemCount() <= 0)
        {
            Debug.LogWarning("Carousel Select() called without any items");
            return;
        }
        
        currentIndex = (index + GetItemCount()) % GetItemCount();
        OnSelected(currentIndex);
    }
    
    public void SelectRelative(int direction) => Select(currentIndex + direction);

    protected abstract void OnSelected(int index);
    protected abstract int GetItemCount();
}