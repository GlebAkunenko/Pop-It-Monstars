using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
    public static int ReadInt(string name)
    {
        return SPlayerPrefs.GetInt(name);
    }

    public static float ReadFloat(string name)
    {
        return SPlayerPrefs.GetFloat(name);
    }

    public static string ReadString(string name)
    {
        return SPlayerPrefs.GetString(name);
    }

    public static bool[] ReadBoolArray(string name)
    {
        byte[] b = SPlayerPrefs.GetBytes(name);
        bool[] res = new bool[b.Length];
        for (int i = 0; i < b.Length; i++)
            res[i] = (b[i] == 1);
        return res;
    }

    public static void WriteBoolArray(string name, bool[] arr)
    {
        byte[] b = new byte[arr.Length];
        for(int i = 0; i < arr.Length; i++) {
            if (arr[i] == true)
                b[i] = 1;
            else b[i] = 0;
        }
        SPlayerPrefs.SetBytes(name, b);
    }

    public static byte[] ReadBytes(string name)
    {
        return SPlayerPrefs.GetBytes(name);
    }

    public static bool ReadBool(string name)
    {
        int v = SPlayerPrefs.GetInt(name);
        if (v == 1)
            return true;
        return false;
    }

    public static Vector3 ReadVector3(string name)
    {
        float x, y, z;
        x = ReadFloat(name + "_x");
        y = ReadFloat(name + "_y");
        z = ReadFloat(name + "_z");
        return new Vector3(x, y, z);
    }

    public static void WriteInt(string name, int value)
    {
        SPlayerPrefs.SetInt(name, value);
    }

    public static void WriteFloat(string name, float value)
    {
        SPlayerPrefs.SetFloat(name, value);
    }

    public static void WriteString(string name, string value)
    {
        SPlayerPrefs.SetString(name, value);
    }

    public static void WriteBytes(string name, byte[] value)
    {
        SPlayerPrefs.SetBytes(name, value);
    }

    public static void WriteBool(string name, bool value)
    {
        if (value == true)
            SPlayerPrefs.SetInt(name, 1);

        if (value == false)
            SPlayerPrefs.SetInt(name, 0);
    }

    public static void WriteVector3(string name, Vector3 value)
    {
        WriteFloat(name + "_x", value.x);
        WriteFloat(name + "_y", value.y);
        WriteFloat(name + "_z", value.z);
    }

    public static void WritePlayerStates(PlayerStats stats)
    {
        WriteInt("player_exp", stats.Experience);
        WriteInt("player_lvl", stats.Level);
        WriteInt("player_money", stats.Money);
        WriteInt("player_hp", stats.HealthPoints);
    }

    public static PlayerStats ReadPlayerStates()
    {
        return new PlayerStats(
            experience: ReadInt("player_exp"),
            level: ReadInt("player_lvl"),
            money: ReadInt("player_money"),
            healthPoints: ReadInt("player_hp")
        );
    }

    public static void Save()
    {
        PlayerPrefs.Save();
    }

    public static bool CheckFirstWrite()
    {
        return SPlayerPrefs.HasKey("First write");
    }

    public static void EndFirstWrite()
    {
        WriteInt("First write", 1);
        Save();
    }

    public static void RemoveDate()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    public static void RemoveData(int hp, int lvl)
    {
        
    }


}
