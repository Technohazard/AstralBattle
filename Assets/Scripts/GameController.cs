using System.Linq;
using System.Text;
using DefaultNamespace;
using UnityEngine;

/// <summary>
/// Handles game state and initialization.
/// </summary>
public class GameController : MonoBehaviour
{
    [Tooltip("Size of the Player Party to generate")]
    public int PartySize;
    
    [Tooltip("Size of the Enemy Party to generate")]
    public int EnemyPartySize;

    [Tooltip("Min level Players")]
    public int PlayerMinLevel;
    
    [Tooltip("Min level Enemies")]
    public int EnemyMinLevel;
    
    [Tooltip("Max level Players")]
    public int PlayerMaxLevel;
    
    [Tooltip("Max level Enemies")]
    public int EnemyMaxLevel;


    /// <summary>
    /// How long has the battle sim run?
    /// Master Time.
    /// </summary>
    public float BattleTimer = 0.0f;

    /// <summary>
    /// The absolute minimum time in the future to queue an action.
    /// </summary>
    public float BaseActionTime = 0.01f;
    
    // How far in the future to queue damage actions
    public float BaseDamageTime = 1.05f;
    
    public enum GameStatesEnum
    {
        Default,
        Initialize,
        Battle,
        Results,
    }
    // Initialize - Game is setting up data
    // Battle - Battle is running
    // Results - battle is finished

    /// <summary>
    /// The current gameplay state
    /// </summary>
    public GameStatesEnum CurrentGameState;

    /// <summary>
    /// Reference to the main UI Controller component / GameObject.
    /// </summary>
    public UIController UIController;
    
    private void Awake()
    {
        UIController ??= FindObjectOfType<UIController>();
        SetState(GameStatesEnum.Initialize);
    }

    private void Update()
    {
        OnStateUpdate(CurrentGameState);
    }

    /// <summary>
    /// Change to a new game state
    /// </summary>
    /// <param name="newState"></param>
    private void SetState(GameStatesEnum setState)
    {
        if (setState == CurrentGameState) return;
        OnExitState(CurrentGameState);
        CurrentGameState = setState;
        UIController.SetStateLabel(CurrentGameState);
        OnEnterState(CurrentGameState);
    }
    
    /// <summary>
    /// Call when exiting a previous state.
    /// </summary>
    /// <param name="oldState"></param>
    private void OnEnterState(GameStatesEnum newState)
    {
        switch (newState)
        {
            case GameStatesEnum.Initialize:
            {
                OnEnterState_Initialize();
            } break;
            case GameStatesEnum.Battle:
            {
                OnEnterState_Battle();
            } break;
            case GameStatesEnum.Results:
            {
                OnEnterState_Results();
            } break;
        }
    }
    
    public Party PlayerParty;
    public Party EnemyParty;

    private void OnEnterState_Initialize()
    {
        // Create both new parties.
        //PlayerParty = new Party(PartySize, Factions.FactionEnum.Player);
        //EnemyParty = new Party(EnemyPartySize, Factions.FactionEnum.Enemy);
        
        // create parties with level range
        PlayerParty = new Party(PartySize, Factions.FactionEnum.Player, PlayerMinLevel, PlayerMaxLevel);
        EnemyParty = new Party(EnemyPartySize, Factions.FactionEnum.Enemy, EnemyMinLevel, EnemyMaxLevel);

        UIController.ShowParty(PlayerParty);
        UIController.ShowParty(EnemyParty);

        UIController.ResultsPanel.SetActive(false);

        // Expected: parties have 1 Agent of the appropriate faction in each.
        SetState(GameStatesEnum.Battle);
    }

    // The cointoss happens on entering battle
    // whoever wins it goes first in the battle update
    private bool cointoss;

    private void OnEnterState_Battle()
    {
        BattleTimer = 0.0f; // Reset battle timer.
        UIController.SetTimeLabel(BattleTimer);
        
        cointoss = Mathf.RoundToInt(Random.Range(0.0f, 1.0f)) == 1;
        if (cointoss)
        {
            Debug.Log("Player Won the Flip");
        }
        else
        {
            Debug.Log("Enemy Won the Flip");
        }
    }

    private void OnEnterState_Results()
    {
        UIController.ResultsPanel.SetActive(true);
        UIController.Results_text.text = GetBattleResultsText();
        switch (VictoriousFaction)
        {
            case Factions.FactionEnum.Player:
            {
                UIController.Victory_text.gameObject.SetActive(true);
                UIController.Defeat_text.gameObject.SetActive(false);
            } break;
            case Factions.FactionEnum.Enemy:
            {
                UIController.Defeat_text.gameObject.SetActive(true);
                UIController.Victory_text.gameObject.SetActive(false);
            } break; 
        }
    }

    private string GetBattleResultsText()
    {
        var sb = new StringBuilder();
        if (VictoriousFaction == Factions.FactionEnum.Player)
        {
            sb.Append($"All is at peace. The forces of light have won.");
        }
        else
        {
            sb.Append($"Darkness rules the heavens. You have lost.");
        } 
        
        
        return sb.ToString();
    }

    /// <summary>
    /// Call when exiting a previous state.
    /// </summary>
    /// <param name="oldState"></param>
    private void OnExitState(GameStatesEnum oldState)
    {
        Debug.Log($"leaving GameState: { oldState }");
        switch (oldState)
        {
            case GameStatesEnum.Battle:
            {
                OnExitState_Battle();
            } break;
        }
        
    }

    private void OnExitState_Battle()
    {
        // Battle timer should stop updating on its own.
        
    }
    

