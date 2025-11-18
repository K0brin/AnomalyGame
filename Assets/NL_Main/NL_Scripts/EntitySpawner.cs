using System;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField] private EntityType_ScriptableObject[] scriptableObjects; 
    [SerializeField] private RoomHolder[] roomHolders;
    private string[] activatedRoom;
    private string[] activatedType;

    void Start()
    {
        activatedRoom = new string[1];
        activatedType = new string[1];
        activatedRoom[0] = "default";
        activatedType[0] = "default";


        SpawnRandomObject();
        SpawnRandomObject();
        SpawnRandomObject();
        SpawnRandomObject();
    }

    public void CheckCalledEntity(string room, string type)
    {
        for(int i = 0; i < activatedRoom.Length; i++)
        {
            if(activatedRoom[i] == room)
            {
                if(activatedType[i] == type)
                {
                    DeleteEntity();
                }
            }
        }
    }

    private void DeleteEntity()
    {
        
    }

    public void SpawnRandomObject()
    {
        //pick room this dictates location
        int randomRoom = UnityEngine.Random.Range(0,roomHolders.Length);
        Transform spawnLocation = roomHolders[randomRoom].RandomLocation();
        //pick type
        int randomType = UnityEngine.Random.Range(0,scriptableObjects.Length);
        //0 for now since only one scriptable object
        GameObject entityPrefab = scriptableObjects[0].RandomGameobject();
        //spawn object

        Instantiate(entityPrefab, spawnLocation);
        
    }
}
