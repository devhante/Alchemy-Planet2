using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleUIscript : MonoBehaviour
{
    public static SampleUIscript Instance;

    public Text PlayerIDText;
    public Text PlayerNameText;

    public Text PlayerUniCoinText;
    public Text PlayerCosmoStonText;

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        PlayerIDText.text = DataManager.Instance.CurrentPlayerData.player_id;
        PlayerNameText.text = DataManager.Instance.CurrentPlayerData.player_name;
        PlayerUniCoinText.text = DataManager.Instance.CurrentPlayerData.unicoin.ToString();
        PlayerCosmoStonText.text = DataManager.Instance.CurrentPlayerData.cosmoston.ToString();
    }
}