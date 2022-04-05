using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class FUILoginComponenAwakeSystem : AwakeSystem<FUILoginComponent>
    {
        public override void Awake(FUILoginComponent self)
        {
            self.Awake();
        }
    }

    public class FUILoginComponenDestroySystem : DestroySystem<FUILoginComponent>
    {
        public override void Destroy(FUILoginComponent self)
        {
            self.Destroy();
        }
    }

    public static class FUILoginComponentSystem
    {
        public static void Awake(this FUILoginComponent self)
        {
            self.FUILogin = self.GetParent<FUI_Login>();
            self.FUILogin.m_LoginButton.FGComp.onClick.Set(self.OnClickLogin);
        }

        public static void OnClickLogin(this FUILoginComponent self)
        {
            LoginHelper.Login(self.ZoneScene(), ConstValue.LoginAddress, self.FUILogin.m_AccountName.text, self.FUILogin.m_Password.text).Coroutine();

        }

        public static void Destroy(this FUILoginComponent self)
        {
            self.FUILogin.m_LoginButton.FGComp.onClick.Clear();
        }
    }
}
