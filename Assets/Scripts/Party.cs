using System.Collections.Generic;

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
        
        public void Populate(int numberAgents, Factions.FactionEnum faction)
        {
            Faction = faction;
            Agents = new List<Agent>();
            for (int i = 0; i < numberAgents; i++)
            {
                Agents.Add(new Agent(Faction));
            }
        }
    }
}