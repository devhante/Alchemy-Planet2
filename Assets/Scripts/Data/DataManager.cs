using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections;
using System;

namespace AlchemyPlanet.Data
{
    public enum CharacterEnum { Popin }

    public class DataManager : MonoBehaviour
    {
        public static DataManager Instance { get; private set; }

        public PlayerData CurrentPlayerData { get; set; }

        //아이템 데이터 프리로드
        public Dictionary<string, Material> materials;
        public Dictionary<string, Structure> structures;

        public int selected_stage = 0;
        public NPCDAta selected_dialog;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(this.gameObject);
            }

            //CreateSampleDialog();
            CreateSampleMaterials();
            CreateSampleFomulas();
            CreateSampleRequest();

            //CreateSampleStructure(); // 타운관리모드 테스트할때 이거 주석 풀기

            LoadMaterials();
            LoadStuructures();
            LoadPlayerData();
        }

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
            {"파릇한 새싹", new Material("파릇한 새싹",ItemKind.Material , "파릇파릇한 연두색 새싹.\n언젠가 큰 나무가 될 수 있을거라 믿으며\n자라나고있다.")},
            {"붉은 꽃잎", new Material("붉은 꽃잎",ItemKind.Material, "붉은색 꽃에서 떨어져나온 꽃잎.\n향기가 좋아 방향제로 자주 쓰인다.")},
            {"작은 오렌지", new Material("작은 오렌지",ItemKind.Material, "새콤한 맛이 나는 작은 오렌지.\n과즙이 팡팡.")},
            {"블루베리", new Material("블루베리",ItemKind.Material, "새콤달콤한 블루베리.\n톡톡터지는 식감이 좋다. ")},
            {"라벤더", new Material("라벤더",ItemKind.Material, "향기로운 라벤더.\n방향제는 물론 장식용으로도 쓰인다.")},
            {"정화약", new Material("정화약",ItemKind.PurifyPosion, "뭐라고 적지...")}
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
                new Formula(new Dictionary<string, int>{ { "작은 오렌지", 3 }, { "블루베리", 3} }, "파릇한 새싹", 1),
                new Formula(new Dictionary<string, int>{ { "붉은 꽃잎", 2 }, { "블루베리", 2} }, "라벤더", 1),
                new Formula(new Dictionary<string, int>{ { "파릇한 새싹", 2 }, { "라벤더", 2} }, "정화약", 1)
            };

            using (StreamWriter file = File.CreateText("Assets/Resources/Datas/Formulas.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, formulas);
            }
        }

        public void CreateSampleRequest()
        {
            List<Request> requests = new List<Request> {
                new Request(new Dictionary<string, int>{ { "작은 오렌지", 3 }, { "블루베리", 2} }, "변비약이 필요해요", 12000, 6, 300),
                new Request(new Dictionary<string, int>{ { "붉은 꽃잎", 3 }, { "파릇한 새싹", 3} }, "감기약이 필요해요", 11000, 6, 300),
                new Request(new Dictionary<string, int>{ { "라벤더", 3 }, { "정화약", 1} }, "위장약이 필요해요", 12000, 4, 300),
                new Request(new Dictionary<string, int>{ { "파릇한 새싹", 3 }, { "정화약", 1} }, "아무튼약이 필요해요", 12000, 6, 200)
            };

            using (StreamWriter file = File.CreateText("Assets/Resources/Datas/Requests.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, requests);
            }
        }

        public void CreateSampleStructure()
        {

            using (StreamWriter file = File.CreateText("Assets/Resources/Datas/Buildings.json"))
            {
                Dictionary<string, Building> Buildings = new Dictionary<string, Building> { { "House", new Building("House", "집", 1) } };
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, Buildings);
            }
            using (StreamWriter file = File.CreateText("Assets/Resources/Datas/Interiors.json"))
            {
                Dictionary<string, Interior> Interiors = new Dictionary<string, Interior> { { "Tree", new Interior("Tree") } };
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, Interiors);
            }
        }

        #endregion CreateSampleData

        private void LoadMaterials()
        {
            using (StreamReader file = new StreamReader(new MemoryStream(Resources.Load<TextAsset>("Datas/Materials").bytes), System.Text.Encoding.UTF8))
            {
                JsonSerializer serializer = new JsonSerializer();
                Dictionary<string, Material> materials = (Dictionary<string, Material>)serializer.Deserialize(file, typeof(Dictionary<string, Material>));

                Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Items/Items/");


                for (int i = 0; i < sprites.Length; i++)
                {
                    if (materials.ContainsKey(sprites[i].name))
                    {
                        if (sprites[i].name == materials[sprites[i].name].item_name)
                            materials[sprites[i].name].image = sprites[i];
                    }
                }

                this.materials = materials;
            }
        }

        private void LoadStuructures()
        {
            structures = new Dictionary<string, Structure>();
            using (StreamReader file = new StreamReader(new MemoryStream(Resources.Load<TextAsset>("Datas/Buildings").bytes), System.Text.Encoding.UTF8))
            {
                JsonSerializer serializer = new JsonSerializer();
                Dictionary<string, Building> Buildings = (Dictionary<string, Building>)serializer.Deserialize(file, typeof(Dictionary<string, Building>));

                foreach (string str in Buildings.Keys)
                {
                    this.structures.Add(str, new Building(Buildings[str].structureName, Buildings[str].buildingDiscription, Buildings[str].buildingLevel));
                    structures[str].StructureObject = Resources.Load<GameObject>("Prefabs/TownScene/Structure");
                }

                Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Town/town");

                for (int i = 0; i < sprites.Length; i++)
                {
                    if (structures.ContainsKey(sprites[i].name))
                    {
                        if (sprites[i].name == structures[sprites[i].name].structureName)
                            structures[sprites[i].name].image = sprites[i];
                    }
                }

            }
            using (StreamReader file = new StreamReader(new MemoryStream(Resources.Load<TextAsset>("Datas/Interiors").bytes), System.Text.Encoding.UTF8))
            {
                JsonSerializer serializer = new JsonSerializer();
                Dictionary<string, Interior> Interiors = (Dictionary<string, Interior>)serializer.Deserialize(file, typeof(Dictionary<string, Interior>));

                foreach (string str in Interiors.Keys)
                {
                    this.structures.Add(str, new Interior(Interiors[str].structureName));
                    structures[str].StructureObject = Resources.Load<GameObject>("Prefabs/TownScene/Structure");
                }

                Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Town/town");

                for (int i = 0; i < sprites.Length; i++)
                {
                    if (structures.ContainsKey(sprites[i].name))
                    {
                        if (sprites[i].name == structures[sprites[i].name].structureName)
                            structures[sprites[i].name].image = sprites[i];
                    }
                }
            }
        }

        #region PlayerData
        public static PlayerData PlayerStringToData(string dataString)
        {
            PlayerData data = JsonConvert.DeserializeObject<PlayerData>(dataString);
            return data;
        }

        public string PlayerDataToString(PlayerData data)
        {
            string dataString = JsonConvert.SerializeObject(data);
            return dataString;
        }

        public string PlayerDataToString()
        {
            string dataString = JsonConvert.SerializeObject(Instance.CurrentPlayerData);
            return dataString;
        }

        public void LoadPlayerData()
        {
            CurrentPlayerData = new PlayerData();
        }
        #endregion PlayerData

        #region Dialog
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
        #endregion Dialog

        public List<Formula> LoadFormulas()
        {
            using (StreamReader file = new StreamReader(new MemoryStream(Resources.Load<TextAsset>("Datas/Formulas").bytes), System.Text.Encoding.UTF8))
            {
                JsonSerializer serializer = new JsonSerializer();
                List<Formula> formulas = (List<Formula>)serializer.Deserialize(file, typeof(List<Formula>));
                return formulas;
            }
        }

        public List<Request> LoadRequests(){
            using (StreamReader file = new StreamReader(new MemoryStream(Resources.Load<TextAsset>("Datas/Requests").bytes), System.Text.Encoding.UTF8))
            {
                JsonSerializer serializer = new JsonSerializer();
                List<Request> requests = (List<Request>)serializer.Deserialize(file, typeof(List<Request>));
                return requests;
            }
        }
    }

    public class PlayerData
    {
        public string player_id;
        public string player_name;

        public int level;
        public int MAX_EXP;
        public int exp;

        //재화
        public int unicoin;
        public int cosmoston;
        public int oxygentank;
        public int Max_oxygentank;

        //재료
        public Dictionary<string, int> inventory;

        public List<Structure> structures;
        public List<GameObject> setupBuildilngs;

        //캐릭터
        public List<Character> characters;

        //파티 편성
        public CharacterEnum[,] party;

        public PlayerData()
        {
            this.player_id = Social.localUser.id;
            this.player_name = "포핀";

            this.level = 0;
            this.MAX_EXP = 100;
            this.exp = 0;

            this.unicoin = 0;
            this.cosmoston = 0;
            this.oxygentank = 10;
            this.Max_oxygentank = 10;

            this.inventory = new Dictionary<string, int>();
            this.structures = new List<Structure>();
            this.characters = new List<Character> { new Character(CharacterEnum.Popin, 1, 50, 10, 6, "아무거나 적어놓는다") };

            party = new CharacterEnum[9,3];
            party[0,0] = CharacterEnum.Popin;

            AddSampleDatas();
        }

        //따로 처리해줘야 하는 경험치는 아래 함수를 사용한다.
        public void UpdateExp(int value)
        {
            exp += value;

            while(exp < MAX_EXP)
            {
                this.level++;
                this.exp -= this.MAX_EXP;
                this.MAX_EXP = this.level * 100;
            }
        }

        public void AddSampleDatas()
        {
            structures.Add(DataManager.Instance.structures["Tree"].Clone());
            structures[0].id = 20;
            structures[0].setup = true;
            structures[0].position = new Vector2(-2f, 2.5f);
            structures.Add(DataManager.Instance.structures["House"].Clone());
            structures[1].id = 10;
            structures[1].setup = true;
            structures[1].position = new Vector2(2f, 1.6f);
            structures.Add(DataManager.Instance.structures["Tree"].Clone());
            structures[2].id = 21;
            structures[2].setup = false;
            structures[2].position = new Vector2(-4f, 2.5f);

            inventory.Add("붉은 꽃잎", 3);
            inventory.Add("블루베리", 2);
        }

        public void SetBuildingImage()
        {
            foreach (GameObject obj in setupBuildilngs)
            {
                foreach (Structure strc in structures)
                {
                    if (strc.id == int.Parse(obj.name.Substring(0, obj.name.Length - 7)))
                    {
                        obj.GetComponent<SpriteRenderer>().sprite = strc.image;
                    }
                }
            }
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
            //dialogs = DataManager.Instance.LoadDialog("Sample");
            dialogs = DataManager.Instance.LoadDialog(this.npc_name);
            illusts = DataManager.Instance.LoadIllust(dialogs);
        }
    }

    public class Character
    {
        public CharacterEnum name;
        public int level;
        public int addtional_health;
        public int speed;
        public int atk;
        public string leader_skill_info;

        public Character(CharacterEnum name, int level, int addtional_health, int speed, int atk, string leader_skill_info)
        {
            this.name = name;
            this.level = level;
            this.addtional_health = addtional_health;
            this.speed = speed;
            this.atk = atk;
            this.leader_skill_info = leader_skill_info;
        }
    }

    #region TownData

    public class Structure
    {
        public int id;
        public string structureName;
        public Sprite image;
        public GameObject StructureObject;
        public Vector2 position;
        public bool setup = false;
        public bool flip = false;


        public virtual Structure Clone()
        {
            Structure strc = new Structure();
            strc.id = id;
            strc.structureName = structureName;
            strc.image = image;
            strc.StructureObject = StructureObject;
            strc.position = position;
            strc.setup = setup;
            return strc;
        }
        public void Build()
        {
            StructureObject.GetComponent<SpriteRenderer>().sprite = image;
            StructureObject.transform.position = position;
            StructureObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);
            StructureObject.GetComponent<BoxCollider2D>().size = StructureObject.GetComponent<SpriteRenderer>().bounds.size;
            StructureObject.GetComponent<SpriteRenderer>().sortingOrder = -90;
            StructureObject.name = id.ToString();
            StructureObject.GetComponent<SpriteRenderer>().flipX = flip;
        }
    }

    public class Building : Structure
    {
        public string buildingDiscription;
        public int buildingLevel;
        public bool upgrading = false;

        public Building(string buildingName, string buildingDiscription, int buildingLevel)
        {
            this.structureName = buildingName;
            this.buildingDiscription = buildingDiscription;
            this.buildingLevel = buildingLevel;
        }
        public override Structure Clone()
        {

            Structure strc = new Building(structureName, buildingDiscription, buildingLevel);
            strc.image = image;
            strc.StructureObject = StructureObject;
            strc.position = position;
            strc.setup = setup;
            return strc;
        }
        public void Upgrade()
        {
            upgrading = true;
            image = Resources.Load<Sprite>("Sprites/Town/Tent");

            buildingLevel++;
            image = Resources.Load<Sprite>("Sprties/Town/" + structureName + buildingLevel.ToString());
            upgrading = false;
        }
    }

    public class Interior : Structure
    {
        public Interior(string interiorName)
        {
            this.structureName = interiorName;
        }
        public override Structure Clone()
        {
            Structure strc = new Interior(structureName);
            strc.image = image;
            strc.StructureObject = StructureObject;
            strc.position = position;
            strc.setup = setup;
            return strc;
        }
    }
    #endregion TownData

    #region AlchemyData
    public enum ItemKind
    {
        Diffuser, Quest, PurifyPosion, Material, Event
    }

    public class Material
    {
        public string item_name;
        public ItemKind item_kind;
        public string discription;
        public Sprite image;

        public Material(string item_name, ItemKind item_kind, string discription)
        {
            this.item_name = item_name;
            this.item_kind = item_kind;
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

    public class Request
    {
        public Dictionary<string, int> requires;
        //필요물품

        public string request_name;
        //리퀘스트 이름

        public int unicoin;
        public int cosmoston;
        public int exp;
        //보상

        public Request(Dictionary<string, int> requires, string request_name, int unicoin, int cosmoston, int exp)
        {
            this.requires = requires;
            this.request_name = request_name;
            this.unicoin = unicoin;
            this.cosmoston = cosmoston;
            this.exp = exp;
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
}