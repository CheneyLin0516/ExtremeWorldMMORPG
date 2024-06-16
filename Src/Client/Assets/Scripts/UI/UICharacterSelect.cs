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
<<<<<<< HEAD
        DataManager.Instance.Load();//网络未连接的时候使用

        InitCharacterSelect(true);
        UserService.Instance.OnCharacterCreate = OnCharacterCreate;//订阅从Userservice处来的消息，并且执行“角色创建成功与否的方法”
    }

=======
        InitCharacterSelect(true);
     //   UserService.Instance.OnCharacterCreate = OnCharacterCreate;
	}
	
>>>>>>> parent of 498d36e (Version 1.2.6)
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
        this.charClass = (CharacterClass)charClass;

        characterView.CurrentCharacter = charClass - 1;

        for (int i = 0; i < 3; i++)
        {
            titles[i].gameObject.SetActive(i == charClass - 1);
            names[i].text = DataManager.Instance.Characters[i + 1].Name;
        }

        descs.text = DataManager.Instance.Characters[charClass].Description;
    }

<<<<<<< HEAD
    void OnCharacterCreate(Result result, string message)//根据逻辑层的消息，执行“角色创建成功与否的方法”
=======
    void OnCharacterCreate (Result result, string message)
>>>>>>> parent of 498d36e (Version 1.2.6)
    {
        if (result == Result.Success)
        {
            InitCharacterSelect(true);
        }
        else
            MessageBox.Show(message, "错误", MessageBoxType.Error);
    }

    public void InitCharacterSelect(bool init)
    {
        panelCreate.SetActive(false);
        panelSelect.SetActive(true);

        if (init)
        {
<<<<<<< HEAD
            foreach (var old in uiChars)//var 是 C# 中的一个关键字，用于在局部变量声明时让编译器自动推断变量的类型。在这里是GameObject
=======
            foreach(var old in uiChars)
>>>>>>> parent of 498d36e (Version 1.2.6)
            {
                Destroy(old);
            }
            uiChars.Clear();

<<<<<<< HEAD
            for (int i = 0; i < User.Instance.Info.Player.Characters.Count; i++)
=======
           /* for (int i = 0; i <User.Instance.Info.Player.Characters.Count; i++)
>>>>>>> parent of 498d36e (Version 1.2.6)
            {
                GameObject go = Instantiate(uiCharInfo, this.uiCharList);
                UICharInfo chrInfo = go.GetComponent<UICharInfo>();
                chrInfo.info = User.Instance.Info.Player.Characters[i];

                Button button = go.GetComponent<Button>();
                int idx = i;
                button.onClick.AddListener(() =>
                {
                    OnSelectCharacter(idx);
                });

                uiChars.Add(go);
                go.SetActive(true);
            }*/
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

<<<<<<< HEAD

    public void OnClickPlay()
    {
        if (selectCharacterIdx >= 0)
        {
           //MessageBox.Show("进入游戏", "进入游戏", MessageBoxType.Confirm);
           UserService.Instance.SendGameEnter(selectCharacterIdx);
        }
    }

=======
    /*public void OnClickPlay()
    {
        if (selectCharacterIdx >= 0)
        {
           // UserService.Instance.SendGameEnter(selectCharacterIdx);
        }
    }*/
>>>>>>> parent of 498d36e (Version 1.2.6)
}
