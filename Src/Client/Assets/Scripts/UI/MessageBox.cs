using UnityEngine;

//工厂类,设计模式，通过负责创建和显示 UIMessageBox 实例，并对其进行初始化。
class MessageBox
{
    static Object cacheObject = null;

    public static UIMessageBox Show(string message, string title="", MessageBoxType type = MessageBoxType.Information, string btnOK = "", string btnCancel = "")//传入到UImessagebox类里
    {
        if (cacheObject==null)
        {
            cacheObject = Resloader.Load<Object>("UI/UIMessageBox"); //Resloader.Load<T>(string Path)加载预制体的方法，地址指在 Unity 项目中 Resources 文件夹下的相对路径
                                                                     //在这里就是UI文件夹下的UIMessageBox文件
                                                                     //这个预制体是一个模板，可以用来实例化多个游戏对象。
        }

        GameObject go = (GameObject)GameObject.Instantiate(cacheObject);//Instantiate实例化一个cacheObject并将其转换成GameObject类型
                                                                        //第一个 GameObject 是类型名称，它代表 Unity 引擎中的游戏对象类，用于声明或指明操作的对象类型。
                                                                        //第二个 GameObject 是静态方法 Instantiate 所属的类名，表示你正在调用 GameObject 类的 Instantiate 静态方法。
                                                                        //这个方法用于创建对象的实例，通常用于复制预制体（prefabs）或其他游戏对象。
        UIMessageBox msgbox = go.GetComponent<UIMessageBox>();//得到这个实例化对象的UI参数，
        msgbox.Init(title, message, type, btnOK, btnCancel);//根据提供的参数初始化它，然后将这个已配置好的实例返回给调用者。
        return msgbox;
    }
}

public enum MessageBoxType
{
    /// <summary>
    /// Information Dialog with OK button
    /// </summary>
    Information = 1,

    /// <summary>
    /// Confirm Dialog whit OK and Cancel buttons
    /// </summary>
    Confirm = 2,

    /// <summary>
    /// Error Dialog with OK buttons
    /// </summary>
    Error = 3
}