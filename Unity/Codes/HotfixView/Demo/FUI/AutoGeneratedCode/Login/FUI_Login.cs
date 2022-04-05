/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using System.Threading.Tasks;

namespace ET
{
    public class FUI_LoginAwakeSystem : AwakeSystem<FUI_Login, GObject>
    {
        public override void Awake(FUI_Login self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public static class FUI_LoginSystem
    {
    
        public static void Awake(this FUI_Login self, GObject go)
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
            
            self.FGComp = (GComponent)go;
            
            self.FGComp.Add(self);
            
            var com = go.asCom;
                
            if(com != null)
            {	
                    
    			self.m_BG = (GImage)com.GetChildAt(0);
    			self.m_n7 = (GImage)com.GetChildAt(1);
    			self.m_LoginButton = FUI_LoginButton.Create(self, com.GetChildAt(2));
    			self.m_n5 = (GTextField)com.GetChildAt(3);
    			self.m_n0 = (GTextField)com.GetChildAt(4);
    			self.m_AccountNameInputBG = (GImage)com.GetChildAt(5);
    			self.m_AccountName = (GTextInput)com.GetChildAt(6);
    			self.m_n2 = (GTextField)com.GetChildAt(7);
    			self.m_PasswordInputBG = (GImage)com.GetChildAt(8);
    			self.m_Password = (GTextInput)com.GetChildAt(9);
    		}
    	}
    }
}