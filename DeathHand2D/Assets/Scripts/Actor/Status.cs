public class Status
{
    private float maxHP = 0;
    private float HP = 0;
    private float maxMP = 0;
    private float MP = 0;
    private float power = 0;

    public Status(float MaxHP, float MaxMP, float Power) { HP = maxHP = MaxHP; MP = maxMP = MaxMP; power = Power; }

    public float MaxHP { get { return maxHP; } set { maxHP = value; } }
    public float curHP { get { return HP; } set { HP = value; } }
    public float MaxMP { get { return maxMP; } set { maxMP = value; } }
    public float curMP { get { return MP; } set { MP = value; } }
    public float Power { get { return power; } set { power = value; } }
    public void ChangeHP(float value)
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
    public float GetHP_Ratio() { return (float)HP / maxHP; }
    public float GetST_Ratio() { return (float)MP / maxMP; }
    public void ChangeMP(float value)
    {
        MP += value;

        if(value > maxMP)
            MP = maxMP;
        else if(HP < 0)
            MP = 0;
    }

}