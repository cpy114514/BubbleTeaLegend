using UnityEngine;

public class DontDestroySingleton : MonoBehaviour
{
    // 同类型只保留一个
    static DontDestroySingleton instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}