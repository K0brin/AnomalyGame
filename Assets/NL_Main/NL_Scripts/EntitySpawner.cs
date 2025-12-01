using System;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField] private EntityType_ScriptableObject[] scriptableObjects;
    [SerializeField] private RoomHolder[] roomHolders;
    [SerializeField] private List<Transform> dissapearTransforms = new List<Transform>();
    private List<string> roomActivated = new List<string>();
    private List<string> typeActivated = new List<string>();
    private List<GameObject> activeObject = new List<GameObject>();

    void Start()
    {
        SpawnDissapearableObjects();

        SpawnExtraObject();
        SpawnExtraObject();
        SpawnExtraObject();

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
                        DeleteEntity(typeActivated.IndexOf(localType), roomActivated.IndexOf(localRoom));
                        return;
                    }
                }
            }
        }
    }

    private void DeleteEntity(int typeIndex, int roomIndex)
    {
        if(typeActivated[typeIndex] == "ExtraObject")
        {
            Destroy(activeObject[roomIndex]);
            activeObject.Remove(activeObject[typeIndex]);
            roomActivated.Remove(roomActivated[roomIndex]);
            typeActivated.Remove(typeActivated[typeIndex]);
        }
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

    private void SpawnDissapearableObjects()
    {
        Debug.Log("spawn");
        for(int i = 0; i < scriptableObjects.Length; i++)
        {
            if(scriptableObjects[i].typeName == "ObjectDissapear")
            {
                Debug.Log("name is same");
                //spawn every prefab
                foreach(GameObject entity in scriptableObjects[i].entityPrefabs)
                {
                    //TODO make sure prefabs have set transforms
                    int randomTransform = UnityEngine.Random.Range(0,dissapearTransforms.Count);
                    Instantiate(entity, dissapearTransforms[randomTransform]);
                    dissapearTransforms.Remove(dissapearTransforms[randomTransform]);
                }
            }
                        
        }
    }
}
