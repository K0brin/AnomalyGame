using System;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField] private EntityType_ScriptableObject[] scriptableObjects;
    [SerializeField] private RoomHolder[] roomHolders;
    private List<string> roomActivated = new List<string>();
    private List<string> typeActivated = new List<string>();
    private List<GameObject> activeObject = new List<GameObject>();

    void Start()
    {
        SpawnExtraObject();
        SpawnExtraObject();
        SpawnExtraObject();
        SpawnExtraObject();

        Debug.Log("attempt delete");
        AttemptDeleteEntity("Room1", "ExtraObject");
    }

    public void AttemptDeleteEntity(string room, string type)
    {
        foreach(string localRoom in roomActivated)
        {
            if(localRoom == room)
            {
                foreach(string localType in typeActivated)
                {
                    if(localType == type)
                    { 
                        DeleteEntity(typeActivated.IndexOf(localType));
                        Debug.Log("Entity Delete Function Ran");
                        return;
                    }
                    Debug.Log("Is Room; Is not Type");
                }
            }
            Debug.Log("Is not Room");
        }
    }

    private void DeleteEntity(int typeIndex)
    {
        //type and room index should be same number
        //Destroy(activeObject[typeIndex]);
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
                if (!CheckAlreadySpawned(roomHolders[randomRoom].roomName, scriptableObjects[i].typeName))
                { 
                    GameObject entityPrefab = scriptableObjects[i].RandomGameobject();
                    GameObject spawnedEntity = Instantiate(entityPrefab, spawnLocation);
                    //spawn object

                    roomActivated.Add(roomHolders[randomRoom].roomName);
                    Debug.Log(roomHolders[randomRoom].roomName);
                    typeActivated.Add(scriptableObjects[i].typeName);
                    activeObject.Add(spawnedEntity);
                    //add to spawned list
                }
                else
                {
                    Debug.Log("failed to spawn");
                }

            }
        }
    }

    private bool CheckAlreadySpawned(string roomName, string entityType)
    {
        foreach(string localRoom in roomActivated)
        {
            if(localRoom == roomName)
            {
                foreach(string localType in typeActivated)
                {
                    if(localType == entityType)
                    { 
                        return true;
                    }
                    Debug.Log("Is Room; Is not Type");
                }
            }
            Debug.Log("Is not Room");
        }
        return false;
    }
}
