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
    class UserService : Singleton<UserService>, IDisposable
    {

<<<<<<< HEAD

        //创建UI层的事件，给UI使用
        public UnityEngine.Events.UnityAction<Result, string> OnRegister; //这是一个委托，如果没有任何方法附加到 OnRegister，它的值就是 null。
        public UnityEngine.Events.UnityAction<Result, string> OnLogin; //允许将OnLogin方法附加到事件并在事件触发时调用这些方法
        public UnityEngine.Events.UnityAction<Result, string> OnCharacterCreate;
=======
        public UnityEngine.Events.UnityAction<Result, string> OnRegister;
>>>>>>> parent of c840d88 (Version1.2.5)

        NetMessage pendingMessage = null;

        bool connected = false;


        //Service层来处理对UI层事件的订阅
        //userservice负责处理所有和用户相关的网络通讯
        public UserService()
        {
            NetClient.Instance.OnConnect += OnGameServerConnect;
            NetClient.Instance.OnDisconnect += OnGameServerDisconnect;

            MessageDistributer.Instance.Subscribe<UserRegisterResponse>(this.OnUserRegister);
<<<<<<< HEAD
            MessageDistributer.Instance.Subscribe<UserCreateCharacterResponse>(this.OnUserCreateCharacter);
            MessageDistributer.Instance.Subscribe<UserGameEnterResponse>(this.OnGameEnter);
           // MessageDistributer.Instance.Subscribe<UserGameLeaveResponse>(this.OnGameLeave);
            MessageDistributer.Instance.Subscribe<MapCharacterEnterResponse>(this.OnCharacterEnter);

=======
>>>>>>> parent of c840d88 (Version1.2.5)


        }

        public void Dispose()
        {

            MessageDistributer.Instance.Unsubscribe<UserRegisterResponse>(this.OnUserRegister);
<<<<<<< HEAD
            MessageDistributer.Instance.Unsubscribe<UserCreateCharacterResponse>(this.OnUserCreateCharacter);
            MessageDistributer.Instance.Subscribe<UserGameEnterResponse>(this.OnGameEnter);
         //   MessageDistributer.Instance.Subscribe<UserGameLeaveResponse>(this.OnGameLeave);
=======
>>>>>>> parent of c840d88 (Version1.2.5)
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
                if (!this.DisconnectNotify(result, reason))
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

        bool DisconnectNotify(int result, string reason)
        {
            if (this.pendingMessage != null)
            {
                if (this.pendingMessage.Request.userRegister != null)
                {
                    if (this.OnRegister != null)
                    {
                        this.OnRegister(Result.Failed, string.Format("服务器断开！\n RESULT:{0} ERROR:{1}", result, reason));
                    }
                }
                return true;
            }
            return false;
        }



        public void SendRegister(string user, string psw)
        {
            Debug.LogFormat("UserRegisterRequest::user :{0} psw:{1}", user, psw);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.userRegister = new UserRegisterRequest();
            message.Request.userRegister.User = user;
            message.Request.userRegister.Passward = psw;

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

        void OnUserRegister(object sender, UserRegisterResponse response)
        {
            Debug.LogFormat("OnUserRegister:{0} [{1}]", response.Result, response.Errormsg);

            if (this.OnRegister != null)
            {
                this.OnRegister(response.Result, response.Errormsg);

            }
        }
<<<<<<< HEAD

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

        void OnUserCreateCharacter(object sender, UserCreateCharacterResponse response)
        {
            Debug.LogFormat("OnUserCreateCharacter:{0} [{1}]", response.Result, response.Errormsg);

            if(response.Result == Result.Success)
            {
                Models.User.Instance.Info.Player.Characters.Clear();
                Models.User.Instance.Info.Player.Characters.AddRange(response.Characters);
            }
            if(this.OnCharacterCreate != null)
            {
                this.OnCharacterCreate(response.Result, response.Errormsg);
            }
        }


        public void SendGameEnter(int characterIdx)//发送进入游戏的请求，需要知道当前选择的哪个角色，所以发送角色索引idx
                                                    //因为登录的时候，服务器会有一个角色列表填充给客户端，所以客户端的角色顺序和服务器是一致的
                                                    //所以只用一个id，服务器就能根据一个正确的对应关系找个该角色
        {
            Debug.LogFormat("UserGameEnterRequest::characterId:{0}", characterIdx);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.gameEnter = new UserGameEnterRequest();
            message.Request.gameEnter.characterIdx = characterIdx;
            NetClient.Instance.SendMessage(message);
        }

        public void OnGameEnter(object sender, UserGameEnterResponse response)
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

        void OnGameLeave(object sender, UserGameEnterResponse response)
        {
            //MapService.Instance.CurrentMapId = 0;
            Debug.LogFormat("OnGameLeave: {0} [{1}]", response.Result, response.Errormsg);
        }

        private void OnCharacterEnter(object sender, MapCharacterEnterResponse message)
        {
            Debug.LogFormat("OnMapCharacterEnter: {0}", message.mapId);
            NCharacterInfo info = message.Characters[0];
            User.Instance.CurrentCharacter = info;
            SceneManager.Instance.LoadScene(DataManager.Instance.Maps[message.mapId].Resource);
        }
=======
>>>>>>> parent of c840d88 (Version1.2.5)
    }
}


