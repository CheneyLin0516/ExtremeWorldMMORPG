using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Network;
using SkillBridge.Message;

namespace GameServer.Services
{
    //遵循我们设计的架构，所有的网络请求都由service来负责
    class HelloWorldService: Singleton<HelloWorldService> //每写一个service都要包含下面三种方法，这个Service还要写成单例
    {
        public void Init()//初始化
        {

        }
        //新写了一条协议，客户端写了发送，服务器端来写接收，接收中写了一条日志，日志里把hello world的内容输出出来
        public void Start()//用来订阅消息，说明服务器要处理什么消息
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FirstTestRequest>(this.OnFirstTestRequest);//（告诉网络底层我要用这个OnFirstTestRequest方法处理这个FirstTestRequest协议）
        }

        void OnFirstTestRequest(NetConnection<NetSession> sender,FirstTestRequest request)//只用来打日志Log的消息处理方法，协议处理器
        {
            Log.InfoFormat("FirstTestRequest: Helloworld:{0}", request.Helloworld);
        }

        //写完要在GameSever里面启动这个Service

        public void Stop()
        {

        }

    }
}
