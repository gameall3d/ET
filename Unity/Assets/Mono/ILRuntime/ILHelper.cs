using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using FairyGUI;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.Utils;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ProtoBuf;
using UnityEngine;
using UnityEngine.Events;

namespace ET
{
    public static class ILHelper
    {
        public static List<Type> list = new List<Type>();

        public static void InitILRuntime(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
        {
            list.Add(typeof(Dictionary<int, ILTypeInstance>));
            list.Add(typeof(Dictionary<long, ILTypeInstance>));
            list.Add(typeof(Dictionary<string, ILTypeInstance>));
            list.Add(typeof(Dictionary<int, int>));
            list.Add(typeof(Dictionary<object, object>));
            list.Add(typeof(Dictionary<int, object>));
            list.Add(typeof(Dictionary<long, object>));
            list.Add(typeof(Dictionary<long, int>));
            list.Add(typeof(Dictionary<int, long>));
            list.Add(typeof(Dictionary<string, long>));
            list.Add(typeof(Dictionary<string, int>));
            list.Add(typeof(Dictionary<string, object>));
            list.Add(typeof(List<ILTypeInstance>));
            list.Add(typeof(List<int>));
            list.Add(typeof(List<long>));
            list.Add(typeof(List<string>));
            list.Add(typeof(List<object>));
            list.Add(typeof(ETTask<int>));
            list.Add(typeof(ETTask<long>));
            list.Add(typeof(ETTask<string>));
            list.Add(typeof(ETTask<object>));
            list.Add(typeof(ETTask<AssetBundle>));
            list.Add(typeof(ETTask<UnityEngine.Object[]>));
            list.Add(typeof(ListComponent<ILTypeInstance>));
            list.Add(typeof(ListComponent<ETTask>));
            list.Add(typeof(ListComponent<Vector3>));
            
            
            
            // 注册重定向函数
            FUICLRMethod(appdomain);

            // 注册委托
            appdomain.DelegateManager.RegisterMethodDelegate<List<object>>();
            appdomain.DelegateManager.RegisterMethodDelegate<object>();
            appdomain.DelegateManager.RegisterMethodDelegate<bool>();
            appdomain.DelegateManager.RegisterMethodDelegate<string>();
            appdomain.DelegateManager.RegisterMethodDelegate<float>();
            appdomain.DelegateManager.RegisterMethodDelegate<long, int>();
            appdomain.DelegateManager.RegisterMethodDelegate<long, MemoryStream>();
            appdomain.DelegateManager.RegisterMethodDelegate<long, IPEndPoint>();
            appdomain.DelegateManager.RegisterMethodDelegate<ILTypeInstance>();
            appdomain.DelegateManager.RegisterMethodDelegate<Transform,int>();
            appdomain.DelegateManager.RegisterMethodDelegate<AsyncOperation>();

            
            appdomain.DelegateManager.RegisterFunctionDelegate<UnityEngine.Events.UnityAction>();
            appdomain.DelegateManager.RegisterFunctionDelegate<System.Object, ET.ETTask>();
            appdomain.DelegateManager.RegisterFunctionDelegate<ILTypeInstance, bool>();
            appdomain.DelegateManager.RegisterFunctionDelegate<System.Collections.Generic.KeyValuePair<System.String, System.Int32>, System.String>();
            appdomain.DelegateManager.RegisterFunctionDelegate<System.Collections.Generic.KeyValuePair<System.Int32, System.Int32>, System.Boolean>();
            appdomain.DelegateManager.RegisterFunctionDelegate<System.Collections.Generic.KeyValuePair<System.String, System.Int32>, System.Int32>();
            appdomain.DelegateManager.RegisterFunctionDelegate<List<int>, int>();
            appdomain.DelegateManager.RegisterFunctionDelegate<List<int>, bool>();
            appdomain.DelegateManager.RegisterFunctionDelegate<int, bool>();//Linq
            appdomain.DelegateManager.RegisterFunctionDelegate<int, int, int>();//Linq
            appdomain.DelegateManager.RegisterFunctionDelegate<KeyValuePair<int, List<int>>, bool>();
            appdomain.DelegateManager.RegisterFunctionDelegate<KeyValuePair<int, int>, KeyValuePair<int, int>, int>();
            appdomain.DelegateManager.RegisterMethodDelegate<System.Int64, System.Collections.Generic.List<System.Int64>>();
            appdomain.DelegateManager.RegisterMethodDelegate<System.Int64, System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>>();
            appdomain.DelegateManager.RegisterFunctionDelegate<System.Int64, System.Collections.Generic.List<System.Int64>, System.Boolean>();
            appdomain.DelegateManager.RegisterFunctionDelegate<System.Int64, System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>, System.Boolean>();

            appdomain.DelegateManager.RegisterMethodDelegate<ET.AService>();
            
            appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction>((act) =>
            {
                return new UnityEngine.Events.UnityAction(() =>
                {
                    ((Action)act)();
                });
            });
            
            appdomain.DelegateManager.RegisterDelegateConvertor<Comparison<KeyValuePair<int, int>>>((act) =>
            {
                return new Comparison<KeyValuePair<int, int>>((x, y) =>
                {
                    return ((Func<KeyValuePair<int, int>, KeyValuePair<int, int>, int>)act)(x, y);
                });
            });
            
            appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction>((act) =>
            {
                return new UnityEngine.Events.UnityAction(() =>
                {
                    ((Action)act)();
                });
            });
            
            #region FGUI

            appdomain.DelegateManager
                    .RegisterMethodDelegate<System.String, System.String, System.Type, FairyGUI.PackageItem>();
            appdomain.DelegateManager.RegisterMethodDelegate<FairyGUI.GObject>();
            appdomain.DelegateManager.RegisterMethodDelegate<FairyGUI.EventContext>();
            appdomain.DelegateManager.RegisterFunctionDelegate<FairyGUI.GComponent>();
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.EventSystems.BaseEventData>();

            appdomain.DelegateManager.RegisterDelegateConvertor<FairyGUI.UIPackage.LoadResourceAsync>((act) =>
            {
                return new FairyGUI.UIPackage.LoadResourceAsync((name, extension, type, item) =>
                {
                    ((Action<System.String, System.String, System.Type, FairyGUI.PackageItem>)act)(name,
                        extension, type, item);
                });
            });
            appdomain.DelegateManager.RegisterDelegateConvertor<FairyGUI.UIPackage.CreateObjectCallback>((act) =>
            {
                return new FairyGUI.UIPackage.CreateObjectCallback((result) =>
                {
                    ((Action<FairyGUI.GObject>)act)(result);
                });
            });
            appdomain.DelegateManager.RegisterDelegateConvertor<FairyGUI.UIObjectFactory.GComponentCreator>((act) =>
            {
                return new FairyGUI.UIObjectFactory.GComponentCreator(() =>
                {
                    return ((Func<FairyGUI.GComponent>)act)();
                });
            });

            appdomain.DelegateManager.RegisterDelegateConvertor<FairyGUI.EventCallback0>((act) =>
            {
                return new FairyGUI.EventCallback0(() => { ((Action)act)(); });
            });

            appdomain.DelegateManager.RegisterDelegateConvertor<FairyGUI.EventCallback1>((act) =>
            {
                return new FairyGUI.EventCallback1((context) =>
                {
                    ((Action<FairyGUI.EventContext>)act)(context);
                });
            });
            
            appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<UnityEngine.EventSystems.BaseEventData>>((act) =>
            {
                return new UnityEngine.Events.UnityAction<UnityEngine.EventSystems.BaseEventData>((arg0) =>
                {
                    ((Action<UnityEngine.EventSystems.BaseEventData>)act)(arg0);
                });
            });

            
            #endregion

            // 注册适配器
            RegisterAdaptor(appdomain);
            
            //注册Json的CLR
            LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(appdomain);
            //注册ProtoBuf的CLR
            PType.RegisterILRuntimeCLRRedirection(appdomain);
           
            
            ////////////////////////////////////
            // CLR绑定的注册，一定要记得将CLR绑定的注册写在CLR重定向的注册后面，因为同一个方法只能被重定向一次，只有先注册的那个才能生效
            ////////////////////////////////////
            Type t = Type.GetType("ILRuntime.Runtime.Generated.CLRBindings");
            if (t != null)
            {
                t.GetMethod("Initialize")?.Invoke(null, new object[] { appdomain });
            }
            //ILRuntime.Runtime.Generated.CLRBindings.Initialize(appdomain);
        }
        
        public static void RegisterAdaptor(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
        {
            //注册自己写的适配器
            appdomain.RegisterCrossBindingAdaptor(new IAsyncStateMachineClassInheritanceAdaptor());
            appdomain.RegisterValueTypeBinder(typeof(Vector2), new Vector2Binder());
            appdomain.RegisterValueTypeBinder(typeof(Vector3), new Vector3Binder());
            appdomain.RegisterValueTypeBinder(typeof(Quaternion), new QuaternionBinder());
            
            appdomain.RegisterCrossBindingAdaptor(new GButtonAdapter());
            appdomain.RegisterCrossBindingAdaptor(new GLoaderAdapter());
            appdomain.RegisterCrossBindingAdaptor(new WindowAdapter());
        }
        
        unsafe static void FUICLRMethod (ILRuntime.Runtime.Enviorment.AppDomain appdomain) {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof (FairyGUI.UIObjectFactory);
            args = new Type[] { typeof (System.String), typeof (System.Type) };
            method = type.GetMethod ("SetPackageItemExtension", flag, null, args, null);
            appdomain.RegisterCLRMethodRedirection (method, SetPackageItemExtension_0);
            
            args = new Type[] { typeof (System.Type) };
            method = type.GetMethod ("SetLoaderExtension", flag, null, args, null);
            appdomain.RegisterCLRMethodRedirection (method, SetLoaderExtension_0);
        }
        unsafe static StackObject * SetPackageItemExtension_0 (ILIntepreter __intp, StackObject * __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj) {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject * ptr_of_this_method;
            StackObject * __ret = ILIntepreter.Minus (__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus (__esp, 1);
            System.Type @type = (System.Type) typeof (System.Type).CheckCLRTypes (StackObject.ToObject (ptr_of_this_method, __domain, __mStack));
            __intp.Free (ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus (__esp, 2);
            System.String @url = (System.String) typeof (System.String).CheckCLRTypes (StackObject.ToObject (ptr_of_this_method, __domain, __mStack));
            __intp.Free (ptr_of_this_method);
            
            FairyGUI.UIObjectFactory.SetPackageItemExtension (@url, () => {
                return __domain.Instantiate<GComponent> (@type.FullName);
            });
            // FairyGUI.UIObjectFactory.SetPackageItemExtension (@url, @type);

            return __ret;
        }
        
        unsafe static StackObject * SetLoaderExtension_0 (ILIntepreter __intp, StackObject * __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj) {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject * ptr_of_this_method;
            StackObject * __ret = ILIntepreter.Minus (__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus (__esp, 1);
            System.Type @type = (System.Type) typeof (System.Type).CheckCLRTypes (StackObject.ToObject (ptr_of_this_method, __domain, __mStack));
            __intp.Free (ptr_of_this_method);
            
            FairyGUI.UIObjectFactory.SetLoaderExtension(() => {
                return __domain.Instantiate<GLoader> (@type.FullName);
            });
            return __ret;
        }
    }
}