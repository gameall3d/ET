namespace ET
{
    public class SkillAwakeSystem: AwakeSystem<Skill>
    {
        protected override void Awake(Skill self)
        {
            self.Awake();
        }
    }

    public static class SkillSystem
    {
        public static void Awake(this Skill self)
        {
            
        }

        public static void CastSkill(this Skill self, GameUnit fromUnit, GameUnit toUnit)
        {
            
        }
    }
}
