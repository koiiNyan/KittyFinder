using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KittyFinder
{
    [Serializable]
    public class KittyTable
    {
        public List<KittyData> Kitties = new List<KittyData>();

        public int KittiesLeft;
        public KittyTable()
        {
            // for serializer
        }

        public KittyTable([NotNull] List<KittyData> kitties)
        {
            Kitties = kitties ?? throw new ArgumentNullException(nameof(kitties));
        }
    }

    [Serializable]
    public class KittyData
    {
        public string Name;
        public bool IsNotClicked;

        public KittyData()
        {
            // for serializer
        }

        public KittyData(string name, bool isNotClicked)
        {
            Name = name;
            IsNotClicked = isNotClicked;
        }
    }

    public class SaveLoad
    {
        public static void SaveKitty(string name, bool isNotClicked, int kittiesLeft)
        {
            KittyTable data = LoadKitties();
            if (data == null) data = new KittyTable();

            KittyData newKitty = new KittyData(name, isNotClicked);

            var kittyFound = false;
            
            foreach (KittyData kittyData in data.Kitties)
            {
                if (kittyData.Name == newKitty.Name)
                {
                    kittyFound = true;
                    kittyData.IsNotClicked = newKitty.IsNotClicked;
                }
            }

            if (!kittyFound) data.Kitties.Add(newKitty);

            if (data.KittiesLeft != kittiesLeft) data.KittiesLeft = kittiesLeft;

            string json = JsonUtility.ToJson(data);

            File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        }

        public static KittyTable LoadKitties()
        {
            string path = Application.persistentDataPath + "/savefile.json";
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                KittyTable data = JsonUtility.FromJson<KittyTable>(json);
                return data;
            }

            return null;
        }
    }
}
