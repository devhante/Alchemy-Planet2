using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class PrefabManager : MonoBehaviour
    {
        public static PrefabManager Instance { get; private set; }

        public GameObject tilePrefab;
        public GameObject backgroundPrefab;
        public GameObject[] materialPrefabs;
        public GameObject[] itemPrefabs;
        public GameObject[] recipePrefabs;
        public GameObject[] dropMaterialPrefabs;
        public GameObject monster;
        public GameObject popinBullet;
        public GameObject harpRadishealBullet;
        public GameObject line;
        public GameObject coin;
        public GameObject scoreText;
        public GameObject chickenCount;

        public Sprite unselectedBubble;
        public Sprite selectedBubble;
        public Sprite highlightedBubble;

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }
    }
}