## 西瓜
CQuark（西瓜） 是一个简单的C#语法的脚本语言。可以用于Unity热更新。支持Windows,iOS,Android平台。

* 本项目是在Unity3D项目环境下运行。如果在非Unity3D环境下运行，直接删除Assets/_Unity文件夹和Demo文件夹即可。

* 西瓜的前身是疯光无线前辈写的C#LightEvil和自己曾经写的一个脚本语言。

项目最新地址：    	https://github.com/flow119/CQuark

疯光无线前辈的项目地址：https://github.com/lightszero/cslightcore

以及疯光无线的Unity案例：https://github.com/lightszero/CSLightStudio



## 西瓜的优势

* 可以热更新。
* 纯C#语法，你不用去学lua了。
* 编辑器下无需生成代码，如果你乐意，你直接拿.cs文件后缀改成.txt就能用。
* 目前支持几乎所有常用C#语法
* 据说执行效率高于lua，但还未在iOS设备上测试过。




## 版本更新记录
2017-09-15 v0.7.2
    
     if后接else if无法解析的问题
    
2017-09-14 v0.7.1
    
    支持协程(Coroutine)
    增加ScriptMono（类似于MonoBehaviour）以及对应的Demo3
    ScriptMono增加Inspector（选择加载类型，重载文本按钮等）
    文件从StreamingAssets或Persistent目录加载（更接近热更方案）

2017-09-13 v0.7.0
    
    把cslightcore迁移过来，这个版本和cslight的0.64.1Beta完全一样
    Unity的Demo1(执行函数块)
    Unity的Demo2(从外部加载类并执行类里的函数)


## TODO
* switch case语法

下个版本
* ScriptMono里增加调用协程的方法
* ScriptMono增加使用单独文件或使用文本作为类的功能

下下个版本
* env改为全局唯一（出于效率考虑）
* 增加载入单个文件的方法

下下下个版本
* 类似XLua和Bridge，把项目里的cs文件转换成可以动态替换为西瓜的脚本

## 联系我
QQ:181664367
