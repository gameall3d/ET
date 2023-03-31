using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ET.Client
{
    [Invoke]
    public class LubanGetAllConfigBytes: AInvokeHandler<LubanConfigComponent.GetAllConfigBytes, Dictionary<Type, byte[]>>
    {
        public override Dictionary<Type, byte[]> Handle(LubanConfigComponent.GetAllConfigBytes args)
        {
            Dictionary<Type, byte[]> output = new Dictionary<Type, byte[]>();
            HashSet<Type> configTypes = EventSystem.Instance.GetTypes(typeof (LubanConfigAttribute));
            
            if (Define.IsEditor)
            {
                foreach (Type configType in configTypes)
                {
                    string configFilePath = $"../Config/GameConfigs/{configType.Name}.bytes";
                    output[configType] = File.ReadAllBytes(configFilePath);
                }
            }
            else
            {
                using (Root.Instance.Scene.AddComponent<ResourcesComponent>())
                {
                    const string configBundleName = "gameconfigs.unity3d";
                    ResourcesComponent.Instance.LoadBundle(configBundleName);
                    
                    foreach (Type configType in configTypes)
                    {
                        TextAsset v = ResourcesComponent.Instance.GetAsset(configBundleName, configType.Name) as TextAsset;
                        output[configType] = v.bytes;
                    }
                }
            }

            return output;
        }
    }
    
    [Invoke]
    public class LubanGetOneConfigBytes: AInvokeHandler<LubanConfigComponent.GetOneConfigBytes, byte[]>
    {
        public override byte[] Handle(LubanConfigComponent.GetOneConfigBytes args)
        {
            //TextAsset v = ResourcesComponent.Instance.GetAsset("config.unity3d", configName) as TextAsset;
            //return v.bytes;
            throw new NotImplementedException("client cant use LoadOneConfig");
        }
    }
}

