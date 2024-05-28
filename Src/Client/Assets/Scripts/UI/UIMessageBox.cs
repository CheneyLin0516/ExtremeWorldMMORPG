using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//整个脚本在设置弹出的消息框的一些外观、行为，相当于消息框的模板

public class UIMessageBox : MonoBehaviour {

    //定义UIMessageBox 类中的一些公共成员变量，这些变量表示消息框的各种UI元素和事件处理器。
    //每个成员变量都有特定的用途，用于[控制消息框的外观和行为]。
    public Text title;
    public Text message;
    public Image[] icons;
    public Button buttonYes;
    public Button buttonNo;
    public Button buttonClose;

    public Text buttonYesTitle;
    public Text buttonNoTitle;

    public UnityAction OnYes;
    public UnityAction OnNo;
    

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //定义一个 Init 方法
    //用于初始化 UIMessageBox 实例的内容和行为。
    //该方法接受一些参数，并根据这些参数设置消息框的标题、消息内容、图标、按钮文本，以及按钮的点击事件处理程序。

    public void Init(string title, string message, MessageBoxType type = MessageBoxType.Information, string btnOK = "", string btnCancel = "")
    {
        if (!string.IsNullOrEmpty(title)) this.title.text = title;
        this.message.text = message;
        this.icons[0].enabled = type == MessageBoxType.Information; //enabled 是一个布尔属性，用于控制 Unity 组件的启用和禁用状态
                                                                    //此处先判断type是不是后面的类型，如果是，enabled将其设置为true
        this.icons[1].enabled = type == MessageBoxType.Confirm;
        this.icons[2].enabled = type == MessageBoxType.Error;

        if (!string.IsNullOrEmpty(btnOK)) this.buttonYesTitle.text = title;
        if (!string.IsNullOrEmpty(btnCancel)) this.buttonNoTitle.text = title;

        this.buttonYes.onClick.AddListener(OnClickYes);//AddListener添加一个监听器，onClick当按钮被点击时，调用 OnClickYes 方法。
        this.buttonNo.onClick.AddListener(OnClickNo);

        this.buttonNo.gameObject.SetActive(type == MessageBoxType.Confirm);//如果type是confirm类型，启动no按钮，否则不启动
    }

    void OnClickYes()
    {
        Destroy(this.gameObject);
        if (this.OnYes != null)
            this.OnYes();
    }

    void OnClickNo()
    {
        Destroy(this.gameObject);
        if (this.OnNo != null)
            this.OnNo();
    }
}
