using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class PrefabManager : MonoBehaviour
    {
        public static PrefabManager Instance { get; private set; }

        public GameObject tilePrefab;
        public GameObject[] materialPrefabs;
        public GameObject[] recipePrefabs;

        public Sprite UnselectedBubble;
        public Sprite SelectedBubble;

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