using System.Collections.Generic;
using ET;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace CubeFighter
{
    [FriendOf(typeof (GameNumericComponent))]
    [FriendOf(typeof (Modifier))]
    public static class GameNumericComponentSystem
    {
        
        public static float GetAsFloat(this GameNumericComponent self, int numericType)
        {
            return (float) self.GetByKey(numericType) / 10000;
        }

        public static int GetAsInt(this GameNumericComponent self, int numericType)
        {
            return (int) self.GetByKey(numericType);
        }

        public static long GetAsLong(this GameNumericComponent self, int numericType)
        {
            return self.GetByKey(numericType);
        }

        public static void Set(this GameNumericComponent self, int nt, float value)
        {
            self[nt] = (int) (value * 10000);
        }

        public static void Set(this GameNumericComponent self, int nt, int value)
        {
            self[nt] = value;
        }

        public static void Set(this GameNumericComponent self, int nt, long value)
        {
            self[nt] = value;
        }

        public static void SetNoEvent(this GameNumericComponent self, int numericType, long value)
        {
            self.Insert(numericType, value, false);
        }

        public static void InitNumericValue(this GameNumericComponent self, int numericType, long value)
        {
            self.NumericDic[numericType] = value;
        }

        public static void Insert(this GameNumericComponent self, int numericType, long value, bool isPublicEvent = true)
        {
            long oldValue = self.GetByKey(numericType);
            if (oldValue == value)
            {
                return;
            }

            self.NumericDic[numericType] = value;

            if (numericType >= NumericType.Max)
            {
                self.Update(numericType, isPublicEvent);
                return;
            }

            if (isPublicEvent)
            {
                EventSystem.Instance.Publish(self.DomainScene(),
                    new EventType.NumericChange() { GameUnit = self.GetParent<GameUnit>(), New = value, Old = oldValue, NumericType = numericType });
            }
        }

        public static void AddModifer(this GameNumericComponent self, Modifier modifier)
        {
            List<Modifier> modifierList;
            if (self.Modifiers.TryGetValue(modifier.NumericType, out modifierList))
            {
                modifierList.Add(modifier);
            }
            else
            {
                modifierList = new List<Modifier>();
                modifierList.Add(modifier);
                self.Modifiers[modifier.NumericType] = modifierList;
            }
            self.Update(modifier.NumericType,false);

        }

        public static void RmoveModifer(this GameNumericComponent self, Modifier modifier)
        {
            List<Modifier> modifierList;
            if (self.Modifiers.TryGetValue(modifier.NumericType, out modifierList))
            {
                modifierList.Remove(modifier);
                self.Update(modifier.NumericType,false);
            }
        }

        public static long GetByKey(this GameNumericComponent self, int key)
        {
            long value = 0;
            self.NumericDic.TryGetValue(key, out value);
            List<Modifier> modifierList;
            if (self.Modifiers.TryGetValue(key, out modifierList))
            {
                foreach (var v in modifierList)
                {
                   // value += v.Value;
                }
            }
            return value;
        }

       

        public static void Update(this GameNumericComponent self, int numericType, bool isPublicEvent)
        {
            int final = (int)numericType / 10;
            int bas = final * 10 + 1;
            int add = final * 10 + 2;
            int pct = final * 10 + 3;
            int finalAdd = final * 10 + 4;
            int finalPct = final * 10 + 5;

            // 一个数值可能会多种情况影响，比如速度,加个buff可能增加速度绝对值100，也有些buff增加10%速度，所以一个值可以由5个值进行控制其最终结果
            // final = (((base + add) * (100 + pct) / 100) + finalAdd) * (100 + finalPct) / 100;
            long result = (long) (((self.GetByKey(bas) + self.GetByKey(add)) * (100 + self.GetByKey(pct)) / 100f + self.GetByKey(finalAdd)) *
                (100 + self.GetByKey(finalPct)) / 100f);
            self.Insert(final, result, isPublicEvent);
        }
    }

    namespace EventType
    {
        public struct NumericChange
        {
            public GameUnit GameUnit;
            public int NumericType;
            public long Old;
            public long New;
        }
    }

    [ComponentOf()]
    public class GameNumericComponent: Entity, IAwake, ISerializeToEntity
    {
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, long> NumericDic = new Dictionary<int, long>();

        [BsonIgnore]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, List<Modifier>> Modifiers = new Dictionary<int, List<Modifier>>();

        public long this[int numericType]
        {
            get
            {
                return this.GetByKey(numericType);
            }
            set
            {
                this.Insert(numericType, value);
            }
        }
    }
}

