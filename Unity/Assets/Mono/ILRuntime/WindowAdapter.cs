using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;

namespace ET
{   
    public class WindowAdapter : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(FairyGUI.Window);
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

        public class Adapter : FairyGUI.Window, CrossBindingAdaptorType
        {
            CrossBindingMethodInfo mLayoutModalWaitPane_0 = new CrossBindingMethodInfo("LayoutModalWaitPane");
            CrossBindingMethodInfo mOnInit_1 = new CrossBindingMethodInfo("OnInit");
            CrossBindingMethodInfo mOnShown_2 = new CrossBindingMethodInfo("OnShown");
            CrossBindingMethodInfo mOnHide_3 = new CrossBindingMethodInfo("OnHide");
            CrossBindingMethodInfo mDoShowAnimation_4 = new CrossBindingMethodInfo("DoShowAnimation");
            CrossBindingMethodInfo mDoHideAnimation_5 = new CrossBindingMethodInfo("DoHideAnimation");
            CrossBindingMethodInfo mDispose_6 = new CrossBindingMethodInfo("Dispose");
            CrossBindingMethodInfo<FairyGUI.EventContext> mcloseEventHandler_7 = new CrossBindingMethodInfo<FairyGUI.EventContext>("closeEventHandler");
            // CrossBindingMethodInfo mCreateDisplayObject_8 = new CrossBindingMethodInfo("CreateDisplayObject");
            CrossBindingFunctionInfo<FairyGUI.GObject, System.Int32, FairyGUI.GObject> mAddChildAt_9 = new CrossBindingFunctionInfo<FairyGUI.GObject, System.Int32, FairyGUI.GObject>("AddChildAt");
            CrossBindingFunctionInfo<System.Int32, System.Boolean, FairyGUI.GObject> mRemoveChildAt_10 = new CrossBindingFunctionInfo<System.Int32, System.Boolean, FairyGUI.GObject>("RemoveChildAt");
            CrossBindingFunctionInfo<System.Int32> mGetFirstChildInView_11 = new CrossBindingFunctionInfo<System.Int32>("GetFirstChildInView");
            CrossBindingMethodInfo mHandleSizeChanged_12 = new CrossBindingMethodInfo("HandleSizeChanged");
            CrossBindingMethodInfo mHandleGrayedChanged_13 = new CrossBindingMethodInfo("HandleGrayedChanged");
            CrossBindingMethodInfo<FairyGUI.Controller> mHandleControllerChanged_14 = new CrossBindingMethodInfo<FairyGUI.Controller>("HandleControllerChanged");
            CrossBindingMethodInfo mUpdateBounds_15 = new CrossBindingMethodInfo("UpdateBounds");
            class GetSnappingPositionWithDir_16Info : CrossBindingMethodInfo
            {
                static Type[] pTypes = new Type[] {typeof(System.Single).MakeByRefType(), typeof(System.Single).MakeByRefType(), typeof(System.Single), typeof(System.Single)};

                public GetSnappingPositionWithDir_16Info()
                    : base("GetSnappingPositionWithDir")
                {

                }

                protected override Type ReturnType { get { return null; } }

                protected override Type[] Parameters { get { return pTypes; } }
                public void Invoke(ILTypeInstance instance, ref System.Single xValue, ref System.Single yValue, System.Single xDir, System.Single yDir)
                {
                    EnsureMethod(instance);

                    if (method != null)
                    {
                        invoking = true;
                        try
                        {
                            using (var ctx = domain.BeginInvoke(method))
                            {
                            ctx.PushFloat(xValue);
                            ctx.PushFloat(yValue);
                                ctx.PushObject(instance);
                                ctx.PushReference(0);
                                ctx.PushReference(1);
                            ctx.PushFloat(xDir);
                            ctx.PushFloat(yDir);
                                ctx.Invoke();
                            xValue = ctx.ReadFloat(0);
                            yValue = ctx.ReadFloat(1);
                            }
                        }
                        finally
                        {
                            invoking = false;
                        }
                    }
                }

