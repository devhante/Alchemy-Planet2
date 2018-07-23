using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class PrefabManager : MonoBehaviour
    {
        public static PrefabManager Instance { get; private set; }

        public GameObject tilePrefab;
        public GameObject[] materialPrefabs;
        public GameObject[] itemPrefabs;
        public GameObject[] recipePrefabs;
        public GameObject monster;
        public GameObject popinBullet;
        public GameObject harpRadishealBullet;
        public GameObject line;
        public GameObject coin;

        public Sprite unselectedBubble;
        public Sprite selectedBubble;

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
        }
    }
}