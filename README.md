## 西瓜
CQuark（西瓜） 是一个简单的C#语法的脚本解析器。可以用于Unity热更新。支持Windows,iOS,Android平台。

* 本项目是在Unity3D项目环境下运行。如果在非Unity3D环境下运行，直接删除Assets/_Unity文件夹和Demo文件夹即可。

* 西瓜的前身是疯光无线前辈写的C#LightEvil和自己曾经写的一个脚本语言。

项目最新地址：        https://github.com/flow119/CQuark

疯光无线前辈的项目地址：https://github.com/lightszero/cslightcore

以及疯光无线的Unity案例：https://github.com/lightszero/CSLightStudio



## 西瓜的优势

* 可以热更新。
* 纯C#语法，你不用去学lua了。
* 编辑器下无需生成代码，如果你乐意，你直接拿.cs文件后缀改成.txt就能用。
* 据说执行效率高于lua，但还未在iOS设备上测试过。




## 版本更新记录
2018-03-21 v0.8.1

    Update和FixedUpdate这样频繁调用的函数有特殊的调用方法，加快速度
    CQ_Content里的string function只在宏定义（#CQURK_DEFINE）的时候用，减少GC
    
    
2018-03-20 v0.8.0

    做了大量优化，运行速度提升40%，GC开销减少50%。（目前效率约是原生C#的1/4）
    1：大部分List改成了Array，减少GC。
    2：Time和Vector3里的静态变量或常量直接从一个字典里获取（比如Time.deltaTime，Vector3.up）等，加快速度
    3：CQ_Content里的Stack和Dictionary默认值为null，只在赋值时创建。
        由于几乎所有的函数都会new CQ_Content，这样可以节省极大的内存开销，减少GC并提高执行效率。
    
2018-03-18 v0.7.9

    大量函数与类改了名字。
    基于CQuarkBehaviour的类不再需要声明gameObject、transform等Unity的只读变量。
    
2018-03-14 v0.7.8

    environment改名叫AppDomain，不再作为content的成员，只包含静态方法。

2018-03-13 v0.7.7
    
    CQ_TokenParser改成了静态类，删除了ICQ_TokenParser。
    environment里不再需要成员icq_tokenParser。
    CQ_TokenParser将缓存所有编译的文本到一个字典。（可以手动清空以重新编译）。
    env里不再存loger，改成用静态类DebugUtil。
    Script改名为CQuarkClass，ScriptMono改名为CQuarkBehaviour
    
2017-10-10 v0.7.6
    
    重新制作了范例。
    类里的协程也能直接被调用。
    完善了ScriptMono类，允许只编译单个脚本（string或者TextAsset都可以），详细见Demo07。
    
2017-10-09 v0.7.5
    
    处理windows下用记事本写脚本会在开头出现一个不可解析的字符。
    修正了带协程的循环中break逻辑错乱的问题。
    完善注册类型（增加Queue和Stack支持）。
     
2017-09-22 v0.7.4
    
    简化RegFunction函数。

2017-09-17 v0.7.3
    
    新增switch case语法，支持switch case嵌套，但还不支持goto。

2017-09-15 v0.7.2
    
    if后接else if无法解析的问题修复。

2017-09-14 v0.7.1
    
    支持协程(Coroutine)。
    增加ScriptMono（类似于MonoBehaviour）以及对应的Demo3。
    ScriptMono增加Inspector（选择加载类型，重载文本按钮等）。
    文件从StreamingAssets或Persistent目录加载（更接近热更方案）。

2017-09-13 v0.7.0
    
    把cslightcore迁移过来，这个版本和cslight的0.64.1Beta完全一样。
    Unity的Demo1(执行函数块)。
    Unity的Demo2(从外部加载类并执行类里的函数)。


## TODO

下个版本

* 重写MonoBehaviour//参考ILRuntime
* 1 如果父类继承MonoBehaviour，子类不写Update\Start等不会走父类的方法。
* 2 劫持GetComponent和AddComponent，重写。

* yield return使用方式Unity来


下下个版本

* 参考ILRuntime和L#，看看西瓜还有哪些不足，补足缺陷

* 把每帧都容易获取的内容存下来（比如Time.deltaTime）

* 优化编译速度，减少gc alloc以及重复的GetCodeKey

下下下个版本

* 类似XLua和Bridge，把项目里的cs文件转换成可以动态替换为西瓜的脚本

* 执行效率测试，主要看反射在iOS上的速度


## 联系我
QQ:181664367