                public override void Invoke(ILTypeInstance instance)
                {
                    throw new NotSupportedException();
                }
            }
            GetSnappingPositionWithDir_16Info mGetSnappingPositionWithDir_16 = new GetSnappingPositionWithDir_16Info();
            CrossBindingMethodInfo mOnUpdate_17 = new CrossBindingMethodInfo("OnUpdate");
            CrossBindingMethodInfo mConstructFromResource_18 = new CrossBindingMethodInfo("ConstructFromResource");
            CrossBindingMethodInfo<FairyGUI.Utils.ByteBuffer> mConstructExtension_19 = new CrossBindingMethodInfo<FairyGUI.Utils.ByteBuffer>("ConstructExtension");
            CrossBindingMethodInfo<FairyGUI.Utils.XML> mConstructFromXML_20 = new CrossBindingMethodInfo<FairyGUI.Utils.XML>("ConstructFromXML");
            CrossBindingMethodInfo<FairyGUI.Utils.ByteBuffer, System.Int32> mSetup_AfterAdd_21 = new CrossBindingMethodInfo<FairyGUI.Utils.ByteBuffer, System.Int32>("Setup_AfterAdd");
            CrossBindingFunctionInfo<FairyGUI.IFilter> mget_filter_22 = new CrossBindingFunctionInfo<FairyGUI.IFilter>("get_filter");
            CrossBindingMethodInfo<FairyGUI.IFilter> mset_filter_23 = new CrossBindingMethodInfo<FairyGUI.IFilter>("set_filter");
            CrossBindingFunctionInfo<FairyGUI.BlendMode> mget_blendMode_24 = new CrossBindingFunctionInfo<FairyGUI.BlendMode>("get_blendMode");
            CrossBindingMethodInfo<FairyGUI.BlendMode> mset_blendMode_25 = new CrossBindingMethodInfo<FairyGUI.BlendMode>("set_blendMode");
            CrossBindingFunctionInfo<System.String> mget_text_26 = new CrossBindingFunctionInfo<System.String>("get_text");
            CrossBindingMethodInfo<System.String> mset_text_27 = new CrossBindingMethodInfo<System.String>("set_text");
            CrossBindingFunctionInfo<System.String> mget_icon_28 = new CrossBindingFunctionInfo<System.String>("get_icon");
            CrossBindingMethodInfo<System.String> mset_icon_29 = new CrossBindingMethodInfo<System.String>("set_icon");
            CrossBindingMethodInfo mHandlePositionChanged_30 = new CrossBindingMethodInfo("HandlePositionChanged");
            CrossBindingMethodInfo mHandleScaleChanged_31 = new CrossBindingMethodInfo("HandleScaleChanged");
            CrossBindingMethodInfo mHandleAlphaChanged_32 = new CrossBindingMethodInfo("HandleAlphaChanged");
            CrossBindingMethodInfo<FairyGUI.Utils.ByteBuffer, System.Int32> mSetup_BeforeAdd_33 = new CrossBindingMethodInfo<FairyGUI.Utils.ByteBuffer, System.Int32>("Setup_BeforeAdd");

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

            protected override void LayoutModalWaitPane()
            {
                if (mLayoutModalWaitPane_0.CheckShouldInvokeBase(this.instance))
                    base.LayoutModalWaitPane();
                else
                    mLayoutModalWaitPane_0.Invoke(this.instance);
            }

            protected override void OnInit()
            {
                if (mOnInit_1.CheckShouldInvokeBase(this.instance))
                    base.OnInit();
                else
                    mOnInit_1.Invoke(this.instance);
            }

            protected override void OnShown()
            {
                if (mOnShown_2.CheckShouldInvokeBase(this.instance))
                    base.OnShown();
                else
                    mOnShown_2.Invoke(this.instance);
            }

            protected override void OnHide()
            {
                if (mOnHide_3.CheckShouldInvokeBase(this.instance))
                    base.OnHide();
                else
                    mOnHide_3.Invoke(this.instance);
            }

