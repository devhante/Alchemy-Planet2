using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.TownScene
{
    public class DialogUI : Common.UI<DialogUI>
    {
        [SerializeField] private Text d_name;
        [SerializeField] private Text d_script;

        [SerializeField] private Image[] d_illust = new Image[3];


        List<Dialog> dialogs = new List<Dialog>();
        Dictionary<string, Sprite> illusts = new Dictionary<string, Sprite>();

        int count = 1;

        protected override void Awake()
        {
            base.Awake();
            Time.timeScale = 0;

            SetDialog("Sample");
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
            SetView(0);
        }

        public void OnCliked()
        {
            if (count >= dialogs.Count)
            {
                count = 1;
                Time.timeScale = 1;
                //MenuManager.Instance.CloseMenu();
                Destroy(this.gameObject);
            }
            else
            {
                SetView(count++);
            }
        }

        public void SetView(int num)
        {
            d_name.text = dialogs[num].name;
            d_script.text = dialogs[num].content;
            d_illust[0].sprite = illusts[dialogs[num].illusts[0].name];
            if (dialogs[num].illusts[0].mode == IllustMode.Back)
                d_illust[0].color = new Color32(255, 255, 255, 120);
            else
                d_illust[0].color = new Color32(255, 255, 255, 255);

            d_illust[2].sprite = illusts[dialogs[num].illusts[1].name];
            if (dialogs[num].illusts[1].mode == IllustMode.Back)
                d_illust[2].color = new Color32(255, 255, 255, 120);
            else
                d_illust[2].color = new Color32(255, 255, 255, 255);
        }
    }
}