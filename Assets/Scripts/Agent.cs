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

    // what level is this?
    // Determines stats on generation.
    public int Level;
    
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
    
    public Agent(Factions.FactionEnum setFaction, int level)
    {
        Level = level;
        Faction = setFaction;
        Stats = new AgentStats(setFaction, Level);
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
                switch (Level)
                {
                    case 0:
                    {
                        tempName = "Bad Goblin";
                        break;
                    }
                    case 1:
                    {
                        tempName = "Skeleton";
                        break;
                    }
                    case 2 :
                    {
                        tempName = "Slime";
                        break;
                    }
                    case 3 :
                    {
                        tempName = "Kobold";
                        break;
                    }
                    case 4 :
                    {
                        tempName = "Strong Goblin";
                        break;
                    }
                    case 5:
                    {
                        tempName = "Carnivore Vine";
                        break;
                    }
                    case > 5 and < 10:
                    {
                        tempName = "Therion";
                        break;
                    }
                    case >= 10 and < 80:
                    {
                        tempName = "Foot Soldier";
                        break;
                    }
                    case > 80 and < 90:
                    {
                        tempName = "Wyrm";
                        break;
                    }
                    case >= 90 and < 96:
                    {
                        tempName = "Demon";
                        break;
                    }
                    case >= 96 and <= 99:
                    {
                        tempName = "Demon Lord";
                        break;
                    }
                    case > 99:
                    {
                        tempName = "Devil King";
                        break;
                    }
                }
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