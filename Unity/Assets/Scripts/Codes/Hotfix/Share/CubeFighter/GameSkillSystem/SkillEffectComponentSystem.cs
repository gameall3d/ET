
namespace ET
{
    public class SkillEffectComponentAwakeSystem: AwakeSystem<SkillEffectComponent>
    {
        protected override void Awake(SkillEffectComponent self)
        {
            self.Awake();
        }
    }
    
    public class SkillEffectComponentDestroySystem: DestroySystem<SkillEffectComponent>
    {
        protected override void Destroy(SkillEffectComponent self)
        {

        }
    }
    
    [FriendOf(typeof (SkillEffectComponent))]
    public static class SkillEffectComponentSystem
    {
        public static void Awake(this SkillEffectComponent self)
        {
        }
    }
}
