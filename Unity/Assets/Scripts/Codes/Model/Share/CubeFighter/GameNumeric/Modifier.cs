using ET;

namespace CubeFighter
{
    [ChildOf()]
    public class Modifier:Entity,IAwake<int>
    {
        public int ConfigId;
        // public SkillEffectConfig Config=>SkillEffectConfigCategory.Instance.Get(ConfigId);
        public float Value;
        public float OneValue;  //单次的数值
        public int NumericType;
        public int ValueType; //1表示绝对值，2表示百分比（不是所有属性）
    }
}