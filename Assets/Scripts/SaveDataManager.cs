using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Core.SaveDataManager
{
    public class SaveDataManager
    {
        static DirectoryInfo dirInfo = new DirectoryInfo(Application.persistentDataPath + "/");
        public static void NewGame()
        {
            string pathCombined = Path.Combine(Application.persistentDataPath, "SaveData" + ".data");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(pathCombined);
            bf.Serialize(file, new Inventory(0, 0, new Dictionary<Veggie, int>(), new Dictionary<Seed, int>()));
            file.Close();
        }

        public static void SaveGame(Inventory inventory)
        {
            string pathCombined = Path.Combine(Application.persistentDataPath, "SaveData" + ".data");
        }

        public static Inventory LoadGame()
        {

            string pathCombined = Path.Combine(Application.persistentDataPath,
                "SaveData" + ".data");

            if (File.Exists(pathCombined))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(pathCombined, FileMode.Open);
                Inventory currentInventory = (Inventory)bf.Deserialize(file);
                file.Close();
                return currentInventory;
            }
            return new Inventory();
        }

        public static void EraseGame(string gameName)
        {
            string pathCombined = Path.Combine(Application.persistentDataPath,
                "SaveData" + ".data");

            if (File.Exists(pathCombined))
            {
                File.Delete(pathCombined);
            }
        }
        public static FileInfo[] FilePaths
        {
            get => new DirectoryInfo(Application.persistentDataPath + "/").GetFiles("*.data*");
        }

    }
}

