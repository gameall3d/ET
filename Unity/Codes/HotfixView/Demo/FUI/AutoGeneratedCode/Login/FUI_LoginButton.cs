/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using System.Threading.Tasks;

namespace ET
{
    public class FUI_LoginButtonAwakeSystem : AwakeSystem<FUI_LoginButton, GObject>
    {
        public override void Awake(FUI_LoginButton self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public static class FUI_LoginButtonSystem
    {
    
        public static void Awake(this FUI_LoginButton self, GObject go)
        {
            if(go == null)
            {
                return;
            }
            
            self.GObject = go;	
            
            if (string.IsNullOrWhiteSpace(self.Name))
            {
                self.Name = self.Id.ToString();
            }
            
            self.FGComp = (GButton)go;
            
            self.FGComp.Add(self);
            
            var com = go.asCom;
                
            if(com != null)
            {	
                    
    			self.m_button = com.GetControllerAt(0);
    			self.m_n0 = (GImage)com.GetChildAt(0);
    			self.m_n1 = (GImage)com.GetChildAt(1);
    		}
    	}
    }
}