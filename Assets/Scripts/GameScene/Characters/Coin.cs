using System.Collections;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class Coin : MonoBehaviour
    {
        private float speed;
        private Rigidbody2D rigidbody2d;

        private void Awake()
        {
            speed = 3;
            rigidbody2d = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            Draw();
        }

        private void Draw()
        {
            StartCoroutine("DrawCoroutine");
        }

        private IEnumerator DrawCoroutine()
        {
            // 코인이 떨어지기 시작할 때까지 잠시 기다림
            yield return new WaitForSeconds(0.5f);

            // 코인이 완전히 떨어질 때까지 기다림
            while(rigidbody2d.velocity.y != 0)
                yield return new WaitForEndOfFrame();

            // 플레이어에게 이동
            rigidbody2d.isKinematic = true;
            GetComponent<CircleCollider2D>().enabled = false;
            while(Vector3.Distance(transform.position, CoinManager.Instance.CoinDestination) > 0.01f)
            {
                transform.position = Vector3.Lerp(transform.position, CoinManager.Instance.CoinDestination, Time.deltaTime * speed);
                yield return new WaitForEndOfFrame();
            }

            // 코인 획득 수치 적용
            CoinManager.Instance.GainCoin(10);

            // 파괴
            Destroy(gameObject);
        }
    }
}
