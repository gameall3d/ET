"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.fuiCodeGenerator = void 0;
const csharp_1 = require("csharp");
const CodeGenConfig_1 = require("./CodeGenConfig");
const CodeWriter_1 = require("./CodeWriter");
class FUICodeGenerator {
    settings;
    codePkgName;
    classes;
    handler;
    // save path
    hotfixViewExportCodePath;
    modelViewExportCodePath;
    hotfixViewLogicExporCodePath;
    modelViewLogicExportCodePath;
    Handle(handler) {
        let settings = handler.project.GetSettings("Publish").codeGeneration;
        let codePkgName = handler.ToFilename(handler.pkg.name); //convert chinese to pinyin, remove special chars etc.
        this.settings = settings;
        this.codePkgName = codePkgName;
        this.handler = handler;
        // 从自定义配置中读取路径和命名空间
        this.hotfixViewExportCodePath = CodeGenConfig_1.CodeGenConfig.HotfixViewCodeOutputPath + '/' + codePkgName;
        this.modelViewExportCodePath = CodeGenConfig_1.CodeGenConfig.ModelViewCodeOutputPath + '/' + codePkgName;
        this.hotfixViewLogicExporCodePath = CodeGenConfig_1.CodeGenConfig.HotfixViewLogicCodeOutputPath + "/" + codePkgName;
        this.modelViewLogicExportCodePath = CodeGenConfig_1.CodeGenConfig.ModelViewLogicCodeOutputPath + "/" + codePkgName;
        // 初始化自定义组件名前缀
        let classNamePrefix = CodeGenConfig_1.CodeGenConfig.ClassNamePrefix;
        // 初始化自定义成员变量名前缀
        let memberVarNamePrefix = CodeGenConfig_1.CodeGenConfig.MemberVarNamePrefix;
        //CollectClasses(stripeMember, stripeClass, fguiNamespace)
        let classes = handler.CollectClasses(settings.ignoreNoname, settings.ignoreNoname, null);
        this.classes = classes;
        handler.SetupCodeFolder(this.hotfixViewExportCodePath, "cs"); //check if target folder exists, and delete old files
        handler.SetupCodeFolder(this.modelViewExportCodePath, "cs"); //check if target folder exists, and delete old files
        let classCnt = classes.Count;
        let hotfixViewCodeWriter = new CodeWriter_1.default();
        let modelViewCodeWriter = new CodeWriter_1.default();
        let mlogicCodeWriter = new CodeWriter_1.default();
        for (let i = 0; i < classCnt; i++) {
            let classInfo = classes.get_Item(i);
            // 是否为自定义类型组件标记数组
            let customComponentFlagsArray = [];
            // 是否为跨包组件标记数组
            let crossPackageFlagsArray = [];
            this.generateModelViewCode(classInfo, modelViewCodeWriter, customComponentFlagsArray, crossPackageFlagsArray);
            this.generateHotfixViewCode(classInfo, hotfixViewCodeWriter, customComponentFlagsArray, crossPackageFlagsArray);
            this.generateLogicCode(classInfo, mlogicCodeWriter);
        }
    }
    // 判断是不是我们自定义类型组件
    isCustomClass(typeName) {
        let classes = this.classes;
        let count = this.classes.Count;
        for (let k = 0; k < count; k++) {
            if (typeName === classes.get_Item(k).className) {
                return true;
            }
        }
        return false;
    }
    generateModelViewCode(classInfo, modelViewCodeWriter, customComponentFlagsArray, crossPackageFlagsArray) {
        let codePkgName = this.codePkgName;
        let modelViewExportCodePath = this.modelViewExportCodePath;
        let namespaceName = CodeGenConfig_1.CodeGenConfig.ModelViewNameSpace;
        // 初始化自定义组件名前缀
        let classNamePrefix = CodeGenConfig_1.CodeGenConfig.ClassNamePrefix;
        // 初始化自定义成员变量名前缀
        let memberVarNamePrefix = CodeGenConfig_1.CodeGenConfig.MemberVarNamePrefix;
        // 组装自定义组件前缀
        let className = classNamePrefix + classInfo.className;
        let members = classInfo.members;
        modelViewCodeWriter.reset();
        modelViewCodeWriter.writeln('using FairyGUI;');
        modelViewCodeWriter.writeln('using System.Threading.Tasks;');
        modelViewCodeWriter.writeln();
        modelViewCodeWriter.writeln('namespace %s', namespaceName);
        modelViewCodeWriter.startBlock();
        // 用const 在ILRuntime中的反射获取Field会得不到值，所以改用static
        modelViewCodeWriter.writeln(`public sealed class %s : FUI
    {	
        public static string UIPackageName = "%s";
        public static string UIResName = "%s";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public %s FGComp;
                        `, className, codePkgName, classInfo.resName, classInfo.superClassName);
        let memberCnt = members.Count;
        for (let j = 0; j < memberCnt; j++) {
            let memberInfo = members.get_Item(j);
            customComponentFlagsArray[j] = false;
            crossPackageFlagsArray[j] = false;
            // 判断是不是我们自定义类型组件
            let typeName = memberInfo.type;
            if (this.isCustomClass(typeName)) {
                typeName = classNamePrefix + typeName;
                customComponentFlagsArray[j] = true;
            }
            // 判断是不是跨包类型组件
            if (memberInfo && memberInfo.res) {
                // 组装自定义组件前缀
                typeName = classNamePrefix + memberInfo.res.name;
                crossPackageFlagsArray[j] = true;
            }
            // 组装自定义成员前缀
            modelViewCodeWriter.writeln('\tpublic %s %s;', typeName, memberVarNamePrefix + memberInfo.varName);
        }
        modelViewCodeWriter.writeln('\tpublic const string URL = "ui://%s%s";', this.handler.pkg.id, classInfo.resId);
        modelViewCodeWriter.writeln();
        modelViewCodeWriter.writeln(`       
        public override void Dispose()
        {
            if(IsDisposed)
            {
                return;
            }
            
            base.Dispose();
            
            FGComp.Remove();
            FGComp = null;
                    `);
        for (let j = 0; j < memberCnt; j++) {
            let memberInfo = members.get_Item(j);
            // 组装自定义成员前缀
            let memberVarName = memberVarNamePrefix + memberInfo.varName;
            if (memberInfo.group === 0) {
                if (customComponentFlagsArray[j] || crossPackageFlagsArray[j]) {
                    modelViewCodeWriter.writeln('\t\t%s.Dispose();', memberVarName);
                }
            }
            modelViewCodeWriter.writeln('\t\t%s = null;', memberVarName);
        }
        modelViewCodeWriter.writeln('\t}');
        modelViewCodeWriter.writeln('}');
        modelViewCodeWriter.endBlock();
        modelViewCodeWriter.save(modelViewExportCodePath + '/' + className + '.cs');
        // 写入fuipackage
        modelViewCodeWriter.reset();
        modelViewCodeWriter.writeln('namespace %s', namespaceName);
        modelViewCodeWriter.startBlock();
        modelViewCodeWriter.writeln('public static partial class FUIPackage');
        modelViewCodeWriter.startBlock();
        modelViewCodeWriter.writeln('public const string %s = "%s";', codePkgName, codePkgName);
        for (let i = 0; i < this.classes.Count; i++) {
            let classInfo = this.classes.get_Item(i);
            modelViewCodeWriter.writeln('public const string %s_%s = "ui://%s/%s";', codePkgName, classInfo.resName, codePkgName, classInfo.resName);
        }
        modelViewCodeWriter.endBlock(); //class
        modelViewCodeWriter.endBlock(); //namespace
        let binderPackageName = 'Package' + codePkgName;
        modelViewCodeWriter.save(modelViewExportCodePath + '/' + binderPackageName + '.cs');
    }
    generateHotfixViewCode(classInfo, hotfixViewCodeWriter, customComponentFlagsArray, crossPackageFlagsArray) {
        let hotfixViewExportCodePath = this.hotfixViewExportCodePath;
        let namespaceName = CodeGenConfig_1.CodeGenConfig.HotfixViewNameSpace;
        let getMemberByName = this.settings.getMemberByName;
        // 初始化自定义组件名前缀
        let classNamePrefix = CodeGenConfig_1.CodeGenConfig.ClassNamePrefix;
        // 初始化自定义成员变量名前缀
        let memberVarNamePrefix = CodeGenConfig_1.CodeGenConfig.MemberVarNamePrefix;
        // 组装自定义组件前缀
        let className = classNamePrefix + classInfo.className;
        let systemName = className + "System";
        let members = classInfo.members;
        hotfixViewCodeWriter.reset();
        hotfixViewCodeWriter.writeln('using FairyGUI;');
        hotfixViewCodeWriter.writeln('using System.Threading.Tasks;');
        hotfixViewCodeWriter.writeln();
        hotfixViewCodeWriter.writeln('namespace %s', namespaceName);
        hotfixViewCodeWriter.startBlock();
        // writer.writeln('public partial class %s : %s', classInfo.className, classInfo.superClassName);
        // writer.startBlock();
        // awake system
        hotfixViewCodeWriter.writeln(`public class %sAwakeSystem : AwakeSystem<%s, GObject>
    {
        public override void Awake(%s self, GObject go)
        {
            self.Awake(go);
        }
    }
        `, className, className, className);
        // add component system
        hotfixViewCodeWriter.writeln(`public static class %s
    {`, systemName);
        hotfixViewCodeWriter.writeln(`
        public static void Awake(this %s self, GObject go)
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
            
            self.FGComp = (%s)go;
            
            self.FGComp.Add(self);
            
            var com = go.asCom;
                
            if(com != null)
            {	
                    `, className, classInfo.superClassName);
        let memberCnt = members.Count;
        for (let j = 0; j < memberCnt; j++) {
            let memberInfo = members.get_Item(j);
            // 组装自定义成员前缀
            let memberVarName = memberVarNamePrefix + memberInfo.varName;
            if (memberInfo.group === 0) {
                if (getMemberByName) {
                    if (customComponentFlagsArray[j]) {
                        // 组装自定义组件前缀
                        hotfixViewCodeWriter.writeln('\t\t\tself.%s = FUI.Create<%s>(self, com.GetChild("%s"));', memberVarName, classNamePrefix + memberInfo.type, memberInfo.name);
                    }
                    else if (crossPackageFlagsArray[j]) {
                        // 组装自定义组件前缀
                        hotfixViewCodeWriter.writeln('\t\t\tself.%s = FUI.Create<%s>(self, com.GetChild("%s"));', memberVarName, classNamePrefix + memberInfo.res.name, memberInfo.name);
                    }
                    else {
                        hotfixViewCodeWriter.writeln('\t\t\tself.%s = (%s)com.GetChild("%s");', memberVarName, memberInfo.type, memberInfo.name);
                    }
                }
                else {
                    if (customComponentFlagsArray[j]) {
                        // 组装自定义组件前缀
                        hotfixViewCodeWriter.writeln('\t\t\tself.%s = FUI.Create<%s>(self, com.GetChildAt(%s));', memberVarName, classNamePrefix + memberInfo.type, memberInfo.index);
                    }
                    else if (crossPackageFlagsArray[j]) {
                        // 组装自定义组件前缀
                        hotfixViewCodeWriter.writeln('\t\t\tself.%s = FUI.Create<%s>(self, com.GetChildAt(%s));', memberVarName, classNamePrefix + memberInfo.res.name, memberInfo.index);
                    }
                    else {
                        hotfixViewCodeWriter.writeln('\t\t\tself.%s = (%s)com.GetChildAt(%s);', memberVarName, memberInfo.type, memberInfo.index);
                    }
                }
            }
            else if (memberInfo.group == 1) {
                if (getMemberByName) {
                    hotfixViewCodeWriter.writeln('\t\t\tself.%s = com.GetController("%s");', memberVarName, memberInfo.name);
                }
                else {
                    hotfixViewCodeWriter.writeln('\t\t\tself.%s = com.GetControllerAt(%s);', memberVarName, memberInfo.index);
                }
            }
            else {
                if (getMemberByName) {
                    hotfixViewCodeWriter.writeln('\t\t\tself.%s = com.GetTransition("%s");', memberVarName, memberInfo.name);
                }
                else {
                    hotfixViewCodeWriter.writeln('\t\t\tself.%s = com.GetTransitionAt(%s);', memberVarName, memberInfo.index);
                }
            }
        }
        hotfixViewCodeWriter.writeln('\t\t}');
        hotfixViewCodeWriter.writeln('\t}');
        hotfixViewCodeWriter.writeln('}');
        hotfixViewCodeWriter.endBlock();
        hotfixViewCodeWriter.save(hotfixViewExportCodePath + '/' + className + '.cs');
    }
    generateLogicCode(classInfo, logicCodeWriter) {
        // 只关注需要导出的类
        if (classInfo.res.exported) {
            let modelViewLogicExportCodePath = this.modelViewLogicExportCodePath;
            let hotfixViewLogicExporCodePath = this.hotfixViewLogicExporCodePath;
            let namespaceName = CodeGenConfig_1.CodeGenConfig.ModelViewNameSpace;
            let classNamePrefix = CodeGenConfig_1.CodeGenConfig.ClassNamePrefix;
            let compClassName = classNamePrefix + classInfo.className;
            let compPropName = "FUI" + classInfo.className;
            let logicClassNamePrefix = CodeGenConfig_1.CodeGenConfig.LogicClassNamePrefix;
            let logicComponentClassName = logicClassNamePrefix + classInfo.className + "Component";
            let logicSystemClassName = logicComponentClassName + "System";
            let logicComponentClassFilePath = modelViewLogicExportCodePath + "/" + logicComponentClassName + '.cs';
            let logicSystemClassFilePath = hotfixViewLogicExporCodePath + "/" + logicSystemClassName + '.cs';
            let isWindow = false;
            if (classInfo.className.endsWith("Window")) {
                isWindow = true;
            }
            if (!csharp_1.System.IO.Directory.Exists(modelViewLogicExportCodePath)) {
                csharp_1.System.IO.Directory.CreateDirectory(modelViewLogicExportCodePath);
            }
            // 生成Component代码
            if (!csharp_1.System.IO.File.Exists(logicComponentClassFilePath)) {
                logicCodeWriter.reset();
                logicCodeWriter.writeln('namespace %s', namespaceName);
                logicCodeWriter.startBlock();
                if (isWindow) {
                    logicCodeWriter.writeln(`
    public class %s : FUIWindowComponent
    {
        public %s %s;
    }
                `, logicComponentClassName, compClassName, compPropName);
                }
                else {
                    logicCodeWriter.writeln(`
    public class %s : Entity, IAwake, IDestroy
    {
        public %s %s;
    }
                `, logicComponentClassName, compClassName, compPropName);
                }
                logicCodeWriter.endBlock();
                logicCodeWriter.save(logicComponentClassFilePath);
            }
            if (!csharp_1.System.IO.Directory.Exists(hotfixViewLogicExporCodePath)) {
                csharp_1.System.IO.Directory.CreateDirectory(hotfixViewLogicExporCodePath);
            }
            // 生成System代码
            if (!csharp_1.System.IO.File.Exists(logicSystemClassFilePath)) {
                let logicAwakeSystemName = logicComponentClassName + "AwakeSystem";
                logicCodeWriter.reset();
                if (isWindow) {
                    logicCodeWriter.writeln('using FairyGUI;');
                }
                logicCodeWriter.writeln('namespace %s', namespaceName);
                logicCodeWriter.startBlock();
                logicCodeWriter.writeln(`
    public class %s : AwakeSystem<%s>
    {
        public override void Awake(%s self)
        {
            self.Awake();
        }
    }
                `, logicAwakeSystemName, logicComponentClassName, logicComponentClassName);
                if (isWindow) {
                    logicCodeWriter.writeln(`
    public static class %s
    {
        public static void Awake(this %s self)
        {
            self.%s = self.GetParent<%s>();      
            self.Window = new Window();
            self.Window.contentPane = self.%s.FGComp;
        }
    } 
                `, logicSystemClassName, logicComponentClassName, compPropName, compClassName, compPropName);
                }
                else {
                    logicCodeWriter.writeln(`
    public static class %s
    {
        public static void Awake(this %s self)
        {
            self.%s = self.GetParent<%s>();  
        }
    }     
                `, logicSystemClassName, logicComponentClassName, compPropName, compClassName);
                }
                logicCodeWriter.endBlock();
                logicCodeWriter.save(logicSystemClassFilePath);
            }
        }
    }
}
const fuiCodeGenerator = new FUICodeGenerator();
exports.fuiCodeGenerator = fuiCodeGenerator;
