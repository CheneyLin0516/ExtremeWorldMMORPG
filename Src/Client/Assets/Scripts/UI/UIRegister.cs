using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;
using SkillBridge.Message;

public class UIRegister : MonoBehaviour
{

    public InputField username;
    public InputField password;
    public InputField passwordConfirm;
    public Button buttonRegister;

    //public GameObject uiLogin;
    // Use this for initialization
    void Start()
    {
        //UserService.Instance.OnRegister = OnRegister;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickRegister()
    {
        //UI界面做的事情，检查界面输入的正确性，做输入的校验
        if (string.IsNullOrEmpty(this.username.text))
        {
            MessageBox.Show("请输入账号");
            return;
        }
        if (string.IsNullOrEmpty(this.password.text))
        {
            MessageBox.Show("请输入密码");
            return;
        }
        if (string.IsNullOrEmpty(this.passwordConfirm.text))
        {
            MessageBox.Show("请输入确认密码");
            return;
        }
        if (this.password.text != this.passwordConfirm.text)
        {
            MessageBox.Show("两次输入的密码不一致");
            return;
        }
        UserService.Instance.SendRegister(this.username.text, this.password.text);
    }
}
