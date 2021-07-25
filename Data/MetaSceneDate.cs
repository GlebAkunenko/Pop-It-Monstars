using System;
using UnityEngine;
using System.Security.Cryptography;
using Firebase.Analytics;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.InteropServices;

public static class MetaSceneDate
{
    public const string buttle_level_name = "Level";
    public const string chest_scene_name = "ChestOpening";
    public const string review_scene_name = "Level Rating";
    public const string first_level_scene_name = "Forest";

    public static LevelData LevelData { get; set; }
    public static bool OptionalLevel { get; set; }
    public static int Level_id { get; set; }
    public static bool Win { get; set; }
    public static bool GoldChest { get; set; }
    public static bool Started { get; set; }


    public static PlayerStats Player { get; set; } = PlayerStats.Empty;
    public static LevelStatistics Statistics { get; set; } = new LevelStatistics();
    public static GameData GameData { get; set; } = new GameData();
    public static OptionsData OptionsData { get; set; } = new OptionsData();
    

    public static bool EnableMounseDown { get; set; }

    public static string AnaliticsLevelName
    {
        get { return GameData.LocationName + "_" + Level_id.ToString(); }
    }

    [Serializable]
    private class SerializableObject
    {
        public PlayerStats Player { get; set; } = PlayerStats.Empty;
        public GameData GameData { get; set; }
        public OptionsData OptionsData { get; set; }
    }

    public static void SaveData()
    {
        foreach (Location loc in GameData.Locations)
            loc.SetSerializeVectoresBeforeSerializing();

        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Create(Application.persistentDataPath + "/Save.dat");

        SerializableObject data = new SerializableObject() {
            Player = Player,
            GameData = GameData,
            OptionsData = OptionsData
        };

        bf.Serialize(file, data);

        file.Close();
        Debug.Log("Game data saved! " + Application.persistentDataPath);
    }

    public static void LoadData(string[] locationsName)
    {
        if (File.Exists(Application.persistentDataPath + "/Save.dat")) {

            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/Save.dat", FileMode.Open);
            file.Position = 0;
            SerializableObject data = (SerializableObject)bf.Deserialize(file);
            file.Close();

            Player = data.Player;
            GameData = data.GameData;
            OptionsData = data.OptionsData;

            foreach (Location loc in GameData.Locations)
                loc.SetVectoresAfterDeserializing();

            if (GameData.Locations.Length < locationsName.Length) {
                Location[] locations = CreateLocations(locationsName);
                for (int i = 0; i < GameData.Locations.Length; i++)
                    locations[i] = GameData.Locations[i];
                GameData.Locations = locations;
            }

            foreach (Location location in MetaSceneDate.GameData.Locations)
                location.UpdateAllStars();

            Debug.Log("Game data loaded!");
        }
        else {
            Player = PlayerStats.Empty;
            GameData = new GameData();
            Location[] locations = CreateLocations(locationsName);
            GameData.Locations = locations;
            GameData.LocationName = locations[0].Name;
        }
    }

    private static Location[] CreateLocations(string[] locationsName)
    {
        Location[] locations = new Location[locationsName.Length];
        for (int i = 0; i < locationsName.Length; i++)
            locations[i] = new Location(locationsName[i]);
        return locations;
    }

    public static bool CheckFirstOpen()
    {
        return File.Exists(Application.persistentDataPath + "/Save.dat");
    }

    
}

