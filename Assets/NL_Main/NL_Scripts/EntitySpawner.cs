using System;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField] public EntityType_ScriptableObject[] scriptableObjects;
    [SerializeField] public RoomHolder[] roomHolders;

    public List<string> roomActivated = new List<string>();
    public List<string> typeActivated = new List<string>();
    public List<GameObject> activeObject = new List<GameObject>();

    // MG added a safe struct for ReportSystem only
    [Serializable]
    public struct ActiveEntity
    {
        public string roomName;
        public string typeName;
    }

    // MG added a read-only list for ReportSystem
    public List<ActiveEntity> activeEntities = new List<ActiveEntity>();


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
        for (int i = 0; i < roomActivated.Count; i++)
        {
            if (roomActivated[i] == room && typeActivated[i] == type)
            {
                Debug.Log("Entity Delete Function Ran");
                DeleteEntity(i);
                return;
            }
        }

        Debug.Log("No matching entity found");
    }

    private void DeleteEntity(int index)
    {
        Destroy(activeObject[index]);

        activeObject.RemoveAt(index);
        roomActivated.RemoveAt(index);
        typeActivated.RemoveAt(index);
        activeEntities.RemoveAt(index); // MG added sync

        Debug.Log("Delete Successful");
    }

    public void SpawnExtraObject()
    {
        int randomRoom = UnityEngine.Random.Range(0, roomHolders.Length);

        string roomName = roomHolders[randomRoom].roomName;
        string typeName = "ExtraObject";

        if (CheckAlreadySpawned(roomName, typeName))
        {
            Debug.Log("Failed to spawn, trying again");
            SpawnExtraObject();
            return;
        }

        Transform spawnLocation = roomHolders[randomRoom].RandomLocation();
        foreach (var obj in scriptableObjects)
        {
            if (obj.typeName == typeName)
            {
                GameObject prefab = obj.RandomGameobject();
                GameObject spawned = Instantiate(prefab, spawnLocation);

                activeObject.Add(spawned);
            }
        }

        roomActivated.Add(roomName);
        typeActivated.Add(typeName);

        // MG add to ReportSystem-accessible list
        activeEntities.Add(new ActiveEntity
        {
            roomName = roomName,
            typeName = typeName
        });

        Debug.Log($"Spawned entity: Room = {roomName}, Type = {typeName}");
    }

    private bool CheckAlreadySpawned(string roomName, string entityType)
    {
        for (int i = 0; i < roomActivated.Count; i++)
        {
            if (roomActivated[i] == roomName && typeActivated[i] == entityType)
                return true;
        }
        return false;
    }
}

