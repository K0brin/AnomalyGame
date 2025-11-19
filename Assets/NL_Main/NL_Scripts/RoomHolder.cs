using System.Collections.Generic;
using UnityEngine;

public class RoomHolder : MonoBehaviour
{
    public Transform[] spawnLocations;
    private List<bool> locationActive = new List<bool>();


    void Start()
    {
        for(int i = 0; i<spawnLocations.Length; i++)
        {
            locationActive.Add(false);
        }
    }

    public Transform RandomLocation()
    {
        int randomTransform = UnityEngine.Random.Range(0, spawnLocations.Length);

        if (!locationActive[randomTransform])
        {
            locationActive[randomTransform] = true;
            return spawnLocations[randomTransform];
        }
        else
        {
            Debug.Log("Returned Null");            
            return RandomLocation();
        }

    }
}
