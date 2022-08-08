//Shady
//Introduce your new variables under Game Variables and pass them accordingly
//in the SaveData() and CreateSaveObject() Function
[System.Serializable]
public class SaveData
{
    public static SaveData Instance;
    //Game Variables
    public int Level         = 1;
    public int Coins         = 0;

    public string HashOfSaveData;

    public SaveData()
    {

    }//SaveData() end

    public SaveData(int level, int coins)
    {
        Level  = level;
        Coins  = coins;
    }//SaveData() end

    public SaveData CreateSaveObject()
    {
        SaveData ReturnSave = new SaveData(SaveData.Instance.Level, SaveData.Instance.Coins);
        return ReturnSave;
    }//CreateSaveObject() end

}//class end