using UnityEngine;

public class SlotCardUI : MonoBehaviour
{
    public bool IsOccupied { get; private set; }

    public void SetOccupied(bool occupied)
    {
        IsOccupied = occupied;
    }
}
