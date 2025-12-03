using UnityEngine;

[CreateAssetMenu(fileName = "New Anomaly", menuName = "Game/Anomaly")]
public class AnomalySO : ScriptableObject
{
    public string anomalyName = "Normal";              // Type of anomaly (Normal, Moved, Missing, Replaced)
    public GameObject normalPrefab;                    
    public GameObject replacedPrefab;                  
    public string roomName;                            
}