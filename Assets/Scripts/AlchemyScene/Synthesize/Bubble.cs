using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

namespace AlchemyPlanet.AlchemyScene
{
    public class Bubble : MonoBehaviour, IPointerDownHandler
    {
        public Vector3 direction;
        public bool isExpanding;
        protected Image bubble;
        protected Button button;
        protected Animator animator;
        private bool isBubblePointerDown;

        protected virtual void Awake()
        {
            bubble = transform.GetChild(0).GetComponent<Image>();
            button = GetComponent<Button>();
            animator = GetComponent<Animator>();
            isExpanding = false;
            isBubblePointerDown = false;
        }

        protected virtual void Start()
        {
            Popup();
            StartCoroutine("Float");
        }

        protected void Popup()
        {
            RectTransform rt = GetComponent<RectTransform>();
            rt.localScale = new Vector3(0, 0, 1);

            Sequence sq = DOTween.Sequence();
            sq.Append(transform.DOScale(1.2f, 0.2f).SetEase(Ease.InQuad));
            sq.Append(transform.DOScale(1, 0.2f).SetEase(Ease.OutSine));
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

        protected void Shrink()
        {
            RectTransform rt = GetComponent<RectTransform>();
            Sequence sq = DOTween.Sequence();
            sq.Append(transform.DOScale(0.97f, 0.2f).SetEase(Ease.OutQuint));
        }

        protected IEnumerator MoveCoroutine()
        {
            while (true)
            {
                transform.position = Input.mousePosition;
                yield return new WaitForEndOfFrame();
            }
        }

        public void ExpandAndDestroy()
        {
            animator.SetTrigger("Pop");
            StartCoroutine("DestroyCoroutine");
        }

        IEnumerator DestroyCoroutine()
        {
            while (animator.GetCurrentAnimatorStateInfo(0).IsName("BubblePop") != true)
                yield return null;

            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
                yield return null;

            Destroy(gameObject);
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (isBubblePointerDown) return;
            else isBubblePointerDown = true;

            StopCoroutine("Float");
            Shrink();
            ChangeBubbleToSelectedBubble();
            StartCoroutine("MoveCoroutine");
        }

        public void ChangeBubbleToSelectedBubble()
        {
            animator.SetTrigger("Selected");
        }

        public virtual void ChangeBubbleToUnselectedBubble()
        {
            animator.SetTrigger("Unselected");
        }

        public virtual void ChangeBubbleToHighlightedBubble()
        {
            animator.SetTrigger("Highlighted");
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