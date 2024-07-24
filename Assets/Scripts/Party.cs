using System.Collections.Generic;
using Unity.Mathematics;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class Party
    {
        public List<Agent> Agents;
        public Factions.FactionEnum Faction;
        
        public Party()
        {
            Populate(1, Factions.FactionEnum.Player);
        }
            
        public Party(int numberAgents)
        {
            Populate(numberAgents, Factions.FactionEnum.Player);
        }
        
        public Party(int numberAgents, Factions.FactionEnum faction)
        {
            Populate(numberAgents, faction);
        }
        
        public Party(int numberAgents, Factions.FactionEnum faction, int minlevel, int maxlevel)
        {
            Populate(numberAgents, faction, minlevel, maxlevel);
        }
        
        public void Populate(int numberAgents, Factions.FactionEnum faction)
        {
            Faction = faction;
            Agents = new List<Agent>();
            for (int i = 0; i < numberAgents; i++)
            {
                Agents.Add(new Agent(Faction));
            }
        }

        public void Populate(int numberAgents, Factions.FactionEnum faction, int minlevel, int maxLevel)
        {
            minlevel = math.clamp(minlevel, 0, maxLevel);
            Faction = faction;
            Agents = new List<Agent>();
            for (int i = 0; i < numberAgents; i++)
            {
                var eLevel = Random.Range(minlevel, maxLevel);
                Agents.Add(new Agent(Faction, eLevel));
            } 
        }
        
    }
}