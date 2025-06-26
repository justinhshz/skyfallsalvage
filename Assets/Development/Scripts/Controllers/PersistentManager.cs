using UnityEngine;

public class PersistentManager : MonoBehaviour
{
    private static PersistentManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("Multiple instances of PersistentManager detected. Destroying duplicate.");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
