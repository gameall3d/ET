/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using System.Threading.Tasks;

namespace ET
{
    public sealed class FUI_LoginButton : FUI
    {	
        public const string UIPackageName = "Login";
        public const string UIResName = "LoginButton";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GButton FGComp;
                        
    	public Controller m_button;
    	public GImage m_n0;
    	public GImage m_n1;
    	public const string URL = "ui://3paxr1cxx6ki3";

     
        private static GObject CreateGObject()
        {
            return UIPackage.CreateObject(UIPackageName, UIResName);
        }

        private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
        {
            UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
        }
                
       
        public static FUI_LoginButton CreateInstance(Entity parent)
        {			
            return parent.AddChild<FUI_LoginButton, GObject>(CreateGObject());
        }
                
        
        public static ETTask<FUI_LoginButton> CreateInstanceAsync(Entity parent)
        {
            ETTask<FUI_LoginButton> tcs = ETTask<FUI_LoginButton>.Create(true);

            CreateGObjectAsync((go) =>
            {
                tcs.SetResult(parent.AddChild<FUI_LoginButton, GObject>(go));
            });

            return tcs;
        }
                
        
        /// <summary>
        /// 仅用于go已经实例化情况下的创建（例如另一个组件引用了此组件）
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="go"></param>
        /// <returns></returns>
        public static FUI_LoginButton Create(Entity parent, GObject go)
        {
            return parent.AddChild<FUI_LoginButton, GObject>(go);
        }
                    
       
        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static FUI_LoginButton GetFormPool(Entity domain, GObject go)
        {
            var fui = go.Get<FUI_LoginButton>();
        
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
                    
    		m_button = null;
    		m_n0 = null;
    		m_n1 = null;
    	}
    }
}