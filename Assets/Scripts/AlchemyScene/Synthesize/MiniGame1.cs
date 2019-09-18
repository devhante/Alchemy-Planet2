using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.AlchemyScene
{
    public class MiniGame1 : MonoBehaviour
    {

        [SerializeField]
        private List<Material> materialList;

        private Material selectedMaterial;
        private List<string> materialNameList;
        private int popMaterialNumber;
        private SynthesizeMiniGame synthesizeMiniGame;
        private int completionTime;

        void SetMiniGame1()
        {
            StartCoroutine("MeasureTime");
            synthesizeMiniGame = GetComponentInParent<SynthesizeMiniGame>();
            popMaterialNumber = 0;
            materialNameList = new List<string>();

            foreach (var m in AlchemyManager.Instance.formulaDictionary[SynthesizeManager.Instance.itemName].formula.Keys)
                materialNameList.Add(m);

            List<int> randomNumberList = new List<int>();

            for (int i = 0; i < materialNameList.Count;)
            {
                int num = Random.Range(0, 10);
                if (randomNumberList.Contains(num))
                {
                    i++;
                    randomNumberList.Add(num);
                }
            }

            for (int i = 0; i < randomNumberList.Count; i++)
                materialList[randomNumberList[i]].SetMaterial(materialNameList[i], true);

            for (int i = 0; i < materialList.Count; i++)
                if (!randomNumberList.Contains(i))
                    materialList[randomNumberList[i]].SetMaterial(materialNameList[i], false);
        }

        public void SetSelectedMaterial(Material material)
        {
            if (selectedMaterial != null)
                selectedMaterial.ExpandAndDestroy();
            selectedMaterial = material;
            popMaterialNumber++;
        }

        public void CheckFail()
        {
            if (popMaterialNumber == materialNameList.Count)
            {
                StopAllCoroutines();
                if(completionTime <= 10)
                {

                }
                synthesizeMiniGame.StartMiniGame2();
            }

            else
                FailMiniGame1();
        }

        public void FailMiniGame1()
        {
            StopAllCoroutines();
            synthesizeMiniGame.AddGreatProbability(0);
        }

        IEnumerator MeasureTime()
        {
            while (completionTime < 10)
            {
                completionTime++;
                yield return new WaitForSecondsRealtime(1);
            }

            FailMiniGame1();
            yield return null;
        }
    }
}