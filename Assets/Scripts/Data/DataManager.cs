using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    public PlayerData currentPlayerData;
    
    //아이템 데이터 프리로드
    public Dictionary<string, Material> materials;
    public Dictionary<string, Building> buildings;

    private void Awake()
    {
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        //CreateSampleDialog();
        //CreateSampleMaterials();
        //CreateSampleFomulas();
        // CreateSampleBuilding(); // 타운관리모드 테스트할때 이거 주석 풀기

        LoadPlayerData();
        LoadMaterials();
    }

    #region PlayerData_Not_Using
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

    #endregion /
    
    #region CreateSampleData
    public void CreateSampleDialog()
    {
        List<Dialog> script = new List<Dialog> {
            new Dialog("시드", "첫번째 대사야.", new Illust[] { new Illust("시드", IllistPos.Left, IllustMode.Front), new Illust("마칭냥", IllistPos.Right, IllustMode.Back) }),
            new Dialog("마칭냥", "두번째 대사는 이거.", new Illust[] { new Illust("시드", IllistPos.Left, IllustMode.Back), new Illust("마칭냥", IllistPos.Right, IllustMode.Front) }),
            new Dialog("시드", "이게 마지막 대사.", new Illust[] { new Illust("시드", IllistPos.Left, IllustMode.Front), new Illust("마칭냥", IllistPos.Right, IllustMode.Back) })
        };

        using (StreamWriter file = File.CreateText(string.Format("{0}/{1}.json", "Assets/Resources/Datas/Dialogs/", "Sample")))
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(file, script);
        }
    }

    public void CreateSampleMaterials()
    {
        Dictionary<string, Material> materials = new Dictionary<string, Material> {
            {"Red", new Material("Red", "빨강")},
            {"Blue", new Material("Blue", "파랑")},
            {"Yellow", new Material("Yellow", "노랑")},
            {"Perple", new Material("Perple", "보라")}
        };

        using (StreamWriter file = File.CreateText("Assets/Resources/Datas/Materials.json"))
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(file, materials);
        }
    }

    public void CreateSampleFomulas()
    {
        List<Formula> formulas = new List<Formula> {
            new Formula(new Dictionary<string, int>{ {"A002",3 }, {"A003", 3} }, "A004", 1),
            new Formula(new Dictionary<string, int>{ {"A001",2 }, {"A003", 2} }, "A002", 1)
        };

        using (StreamWriter file = File.CreateText("Assets/Resources/Datas/Formulas.json"))
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(file, formulas);
        }
    }

    public void CreateSampleBuilding()
    {
        Dictionary<string, Building> buildings = new Dictionary<string, Building> {
            { "House", new Building("House","집",1)},
            { "Tree", new Building("Tree","나무",1)}
        };

        using (StreamWriter file = File.CreateText("Assets/Resources/Datas/Buildings.json"))
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(file, buildings);
        }
    }

    #endregion CreateSampleData

    private void LoadMaterials()
    {
        using (StreamReader file = new StreamReader(new MemoryStream(Resources.Load<TextAsset>("Datas/Materials").bytes), System.Text.Encoding.UTF8))
        {
            JsonSerializer serializer = new JsonSerializer();
            Dictionary<string, Material> materials = (Dictionary<string, Material>)serializer.Deserialize(file, typeof(Dictionary<string, Material>));
            this.materials = materials;

            Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Items/UI");


            for (int i = 0; i < sprites.Length; i++)
            {
                if (materials.ContainsKey(sprites[i].name)) {
                    if (sprites[i].name == materials[sprites[i].name].item_name)
                        materials[sprites[i].name].image = sprites[i];
                }
            }
        }
    }

    private void LoadBuildings()
    {
        using (StreamReader file = new StreamReader(new MemoryStream(Resources.Load<TextAsset>("Datas/Buildings").bytes), System.Text.Encoding.UTF8))
        {
            JsonSerializer serializer = new JsonSerializer();
            Dictionary<string, Building> buildings = (Dictionary<string, Building>)serializer.Deserialize(file, typeof(Dictionary<string, Building>));
            this.buildings = buildings;

            Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Town/town");

            for (int i = 0; i < sprites.Length; i++)
            {
                if (buildings.ContainsKey(sprites[i].name))
                {
                    if (sprites[i].name == buildings[sprites[i].name].buildingName)
                        buildings[sprites[i].name].image = sprites[i];
                }
            }
        }
    }


    public void SavePlayerData()
    {

    }

    public void LoadPlayerData()
    {
        currentPlayerData =  new PlayerData("AAA001", "ISHNN", 0, 0, 
            new Dictionary<string, int>{
            { "Red", 1 },
            { "Blue",2 },
            { "Yellow", 3 },
            { "Perple", 4 }},
            new Dictionary<string, int>{
                { "House", 1 },
                { "Tree", 2}}
           );
    }

    public List<Dialog> LoadDialog(string dialog_name)
    {
        using (StreamReader file = new StreamReader(new MemoryStream(Resources.Load<TextAsset>(string.Format("Datas/Dialogs/{0}", dialog_name)).bytes), System.Text.Encoding.UTF8))
        {
            JsonSerializer serializer = new JsonSerializer();
            List<Dialog> script = (List<Dialog>)serializer.Deserialize(file, typeof(List<Dialog>));
            return script;
        }
    }

    public Dictionary<string, Sprite> LoadIllust(List<Dialog> dialogs)
    {
        Dictionary<string, Sprite> illusts = new Dictionary<string, Sprite>();

        foreach (Dialog d in dialogs)
        {
            if (!illusts.ContainsKey(d.illusts[0].name))
                illusts.Add(d.illusts[0].name, Resources.Load<Sprite>(string.Format("Sprites/Illusts/{0}", d.illusts[0].name)));

            if (!illusts.ContainsKey(d.illusts[1].name))
                illusts.Add(d.illusts[1].name, Resources.Load<Sprite>(string.Format("Sprites/Illusts/{0}", d.illusts[1].name)));
        }
        return illusts;
    }

    public List<Formula> LoadFormulas()
    {
        using (StreamReader file = new StreamReader(new MemoryStream(Resources.Load<TextAsset>("Datas/Formulas").bytes), System.Text.Encoding.UTF8))
        {
            JsonSerializer serializer = new JsonSerializer();
            List<Formula> formulas = (List<Formula>)serializer.Deserialize(file, typeof(List<Formula>));
            return formulas;
        }
    }
}

