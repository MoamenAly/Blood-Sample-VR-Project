using UnityEngine;

public class MonoSinglton<T> : MonoBehaviour where T : MonoBehaviour {
    public static T Instance;
    protected virtual void Awake() {
        Instance = this as T;
    }
}