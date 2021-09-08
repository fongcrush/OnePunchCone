public class SkillInfo
{
	public int damage;
	public float coolTime;
	public float curTime;

	public SkillInfo(int _damage, float _coolTime, float _curTime)
	{
		damage = _damage;
		coolTime = _coolTime;
		curTime = _curTime;
	}

	public void SetSkill(int _damage, float _coolTime, float _curTime)
	{
		damage = _damage;
		coolTime = _coolTime;
		curTime = _curTime;
	}
}