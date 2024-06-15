using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Models;
using Services;
using SkillBridge.Message;

public class UICharacterSelect : MonoBehaviour
{

    public GameObject panelCreate;
    public GameObject panelSelect;

    public GameObject btnCreateCancel;

    public InputField charName;
    CharacterClass charClass;

    public Transform uiCharList;
    public GameObject uiCharInfo;

    public List<GameObject> uiChars = new List<GameObject>();

    public Image[] titles;

    public Text descs;

    public Text[] names;

    private int selectCharacterIdx = -1;

    public UICharacterView characterView;

    void Start()
    {
        DataManager.Instance.Load();//网络未连接的时候使用

        InitCharacterSelect(true);
        UserService.Instance.OnCharacterCreate = OnCharacterCreate;//订阅从Userservice处来的消息，并且执行“角色创建成功与否的方法”
    }

    public void InitCharacterCreate()
    {
        panelCreate.SetActive(true);
        panelSelect.SetActive(false);
        OnSelectClass(1);
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickCreate()
    {
        if (string.IsNullOrEmpty(this.charName.text))
        {
            MessageBox.Show("请输入角色名称");
            return;//return 语句用于提前退出方法。确保当某些条件不满足时，方法不会继续执行其余的代码。
        }
        UserService.Instance.SendCharacterCreate(this.charName.text, this.charClass);
    }

    //选择职业
    public void OnSelectClass(int charClass)
    {
        this.charClass = (CharacterClass)charClass;//传入的整数索引转换为 CharacterClass 枚举类型,并赋值给类的成员变量 this.charClass

        characterView.CurrentCharacter = charClass - 1;//初始值传入的是0，在OnSelectClass（1） 这个实现的时候被引用

        for (int i = 0; i < 3; i++)
        {
            titles[i].gameObject.SetActive(i == charClass - 1);
            names[i].text = DataManager.Instance.Characters[i + 1].Name;//在登录的时候做的预加载，所以本地的时候需要加载datamanager
        }

        descs.text = DataManager.Instance.Characters[charClass].Description;
    }

    void OnCharacterCreate(Result result, string message)//根据逻辑层的消息，执行“角色创建成功与否的方法”
    {
        if (result == Result.Success)
        {
            InitCharacterSelect(true);//角色创建成功消息收到，执行角色初始化方法
        }
        else
            MessageBox.Show(message, "错误", MessageBoxType.Error);
    }

    public void InitCharacterSelect(bool init)
    {
        //创建已经成功，隐藏创建面板，打开选择面板
        panelCreate.SetActive(false);
        panelSelect.SetActive(true);

        if (init)
        {
            foreach (var old in uiChars)//var 是 C# 中的一个关键字，用于在局部变量声明时让编译器自动推断变量的类型。在这里是GameObject
            {
                Destroy(old);
            }
            uiChars.Clear();

            for (int i = 0; i < User.Instance.Info.Player.Characters.Count; i++)
            {
                GameObject go = Instantiate(uiCharInfo, this.uiCharList);//角色选择面板uiCharList滚动区
                UICharInfo chrInfo = go.GetComponent<UICharInfo>();
                chrInfo.info = User.Instance.Info.Player.Characters[i];

                Button button = go.GetComponent<Button>();//这个按钮是确认选择的角色，在角色选择面板，这个button是动态创建的
                int idx = i;
                button.onClick.AddListener(() =>
                {
                    OnSelectCharacter(idx);
                });//按下的时候执行选择角色

                uiChars.Add(go);
                go.SetActive(true);
            }
        }
    }

    public void OnSelectCharacter(int idx)
    {
        this.selectCharacterIdx = idx;
        var cha = User.Instance.Info.Player.Characters[idx];
        Debug.LogFormat("Select Char:[{0}]{1}[{2}]", cha.Id, cha.Name, cha.Class);
        User.Instance.CurrentCharacter = cha;
        characterView.CurrentCharacter = ((int)cha.Class - 1);

        for (int i = 0; i < User.Instance.Info.Player.Characters.Count; i++)
        {
            UICharInfo ci = this.uiChars[i].GetComponent<UICharInfo>();
            ci.Selected = idx == i;
        }
    }


    public void OnClickPlay()
    {
        if (selectCharacterIdx >= 0)
        {
           //MessageBox.Show("进入游戏", "进入游戏", MessageBoxType.Confirm);
           UserService.Instance.SendGameEnter(selectCharacterIdx);
        }
    }

}