            protected override void DoShowAnimation()
            {
                if (mDoShowAnimation_4.CheckShouldInvokeBase(this.instance))
                    base.DoShowAnimation();
                else
                    mDoShowAnimation_4.Invoke(this.instance);
            }

            protected override void DoHideAnimation()
            {
                if (mDoHideAnimation_5.CheckShouldInvokeBase(this.instance))
                    base.DoHideAnimation();
                else
                    mDoHideAnimation_5.Invoke(this.instance);
            }

            public override void Dispose()
            {
                if (mDispose_6.CheckShouldInvokeBase(this.instance))
                    base.Dispose();
                else
                    mDispose_6.Invoke(this.instance);
            }

            protected override void closeEventHandler(FairyGUI.EventContext context)
            {
                if (mcloseEventHandler_7.CheckShouldInvokeBase(this.instance))
                    base.closeEventHandler(context);
                else
                    mcloseEventHandler_7.Invoke(this.instance, context);
            }

            // protected override void CreateDisplayObject()
            // {
            //     if (mCreateDisplayObject_8.CheckShouldInvokeBase(this.instance))
            //         base.CreateDisplayObject();
            //     else
            //         mCreateDisplayObject_8.Invoke(this.instance);
            // }

            public override FairyGUI.GObject AddChildAt(FairyGUI.GObject child, System.Int32 index)
            {
                if (mAddChildAt_9.CheckShouldInvokeBase(this.instance))
                    return base.AddChildAt(child, index);
                else
                    return mAddChildAt_9.Invoke(this.instance, child, index);
            }

            public override FairyGUI.GObject RemoveChildAt(System.Int32 index, System.Boolean dispose)
            {
                if (mRemoveChildAt_10.CheckShouldInvokeBase(this.instance))
                    return base.RemoveChildAt(index, dispose);
                else
                    return mRemoveChildAt_10.Invoke(this.instance, index, dispose);
            }

            public override System.Int32 GetFirstChildInView()
            {
                if (mGetFirstChildInView_11.CheckShouldInvokeBase(this.instance))
                    return base.GetFirstChildInView();
                else
                    return mGetFirstChildInView_11.Invoke(this.instance);
            }

            protected override void HandleSizeChanged()
            {
                if (mHandleSizeChanged_12.CheckShouldInvokeBase(this.instance))
                    base.HandleSizeChanged();
                else
                    mHandleSizeChanged_12.Invoke(this.instance);
            }

            protected override void HandleGrayedChanged()
            {
                if (mHandleGrayedChanged_13.CheckShouldInvokeBase(this.instance))
                    base.HandleGrayedChanged();
                else
                    mHandleGrayedChanged_13.Invoke(this.instance);
            }

            public override void HandleControllerChanged(FairyGUI.Controller c)
            {
                if (mHandleControllerChanged_14.CheckShouldInvokeBase(this.instance))
                    base.HandleControllerChanged(c);
                else
                    mHandleControllerChanged_14.Invoke(this.instance, c);
            }

            protected override void UpdateBounds()
            {
                if (mUpdateBounds_15.CheckShouldInvokeBase(this.instance))
                    base.UpdateBounds();
                else
                    mUpdateBounds_15.Invoke(this.instance);
            }

            public override void GetSnappingPositionWithDir(ref System.Single xValue, ref System.Single yValue, System.Single xDir, System.Single yDir)
            {
                if (mGetSnappingPositionWithDir_16.CheckShouldInvokeBase(this.instance))
                    base.GetSnappingPositionWithDir(ref xValue, ref yValue, xDir, yDir);
                else
                    mGetSnappingPositionWithDir_16.Invoke(this.instance, ref xValue, ref yValue, xDir, yDir);
            }

            protected override void OnUpdate()
            {
                if (mOnUpdate_17.CheckShouldInvokeBase(this.instance))
                    base.OnUpdate();
                else
                    mOnUpdate_17.Invoke(this.instance);
            }

