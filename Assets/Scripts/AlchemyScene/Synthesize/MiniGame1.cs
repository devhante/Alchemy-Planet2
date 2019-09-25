using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace AlchemyPlanet.AlchemyScene
{
    public class MiniGame1 : MonoBehaviour
    {
        [SerializeField]
        private Image potImage;
        [SerializeField]
        private List<Material> materialList;

        private List<string> materialNameList;
        private List<Material> popMaterialList;
        private SynthesizeMiniGame synthesizeMiniGame;

        private void Start()
        {
            synthesizeMiniGame = GetComponentInParent<SynthesizeMiniGame>();
            materialNameList = new List<string>();
            popMaterialList = new List<Material>();

            foreach (var m in AlchemyManager.Instance.formulaDictionary[SynthesizeManager.Instance.itemName].formula.Keys)
                materialNameList.Add(m);

            List<int> randomNumberList = new List<int>();
            int num = Random.Range(0, 2);

            for (int i = 0; i < materialNameList.Count; i++)
            {
                randomNumberList.Add(num);
                num += Random.Range(1, 3);
            }

            for (int i = 0; i < randomNumberList.Count; i++)
                materialList[randomNumberList[i]].SetMaterial(materialNameList[i], true);

            List<string> notMaterialNameList = new List<string>();

            foreach (var item in Data.DataManager.Instance.itemInfo.Values)
                if (!materialNameList.Contains(item.item_name))
                    notMaterialNameList.Add(item.item_name);

            for (int i = 0; i < materialList.Count; i++)
                if (!randomNumberList.Contains(i))
                    materialList[i].SetMaterial(notMaterialNameList[Random.Range(0, notMaterialNameList.Count)], false);
        }

        public void SetSelectedMaterial(Material material)
        {
            popMaterialList.Add(material);
        }

        public bool IsFirstBubble()
        {
            return popMaterialList.Count < 1;
        }

        public void CheckSuccess()
        {
            if (popMaterialList.Count == materialNameList.Count)
            {
                StopAllCoroutines();
                StartCoroutine("SuccessMiniGame1");
            }
        }

        public IEnumerator SuccessMiniGame1()
        {
            potImage.gameObject.SetActive(true);
            popMaterialList.ForEach((m) =>
            {
                m.StopAllCoroutines();
                m.gameObject.transform.position = potImage.gameObject.transform.position + new Vector3(0, 300, 0);
                m.gameObject.transform.DOLocalMoveY(-300, 1);
            });
            yield return new WaitForSeconds(2f);

            synthesizeMiniGame.StartMiniGame2();
            Destroy(gameObject);

            yield break;
        }

        public void FailMiniGame1()
        {
            if (popMaterialList.Count == materialNameList.Count)
                return;
            popMaterialList.ForEach((m) =>
            {
                m.RemoveMaterial();
            });
            popMaterialList.Clear();
            // 경고 띄우기
            synthesizeMiniGame.greatProbability = 100;
            synthesizeMiniGame.completionTime++;
        }
    }
}