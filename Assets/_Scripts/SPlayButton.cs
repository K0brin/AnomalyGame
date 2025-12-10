using UnityEngine;

public class SPlayButton : MonoBehaviour
{
    public void PlayFreddyHonk()
    {
        // Calls the persistent AudioManager
        SAudioManager.Instance.Play("FreddyHonk");
    }
}
