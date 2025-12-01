using System;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField] private EntityType_ScriptableObject[] scriptableObjects;
    [SerializeField] private RoomHolder[] roomHolders;
    public List<string> roomActivated = new List<string>();
    public List<string> typeActivated = new List<string>();
    private List<GameObject> activeObject = new List<GameObject>();

    void Start()
    {
        SpawnExtraObject();
        SpawnExtraObject();
        SpawnExtraObject();

        Debug.Log("attempt delete");
        AttemptDeleteEntity("Room3", "ExtraObject");
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
                        Debug.Log("Entity Delete Function Ran");
                        DeleteEntity(typeActivated.IndexOf(localType));
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
        Destroy(activeObject[typeIndex]);
        activeObject.Remove(activeObject[typeIndex]);
        roomActivated.Remove(roomActivated[typeIndex]);
        typeActivated.Remove(typeActivated[typeIndex]);
        Debug.Log("Delete Successful");
    }

    public void SpawnExtraObject()
    {
        
        int randomRoom = UnityEngine.Random.Range(0,roomHolders.Length);
        
            
                if (!CheckAlreadySpawned(roomHolders[randomRoom].roomName, "ExtraObject"))
                { 
                    Transform spawnLocation = roomHolders[randomRoom].RandomLocation();

                    for(int i = 0; i < scriptableObjects.Length; i++)
                    {
                        if(scriptableObjects[i].typeName == "ExtraObject")
                        {
                            GameObject entityPrefab = scriptableObjects[i].RandomGameobject();
                            GameObject spawnedEntity = Instantiate(entityPrefab, spawnLocation);
                    
                            activeObject.Add(spawnedEntity);
                        }
                        
                    }

                    //spawn object

                    roomActivated.Add(roomHolders[randomRoom].roomName);
                    typeActivated.Add("ExtraObject");
                    //add to spawned list
                }
                else
                {
                    SpawnExtraObject();
                    Debug.Log("failed to spawn");
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
                        Debug.Log("already spawned");
                        return true;
                    }
                    
                }
            }
            
        }
        return false;
    }

    private void SpawnAnomaly()
    {
        //call random spawn function
        int randNum = UnityEngine.Random.Range(0,scriptableObjects.Length);
        switch(randNum)
        {
            case 0: SpawnExtraObject(); break;


        }
    }
}
