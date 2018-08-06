using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AlchemyPlanet.GameScene
{
    public class Bubble : MonoBehaviour, IPointerDownHandler
    {
        public Vector3 direction;
        public bool isExpanding;
        protected Image bubble;
        protected Button button;

        protected virtual void Awake()
        {
            bubble = transform.GetChild(0).GetComponent<Image>();
            button = GetComponent<Button>();
            isExpanding = false;
        }

        protected virtual void Start()
        {
            StartCoroutine("Popup");
            StartCoroutine("Float");
        }

        private IEnumerator Popup()
        {
            float speed = 5f;

            RectTransform rt = GetComponent<RectTransform>();
            Vector3 scale = new Vector3(0, 0, 1);
            rt.localScale = scale;

            while (scale.x < 1)
            {
                scale += new Vector3(Time.deltaTime * speed, Time.deltaTime * speed);
                rt.localScale = scale;

                yield return new WaitForEndOfFrame();
            }

            while (scale.x < 1.2f)
            {
                scale += new Vector3(Time.deltaTime * speed, Time.deltaTime * speed);
                rt.localScale = scale;
                yield return new WaitForEndOfFrame();
            }

            while (scale.x > 1)
            {
                scale -= new Vector3(Time.deltaTime * speed, Time.deltaTime * speed);
                rt.localScale = scale;
                yield return new WaitForEndOfFrame();
            }

            rt.localScale = new Vector3(1, 1, 1);
        }

        protected IEnumerator Float()
        {
            float speed = 15;
            direction = Random.insideUnitCircle;

            while (true)
            {
                transform.position += direction * Time.deltaTime * speed;
                yield return new WaitForEndOfFrame();
            }
        }

        protected IEnumerator Shrink()
        {
            float speed = 0.5f;

            RectTransform rt = GetComponent<RectTransform>();
            Vector3 scale = rt.localScale;

            while (scale.x > 0.97f)
            {
                scale -= new Vector3(Time.deltaTime * speed, Time.deltaTime * speed);
                rt.localScale = scale;

                yield return new WaitForEndOfFrame();
            }

            rt.localScale = new Vector3(0.97f, 0.97f, 1);
        }

        protected IEnumerator Expand()
        {
            float speed = 0.5f;

            RectTransform rt = GetComponent<RectTransform>();
            Vector3 scale = rt.localScale;

            isExpanding = true;
            while (scale.x > 1.1f)
            {
                scale += new Vector3(Time.deltaTime * speed, Time.deltaTime * speed);
                rt.localScale = scale;

                yield return new WaitForEndOfFrame();
            }

            rt.localScale = new Vector3(1.1f, 1.1f, 1);
            isExpanding = false;
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            StopCoroutine("Float");
            StartCoroutine("Shrink");
            ChangeBubbleToSelectedBubble();
        }

        public void ChangeBubbleToSelectedBubble()
        {
            bubble.sprite = PrefabManager.Instance.selectedBubble;
        }

        public virtual void ChangeBubbleToUnselectedBubble()
        {
            bubble.sprite = PrefabManager.Instance.unselectedBubble;
        }

        public virtual void ChangeBubbleToHighlightedBubble()
        {
            bubble.sprite = PrefabManager.Instance.highlightedBubble;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Bubble")
            {
                Vector3 dir = collision.GetComponent<Bubble>().direction;
                collision.GetComponent<Bubble>().direction = Rotate(-dir, 2 * GetAngle(dir, (transform.position - collision.transform.position)));
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