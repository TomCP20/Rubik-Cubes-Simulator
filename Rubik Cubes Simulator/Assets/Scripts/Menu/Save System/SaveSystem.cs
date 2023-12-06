using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/*
The core system for saving and loading cubes.
SaveCube takes a cube, either in Cube or sting format, and saves it as a txt file with the name cubeN.txt where N is the saveID.
LoadCube  loads the save file saved under cubeN.txt where N is the saveID, converts it from string format to cube and returns the cube.
*/

namespace Menu.Save
{
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
}