public class PlayerData
{
    public string player_id;
    public string player_name;

    //재화
    public int unicoin;
    public int cosmoston;

    //재료
    public Dictionary<string, int> inventory;

    //건물
    public Dictionary<string,int > ownBuildings;  // 소유중인 건물들
    public List<GameObject> setupBulidings;  // 설치된 건물들
    
    

    public PlayerData(string player_id, string player_name, int unicoin, int cosmoston, Dictionary<string, int> inventory, Dictionary<string, int> buildings)
    {
        this.player_id = player_id;
        this.player_name = player_name;
        this.unicoin = unicoin;
        this.cosmoston = cosmoston;
        this.inventory = inventory;
        this.ownBuildings = buildings;
    }
} 

public class NPCDAta
{
    public string npc_name;
    public List<Dialog> dialogs;
    public Dictionary<string, Sprite> illusts;

    public NPCDAta(string npc_name)
    {
        this.npc_name = npc_name;
        //Sample -> this.npc_name 으로 수정해야 함
        dialogs = DataManager.Instance.LoadDialog("Sample");
        illusts = DataManager.Instance.LoadIllust(dialogs);
    }
}

public class Building
{
    public string buildingName;
    public string buildingDiscription;
    public int buildingLevel;
    public Vector3 size;
    public Sprite image;

    public Building(string buildingName, string buildingDiscription, int buildingLevel)
    {
        this.buildingName = buildingName;
        this.buildingDiscription = buildingDiscription;
        this.buildingLevel = buildingLevel;
    }
}

#region AlchemyData
public class Material
{
    public string item_name;
    public string discription;
    public Sprite image;

    public Material(string item_name, string discription)
    {
        this.item_name = item_name;
        this.discription = discription;
    }
}

public class Formula
{
    public Dictionary<string, int> formula;
    public string result;
    public int resultcount;

    public Formula(Dictionary<string, int> formula, string result, int resultcount)
    {
        this.formula = formula;
        this.result = result;
        this.resultcount = resultcount;
    }
}
#endregion AlchemyData

#region ShopData
public enum CurrencyType
{
    Unicoin = 0, Cosmoston
}

public struct ShopItem
{
    public string item_id;
    public string item_name;
    public string discription;
    public CurrencyType currencyType;
    public int price;
    public string image_address;
}
#endregion ShopData

#region DialogData
public class Dialog
{
    public string name;
    public string content;
    public Illust[] illusts = new Illust[2];

    public Dialog(string name, string content, Illust[] illusts)
    {
        this.name = name;
        this.content = content;
        this.illusts = illusts;
    }
}

public enum IllistPos { Left = 0, Center, Right }
public enum IllustMode { Front = 0, Back }
public struct Illust
{
    public string name;
    public IllistPos pos;
    public IllustMode mode;

    public Illust(string name, IllistPos pos, IllustMode mode)
    {
        this.name = name;
        this.pos = pos;
        this.mode = mode;
    }
}
#endregion DialogData