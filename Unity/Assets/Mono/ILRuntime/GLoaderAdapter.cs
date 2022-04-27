using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;

namespace ET
{   
    public class GLoaderAdapter : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(FairyGUI.GLoader);
            }
        }

        public override Type AdaptorType
        {
            get
            {
                return typeof(Adapter);
            }
        }

        public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
        {
            return new Adapter(appdomain, instance);
        }

        public class Adapter : FairyGUI.GLoader, CrossBindingAdaptorType
        {
            // CrossBindingMethodInfo mCreateDisplayObject_0 = new CrossBindingMethodInfo("CreateDisplayObject");
            CrossBindingMethodInfo mDispose_1 = new CrossBindingMethodInfo("Dispose");
            CrossBindingFunctionInfo<System.String> mget_icon_2 = new CrossBindingFunctionInfo<System.String>("get_icon");
            CrossBindingMethodInfo<System.String> mset_icon_3 = new CrossBindingMethodInfo<System.String>("set_icon");
            CrossBindingFunctionInfo<FairyGUI.IFilter> mget_filter_4 = new CrossBindingFunctionInfo<FairyGUI.IFilter>("get_filter");
            CrossBindingMethodInfo<FairyGUI.IFilter> mset_filter_5 = new CrossBindingMethodInfo<FairyGUI.IFilter>("set_filter");
            CrossBindingFunctionInfo<FairyGUI.BlendMode> mget_blendMode_6 = new CrossBindingFunctionInfo<FairyGUI.BlendMode>("get_blendMode");
            CrossBindingMethodInfo<FairyGUI.BlendMode> mset_blendMode_7 = new CrossBindingMethodInfo<FairyGUI.BlendMode>("set_blendMode");
            CrossBindingMethodInfo mLoadExternal_8 = new CrossBindingMethodInfo("LoadExternal");
            CrossBindingMethodInfo<FairyGUI.NTexture> mFreeExternal_9 = new CrossBindingMethodInfo<FairyGUI.NTexture>("FreeExternal");
            CrossBindingMethodInfo mHandleSizeChanged_10 = new CrossBindingMethodInfo("HandleSizeChanged");
            CrossBindingMethodInfo<FairyGUI.Utils.ByteBuffer, System.Int32> mSetup_BeforeAdd_11 = new CrossBindingMethodInfo<FairyGUI.Utils.ByteBuffer, System.Int32>("Setup_BeforeAdd");
            CrossBindingMethodInfo<FairyGUI.Controller> mHandleControllerChanged_12 = new CrossBindingMethodInfo<FairyGUI.Controller>("HandleControllerChanged");
            CrossBindingFunctionInfo<System.String> mget_text_13 = new CrossBindingFunctionInfo<System.String>("get_text");
            CrossBindingMethodInfo<System.String> mset_text_14 = new CrossBindingMethodInfo<System.String>("set_text");
            CrossBindingMethodInfo mHandlePositionChanged_15 = new CrossBindingMethodInfo("HandlePositionChanged");
            CrossBindingMethodInfo mHandleScaleChanged_16 = new CrossBindingMethodInfo("HandleScaleChanged");
            CrossBindingMethodInfo mHandleGrayedChanged_17 = new CrossBindingMethodInfo("HandleGrayedChanged");
            CrossBindingMethodInfo mHandleAlphaChanged_18 = new CrossBindingMethodInfo("HandleAlphaChanged");
            CrossBindingMethodInfo mConstructFromResource_19 = new CrossBindingMethodInfo("ConstructFromResource");
            CrossBindingMethodInfo<FairyGUI.Utils.ByteBuffer, System.Int32> mSetup_AfterAdd_20 = new CrossBindingMethodInfo<FairyGUI.Utils.ByteBuffer, System.Int32>("Setup_AfterAdd");

            bool isInvokingToString;
            ILTypeInstance instance;
            ILRuntime.Runtime.Enviorment.AppDomain appdomain;

            public Adapter()
            {

            }

            public Adapter(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
            {
                this.appdomain = appdomain;
                this.instance = instance;
            }

            public ILTypeInstance ILInstance { get { return instance; } }

            // protected override void CreateDisplayObject()
            // {
            //     if (mCreateDisplayObject_0.CheckShouldInvokeBase(this.instance))
            //         base.CreateDisplayObject();
            //     else
            //         mCreateDisplayObject_0.Invoke(this.instance);
            // }

            public override void Dispose()
            {
                if (mDispose_1.CheckShouldInvokeBase(this.instance))
                    base.Dispose();
                else
                    mDispose_1.Invoke(this.instance);
            }

            protected override void LoadExternal()
            {
                if (mLoadExternal_8.CheckShouldInvokeBase(this.instance))
                    base.LoadExternal();
                else
                    mLoadExternal_8.Invoke(this.instance);
            }

            protected override void FreeExternal(FairyGUI.NTexture texture)
            {
                if (mFreeExternal_9.CheckShouldInvokeBase(this.instance))
                    base.FreeExternal(texture);
                else
                    mFreeExternal_9.Invoke(this.instance, texture);
            }

            protected override void HandleSizeChanged()
            {
                if (mHandleSizeChanged_10.CheckShouldInvokeBase(this.instance))
                    base.HandleSizeChanged();
                else
                    mHandleSizeChanged_10.Invoke(this.instance);
            }

            public override void Setup_BeforeAdd(FairyGUI.Utils.ByteBuffer buffer, System.Int32 beginPos)
            {
                if (mSetup_BeforeAdd_11.CheckShouldInvokeBase(this.instance))
                    base.Setup_BeforeAdd(buffer, beginPos);
                else
                    mSetup_BeforeAdd_11.Invoke(this.instance, buffer, beginPos);
            }

            public override void HandleControllerChanged(FairyGUI.Controller c)
            {
                if (mHandleControllerChanged_12.CheckShouldInvokeBase(this.instance))
                    base.HandleControllerChanged(c);
                else
                    mHandleControllerChanged_12.Invoke(this.instance, c);
            }

            protected override void HandlePositionChanged()
            {
                if (mHandlePositionChanged_15.CheckShouldInvokeBase(this.instance))
                    base.HandlePositionChanged();
                else
                    mHandlePositionChanged_15.Invoke(this.instance);
            }

            protected override void HandleScaleChanged()
            {
                if (mHandleScaleChanged_16.CheckShouldInvokeBase(this.instance))
                    base.HandleScaleChanged();
                else
                    mHandleScaleChanged_16.Invoke(this.instance);
            }

            protected override void HandleGrayedChanged()
            {
                if (mHandleGrayedChanged_17.CheckShouldInvokeBase(this.instance))
                    base.HandleGrayedChanged();
                else
                    mHandleGrayedChanged_17.Invoke(this.instance);
            }

            protected override void HandleAlphaChanged()
            {
                if (mHandleAlphaChanged_18.CheckShouldInvokeBase(this.instance))
                    base.HandleAlphaChanged();
                else
                    mHandleAlphaChanged_18.Invoke(this.instance);
            }

            public override void ConstructFromResource()
            {
                if (mConstructFromResource_19.CheckShouldInvokeBase(this.instance))
                    base.ConstructFromResource();
                else
                    mConstructFromResource_19.Invoke(this.instance);
            }

            public override void Setup_AfterAdd(FairyGUI.Utils.ByteBuffer buffer, System.Int32 beginPos)
            {
                if (mSetup_AfterAdd_20.CheckShouldInvokeBase(this.instance))
                    base.Setup_AfterAdd(buffer, beginPos);
                else
                    mSetup_AfterAdd_20.Invoke(this.instance, buffer, beginPos);
            }

            public override System.String icon
            {
            get
            {
                if (mget_icon_2.CheckShouldInvokeBase(this.instance))
                    return base.icon;
                else
                    return mget_icon_2.Invoke(this.instance);

            }
            set
            {
                if (mset_icon_3.CheckShouldInvokeBase(this.instance))
                    base.icon = value;
                else
                    mset_icon_3.Invoke(this.instance, value);

            }
            }

            public override FairyGUI.IFilter filter
            {
            get
            {
                if (mget_filter_4.CheckShouldInvokeBase(this.instance))
                    return base.filter;
                else
                    return mget_filter_4.Invoke(this.instance);

            }
            set
            {
                if (mset_filter_5.CheckShouldInvokeBase(this.instance))
                    base.filter = value;
                else
                    mset_filter_5.Invoke(this.instance, value);

            }
            }

            public override FairyGUI.BlendMode blendMode
            {
            get
            {
                if (mget_blendMode_6.CheckShouldInvokeBase(this.instance))
                    return base.blendMode;
                else
                    return mget_blendMode_6.Invoke(this.instance);

            }
            set
            {
                if (mset_blendMode_7.CheckShouldInvokeBase(this.instance))
                    base.blendMode = value;
                else
                    mset_blendMode_7.Invoke(this.instance, value);

            }
            }

            public override System.String text
            {
            get
            {
                if (mget_text_13.CheckShouldInvokeBase(this.instance))
                    return base.text;
                else
                    return mget_text_13.Invoke(this.instance);

            }
            set
            {
                if (mset_text_14.CheckShouldInvokeBase(this.instance))
                    base.text = value;
                else
                    mset_text_14.Invoke(this.instance, value);

            }
            }

            public override string ToString()
            {
                IMethod m = appdomain.ObjectType.GetMethod("ToString", 0);
                m = instance.Type.GetVirtualMethod(m);
                if (m == null || m is ILMethod)
                {
                    if (!isInvokingToString)
                    {
                        isInvokingToString = true;
                        string res = instance.ToString();
                        isInvokingToString = false;
                        return res;
                    }
                    else
                        return instance.Type.FullName;
                }
                else
                    return instance.Type.FullName;
            }
        }
    }
}

