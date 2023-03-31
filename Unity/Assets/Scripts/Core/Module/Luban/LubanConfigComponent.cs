using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bright.Serialization;

namespace ET
{
    /// <summary>
    /// LubanConfig组件会扫描所有的有ConfigAttribute标签的配置,加载进来
    /// </summary>
    public class LubanConfigComponent : Singleton<LubanConfigComponent>
    {
	    public struct GetAllConfigBytes
        {
        }
        
        public struct GetOneConfigBytes
        {
            public string ConfigName;
        }
		
        private readonly Dictionary<Type, IConfigSingleton> allConfig = new Dictionary<Type, IConfigSingleton>();

		public override void Dispose()
		{
			foreach (var kv in this.allConfig)
			{
				kv.Value.Destroy();
			}
		}

		public object LoadOneConfig(Type configType)
		{
			this.allConfig.TryGetValue(configType, out IConfigSingleton oneConfig);
			if (oneConfig != null)
			{
				oneConfig.Destroy();
			}
			
			byte[] oneConfigBytes = EventSystem.Instance.Invoke<GetOneConfigBytes, byte[]>(new GetOneConfigBytes() {ConfigName = configType.FullName});

			object category = CreateCategoryFromBytes(configType, oneConfigBytes);;
			IConfigSingleton singleton = category as IConfigSingleton;
			singleton.Register();
			
			this.allConfig[configType] = singleton;
			return category;
		}
		
		public void Load()
		{
			this.allConfig.Clear();
			Dictionary<Type, byte[]> configBytes = EventSystem.Instance.Invoke<GetAllConfigBytes, Dictionary<Type, byte[]>>(new GetAllConfigBytes());

			foreach (Type type in configBytes.Keys)
			{
				byte[] oneConfigBytes = configBytes[type];
				this.LoadOneInThread(type, oneConfigBytes);
			}
		}
		
		public async ETTask LoadAsync()
		{
			this.allConfig.Clear();
			Dictionary<Type, byte[]> configBytes = EventSystem.Instance.Invoke<GetAllConfigBytes, Dictionary<Type, byte[]>>(new GetAllConfigBytes());

			using ListComponent<Task> listTasks = ListComponent<Task>.Create();
			
			foreach (Type type in configBytes.Keys)
			{
				byte[] oneConfigBytes = configBytes[type];
				Task task = Task.Run(() => LoadOneInThread(type, oneConfigBytes));
				listTasks.Add(task);
			}

			await Task.WhenAll(listTasks.ToArray());
		}
		
		private void LoadOneInThread(Type configType, byte[] oneConfigBytes)
		{
			object category = CreateCategoryFromBytes(configType, oneConfigBytes);
			
			lock (this)
			{
				IConfigSingleton singleton = category as IConfigSingleton;
				singleton.Register();
				this.allConfig[configType] = singleton;
			}
		}
		
		private object CreateCategoryFromBytes(Type type, byte[] bytes)
		{
			object category = Activator.CreateInstance(type, new ByteBuf(bytes));
			return category;
		}
    }
}

