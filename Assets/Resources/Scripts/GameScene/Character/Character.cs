using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class Character : MonoBehaviour, IHealth
    {
        public int MaxHealth { get; set; }

        private int health;
        public int Health
        {
            get { return health; }
            set { health = Mathf.Clamp(value, 0, MaxHealth); }
        }

        protected virtual void Awake()
        {
            MaxHealth = 100;
            Health = MaxHealth;
        }
    }
}