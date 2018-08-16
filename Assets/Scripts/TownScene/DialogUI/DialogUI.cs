using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AlchemyPlanet.Data;

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

        [SerializeField] private Text d_name;
        [SerializeField] private Text d_script;

        [SerializeField] private Image[] d_illust = new Image[3];

        bool writting = false;
        bool touched = false;

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

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                touched = true;
            }
        }

        public void SetDialog(string dialog_name, NPC obj)
        {
            NPC = obj;

            StartCoroutine(SetView(0));
        }

        public void OnCliked()
        {
            autoplay = false;
            if (!writting)
            {
                StartCoroutine(SetView(count++));
            }
        }

        public void Skip()
        {
            count = 1;
            UIManager.Instance.CloseMenu();
            NPC.StartCoroutine("TalkEnd");
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

        private IEnumerator SetView(int num)
        {
            do
            {
                if (num >= NPC.data.dialogs.Count)
                {
                    count = 1;
                    UIManager.Instance.CloseMenu();
                    NPC.StartCoroutine("TalkEnd");
                    break;
                }

                else
                {
                    writting = true;

                    d_name.text = NPC.data.dialogs[num].name;

                    d_illust[0].sprite = NPC.data.illusts[NPC.data.dialogs[num].illusts[0].name];
                    if (NPC.data.dialogs[num].illusts[0].mode == IllustMode.Back)
                    {
                        d_illust[0].color = new Color32(255, 255, 255, 120);
                    }
                    else
                    {
                        d_illust[0].color = new Color32(255, 255, 255, 255);
                    }

                    d_illust[2].sprite = NPC.data.illusts[NPC.data.dialogs[num].illusts[1].name];

                    if (NPC.data.dialogs[num].illusts[1].mode == IllustMode.Back)
                    {
                        d_illust[2].color = new Color32(255, 255, 255, 120);
                    }
                    else
                    {
                        d_illust[2].color = new Color32(255, 255, 255, 255);
                    }

                    //다음 내용을 불러오고, Text를 초기화
                    string script = NPC.data.dialogs[num].content;
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
                    d_script.text = NPC.data.dialogs[num].content;
                    
                    if(count == 1)
                    {
                        yield return dialog_interval;
                        GetComponent<Animator>().cullingMode = AnimatorCullingMode.CullCompletely;
                    }
                    writting = false;
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