public class AgentStats
{
    public int HP;
    public int Attack;
    public int Defense;
    public float Speed; // higher = less time to take actions.
    
    public int Max_HP; // added stat for game balance

    public AgentStats()
    {
        Max_HP = 10;
        HP = Max_HP;
        Attack = 1;
        Defense = 1;
        Speed = 1.0f;
    }
    
    public AgentStats(Factions.FactionEnum faction)
    {
        switch (faction)
        {
            case Factions.FactionEnum.Player:
            {
                Max_HP = 10;
                HP = Max_HP;
                Attack = 1;
                Defense = 1;
                Speed = 1.0f; 
            } break;
            case Factions.FactionEnum.Enemy:
            {
                Max_HP = 2;
                HP = Max_HP;
                Attack = 1;
                Defense = 0;
                Speed = 1.0f;
            } break;
        }

    }
}