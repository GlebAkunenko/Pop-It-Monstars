using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class SPlayerPrefs
{
    private static byte[] iv;

    private static byte[] key = null;
    public static byte[] Key 
    {
        get
        {
            if (key == null)
                key = GetKey();
            return key;
        }
    }

    private static byte[] GetKey()
    {
        byte[] tail = new byte[32] { 244, 50, 32, 118, 134, 220, 129, 166, 91, 200, 118, 241, 52, 195, 113, 240, 65, 62, 142, 173, 130, 151, 61, 59, 76, 18, 3, 98, 237, 152, 169, 95 };
        //byte[] id = Encoding.ASCII.GetBytes(SystemInfo.deviceUniqueIdentifier);
        //byte[] ret = new byte[32];

        //int i = 0;
        //for (i = 0; i < Mathf.Min(ret.Length, id.Length); i++)
        //    ret[i] = (byte)(id[i] ^ tail[i]);

        //for (; i < ret.Length; i++)
        //    ret[i] = tail[i];

        //return ret;

        return tail;
    }

    public static void SetInt(string name, int value)
    {
        string crpt_value = Encrypt(value.ToString());
        PlayerPrefs.SetString(name, crpt_value);
    }

    public static void SetFloat(string name, float value)
    {
        string crpt_value = Encrypt(value.ToString());
        PlayerPrefs.SetString(name, crpt_value);
    }

    public static void SetString(string name, string value)
    {
        string crpt_value = Encrypt(value);
        PlayerPrefs.SetString(name, crpt_value);
    }

    public static void SetBytes(string name, byte[] value)
    {
        string v = ByteToString(value);
        PlayerPrefs.SetString(name, Encrypt(v));
    }

    public static byte[] GetBytes(string name)
    {
        Debug.Log(name);
        string v = PlayerPrefs.GetString(name);
        string d = Decrypt(v);
        byte[] b = StringToByte(d);
        return b;
    }

    public static int GetInt(string name)
    {
        string crpt_value = PlayerPrefs.GetString(name);
        string str_value = Decrypt(crpt_value);
        return int.Parse(str_value);
    }

    public static float GetFloat(string name)
    {
        string crpt_value = PlayerPrefs.GetString(name);
        string str_value = Decrypt(crpt_value);
        return float.Parse(str_value);
    }

    public static string GetString(string name)
    {
        string crpt_value = PlayerPrefs.GetString(name);
        string str_value = Decrypt(crpt_value);
        return str_value;
    }

    public static bool HasKey(string name)
    {
        return PlayerPrefs.HasKey(name);
    }


    private static byte[] StringToByte(string s)
    {
        byte[] b = new byte[s.Length / 2];
        for (int i = 0; i < s.Length - 1; i += 2) {
            string hex = s[i].ToString() + s[i + 1].ToString();
            b[i / 2] = Convert.ToByte(hex, 16);
        }
        return b;

    }

    private static string ByteToString(byte[] b)
    {
        string s = "";
        foreach (byte n in b) {
            string a = Convert.ToString(n, 16);
            if (a.Length == 0)
                a += "00";
            if (a.Length == 1)
                a = "0" + a;
            s += a;
        }
        return s;
    }


    private static string Encrypt(string src)
    {
        Aes aes = Aes.Create();
        aes.GenerateIV();
        aes.Key = Key;
        byte[] encrypted;
        ICryptoTransform crypt = aes.CreateEncryptor(aes.Key, aes.IV);
        using (MemoryStream ms = new MemoryStream()) {
            using (CryptoStream cs = new CryptoStream(ms, crypt, CryptoStreamMode.Write)) {
                using (StreamWriter sw = new StreamWriter(cs)) {
                    sw.Write(src);
                }
            }
            encrypted = ms.ToArray();
        }
        return ByteToString(encrypted.Concat(aes.IV).ToArray());
    }

    private static string Decrypt(string s)
    {
        byte[] shifr = StringToByte(s);

        byte[] bytesIv = new byte[16];
        byte[] mess = new byte[shifr.Length - 16];
        for (int i = shifr.Length - 16, j = 0; i < shifr.Length; i++, j++)
            bytesIv[j] = shifr[i];
        for (int i = 0; i < shifr.Length - 16; i++)
            mess[i] = shifr[i];
        Aes aes = Aes.Create();
        aes.Key = Key;
        aes.IV = bytesIv;
        string text = "";
        byte[] data = mess;
        ICryptoTransform crypt = aes.CreateDecryptor(aes.Key, aes.IV);
        using (MemoryStream ms = new MemoryStream(data)) {
            using (CryptoStream cs = new CryptoStream(ms, crypt, CryptoStreamMode.Read)) {
                using (StreamReader sr = new StreamReader(cs)) {
                    text = sr.ReadToEnd();
                }
            }
        }
        return text;
    }

}