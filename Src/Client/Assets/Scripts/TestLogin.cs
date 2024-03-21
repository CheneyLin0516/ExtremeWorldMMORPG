using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLogin : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Network.NetClient.Instance.Init("127.0.0.1", 8000);
        Network.NetClient.Instance.Connect();

        SkillBridge.Message.NetMessage msg = new SkillBridge.Message.NetMessage();//所有的消息发送数据类型是netmessage，然后做一个消息的封装
        msg.Request = new SkillBridge.Message.NetMessageRequest();
        SkillBridge.Message.FirstTestRequest firstTestRequest = new SkillBridge.Message.FirstTestRequest();//FirstTestRequest在消息请求类下面
        firstTestRequest.Helloworld = "Hello World";//helloworld是在FirstTestRequest类下的属性，用实例来调用这个属性（也叫字段）
        msg.Request.firstRequest = firstTestRequest;//Request用于封装所有的请求数据。firstRequest用于封装具体的请求数据。
                                                    //也就是说，msg是NetMessage的一个实例，这个实例在调用NetMessage下的两个属性，封装FirstTestRequest类下的firstTestRequest实例中的具体信息。
                                                    /*msg（即 NetMessage 的实例）可以携带一个具体的请求（firstRequest），而这个请求是通过 NetMessageRequest 这个中间层来封装的。
                                                       这种设计允许 NetMessage 既可以用来发送请求（通过填充 Request 字段），也可以用来接收响应（通过填充 Response 字段），
                                                       从而使得网络通信的处理变得更加灵活和统一。msg.Request.firstRequest 的用法体现了在 Protocol Buffers 和网络通信设计中常见的分层和封装原则，通过具体的层级关系来组织和管理复杂的数据结构。*/

        /*另外一种写法：
         * SkillBridge.Message.NetMessage msg = new SkillBridge.Message.NetMessage();创建主消息
         * msg.Request = new SkillBridge.Message.NetMessageRequest();创建请求消息
         * msg.Request.firstRequest = new SkillBridge.Message.FirstTestRequest();创建我们定义的消息
         * msg.Request.firstRequest.Helloworld = "Hello World";给我们定义的消息填充数据*/

        Network.NetClient.Instance.SendMessage(msg);//这样客户端就可以给服务端去发送消息，服务端GameServer要想接到消息要在Service里新建一个类
     }

// Update is called once per frame
void Update () {

}
}
