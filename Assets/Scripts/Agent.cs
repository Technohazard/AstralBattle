using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Agent
{
    public Factions.FactionEnum Faction;
    public AgentStats Stats;
    public string DisplayName;
    
    // when a UI tile gets created for this agent
    // hold a reference here
    // so we can do stuff to it based on state.
    public AgentTileScript TileReference;
    
    public Agent()
    {
        Faction = Factions.FactionEnum.Player;
        Stats = new AgentStats();
        SetName();
    }

    public Agent(Factions.FactionEnum setFaction)
    {
        Faction = setFaction;
        Stats = new AgentStats(setFaction);
        SetName();
    }
    
    public void SetName()
    {
        string tempName = string.Empty;
        switch (Faction)
        {
            case Factions.FactionEnum.Player:
            {
                tempName = "Knight Able";
            }  break;
            case Factions.FactionEnum.Enemy:
            {
                tempName = "Bad Goblin";
            } break;
        }
        DisplayName = tempName;
    }
    
    #region Agent Action Tasks
    private List<AgentActionTask> _actionTasks = new List<AgentActionTask>();

    // * Characters queue up actions to be executed based on their speed, in real time

    /// <summary>
    /// Does the agent have an action ready
    /// waiting to execute at time.
    /// </summary>
    public bool ActionReady = false;
    private AgentActionTask _nextActionTask;

    public AgentActionTask NextActionTask
    {
        get => _nextActionTask;
        private set => _nextActionTask = value;
    }
    public void QueueAction(float time)
    {
        QueueAction(time, AgentAction.AgentActionTypesEnum.Damage);
    }

    public void QueueAction(float time, AgentAction.AgentActionTypesEnum type)
    {
        _nextActionTask = new AgentActionTask(time, new AgentAction(type), 0);
        _actionTasks.Add(_nextActionTask);
        ActionReady = true;
    }

    /// <summary>
    /// Is this agent ready to execute the first task in its queue?
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public bool ShouldExecuteAction(float time)
    {
        if (Stats.HP > 0
        && _nextActionTask.taskTime <= time)
        {
            return true;
        }

        return false;
    }

    // what type of action should i do next?
    public AgentAction.AgentActionTypesEnum PickNextAction()
    {
        return AgentAction.AgentActionTypesEnum.Damage;
    }
    #endregion

    private int calcDamage;
    public void Damage(int amount)
    {
        Debug.Log($"{DisplayName}: Receiving { amount } Damage ");
        calcDamage = amount - Stats.Defense;
        if (calcDamage > 0)
        {        
            Debug.Log($"{DisplayName}: took { calcDamage } damage (HP: { Stats.HP }");
            Stats.HP -= calcDamage;
            
            // Update Tile UI with new value.
            TileReference.SetTileData(DisplayName, Stats);
            TileReference.SetTileStyle(Faction);
        }
        else
        {
            Debug.Log($"{DisplayName}: blocked { amount } damage. (DEF: { Stats.Defense })");

        }
    }

    public void ClearTask()
    {
        _actionTasks.Remove(_nextActionTask);
        _nextActionTask = null;
        
        // if we have more tasks, queue the next
        if (_actionTasks.Count > 0)
        {
            _nextActionTask = _actionTasks.First();
            ActionReady = true;
        }
        
        ActionReady = false;
    }
}