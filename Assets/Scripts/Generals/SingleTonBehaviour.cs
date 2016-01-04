using UnityEngine;

public class SingleTonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    // Singleton codes
    private static T s_Inst = null;

    // Get singleton
    public static T Inst()
    {
        if (s_Inst == null)
        {
            s_Inst = (T)FindObjectOfType(typeof(T));

            if (FindObjectsOfType(typeof(T)).Length > 1)
            {
                Debug.LogError("Multiple Singlton Exist");
                return null;
            }

            if (s_Inst == null)
            {
                GameObject singleton = new GameObject();
                s_Inst = singleton.AddComponent<T>();
                singleton.name = typeof(T).ToString();
                Debug.Log("Create new singleton : " + s_Inst.gameObject.name);
            }
            else
            {
                Debug.Log("Referencing existing singleton : " + s_Inst.gameObject.name);
            }
        }

        return s_Inst;
    }

    

    protected void SetStatic()
    {
        var tmp = s_Inst.gameObject.transform;

        do
        {
            DontDestroyOnLoad(tmp.gameObject);
            tmp = tmp.transform.parent;
        } while (tmp != null);
    }
}