            public override void ConstructFromResource()
            {
                if (mConstructFromResource_18.CheckShouldInvokeBase(this.instance))
                    base.ConstructFromResource();
                else
                    mConstructFromResource_18.Invoke(this.instance);
            }

            protected override void ConstructExtension(FairyGUI.Utils.ByteBuffer buffer)
            {
                if (mConstructExtension_19.CheckShouldInvokeBase(this.instance))
                    base.ConstructExtension(buffer);
                else
                    mConstructExtension_19.Invoke(this.instance, buffer);
            }

            public override void ConstructFromXML(FairyGUI.Utils.XML xml)
            {
                if (mConstructFromXML_20.CheckShouldInvokeBase(this.instance))
                    base.ConstructFromXML(xml);
                else
                    mConstructFromXML_20.Invoke(this.instance, xml);
            }

            public override void Setup_AfterAdd(FairyGUI.Utils.ByteBuffer buffer, System.Int32 beginPos)
            {
                if (mSetup_AfterAdd_21.CheckShouldInvokeBase(this.instance))
                    base.Setup_AfterAdd(buffer, beginPos);
                else
                    mSetup_AfterAdd_21.Invoke(this.instance, buffer, beginPos);
            }

            protected override void HandlePositionChanged()
            {
                if (mHandlePositionChanged_30.CheckShouldInvokeBase(this.instance))
                    base.HandlePositionChanged();
                else
                    mHandlePositionChanged_30.Invoke(this.instance);
            }

            protected override void HandleScaleChanged()
            {
                if (mHandleScaleChanged_31.CheckShouldInvokeBase(this.instance))
                    base.HandleScaleChanged();
                else
                    mHandleScaleChanged_31.Invoke(this.instance);
            }

            protected override void HandleAlphaChanged()
            {
                if (mHandleAlphaChanged_32.CheckShouldInvokeBase(this.instance))
                    base.HandleAlphaChanged();
                else
                    mHandleAlphaChanged_32.Invoke(this.instance);
            }

            public override void Setup_BeforeAdd(FairyGUI.Utils.ByteBuffer buffer, System.Int32 beginPos)
            {
                if (mSetup_BeforeAdd_33.CheckShouldInvokeBase(this.instance))
                    base.Setup_BeforeAdd(buffer, beginPos);
                else
                    mSetup_BeforeAdd_33.Invoke(this.instance, buffer, beginPos);
            }

            public override FairyGUI.IFilter filter
            {
            get
            {
                if (mget_filter_22.CheckShouldInvokeBase(this.instance))
                    return base.filter;
                else
                    return mget_filter_22.Invoke(this.instance);

            }
            set
            {
                if (mset_filter_23.CheckShouldInvokeBase(this.instance))
                    base.filter = value;
                else
                    mset_filter_23.Invoke(this.instance, value);

            }
            }

            public override FairyGUI.BlendMode blendMode
            {
            get
            {
                if (mget_blendMode_24.CheckShouldInvokeBase(this.instance))
                    return base.blendMode;
                else
                    return mget_blendMode_24.Invoke(this.instance);

            }
            set
            {
                if (mset_blendMode_25.CheckShouldInvokeBase(this.instance))
                    base.blendMode = value;
                else
                    mset_blendMode_25.Invoke(this.instance, value);

            }
            }

            public override System.String text
            {
            get
            {
                if (mget_text_26.CheckShouldInvokeBase(this.instance))
                    return base.text;
                else
                    return mget_text_26.Invoke(this.instance);

            }
            set
            {
                if (mset_text_27.CheckShouldInvokeBase(this.instance))
                    base.text = value;
                else
                    mset_text_27.Invoke(this.instance, value);

            }
            }

            public override System.String icon
            {
            get
            {
                if (mget_icon_28.CheckShouldInvokeBase(this.instance))
                    return base.icon;
                else
                    return mget_icon_28.Invoke(this.instance);

            }
            set
            {
                if (mset_icon_29.CheckShouldInvokeBase(this.instance))
                    base.icon = value;
                else
                    mset_icon_29.Invoke(this.instance, value);

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

