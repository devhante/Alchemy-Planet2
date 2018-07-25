using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.TownScene
{
    public class DialogUI : Common.UI<DialogUI>
    {
        //글자->글자 시간 간격
        [SerializeField] private float w_interval = 0.02f;
        private WaitForSeconds write_interval;
        //대화->대화 시간 간격
        [SerializeField] private float d_interval = 0.5f;
        private WaitForSeconds dialog_interval;

        [SerializeField] private Text d_name;
        [SerializeField] private Text d_script;

        [SerializeField] private Image[] d_illust = new Image[3];

        List<Dialog> dialogs = new List<Dialog>();
        Dictionary<string, Sprite> illusts = new Dictionary<string, Sprite>();

        bool writting = false;
        bool touched = false;

        public int count = 0;

        //상단의 버튼
        [SerializeField] private Button SkipButton;
        [SerializeField] private Button AutoButton;
        private bool autoplay = false;

        [SerializeField] private Button LogButton;


        protected override void Awake()
        {
            base.Awake();

            SetDialog("Sample");
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

        public void SetDialog(string dialog_name)
        {
            dialogs = DataManager.LoadDialog(dialog_name);

            Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/dialog");

            foreach (Dialog d in dialogs)
            {
                if (!illusts.ContainsKey(d.illusts[0].name))
                    illusts.Add(d.illusts[0].name, sprites[0]);

                if (!illusts.ContainsKey(d.illusts[1].name))
                    illusts.Add(d.illusts[1].name, sprites[1]);
            }
            StartCoroutine(SetView(0));
        }

        public void OnCliked()
        {
            GetComponent<Animator>().cullingMode = AnimatorCullingMode.CullCompletely;
            if (!writting)
            {
                StartCoroutine(SetView(count++));
            }
        }

        public void Skip()
        {
            count = 1;
            UIManager.Instance.CloseMenu();
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

        }

        private IEnumerator SetView(int num)
        {
            do
            {
                if (num >= dialogs.Count)
                {
                    count = 1;
                    UIManager.Instance.CloseMenu();
                    StopCoroutine("SetView");
                }
                else
                {
                    writting = true;

                    d_name.text = dialogs[num].name;

                    d_illust[0].sprite = illusts[dialogs[num].illusts[0].name];
                    if (dialogs[num].illusts[0].mode == IllustMode.Back)
                    {
                        d_illust[0].color = new Color32(255, 255, 255, 120);
                    }
                    else
                    {
                        d_illust[0].color = new Color32(255, 255, 255, 255);
                    }

                    d_illust[2].sprite = illusts[dialogs[num].illusts[1].name];

                    if (dialogs[num].illusts[1].mode == IllustMode.Back)
                    {
                        d_illust[2].color = new Color32(255, 255, 255, 120);
                    }
                    else
                    {
                        d_illust[2].color = new Color32(255, 255, 255, 255);
                    }

                    //다음 내용을 불러오고, Text를 초기화
                    string script = dialogs[num].content;
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
                    d_script.text = dialogs[num].content;

                    yield return dialog_interval;
                    writting = false;
                }
                
            } while (autoplay && num++ < dialogs.Count);
        }
    }
}