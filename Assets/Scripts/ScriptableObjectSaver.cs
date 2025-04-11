using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ScriptableObjectSaver : MonoBehaviour
{
    [SerializeField]
    string objectName;

    [SerializeField]
    List<ScriptableObject> objectsToSave;


    void OnEnable()
    {
        for (int i = 0; i < objectsToSave.Count; i++)
        {
            if (File.Exists(Application.persistentDataPath + string.Format("/{0}_{1}.pso", objectName, i))) {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fileStream = File.Open(Application.persistentDataPath + string.Format("/{0}_{1}.pso", objectName, i), FileMode.Open);
                JsonUtility.FromJsonOverwrite((string)bf.Deserialize(fileStream), objectsToSave[i]);
                fileStream.Close();
            }
            else {}
        }
    }

    void OnDisable()
    {
        for (int i = 0; i < objectsToSave.Count; i++)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fileStream = File.Create(Application.persistentDataPath + string.Format("/{0}_{1}.pso", objectName, i));
            var json = JsonUtility.ToJson(objectsToSave[i]);
            bf.Serialize(fileStream, json);
            fileStream.Close();
        }

    }
}
