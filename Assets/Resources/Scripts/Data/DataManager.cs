using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataManager : MonoBehaviour {

    public static DataManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);

        Test();
    }

    /*

    //현재 이용중인 플레이어 데이터
    //플레이 중에 데이터 수정이 이루어지고, 저장시 대입되는 데이터이다.
    public static PlayerData Current_Player { get; private set; }

    //    X     프로필을 불러오는 로직에서 임시적으로 사용중이다. 이후에는 필요없음. // 플레이어 정보를 포멧에 맞추어 string으로 반환한다.
    public string LoadCurrentPlayerInfo()
    {
        return string.Format("Player Rank: {0}\nPlayer Name: {1}\nSortie : {2}\nSuccess : {3}",
            Current_Player.rank, Current_Player.player_name, Current_Player.sortie, Current_Player.success);
    }


    // .data 형식을 가지고 있는 파일을 검색해서 파일명의 목록을 List<string> 로 반환한다.
    public List<string> GetPlayerSaves()
    {
        List<string> list = new List<string>();
        DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath);
        foreach (var item in di.GetFiles())
        {
            if (item.Extension.ToLower().CompareTo(".data") == 0)
            {
                list.Add(item.Name.Replace(".data", ""));
            }
        }
        return list;
    }

    //{player_name}.data 파일을 생성하고 플레이어 데이터를 초기화해 저장한다.
    public void CreateData(string player_name)
    {
        FileStream stream = new FileStream(string.Format("{0}/{1}.data", Application.persistentDataPath, player_name), FileMode.Create);
        PlayerData data = new PlayerData(player_name);

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(stream, data);

        Debug.Log(string.Format("{0}/{1}.data 저장", Application.persistentDataPath, player_name));
        stream.Close();
    }

    //Current_Player 를 {Current_Player . player_name}.data 파일에 저장한다.
    public void SaveData()
    {
        FileStream stream = new FileStream(string.Format("{0}/{1}.data", Application.persistentDataPath, Current_Player.player_name), FileMode.Create);
        PlayerData data = Current_Player;

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(stream, data);

        Debug.Log(string.Format("저장 : {0}/{1}.data", Application.persistentDataPath, Current_Player.player_name));
        stream.Close();
    }

    //{player_name}.data 를 불러와 Current_Player 에 대입한다.
    public void LoadData(string player_name)
    {
        FileStream stream = File.Open(string.Format("{0}/{1}.data", Application.persistentDataPath, player_name), FileMode.Open);

        BinaryFormatter bf = new BinaryFormatter();
        Current_Player = (PlayerData)bf.Deserialize(stream);

        stream.Close();
    }

    */


    /*
    public List<Dialog> LoadDialog(string dialog_index)
    {
        FileStream stream = File.Open(string.Format("{0}/{1}.txt", Application.dataPath + "/Resources/Dialogs/", dialog_index), FileMode.Open);
        StreamReader sr = new StreamReader(stream, System.Text.Encoding.Default);
        string[] dialogdata = sr.ReadToEnd().Split('/');
        stream.Close();

        List<Dialog> DialogScene = new List<Dialog>();
        for (int i = 0; i< dialogdata.Length; i++)
        {
            DialogScene.Add(new Dialog());
        }

        return DialogScene;
    }
    */

    public void Test()
    {
        TextAsset asset = Resources.Load<TextAsset>("Datas/Dialogs/DialogScene_01");
        string data = System.Text.Encoding.Default.GetString(asset.bytes);
        Debug.Log(data);
        Dialog dialog = JsonUtility.FromJson<Dialog>(data);
        Debug.Log(string.Format("{0} :: {1} :: {2}", dialog.name, dialog.content, dialog.illusts));
    }
}


public struct Dialog
{
    public string name;
    public string content;
    public Illust[] illusts { get; set; }

    public Dialog(string name, string content, Illust[] illusts)
    {
        this.name = name;
        this.content = content;

        this.illusts = new Illust[illusts.Length];
        for (int i=0; i< illusts.Length; i++)
        {
            this.illusts[i] = illusts[i];
            Debug.Log(illusts[i]);
        }
    }
}

public enum IllistPos { Left, Center, Right }
public enum IllustMode { Front = 0 , Back }

public struct Illust
{
    string a;

    public Illust(string a)
    {
        this.a = a;
    }
}

/*
public struct Illust
{
    IllistPos pos;
    IllustMode mode;

    public Illust(IllistPos pos, IllustMode mode)
    {
        this.pos = pos;
        this.mode = mode;
    }
}
*/
