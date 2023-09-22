using UnityEditor;

namespace ET
{
    public static class ToolsEditor
    {
        public static void ExcelExporter()
        {
#if UNITY_EDITOR_OSX || UNITY_EDITOR_LINUX
            const string tools = "./Tool";
#else
            const string tools = ".\\Tool.exe";
#endif
            ShellHelper.Run($"{tools} --AppType=ExcelExporter --Console=1", "../Bin/");
        }
        
        public static void Proto2CS()
        {
#if UNITY_EDITOR_OSX || UNITY_EDITOR_LINUX
            const string tools = "./Tool";
#else
            const string tools = ".\\Tool.exe";
#endif
            ShellHelper.Run($"{tools} --AppType=Proto2CS --Console=1", "../Bin/");
        }
        
        public static void GameConfigExporter()
        {
#if UNITY_EDITOR_OSX
            const string tools = "./gen.sh";
#else
            const string tools = ".\\gen.bat";
#endif
            ProcessHelper.Run($"{tools}", "", "../ConfigSource/");
        }
    }
}