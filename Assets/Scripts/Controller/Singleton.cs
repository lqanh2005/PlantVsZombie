using UnityEngine;
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;
    public bool m_DontDestroyOnLoad = true;
    private void Awake()
    {
        if (Instance == null)
        {
            //If I am the first instance, make me the Singleton
            Instance = this as T;

            if (transform.parent == null && m_DontDestroyOnLoad)
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }
        else
        {
            //If a Singleton already exists and you find
            //another reference in scene, destroy it!
            if (this != Instance)
            {
                DestroyImmediate(this.gameObject);
            }
            return;
        }

        OnAwake();
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
    protected virtual void OnAwake()
    {
    }
}