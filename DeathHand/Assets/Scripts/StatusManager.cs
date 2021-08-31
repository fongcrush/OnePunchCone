public class StatusManager
{
    private int maxHP = 0;
    private int HP = 0;
    private int maxMP = 0;
    private int MP = 0;
    private int power = 0;



    public StatusManager(int MaxHP, int MaxMP, int Power) { HP = maxHP = MaxHP; MP = maxMP = MaxMP; power = Power; }

    public int MaxHP { get { return maxHP; } set { maxHP = value; } }
    public int curHP { get { return HP; } set { HP = value; } }
    public int MaxMP { get { return maxMP; } set { maxMP = value; } }
    public int curMP { get { return MP; } set { MP = value; } }
    public int Power { get { return power; } set { power = value; } }
    public void ChangeHP(int value)
    {
        HP += value;
        
        if(HP > maxHP)
            HP = maxHP;
        else if(HP < 0)
            HP = 0;
    }
    public void MaximizIngHP()
    {
        HP = maxHP;
    }
    public void MakeZero()
    {
        HP = 0;
    }
    public bool IsZero() { return HP == 0; }
    public int GetHP() { return HP; }
    public int GetMaxHP() { return maxHP; }
    public float GetHP_Ratio() { return (float)HP / maxHP; }
    public float GetST_Ratio() { return (float)MP / maxMP; }
    public void ChangeMP(int value)
    {
        MP += value;

        if(value > maxMP)
            MP = maxMP;
        else if(HP < 0)
            MP = 0;
    }

}