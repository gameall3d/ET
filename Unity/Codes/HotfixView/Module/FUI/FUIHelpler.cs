using System;
using FairyGUI;

namespace ET
{
    public static class FUIHelpler
    {
        public static void AddClickListenerAsync(this GButton button, Func<ETTask> action)
        {
            button.onClick.Clear();

            async ETTask ClickActionAsync()
            {
                FUIComponent.Instance.IsButtonClicked = true;
                await action();
                FUIComponent.Instance.IsButtonClicked = false;
            }
            
            button.onClick.Add(() =>
            {
                if (FUIComponent.Instance.IsButtonClicked)
                {
                    // 可以加个定时器清除？否则出现异常会把点击事件都卡住
                    return;
                }
                
                ClickActionAsync().Coroutine();
            });
        }
    }
}