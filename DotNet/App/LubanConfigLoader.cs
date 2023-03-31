using System;
using System.Collections.Generic;
using System.IO;

namespace ET.Server
{
    [Invoke]
    public class LubanGetAllConfigBytes: AInvokeHandler<LubanConfigComponent.GetAllConfigBytes, Dictionary<Type, byte[]>>
    {
        public override Dictionary<Type, byte[]> Handle(LubanConfigComponent.GetAllConfigBytes a)
        {
            Dictionary<Type, byte[]> output = new Dictionary<Type, byte[]>();
            
            HashSet<Type> configTypes = EventSystem.Instance.GetTypes(typeof (LubanConfigAttribute));
            foreach (Type configType in configTypes)
            {
                string  configFilePath = $"../Config/GameConfigs/{configType.Name}.bytes";
                output[configType] = File.ReadAllBytes(configFilePath);
            }

            return output;
        }
    }
    
    [Invoke]
    public class LubanGetOneConfigBytes: AInvokeHandler<LubanConfigComponent.GetOneConfigBytes, byte[]>
    {
        public override byte[] Handle(LubanConfigComponent.GetOneConfigBytes args)
        {
            byte[] configBytes = File.ReadAllBytes($"../Config/GameConfigs/{args.ConfigName}.bytes");
            return configBytes;
        }
    }
}

