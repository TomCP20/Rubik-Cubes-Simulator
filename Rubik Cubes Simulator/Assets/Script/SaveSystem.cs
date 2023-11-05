using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveCube(Cube cube, int saveID)
    {
        SimpleCube sc = new SimpleCube(cube.simpleRep());
        SaveCube(sc.ToString(), saveID);
    }

    public static void SaveCube(string cube, int saveID)
    {
        string path = Application.persistentDataPath + "/cube" + saveID + ".txt";
        File.WriteAllText(path, cube);
    }

    public static Cube LoadCube(int saveID)
    {
        Cube c = new Cube();
        string path = Application.persistentDataPath + "/cube" + saveID + ".txt";
        if (System.IO.File.Exists(path))
        {
            string saveData = File.ReadAllText(path);
            SimpleCube sc = new SimpleCube(saveData);
            c.applySimpleRep(sc);
        }
        else
        {
            Debug.LogError("Save file not found");
        }
        return c;
    }
}
