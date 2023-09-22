namespace ET
{
    public class SkillComponentAwakeSystem: AwakeSystem<SkillComponent>
    {
        protected override void Awake(SkillComponent self)
        {
            self.Awake();
        }
    }

    public static class SkillComponentSystem
    {
        public static void Awake(this SkillComponent self)
        {
            
        }
    }
}
