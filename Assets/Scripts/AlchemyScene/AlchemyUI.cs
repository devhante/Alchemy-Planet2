using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.AlchemyScene
{
    public class AlchemyUI : Common.UI<AlchemyUI>
    {
        [SerializeField] private Button[] PosionButtons = new Button[5];
        //제작할 아이템의 5 가지 종류를 나타내는 선반위의 UI

        [SerializeField] private List<Button> ProduceList = new List<Button>();
        //연금술 책에 있는 목표로 할 아이템의 리스트
        [SerializeField] private Button ProducePrefab;

        [SerializeField] private Button RequestButton;
        [SerializeField] private Text PageInfo;
        [SerializeField] private Button BookmarkButton;
        [SerializeField] private Button SortButton;

        protected override void Awake()
        {
            base.Awake();
            StartCoroutine(LateAwake());
        }

        public IEnumerator LateAwake()
        {
            while (! AlchemyManager.Instance)
            {
                yield return null;
            }
            foreach (var produce in AlchemyManager.Instance.formulas)
            {
                Data.Material m = Data.DataManager.Instance.materials[produce.result];
                var button = GameObject.Instantiate(ProducePrefab);
                button.GetComponent<Image>().sprite = m.image;
                ProduceList.Add(button);
            }

            foreach (var posions in PosionButtons)
                posions.onClick.AddListener(OnClickPosionButton);

            RequestButton.onClick.AddListener(OnClickRequestButton);
        }

        private void OnClickPosionButton()
        {

        }

        private void OnClickRequestButton()
        {
            UIManager.Instance.OpenMenu<RequestUI>();
        }

        private void OnClickInventoryButton()
        {
            TownScene.UIManager.Instance.OpenMenu<TownScene.InventoryCell>();
        }
    }
}