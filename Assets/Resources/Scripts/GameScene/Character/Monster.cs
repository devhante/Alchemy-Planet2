namespace AlchemyPlanet.GameScene
{
    public class Monster : Character, IAttackable
    {
        public float AttackCoolTime { get; private set; }
        public bool CanAttack { get; private set; }

        protected override void Awake()
        {
            base.Awake();
        }

        protected virtual void Update()
        {
            if (CanAttack == true)
            {

            }
        }

        public void Attack()
        {

        }
    }
}