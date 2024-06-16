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
    public GameObject uiLogin;

    // Use this for initialization
    void Start()
    {
        UserService.Instance.OnRegister = OnRegister;//使用赋值的方式订阅 OnRegister 事件，覆盖之前的任何订阅。
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickRegister()
    {
        //UI界面做的事情，检查界面输入的正确性，做输入的校验
        if (string.IsNullOrEmpty(this.username.text)) //string.IsNullOrEmpty(String str) 返回true 或者false 判断字符串是否为空
        {
            MessageBox.Show("请输入账号");
            return;
        }
        if (string.IsNullOrEmpty(this.password.text))
        {
            MessageBox.Show("请输入密码");//需要考虑的是如何把这个字符串作为内容传递给box显示的，形参实参问题
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

    void OnRegister(Result result, string message)
    {
        if (result == Result.Success)
        {
            MessageBox.Show("注册成功，请登录", "提示", MessageBoxType.Information).OnYes = this.CloseRegister;
        }
        else
        {
            MessageBox.Show(message, "错误", MessageBoxType.Error);
        }
    }

    void CloseRegister()
    {
        this.gameObject.SetActive(false);
        uiLogin.SetActive(true);
    }
}
