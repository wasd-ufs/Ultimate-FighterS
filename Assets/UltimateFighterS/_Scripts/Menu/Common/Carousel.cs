using UnityEngine;

public abstract class Carousel : MonoBehaviour
{
    public int CurrentIndex { get; private set; }
    public int PreviousIndex => CircularIndex(CurrentIndex - 1);
    public int NextIndex => CircularIndex(CurrentIndex + 1);

    public void Select(int index)
    {
        if (GetItemCount() <= 0)
        {
            Debug.LogWarning("Carousel Select() called without any items");
            return;
        }

        CurrentIndex = CircularIndex(index);
        OnSelected(CurrentIndex);
    }

    public void SelectRelative(int direction)
    {
        Select(CurrentIndex + direction);
    }

    public int CircularIndex(int index)
    {
        return (index + GetItemCount()) % GetItemCount();
    }

    protected abstract void OnSelected(int index);
    protected abstract int GetItemCount();
}