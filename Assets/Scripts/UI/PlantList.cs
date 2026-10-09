using UnityEngine;
using System.Collections.Generic;

public class PlantList : MonoBehaviour
{
    public List<PlantCard> plants;
    public void Init()
    {
        for (int i = 0; i < plants.Count; i++) plants[i].Init();
    }
}
