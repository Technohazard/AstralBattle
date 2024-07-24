using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    // Display the current Game State
    #region Game State Display
    public TextMeshProUGUI State_Label;
    public string state_label_text;
    public void SetStateLabel(GameController.GameStatesEnum gameState)
    {
        switch (gameState)
        {
            case GameController.GameStatesEnum.Initialize:
            {
                state_label_text = "Initialize";
            } break;
            case GameController.GameStatesEnum.Battle:
            {
                state_label_text = "Battle";
            } break;
            case GameController.GameStatesEnum.Results:
            {
                state_label_text = "Results";
            } break;
        }

        State_Label.text = state_label_text;
    }
    #endregion // Game State Display
    
    // Display the current Battle Time
    #region Battle Time Display
    public TextMeshProUGUI Time_Label;
    public string time_label_text;
    private string time_label_default = "Time: {0}";

    public void SetTimeLabel(float timeAmount)
    {
        time_label_text = String.Format(time_label_default, timeAmount);
        Time_Label.text = time_label_text;
        ShowTimeLabel(true);
    }

    public void ShowTimeLabel(bool show)
    {
        if (Time_Label.gameObject.activeSelf != show)
        {
            Time_Label.gameObject.SetActive(show);
        }
    }
    #endregion // Battle Time Display

    [Header("Prefabs")]
    [Tooltip("Prefab for Agent Tile")]
    public AgentTileScript AgentTilePrefab;

    /// <summary>
    /// Add Tiles to these parents.
    /// </summary>
    [Tooltip("Scroll Rect Faction Parent Context")]
    public Transform [] ScrollContexts;
    
    // Turn off/on the results at game end.
    [Tooltip("Battle Results Panel Scene Object")]
    public GameObject ResultsPanel;
    
    // container for battle stats results
    [Tooltip("Battle Results text box")]
    public TextMeshProUGUI Results_text;

    // container for battle stats results
    [Tooltip("Victory text")]
    public TextMeshProUGUI Victory_text;

    
    // container for battle stats results
    [Tooltip("Defeat text")]
    public TextMeshProUGUI Defeat_text;
    
    public void ShowParty(Party party)
    {
        foreach (var agent in party.Agents)
        {
            var factionInt = (int)agent.Faction;
            if (factionInt >= 0 && factionInt < ScrollContexts.Length)
            {
                var newTile = AddTile(agent, ScrollContexts[factionInt]);
                newTile.SetTileData(agent.DisplayName, agent.Stats);
                newTile.SetTileStyle(agent.Faction);
                agent.TileReference = newTile;
            }
        }
    }

    /// <summary>
    /// Keep track of which tiles we create.
    /// </summary>
    private List<AgentTileScript> createdAgentTiles = new List<AgentTileScript>();
    private AgentTileScript AddTile(Agent agent, Transform parentTransform)
    {
        var newTile = Instantiate(AgentTilePrefab, parentTransform);
        createdAgentTiles.Add(newTile);
        return newTile;
    }
}
