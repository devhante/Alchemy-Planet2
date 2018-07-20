using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace AlchemyPlanet.GameScene
{
    public class Material : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
    {
        public string materialName;
        public Vector3 originalPosition;
        public Vector3 direction;
        Image bubble;
        Button button;
        bool isChainSelected;

        private void Awake()
        {
            originalPosition = transform.position;
            bubble = transform.GetChild(0).GetComponent<Image>();
            button = GetComponent<Button>();
            isChainSelected = false;
        }

        private void Start()
        {
            StartCoroutine("Float");
        }

        private void Update()
        {
            if (Time.timeScale == 1)
                button.enabled = true;
            else
                button.enabled = false;
                
        }

        IEnumerator Float()
        {
            float speed = 15;
            direction = Random.insideUnitCircle;

            while (true)
            {
                transform.position += direction * Time.deltaTime * speed;
                yield return new WaitForEndOfFrame();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (Time.timeScale == 0) return;
            StopCoroutine("Float");
            ChangeBubbleToSelectedBubble();

            if (RecipeManager.Instance.GetQueuePeekName() == materialName)
            {
                MaterialManager.Instance.IsClickedRightMaterial = true;
                RecipeManager.Instance.UpdateRecipeNameList();
                isChainSelected = true;
                MaterialManager.Instance.Lines.Add(Instantiate(PrefabManager.Instance.line, transform.parent).GetComponent<Line>());
                MaterialManager.Instance.Lines[MaterialManager.Instance.Lines.Count - 1].start = transform.position;
                MaterialManager.Instance.Lines[MaterialManager.Instance.Lines.Count - 1].transform.SetAsFirstSibling();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (Time.timeScale == 0) return;

            MaterialManager.Instance.RespawnMaterial(this);

            if (RecipeManager.Instance.GetQueuePeekName() == materialName)
            {
                GameManager.Instance.GainScore(ScoreType.TouchRightRecipe);
                RecipeManager.Instance.DestroyQueuePeek();
            }
            else GameUI.Instance.UpdateGage(Gages.PURIFY, -5);

            if (!MaterialManager.Instance.IsClickedRightMaterial) return;
            MaterialManager.Instance.IsClickedRightMaterial = false;

            foreach (var item in MaterialManager.Instance.MaterialChain)
            {
                item.ChangeBubbleToUnselectedBubble();
                MaterialManager.Instance.RespawnMaterial(item);
                RecipeManager.Instance.DestroyQueuePeek();
            }

            if (MaterialManager.Instance.MaterialChain.Count > 0)
                Player.Instance.Attack(MaterialManager.Instance.MaterialChain.Count);

            foreach(var item in MaterialManager.Instance.Lines)
                Destroy(item.gameObject);

            MaterialManager.Instance.MaterialChain.Clear();
            MaterialManager.Instance.Lines.Clear();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (MaterialManager.Instance.IsClickedRightMaterial && !isChainSelected && MaterialManager.Instance.MaterialChain.Count < MaterialManager.Instance.MaxChainNumber - 1)
                if(RecipeManager.Instance.RecipeNameList[MaterialManager.Instance.MaterialChain.Count + 1] == materialName)
                {
                    StopCoroutine("Float");
                    ChangeBubbleToSelectedBubble();
                    MaterialManager.Instance.MaterialChain.Add(this);
                    isChainSelected = true;

                    MaterialManager.Instance.Lines[MaterialManager.Instance.Lines.Count - 1].end = transform.position;
                    MaterialManager.Instance.Lines[MaterialManager.Instance.Lines.Count - 1].StopCoroutine("DrawCoroutine");
                    MaterialManager.Instance.Lines[MaterialManager.Instance.Lines.Count - 1].Draw();

                    if (MaterialManager.Instance.MaterialChain.Count < MaterialManager.Instance.MaxChainNumber - 1)
                    { 
                        MaterialManager.Instance.Lines.Add(Instantiate(PrefabManager.Instance.line, transform.parent).GetComponent<Line>());
                        MaterialManager.Instance.Lines[MaterialManager.Instance.Lines.Count - 1].transform.SetAsFirstSibling();
                        MaterialManager.Instance.Lines[MaterialManager.Instance.Lines.Count - 1].start = transform.position;
                    }
                }
        }

        public void ChangeBubbleToSelectedBubble()
        {
            bubble.sprite = PrefabManager.Instance.selectedBubble;
        }

        public void ChangeBubbleToUnselectedBubble()
        {
            bubble.sprite = PrefabManager.Instance.unselectedBubble;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Material")
            {
                Vector3 dir = collision.GetComponent<Material>().direction;
                collision.GetComponent<Material>().direction = Rotate(-dir, 2 * GetAngle(dir, (transform.position - collision.transform.position)));
            }
        }

        private float GetAngle(Vector3 vector1, Vector3 vector2)
        {
            float angle = (Mathf.Atan2(vector2.y, vector2.x) - Mathf.Atan2(vector1.y, vector1.x)) * Mathf.Rad2Deg;
            return angle;
        }

        private Vector3 Rotate(Vector3 point, float degree)
        {
            float radius = degree * Mathf.Deg2Rad;
            float sin = Mathf.Sin(radius);
            float cos = Mathf.Cos(radius);
            float posX = point.x * cos - point.y * sin;
            float posY = point.y * cos + point.x * sin;
            return new Vector3(posX, posY);
        }
    }
}