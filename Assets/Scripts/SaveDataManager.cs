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

            Debug.Log(Application.persistentDataPath);

            Inventory inv = new Inventory(0, 0, new List<InventoryItem>());

            List<InventoryItem> itemList = new List<InventoryItem>();
            List<SaveInventoryItem> wrappedList = new List<SaveInventoryItem>();

            foreach (InventoryItem xItem in itemList)
            {
                Debug.Log(xItem.Name);
                wrappedList.Add(new SaveInventoryItem(xItem));
            }

            bf.Serialize(file, new SaveInventory(inv));
            Debug.Log("Save initialized at: " + pathCombined);
            file.Close();
        }

        public static void SaveGame(SaveInventory inventory)
        {
            string pathCombined = Path.Combine(Application.persistentDataPath, "SaveData" + ".data");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(pathCombined);
            Debug.Log("Saving at: " + pathCombined);
            Debug.Log("Save:" + inventory.Beans);
            bf.Serialize(file, inventory);
            file.Close();
        }

        public static Inventory LoadGame()
        {

            string pathCombined = Path.Combine(Application.persistentDataPath,
                "SaveData" + ".data");

            if (File.Exists(pathCombined))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(pathCombined, FileMode.Open);
                SaveInventory inv = (SaveInventory)bf.Deserialize(file);

                Inventory ninv = new Inventory();

                ninv.Beans = inv.Beans;
                ninv.Apples = inv.Apples;

                foreach(SaveInventoryItem item in inv.items)
                {
                    ninv._items.Add(new InventoryItem(item));
                }

                Inventory currentInventory = ninv;

                Debug.Log("Loaded: " + pathCombined);
                Debug.Log("File:" + ninv.Beans);

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

