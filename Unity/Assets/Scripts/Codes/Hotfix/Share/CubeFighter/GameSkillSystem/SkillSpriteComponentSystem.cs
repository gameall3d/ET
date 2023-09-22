namespace ET
{
    public class SkillSpriteComponentAwakeSystem: AwakeSystem<SkillSpriteComponent>
    {
        protected override void Awake(SkillSpriteComponent self)
        {
            self.Awake();
        }
    }

    public static class SkillSpriteComponentSystem
    {
        public static void Awake(this SkillSpriteComponent self)
        {
            
        }
    }
}
