using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class Player : MonoBehaviour
    {
        public static Player Instance { get; private set; }
        public int AttackPower { get; private set; }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            Instance = this;
            AttackPower = 30;
        }

        public void Attack(int chainNumber)
        {
            PlayAttackAnimation();
            Debug.Log(MonsterManager.Instance.Monsters.Values.GetEnumerator().MoveNext());
            MonsterManager.Instance.Monsters.Values.GetEnumerator().Current.Health -= (int)(AttackPower * chainNumber * (1 + chainNumber * 0.1));
        }

        public void PlayAttackAnimation()
        {
            GetComponent<Animator>().SetTrigger("isAttacking");
        }
    }
}