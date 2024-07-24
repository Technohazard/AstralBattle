using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AgentTileScript : MonoBehaviour
{
    private AgentStats statsCache;

    public TextMeshProUGUI displayName_label;
    public TextMeshProUGUI HP_label;
    public TextMeshProUGUI Attack_label;
    public TextMeshProUGUI Defense_label;
    public TextMeshProUGUI Speed_label;

    public Image Tile_BG;
    public CanvasGroup canvasGroup;

    public void SetTileData(string setName, AgentStats setStats)
    {
        statsCache = setStats;
        displayName_label.text = setName;
        HP_label.text =      $"HP: { statsCache.HP.ToString() }";
        Attack_label.text =  $"Attack: { statsCache.Attack.ToString() }";
        Defense_label.text = $"Defense: { statsCache.Defense.ToString() }";
        Speed_label.text =   $"Speed: { statsCache.Speed.ToString() }";
    }

    /// <summary>
    /// Set internal panel style based on heuristic
    /// </summary>
    public void SetTileStyle(Factions.FactionEnum faction = Factions.FactionEnum.Player)
    {
        if (statsCache.HP <= 0)
        {
            // Dead Style
            Tile_BG.color = Color.red;
            canvasGroup.alpha = 0.5f;
        }
        else
        {
            switch (faction)
            {
                case Factions.FactionEnum.Player: Tile_BG.color = Color.white;
                    break;
                case Factions.FactionEnum.Enemy: Tile_BG.color = Color.gray;
                    break;
            }
            canvasGroup.alpha = 1.0f;
        }
    }
}
