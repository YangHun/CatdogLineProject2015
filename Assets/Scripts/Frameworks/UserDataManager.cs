using UnityEngine;
using System.Collections;


public class UserDataManager : SingleTonBehaviour<UserDataManager>
{
    public bool IsEmpty(int slot) { return false; }
    public UserData GetData(int slot) { return null; }

    public void LoadData(int slot) { }
    public void SaveData(int slot) { }
}
