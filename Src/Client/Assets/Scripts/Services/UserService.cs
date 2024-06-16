using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Network;
using UnityEngine;

using SkillBridge.Message;
using Models;

namespace Services
{
    class UserService : Singleton<UserService>, IDisposable //Singleton<T>单例模式，统一管理网络连接、消息分发等资源，避免重复创建和销毁这些资源。
                                                            //通过Instance 属性，任何地方都可以方便地访问这个唯一的 UserService 实例，而不需要显式地创建或传递实例。
                                                            //在 UserService 类中，实现 IDisposable 接口，可以确保在不再需要 UserService 实例时，正确地释放相关资源（如网络连接、事件订阅等）。
    {

        public UnityEngine.Events.UnityAction<Result, string> OnRegister; //这是一个委托，如果没有任何方法附加到 OnRegister，它的值就是 null。
        public UnityEngine.Events.UnityAction<Result, string> OnLogin; //允许将OnLogin方法附加到事件并在事件触发时调用这些方法
        public UnityEngine.Events.UnityAction<Result, string> OnCharacterCreate;

        NetMessage pendingMessage = null;

        bool connected = false;

        public UserService()
        {
            NetClient.Instance.OnConnect += OnGameServerConnect; //订阅连接消息
            NetClient.Instance.OnDisconnect += OnGameServerDisconnect;

            MessageDistributer.Instance.Subscribe<UserLoginResponse>(this.OnUserLogin);
            MessageDistributer.Instance.Subscribe<UserRegisterResponse>(this.OnUserRegister);
            MessageDistributer.Instance.Subscribe<UserCreateCharacterResponse>(this.OnUserCreateCharacter);
            MessageDistributer.Instance.Subscribe<UserGameEnterResponse>(this.OnGameEnter);
            MessageDistributer.Instance.Subscribe<UserGameLeaveResponse>(this.OnGameLeave);
            MessageDistributer.Instance.Subscribe<MapCharacterEnterResponse>(this.OnCharacterEnter);

            // += 操作符：用于订阅基于委托的事件，当事件触发时，调用附加到该事件的处理程序。
            //Subscribe 方法：用于订阅特定类型的消息，通过消息分发器分发消息并调用相应的处理程序。
        }

        public void Dispose()//用于取消订阅之前订阅的消息和事件
        {
            MessageDistributer.Instance.Unsubscribe<UserLoginResponse>(this.OnUserLogin);
            MessageDistributer.Instance.Unsubscribe<UserRegisterResponse>(this.OnUserRegister);
            MessageDistributer.Instance.Unsubscribe<UserCreateCharacterResponse>(this.OnUserCreateCharacter);
            MessageDistributer.Instance.Unsubscribe<UserGameEnterResponse>(this.OnGameEnter);
            MessageDistributer.Instance.Unsubscribe<UserGameLeaveResponse>(this.OnGameLeave);

            NetClient.Instance.OnConnect -= OnGameServerConnect;
            NetClient.Instance.OnDisconnect -= OnGameServerDisconnect;
        }

        public void Init()
        {

        }

        public void ConnectToServer()
        {
            Debug.Log("ConnectToServer() Start ");
            //NetClient.Instance.CryptKey = this.SessionId;
            NetClient.Instance.Init("127.0.0.1", 8000);
            NetClient.Instance.Connect();
        }


        void OnGameServerConnect(int result, string reason)
        {
            Log.InfoFormat("LoadingMesager::OnGameServerConnect :{0} reason:{1}", result, reason);
            if (NetClient.Instance.Connected)
            {
                this.connected = true;
                if (this.pendingMessage != null)
                {
                    NetClient.Instance.SendMessage(this.pendingMessage);
                    this.pendingMessage = null;
                }
            }
            else
            {
                if (!this.DisconnectNotify(result, reason)) //表示逻辑非操作符。如果 DisconnectNotify 返回 false，逻辑非操作符会将其结果变为 true，因此 if 语句中的代码块会执行。
                {
                    MessageBox.Show(string.Format("网络错误，无法连接到服务器！\n RESULT:{0} ERROR:{1}", result, reason), "错误", MessageBoxType.Error);
                  
                }
            }
        }

        public void OnGameServerDisconnect(int result, string reason)
        {
            this.DisconnectNotify(result, reason);
            return;
        }

        bool DisconnectNotify(int result, string reason)//返回值是true 和false
                                                        //true：表示存在挂起的消息，并且已经处理了断开连接的通知。
                                                        //false：表示没有挂起的消息，因此没有处理断开连接的通知。
        {
            //Step 1:检查是否有挂起的消息：
            if (this.pendingMessage != null)
            {
                //Step 2:检查挂起的消息是否是登录请求：
                if (this.pendingMessage.Request.userLogin != null)
                {
                    //Step 3: 触发登录失败事件处理程序
                    if (this.OnLogin != null)
                    {
                        this.OnLogin(Result.Failed, string.Format("服务器断开！\n RESULT:{0} ERROR:{1}", result, reason));
                    }
                }
                // Step 4: 检查是否是挂起的注册请求
                else if (this.pendingMessage.Request.userRegister != null)
                {
                    //Step 5: 触发注册失败事件处理程序
                    if (this.OnRegister != null) //在调用委托之前，通常需要检查它是否为 null，以防止运行时错误：
                    {
                        this.OnRegister(Result.Failed, string.Format("服务器断开！\n RESULT:{0} ERROR:{1}", result, reason));
                    }
                }
                else
                {
                    if(this.OnCharacterCreate != null)
                    {
                        this.OnCharacterCreate(Result.Failed, string.Format("服务器断开！\n RESULT:{0} ERROR:{1}", result, reason));
                    }
                }
                return true;// Step 6: 返回 true 表示处理了挂起的消息，跳出if
            }
            return false;// Step 7: 返回 false 表示没有挂起的消息
        }

        public void SendLogin(string user, string psw)
        {
            Debug.LogFormat("UserLoginRequest::user :{0} psw:{1}", user, psw);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.userLogin = new UserLoginRequest();
            message.Request.userLogin.User = user;
            message.Request.userLogin.Passward = psw;

            if (this.connected && NetClient.Instance.Connected)
            {
                this.pendingMessage = null;
                NetClient.Instance.SendMessage(message);
            }
            else
            {
                this.pendingMessage = message;
                this.ConnectToServer();
            }
        }

        //把用户登录成功的消息发送给关注的人，在这里是OnLogin需要关注
        void OnUserLogin(object sender, UserLoginResponse response)//object sender是一个在 C# 事件处理程序中表示触发事件的对象。比如点击确认按钮
        {
            Debug.LogFormat("OnLogin:{0} [{1}]", response.Result, response.Errormsg);

            if(response.Result == Result.Success)
            {
                Models.User.Instance.SetupUserInfo(response.Userinfo);
            }

            if (this.OnLogin != null)
            {
                this.OnLogin(response.Result, response.Errormsg);
            }
        }
        

        public void SendRegister(string user, string psw)
        {
            Debug.LogFormat("UserRegisterRequest::user :{0} psw:{1}", user, psw);
            NetMessage message = new NetMessage();  //通过创建 NetMessageRequest 和 UserRegisterRequest 两个实例，可以构建一个完整的用户注册请求消息，并将其封装在 NetMessage 对象中进行传输。
            message.Request = new NetMessageRequest();
            message.Request.userRegister = new UserRegisterRequest();//有很多请求，需要单独创建用户的注册请求
            message.Request.userRegister.User = user;
            message.Request.userRegister.Passward = psw;

            if (this.connected && NetClient.Instance.Connected)//逻辑层connected和物理层netclient都认为连接已建立时才发送消息
                                                               //逻辑层状态管理可以防止在物理层连接频繁波动时导致的重复操作。
                                                                //例如，避免在网络短暂中断后重复初始化某些资源或重新发送某些消息。
            {
                this.pendingMessage = null;
                NetClient.Instance.SendMessage(message);
            }
            else
            {
                this.pendingMessage = message;
                this.ConnectToServer();
            }
        }

        void OnUserRegister(object sender, UserRegisterResponse response) //当服务器返回用户注册的响应时，这个方法会被调用，并根据响应结果执行相应的逻辑。
        {
            Debug.LogFormat("OnUserRegister:{0} [{1}]", response.Result, response.Errormsg);

            if (this.OnRegister != null)
            {
                this.OnRegister(response.Result, response.Errormsg);
            }
        }

        public void SendCharacterCreate(string name, CharacterClass cls)
        {
            Debug.LogFormat("UserCreateCharacterRequest::name :{0} class:{1}", name, cls);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.createChar = new UserCreateCharacterRequest();
            message.Request.createChar.Name = name;
            message.Request.createChar.Class = cls;

            if (this.connected && NetClient.Instance.Connected)
            {
                this.pendingMessage = null;
                NetClient.Instance.SendMessage(message);
            }
            else
            {
                this.pendingMessage = message;
                this.ConnectToServer();
            }
        }

        void OnUserCreateCharacter(object sender, UserCreateCharacterResponse response)//sender 参数表示触发事件的对象,即服务器是否发送response
        {
            Debug.LogFormat("OnUserCreateCharacter:{0} [{1}]", response.Result, response.Errormsg);

            if(response.Result == Result.Success)
            {
                Models.User.Instance.Info.Player.Characters.Clear();
                Models.User.Instance.Info.Player.Characters.AddRange(response.Characters);
            }
            if(this.OnCharacterCreate != null)//判断这个消息是否有人订阅，如果有人订阅，做一个消息的分发，在UICharacterSelect面板进行订阅
            {
                this.OnCharacterCreate(response.Result, response.Errormsg);
            }
        }
        //接收完消息后要在UserService方法里注册来订阅消息和取消

        public void SendGameEnter(int characterIdx)
        {
            Debug.LogFormat("UserGameEnterRequest::characterId :{0}", characterIdx);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.gameEnter = new UserGameEnterRequest();
            message.Request.gameEnter.characterIdx = characterIdx;
            NetClient.Instance.SendMessage(message);
        }

        void OnGameEnter(object sender, UserGameEnterResponse response)
        {
            Debug.LogFormat("OnGameEnter:{0} [{1}]", response.Result, response.Errormsg);

            if (response.Result == Result.Success)
            {

            }
        }


        public void SendGameLeave()
        {
            Debug.Log("UserGameLeaveRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.gameLeave = new UserGameLeaveRequest();
            NetClient.Instance.SendMessage(message);
        }

        void OnGameLeave(object sender, UserGameLeaveResponse response)
        {
            Debug.LogFormat("OnGameLeave:{0} [{1}]", response.Result, response.Errormsg);
        }

        private void OnCharacterEnter(object sender, MapCharacterEnterResponse message)
        {
            Debug.LogFormat("OnMapCharacterEnter:{0}", message.mapId);
            NCharacterInfo info = message.Characters[0];
            User.Instance.CurrentCharacter = info;
            SceneManager.Instance.LoadScene(DataManager.Instance.Maps[message.mapId].Resource);
        }
    }
}


