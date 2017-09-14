using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이 클래스를 상속받은 애만 사용가능
public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    static bool bShutdown = false;
    private static T _instance = null;
    // MonoSingleton _instance = null;
    public static T Instance
    {
        get
        {
            if(_instance == null)
            {
                if(bShutdown == false)
                {
                    T instance = GameObject.FindObjectOfType<T>() as T;
                    if(instance == null)
                    {
                        instance = new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();
                    }

                    InstanceInit(instance);

                    Debug.Assert(_instance != null, typeof(T).ToString() + "싱글턴 생성 실패"); 
                }
            }
            return _instance;
        }
    }

    private static void InstanceInit(Object instance)
    {
        _instance = instance as T;
        _instance.Init();
    }

    public virtual void Init()
    {
        DontDestroyOnLoad(_instance);
    }

    public virtual void OnDestroy()
    {
        _instance = null;
    }

    private void OnApplicationQuit()
    {
        _instance = null;
        bShutdown = true;
    }
}
