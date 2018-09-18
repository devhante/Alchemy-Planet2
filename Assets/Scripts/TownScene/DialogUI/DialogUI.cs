using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using AlchemyPlanet.Data;
using DG.Tweening;

namespace AlchemyPlanet.TownScene
{
    public class DialogUI : Common.UI<DialogUI>
    {
        //글자->글자 시간 간격
        [SerializeField] private float w_interval = 0.02f;
        private WaitForSeconds write_interval;
        //대화->대화 시간 간격
        [SerializeField] private float d_interval = 1f;
        private WaitForSeconds dialog_interval;

        [SerializeField] private Image d_box;

        [SerializeField] private Text d_name;
        [SerializeField] private Text d_script;

        [SerializeField] private Image[] d_illust = new Image[2];

        bool writting = false;
        bool touched = false;

        //특수 요청으로 다음 대화를 불러오기 전 플레이어 이름을 세팅한다
        bool namebox = false;

        public int count = 0;

        //상단의 버튼
        [SerializeField] private Button SkipButton;
        [SerializeField] private Button AutoButton;
        private bool autoplay = false;

        [SerializeField] private Button LogButton;

        public NPC NPC;     // 다이얼로그 사용NPC

        protected override void Awake()
        {
            base.Awake();
            
            write_interval = new WaitForSeconds(w_interval);
            dialog_interval = new WaitForSeconds(d_interval);

            SkipButton.onClick.AddListener(() => { Skip(); });
            AutoButton.onClick.AddListener(() => { AutoPlay(); });
            LogButton.onClick.AddListener(() => { GetLog(); });

            count = 1;
        }

        private void Start()
        {
            d_illust[0].transform.DOMoveX(140, 0.4f).SetEase(Ease.OutQuart);
            d_illust[1].transform.DOMoveX(540, 0.4f).SetEase(Ease.OutQuart);
            d_box.transform.DOScale(1, 0.3f).SetEase(Ease.InBack);
            d_box.transform.DOMove(new Vector2(Screen.width / 2, Screen.height / 4), 0.3f).SetEase(Ease.InBack);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                touched = true;
            }
        }

        public void SetDialog()
        {
            StartCoroutine(SetView(0));
        }

        public void SetDialog(NPC obj)
        {
            NPC = obj;

            StartCoroutine(SetView(0));
        }

        public void OnCliked()
        {
            autoplay = false;

            if (namebox)
            {
                UIManager.Instance.OpenMenu<NameSetUI>();
                namebox = false;
                return;
            }

            if (!writting)
            {
                StartCoroutine(SetView(count++));
            }
        }

        public void Skip()
        {
            count = 1;
            UIManager.Instance.CloseMenu();
            
            if (NPC != null)
            {
                NPC.StartCoroutine("TalkEnd");
                NPC = null;
            }
            else
            {
                DataManager.Instance.selected_dialog = null;
                Common.DialogScene.Instance.IsOver();
            }
        }
        public void AutoPlay()
        {
            if (autoplay)
                autoplay = false;
            else
            {
                StartCoroutine(SetView(count++));
                autoplay = true;
            }
        }
        public void GetLog()
        {
            autoplay = false;
            if (UIManager.Instance.menuStack.Peek() != DialogLogMenu.Instance)
            {
                UIManager.Instance.OpenMenu<DialogLogMenu>();
            }
        }

        public void CloseMenu()
        {
            count = 1;
            UIManager.Instance.CloseMenu();

            //NPC에게서 불러온 대화일 경우
            if (NPC != null)
            {
                NPC.StartCoroutine("TalkEnd");
                NPC = null;
            }
            //대화씬에서 불러온 대화일 경우
            else
            {
                DataManager.Instance.selected_dialog = null;
                Common.DialogScene.Instance.IsOver();
            }
        }

        private IEnumerator SetView(int num)
        {
            NPCDAta data;

            if (NPC != null)
                data = NPC.data;
            else
                data = DataManager.Instance.selected_dialog;

            do
            {
                if (num < data.dialogs.Count)
                {
                    writting = true;

                    d_name.text = data.dialogs[num].name;
                    //만약 이름이 {P}로 되어있으면 플레이어 이름으로 적용
                    if (data.dialogs[num].name.Equals("{P}"))
                        d_name.text = Data.DataManager.Instance.CurrentPlayerData.player_name;

                    #region SetIllust

                    for(int i=0; i<d_illust.Length; ++i)
                    {
                        d_illust[i].sprite = data.illusts[data.dialogs[num].illusts[i].name];
                        if (data.dialogs[num].illusts[i].mode == IllustMode.Back)
                        {
                            d_illust[i].color = new Color32(120, 120, 120, 255);
                            d_illust[i].transform.DOScale(0.95f, 0.2f).SetEase(Ease.OutExpo);
                        }
                        else
                        {
                            d_illust[i].color = new Color32(255, 255, 255, 255);
                            d_illust[i].transform.DOScale(1, 0.2f).SetEase(Ease.OutExpo);
                        }
                    }
                    #endregion SetIllust

                    #region PrintDialog
                    //다음 내용을 불러오고, Text를 초기화
                    string script = data.dialogs[num].content;
                    //{P}라고 세팅 된 부분을 플레이어 이름으로 변경
                    script = script.Replace("{P}", Data.DataManager.Instance.CurrentPlayerData.player_name);

                    if (script.Contains("{{GiveMeYourName}}"))
                    {
                        namebox = true;
                        script = script.Replace("{{GiveMeYourName}}", "");
                    }
                    

                    d_script.text = "";

                    touched = false;
                    //한 글자씩 불러오기
                    foreach (char c in script)
                    {
                        //만약 터치를 받으면 중지
                        if (touched) break;
                        d_script.text += c;
                        yield return write_interval;
                    }
                    //완성본으로 변경
                    d_script.text = script;
                    #endregion PrintDialog
                    
                    writting = false;
                }

                else
                {
                    d_illust[0].transform.DOMoveX(-300, 0.4f).SetEase(Ease.OutQuart);
                    d_illust[1].transform.DOMoveX(Screen.width + 300, 0.4f).SetEase(Ease.OutQuart);
                    d_box.transform.DOMoveY(- Screen.height / 4, 0.3f).SetEase(Ease.InBack)
                        .OnComplete(() => CloseMenu());
                    break;
                }
                if (autoplay)
                {
                    count++;
                    num++;
                    yield return dialog_interval;
                }
            } while (autoplay);
        }
    }
}