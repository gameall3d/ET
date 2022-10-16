namespace ET
{
    public static class CastSkillComponentSystem
    {
        public class CastSkillComponentAwakeSystem : AwakeSystem<CastSkillComponent>
        {
            protected override void Awake(CastSkillComponent self)
            {
                self.Awake();
            }
        }

    

        private static void Awake(this CastSkillComponent self)
        {
        }

        public static void CastSkill(this CastSkillComponent self, int skillConfigId)
        {
            
        }
    }
    
}

