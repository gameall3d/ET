/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using System.Threading.Tasks;

namespace ET
{
    public sealed class FUI_Login : FUI
    {	
        public const string UIPackageName = "Login";
        public const string UIResName = "Login";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GComponent FGComp;
                        
    	public GImage m_BG;
    	public GImage m_n7;
    	public FUI_LoginButton m_LoginButton;
    	public GTextField m_n5;
    	public GTextField m_n0;
    	public GImage m_AccountNameInputBG;
    	public GTextInput m_AccountName;
    	public GTextField m_n2;
    	public GImage m_PasswordInputBG;
    	public GTextInput m_Password;
    	public const string URL = "ui://3paxr1cxt9ak0";

     
        private static GObject CreateGObject()
        {
            return UIPackage.CreateObject(UIPackageName, UIResName);
        }

        private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
        {
            UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
        }
                
       
        public static FUI_Login CreateInstance(Entity parent)
        {			
            return parent.AddChild<FUI_Login, GObject>(CreateGObject());
        }
                
        
        public static ETTask<FUI_Login> CreateInstanceAsync(Entity parent)
        {
            ETTask<FUI_Login> tcs = ETTask<FUI_Login>.Create(true);

            CreateGObjectAsync((go) =>
            {
                tcs.SetResult(parent.AddChild<FUI_Login, GObject>(go));
            });

            return tcs;
        }
                
        
        /// <summary>
        /// 仅用于go已经实例化情况下的创建（例如另一个组件引用了此组件）
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="go"></param>
        /// <returns></returns>
        public static FUI_Login Create(Entity parent, GObject go)
        {
            return parent.AddChild<FUI_Login, GObject>(go);
        }
                    
       
        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static FUI_Login GetFormPool(Entity domain, GObject go)
        {
            var fui = go.Get<FUI_Login>();
        
            if(fui == null)
            {
                fui = Create(domain, go);
            }
        
            fui.isFromFGUIPool = true;
        
            return fui;
        }
                
           
        public override void Dispose()
        {
            if(IsDisposed)
            {
                return;
            }
            
            base.Dispose();
            
            FGComp.Remove();
            FGComp = null;
                    
    		m_BG = null;
    		m_n7 = null;
    		m_LoginButton.Dispose();
    		m_LoginButton = null;
    		m_n5 = null;
    		m_n0 = null;
    		m_AccountNameInputBG = null;
    		m_AccountName = null;
    		m_n2 = null;
    		m_PasswordInputBG = null;
    		m_Password = null;
    	}
    }
}