    /// <summary>
    /// Call during update to execute state-dependent logic.
    /// </summary>
    /// <param name="state"></param>
    private void OnStateUpdate(GameStatesEnum state)
    {
        switch (state)
        {
            case GameStatesEnum.Battle:
            {
                BattleTimer += Time.deltaTime;
                UIController.SetTimeLabel(BattleTimer);
                
                if (cointoss)
                {
                    UpdateAgents(Factions.FactionEnum.Player);
                    UpdateAgents(Factions.FactionEnum.Enemy);
                }
                else
                {
                    UpdateAgents(Factions.FactionEnum.Enemy);
                    UpdateAgents(Factions.FactionEnum.Player);
                }
            } break;
        }
    }
    
    private void UpdateAgents(Factions.FactionEnum updateFaction)
    {
        switch (updateFaction)
        {
            case Factions.FactionEnum.Player:
            {
                if (!PlayerParty.Agents.Any(agent => agent.Stats.HP >0))
                {
                    // quick check to see if we don't have any alive players
                    SetGameOver(Factions.FactionEnum.Enemy);
                }
                else
                {
                    foreach (var agent in PlayerParty.Agents)
                    {
                        UpdateAgent(agent);
                    }
                }
            }
                break;
            case Factions.FactionEnum.Enemy:
            { if (!EnemyParty.Agents.Any(agent => agent.Stats.HP >0))
                {
                    // quick check to see if we don't have any alive players
                    SetGameOver(Factions.FactionEnum.Player);
                }
                else
                {
                    foreach (var agent in EnemyParty.Agents)
                    {
                        UpdateAgent(agent);
                    }
                }
            }
                break;
        }
    }

    private void UpdateAgent(Agent agent)
    {        
        if (agent.ActionReady)
        {
            if (agent.ShouldExecuteAction(BattleTimer))
            {
                ExecuteAgentTask(agent);
            }
        }
        else
        {
            // agent doesn't have an action ready, get a new one
            var newAction = agent.PickNextAction();
            // Queue this action based on their speed
            float newTime = BattleTimer + BaseActionTime;
            switch (newAction)
            {
                case AgentAction.AgentActionTypesEnum.Damage:
                {
                    if (agent.Stats.Speed > 0)
                    {
                        // scale base damage time by the inverse of our speed.
                        newTime += BaseDamageTime * (1 / agent.Stats.Speed);
                    }
                    else
                    {
                        newTime += BaseDamageTime;
                    }
                } break;
            } 
            agent.QueueAction(newTime);
        }
    }
    
    

    /// <summary>
    /// Execute this agent's next task
    /// </summary>
    /// <param name="agent"></param>
    private void ExecuteAgentTask(Agent agent)
    {
        switch (agent.NextActionTask.agentAction.ActionType)
        {
            case AgentAction.AgentActionTypesEnum.Damage:
            {
                DamageTask(agent);
                agent.ClearTask();
            } break;
        }
    }

    /// <summary>
    /// this agent has a damage task, do it!
    /// </summary>
    /// <param name="agent"></param>
    private void DamageTask(Agent agent)
    {
        Party targetParty;
        if (agent.Faction == Factions.FactionEnum.Player)
        {
            targetParty = EnemyParty;
        }
        else
        {
           targetParty = PlayerParty;
        }
        
        Agent targetAgent = null;
        if (agent.NextActionTask.targetIndex > 0
            && agent.NextActionTask.targetIndex < targetParty.Agents.Count)
        {
            // Perform action on specific agent
            targetAgent = targetParty.Agents[agent.NextActionTask.targetIndex];
        }
        else
        {
            if (targetParty.Agents.Count > 0)
            {
                targetAgent = targetParty.Agents.FirstOrDefault(target => target.Stats.HP > 0);
            }
            else
            {
                Debug.Log("No Agents in Target Party!");
            }
        }

        if (targetAgent != null)
        {
            targetAgent.Damage(agent.Stats.Attack);
        }
        else
        {
            Debug.Log("No valid Agent to target!");
            // game is probably over at this point
            SetGameOver(agent);
        }
    }

    /// <summary>
    /// Whoever the last 
    /// </summary>
    /// <param name="finalAgent"></param>
    public void SetGameOver(Agent finalAgent)
    {
        // finalAgent couldn't find a target with > HP on its opponent team
        // Confirm this
       SetGameOver(finalAgent.Faction);
        
        // no victory actually happened?
        // This would be a weird edge case.
    }

    /// <summary>
    /// Try and end the game
    /// Set the victorious Faction to finalFaction
    /// Checks to make sure we have 0 agents in opposing faction.
    /// </summary>
    /// <param name="finalFaction"></param>
    public void SetGameOver(Factions.FactionEnum finalFaction)
    {
        switch (finalFaction)
        {
            case Factions.FactionEnum.Player:
            {
                if (!EnemyParty.Agents.Any(agent => agent.Stats.HP > 0))
                {
                    // no enemy has HP > 0, player wins!
                    DeclareVictory(finalFaction);
                    return;
                }
            } break;
            case Factions.FactionEnum.Enemy:
            {
                if (!PlayerParty.Agents.Any(agent => agent.Stats.HP > 0))
                {
                    // no player has HP > 0, Enemy wins!
                    DeclareVictory(finalFaction);
                    return;
                }
            } break;
        }
    }

    private Factions.FactionEnum VictoriousFaction;
    public void DeclareVictory(Factions.FactionEnum winningFaction)
    {
        VictoriousFaction = winningFaction;
        SetState(GameStatesEnum.Results);
    }
}
