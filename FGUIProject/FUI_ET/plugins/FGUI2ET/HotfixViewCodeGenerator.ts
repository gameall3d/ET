import { FairyEditor } from "csharp";
import { CodeGenConfig } from "./CodeGenConfig";
import CodeWriter from "./CodeWriter";

class HotfixViewCodeGenerator
{
    public Handle(handler: FairyEditor.PublishHandler)
    {
        let settings = (<FairyEditor.GlobalPublishSettings>handler.project.GetSettings("Publish")).codeGeneration;
        let codePkgName = handler.ToFilename(handler.pkg.name); //convert chinese to pinyin, remove special chars etc.
        
        // 从自定义配置中读取路径和命名空间
        let HotfixViewExportCodePath = CodeGenConfig.HotfixViewCodeOutputPath + '/' + codePkgName
        let ModelViewExportCodePath = CodeGenConfig.ModelViewCodeOutputPath + '/' + codePkgName
        let namespaceName = CodeGenConfig.HotfixNameSpace

        // 初始化自定义组件名前缀
        let classNamePrefix = CodeGenConfig.ClassNamePrefix;
        // 初始化自定义成员变量名前缀
        let memberVarNamePrefix = CodeGenConfig.MemberVarNamePrefix;

        //CollectClasses(stripeMember, stripeClass, fguiNamespace)
        let classes = handler.CollectClasses(settings.ignoreNoname, settings.ignoreNoname, null);
        handler.SetupCodeFolder(HotfixViewExportCodePath, "cs"); //check if target folder exists, and delete old files
        handler.SetupCodeFolder(ModelViewExportCodePath, "cs"); //check if target folder exists, and delete old files

        let getMemberByName = settings.getMemberByName;

        let classCnt = classes.Count;
        let hotfixViewCodeWriter = new CodeWriter();
        let modelViewCodeWriter = new CodeWriter();
        for (let i: number = 0; i < classCnt; i++) {
            let classInfo = classes.get_Item(i);
            let members = classInfo.members;

            hotfixViewCodeWriter.reset();
            hotfixViewCodeWriter.writeln('using FairyGUI;');
            hotfixViewCodeWriter.writeln('using System.Threading.Tasks;');
            hotfixViewCodeWriter.writeln();
            hotfixViewCodeWriter.writeln('namespace %s', namespaceName);
            hotfixViewCodeWriter.startBlock();

            modelViewCodeWriter.reset();
            modelViewCodeWriter.writeln('using FairyGUI;');
            modelViewCodeWriter.writeln('using System.Threading.Tasks;');
            modelViewCodeWriter.writeln();
            modelViewCodeWriter.writeln('namespace %s', namespaceName);
            modelViewCodeWriter.startBlock();
            // writer.writeln('public partial class %s : %s', classInfo.className, classInfo.superClassName);
            // writer.startBlock();

            // 组装自定义组件前缀
            let className = classNamePrefix + classInfo.className
            let systemName = className + "System";

            // awake system
            hotfixViewCodeWriter.writeln(
    `public class %sAwakeSystem : AwakeSystem<%s, GObject>
    {
        public override void Awake(%s self, GObject go)
        {
            self.Awake(go);
        }
    }
        `, className, className, className);
            
            modelViewCodeWriter.writeln(`public sealed class %s : FUI
    {	
        public const string UIPackageName = "%s";
        public const string UIResName = "%s";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public %s FGComp;
                        `, className, codePkgName, classInfo.resName, classInfo.superClassName)
    
            let memberCnt = members.Count

            // 是否为自定义类型组件标记数组
            let customComponentFlagsArray = {}
            // 是否为跨包组件标记数组
            let crossPackageFlagsArray = {}
            for (let j: number = 0; j < memberCnt; j++) {
                let memberInfo = members.get_Item(j);
                customComponentFlagsArray[j] = false;
                crossPackageFlagsArray[j] = false;

                // 判断是不是我们自定义类型组件
                let typeName = memberInfo.type;
                for (let k = 0; k < classCnt; k++)
                {
                    if (typeName === classes.get_Item(k).className)
                    {
                        typeName = classNamePrefix + classes.get_Item(k).className;
                        customComponentFlagsArray[j] = true;
                        break;
                    }
                }

                // 判断是不是跨包类型组件
                if (memberInfo && memberInfo.res)
                {
                    // 组装自定义组件前缀
                    typeName = classNamePrefix + memberInfo.res.name;
                    crossPackageFlagsArray[j] = true;
                }

                // 组装自定义成员前缀
                modelViewCodeWriter.writeln('\tpublic %s %s;', typeName, memberVarNamePrefix + memberInfo.varName);
            }
            modelViewCodeWriter.writeln('\tpublic const string URL = "ui://%s%s";', handler.pkg.id, classInfo.resId);
            modelViewCodeWriter.writeln();

            modelViewCodeWriter.writeln(` 
        private static GObject CreateGObject()
        {
            return UIPackage.CreateObject(UIPackageName, UIResName);
        }

        private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
        {
            UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
        }
                `);

            modelViewCodeWriter.writeln(`   
        public static %s CreateInstance(Entity parent)
        {			
            return parent.AddChild<%s, GObject>(CreateGObject());
        }
                `, className, className);

            modelViewCodeWriter.writeln(`    
        public static ETTask<%s> CreateInstanceAsync(Entity parent)
        {
            ETTask<%s> tcs = ETTask<%s>.Create(true);

            CreateGObjectAsync((go) =>
            {
                tcs.SetResult(parent.AddChild<%s, GObject>(go));
            });

            return tcs;
        }
                `, className, className, className, className);

            modelViewCodeWriter.writeln(`    
        /// <summary>
        /// 仅用于go已经实例化情况下的创建（例如另一个组件引用了此组件）
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="go"></param>
        /// <returns></returns>
        public static %s Create(Entity parent, GObject go)
        {
            return parent.AddChild<%s, GObject>(go);
        }
                    `, className, className);
            
            modelViewCodeWriter.writeln(`   
        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static %s GetFormPool(Entity domain, GObject go)
        {
            var fui = go.Get<%s>();
        
            if(fui == null)
            {
                fui = Create(domain, go);
            }
        
            fui.isFromFGUIPool = true;
        
            return fui;
        }
                `, className, className);

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
                    `,className, classInfo.superClassName);   
                    
            for(let j = 0; j < memberCnt; j++)
            {
                let memberInfo = members.get_Item(j);
                // 组装自定义成员前缀
                let memberVarName = memberVarNamePrefix + memberInfo.varName;
                if (memberInfo.group === 0)
                {
                    if (getMemberByName)
                    {
                        if (customComponentFlagsArray[j])
                        {
                            // 组装自定义组件前缀
                            hotfixViewCodeWriter.writeln('\t\t\tself.%s = %s.Create(self, com.GetChild("%s"));', memberVarName, classNamePrefix + memberInfo.type, memberInfo.name);
                        } 
                        else if(crossPackageFlagsArray[j])
                        {
                            // 组装自定义组件前缀
                            hotfixViewCodeWriter.writeln('\t\t\tself.%s = %s.Create(self, com.GetChild("%s"));', memberVarName, classNamePrefix + memberInfo.res.name, memberInfo.name);
                        } 
                        else
                        {
                            hotfixViewCodeWriter.writeln('\t\t\tself.%s = (%s)com.GetChild("%s");', memberVarName, memberInfo.type, memberInfo.name);
                        }
                    }
                    else
                    {
                        if (customComponentFlagsArray[j])
                        {
                            // 组装自定义组件前缀
                            hotfixViewCodeWriter.writeln('\t\t\tself.%s = %s.Create(self, com.GetChildAt(%s));', memberVarName, classNamePrefix + memberInfo.type, memberInfo.index);
                        }
                        else if(crossPackageFlagsArray[j])
                        {
                            // 组装自定义组件前缀
                            hotfixViewCodeWriter.writeln('\t\t\tself.%s = %s.Create(self, com.GetChildAt(%s));', memberVarName, classNamePrefix + memberInfo.res.name, memberInfo.index);
                        }
                        else
                        {
                            hotfixViewCodeWriter.writeln('\t\t\tself.%s = (%s)com.GetChildAt(%s);', memberVarName, memberInfo.type, memberInfo.index);
                        }
                        
                    }
                } 
                else if (memberInfo.group == 1)
                {
                    if (getMemberByName)
                    {
                        hotfixViewCodeWriter.writeln('\t\t\tself.%s = com.GetController("%s");', memberVarName, memberInfo.name)
                    }
                    else
                    {
                        hotfixViewCodeWriter.writeln('\t\t\tself.%s = com.GetControllerAt(%s);', memberVarName, memberInfo.index)
                    }
                }
                else
                {
                    if (getMemberByName)
                    {
                        hotfixViewCodeWriter.writeln('\t\t\tself.%s = com.GetTransition("%s");', memberVarName, memberInfo.name)
                    }
                    else
                    {
                        hotfixViewCodeWriter.writeln('\t\t\tself.%s = com.GetTransitionAt(%s);', memberVarName, memberInfo.index)
                    }
                }
            }

            hotfixViewCodeWriter.writeln('\t\t}')

            hotfixViewCodeWriter.writeln('\t}')

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
                    `)
    
            for (let j = 0; j < memberCnt; j++)
            {
                let memberInfo = members.get_Item(j);
                // 组装自定义成员前缀
                let memberVarName = memberVarNamePrefix + memberInfo.varName;
                if (memberInfo.group === 0)
                {
                    if (customComponentFlagsArray[j] || crossPackageFlagsArray[j])
                    {
                        modelViewCodeWriter.writeln('\t\t%s.Dispose();', memberVarName)
                    }
                }
                modelViewCodeWriter.writeln('\t\t%s = null;', memberVarName)
            }
            modelViewCodeWriter.writeln('\t}');

            modelViewCodeWriter.writeln('}');
            modelViewCodeWriter.endBlock();
    
            modelViewCodeWriter.save(ModelViewExportCodePath + '/' + className + '.cs');

            hotfixViewCodeWriter.writeln('}');
            hotfixViewCodeWriter.endBlock();
            hotfixViewCodeWriter.save(HotfixViewExportCodePath + '/' + className + '.cs');
        }

        // 写入fuipackage
        modelViewCodeWriter.reset();
    
        modelViewCodeWriter.writeln('namespace %s', namespaceName);
        modelViewCodeWriter.startBlock();
        modelViewCodeWriter.writeln('public static partial class FUIPackage');
        modelViewCodeWriter.startBlock();
    
        modelViewCodeWriter.writeln('public const string %s = "%s";', codePkgName, codePkgName);

        for(let i = 0; i < classCnt; i++)
        {
            let classInfo = classes.get_Item(i);
            modelViewCodeWriter.writeln('public const string %s_%s = "ui://%s/%s";', codePkgName, classInfo.resName, codePkgName, classInfo.resName);
        }

        modelViewCodeWriter.endBlock(); //class
        modelViewCodeWriter.endBlock(); //namespace
        let binderPackageName = 'Package' + codePkgName;
        modelViewCodeWriter.save(ModelViewExportCodePath + '/' + binderPackageName + '.cs');
    }
}

const hotfixViewCodeGenerator = new HotfixViewCodeGenerator();

export { hotfixViewCodeGenerator }