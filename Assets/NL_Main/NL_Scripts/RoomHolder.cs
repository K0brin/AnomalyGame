using UnityEngine;

public class RoomHolder : MonoBehaviour
{
    public Transform[] spawnLocations;

    public Transform RandomLocation()
    {
        int randomTransform = UnityEngine.Random.Range(0, spawnLocations.Length);

        return spawnLocations[randomTransform];
    }
}
