using System;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField] private EntityType_ScriptableObject[] scriptableObjects; 
    [SerializeField] private RoomHolder[] roomHolders;
    private string[] activatedRoom; // needs to be list to be updated during runtime
    private string[] activatedType;// needs to be list to be updated during runtime

    void Start()
    {
        activatedRoom = new string[1];
        activatedType = new string[1];
        activatedRoom[0] = "default";
        activatedType[0] = "default";


        SpawnExtraObject();
        SpawnExtraObject();
        SpawnExtraObject();
        SpawnExtraObject();
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

    public void SpawnExtraObject()
    {
        //pick room this dictates location
        int randomRoom = UnityEngine.Random.Range(0,roomHolders.Length);
        Transform spawnLocation = roomHolders[randomRoom].RandomLocation();
        //pick type
        int randomType = UnityEngine.Random.Range(0,scriptableObjects.Length);
        //0 for now since only one scriptable object
        
        for(int i = 0; i < scriptableObjects.Length; i++)
        {
            if(scriptableObjects[i].typeName == "ExtraObject")
            {
                GameObject entityPrefab = scriptableObjects[i].RandomGameobject();
                Instantiate(entityPrefab, spawnLocation);
                //spawn object
            }
        }


        
    }
}
