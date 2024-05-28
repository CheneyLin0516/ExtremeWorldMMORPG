using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;
using SkillBridge.Message;

public class UILogin : MonoBehaviour {

    public InputField username;
    public InputField password;
    public Button buttonLogin;
    public Button buttonRegister;

	// Use this for initialization
	void Start ()
    {
        UserService.Instance.OnLogin = OnLogin;//在ui层启用，告诉服务器要接收登录成功的通知
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickLogin()
    {
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

        UserService.Instance.SendLogin(this.username.text, this.password.text);
    }

    void OnLogin(Result result, string message)//UI层收到用户成功登录的消息，转到角色选择的场景
    {
        if (result == Result.Success)
        {
            MessageBox.Show("登陆成功，准备角色选择" + message, "提示", MessageBoxType.Information);
            SceneManager.Instance.LoadScene("CharSelect");
        }
        else
            MessageBox.Show(message, "错误", MessageBoxType.Error);
    }

}
