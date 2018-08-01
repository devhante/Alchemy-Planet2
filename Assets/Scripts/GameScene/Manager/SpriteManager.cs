using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class SpriteManager : MonoBehaviour
    {
        public static SpriteManager Instance { get; private set; }

        public Dictionary<MaterialName, int> MaterialNameIndex;
        public Sprite[] MaterialSprites;
        public Sprite[] HighlightedMaterialSprites;

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            MaterialNameIndex = new Dictionary<MaterialName, int>();
            MaterialName[] materialNames = (MaterialName[])Enum.GetValues(typeof(MaterialName));
            for (int i = 0; i < materialNames.Length; i++)
                MaterialNameIndex.Add(materialNames[i], i);  
        }

        public Sprite GetHighlightedMaterialSprite(MaterialName materialName)
        {
            int index = MaterialNameIndex[materialName];
            return HighlightedMaterialSprites[index];
        }
    }
}