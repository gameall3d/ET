namespace ET
{
    [ChildOf(typeof(SkillComponent))]
    public class Skill: Entity, IAwake
    {
        // 技能的配置Id
        public int ConfigId;
    }
}
