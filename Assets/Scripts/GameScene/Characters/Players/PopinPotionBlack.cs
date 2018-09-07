using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AlchemyPlanet.GameScene
{
    public class PopinPotionBlack : Item
    {
        public Image mask;
        public Image danger;

        private Rigidbody2D rb2d;
        private Vector3 startPosition;
        private Vector3 endPosition;
        private Vector3 destinationDirection;

        protected override void Awake()
        {
            base.Awake();
            rb2d = GetComponent<Rigidbody2D>();
        }

        protected override void Start()
        {
            base.Start();
            StartCoroutine("CountdownCoroutine");
            StartCoroutine("DangerCoroutine");
        }

        private void Update()
        {
            destinationDirection = (GameUI.Instance.BombDestination.transform.position - transform.position).normalized;
        }

        public override void OnPointerDown(PointerEventData eventdata)
        {
            startPosition = transform.position;
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            endPosition = Input.mousePosition;
            Vector3 swipeDirection = (endPosition - startPosition).normalized;
            direction = swipeDirection;

            float angle = ContAngle(destinationDirection, swipeDirection);
            if (Mathf.Abs(angle) <= 90)
                StartCoroutine("ThrowCoroutine");
        }

        IEnumerator CountdownCoroutine()
        {
            Color color = new Color(1, 0, 0, 0);
            float countdown = 5;

            while(countdown > 0)
            {
                color += new Color(0, 0, 0, 0.01f);
                countdown -= 0.1f;
                mask.color = color;
                yield return new WaitForSeconds(0.1f);
            }

            StartCoroutine("ExplodeCoroutine");
        }

        IEnumerator ExplodeCoroutine()
        {
            StopCoroutine("Float");
            button.enabled = false;
            mask.color = new Color(1, 0, 0, 0);

            animator.SetTrigger("Explode");
            StartCoroutine("ShakeCoroutine");

            GameUI.Instance.UpdateGage(Gages.PURIFY, -30);

            while (animator.GetCurrentAnimatorStateInfo(0).IsName("PopinPotionBlackEnd") == false)
                yield return new WaitForEndOfFrame();

            ItemManager.Instance.Objects.Remove(gameObject);
            Destroy(gameObject);
        }

        IEnumerator ThrowCoroutine()
        {
            StopCoroutine("Float");
            StopCoroutine("DangerCoroutine");
            StopCoroutine("CountdownCoroutine");
            button.enabled = false;
            mask.color = new Color(1, 0, 0, 0);
            danger.gameObject.SetActive(false);

            while ((GameUI.Instance.BombDestination.transform.position - transform.position).sqrMagnitude > 10)
            {
                Vector3 destination = Vector3.Lerp(transform.position, GameUI.Instance.BombDestination.transform.position, 0.1f);
                transform.position = destination;
                yield return new WaitForSeconds(0.01f);
            }
            transform.position = GameUI.Instance.BombDestination.transform.position;

            animator.SetTrigger("Explode");
            StartCoroutine("ShakeCoroutine");

            int count = 0;
            foreach(var item in MonsterManager.Instance.Monsters)
            {
                item.Value.Hit(Player.Instance.GetDamage(5));
                count++;
                if (count >= 3) break;
            }

            while (animator.GetCurrentAnimatorStateInfo(0).IsName("PopinPotionBlackEnd") == false)
                yield return new WaitForEndOfFrame();

            ItemManager.Instance.Objects.Remove(gameObject);
            Destroy(gameObject);
        }

        IEnumerator ShakeCoroutine()
        {
            Vector3 originPos = Camera.main.transform.position;
            float timer = 0;
            while (timer <= 0.2f)
            {
                Camera.main.transform.localPosition = (Vector3)Random.insideUnitCircle * 0.1f + originPos;

                timer += Time.deltaTime;
                yield return null;
            }
            Camera.main.transform.localPosition = originPos;
        }

        IEnumerator DangerCoroutine()
        {
            float alpha = danger.color.a;

            while(true)
            {
                while(alpha <= 1)
                {
                    alpha += Time.deltaTime;
                    danger.color = new Color(danger.color.r, danger.color.g, danger.color.b, alpha);
                    yield return null;
                }

                alpha = 1;

                while(alpha >= 0)
                {
                    alpha -= Time.deltaTime;
                    danger.color = new Color(danger.color.r, danger.color.g, danger.color.b, alpha);
                    yield return null;
                }

                alpha = 0;
            }
        }

        private float ContAngle(Vector3 fwd, Vector3 targetDir)
        {
            float angle = Vector3.Angle(fwd, targetDir);

            if (AngleDir(fwd, targetDir, Vector3.up) == -1)
            {
                angle = 360.0f - angle;
                if (angle > 359.9999f)
                    angle -= 360.0f;
                return angle;
            }
            else
                return angle;
        }

        private int AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
        {
            Vector3 perp = Vector3.Cross(fwd, targetDir);
            float dir = Vector3.Dot(perp, up);

            if (dir > 0.0)
                return 1;
            else if (dir < 0.0)
                return -1;
            else
                return 0;
        }
    }
}