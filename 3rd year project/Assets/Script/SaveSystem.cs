using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveCube(Cube cube)
    {
        SimpleCube sc = new SimpleCube(cube.simpleRep());
        SaveCube(sc.ToString());
    }

    public static void SaveCube(string cube)
    {
        string path = Application.persistentDataPath + "/cube.txt";
        File.WriteAllText(path, cube);
    }

    public static Cube LoadCube()
    {
        Cube c = new Cube();
        string path = Application.persistentDataPath + "/cube.txt";
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
