//Shady
using System.IO;
using System.Text;
using UnityEngine;
using System.Security.Cryptography;

public class SaveSystem
{
    public static void SaveProgress()
    {
        SaveData.Instance.HashOfSaveData = HashGenerator(SaveObjectJSON());
        string SaveDataHashed = JsonUtility.ToJson(SaveData.Instance, true);
        File.WriteAllText(GetSavePath(), SaveDataHashed);
    }//SaveProgress() end

    public static string SaveObjectJSON() => JsonUtility.ToJson(SaveData.Instance.CreateSaveObject(), true);

    public static void LoadProgress()
    {
        if(File.Exists(GetSavePath())) 
        {
            string FileContent = File.ReadAllText(GetSavePath());
            JsonUtility.FromJsonOverwrite(FileContent, SaveData.Instance);

#if !UNITY_EDITOR
            if((HashGenerator (SaveObjectJSON()) != SaveData.Instance.HashOfSaveData))
            {
                SaveData.Instance = null;
                SaveData.Instance = new SaveData();
                DeleteProgress();
                SaveProgress();
            }//if end
#endif
        }//if end
        else  
            SaveProgress();
    }//LoadProgress() end

    public static string HashGenerator(string SaveContent)
    {
        SHA256Managed Crypt = new SHA256Managed();
        string Hash = string.Empty;
        byte[] Crypto = Crypt.ComputeHash(Encoding.UTF8.GetBytes(SaveContent), 0, Encoding.UTF8.GetByteCount(SaveContent));
        foreach (byte Bit in Crypto)
            Hash += Bit.ToString("x2");
        return Hash;
    }//HashGenerator() end

    public static void DeleteProgress()
    {
        if(File.Exists(GetSavePath()))
            File.Delete(GetSavePath());
    }//DeleteProgress() end

    private static string GetSavePath() => Path.Combine(Application.persistentDataPath, "SavedGame.json");

}//class end