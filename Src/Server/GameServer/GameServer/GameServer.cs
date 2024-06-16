﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Configuration;

using System.Threading;

using Network;
using GameServer.Services;
using GameServer.Managers;

namespace GameServer
{
    class GameServer
    {
        Thread thread;
        bool running = false;
        NetService network;
        public bool Init()
        {
            network = new NetService();
            network.Init(8000);
            //HelloWorldService.Instance.Init();//初始化
            DBService.Instance.Init();
            UserService.Instance.Init();
            //var a = DBService.Instance.Enities.Characters.Where(s => s.TID ==1);
            //Console.WriteLine("{0}",a.FirstOrDefault<TCharacter>().Name);
            DataManager.Instance.Load();
            MapManager.Instance.Init();
            thread = new Thread(new ThreadStart(this.Update));
            return true;
        }

        public void Start()
        {
            //HelloWorldService.Instance.Start();//服务器来启动新加的协议，启动过后消息开始订阅
            network.Start();
            running = true;
            thread.Start();
        }


        public void Stop()
        {
            running = false;
            thread.Join();
            network.Stop();
        }

        public void Update()
        {
            while (running)
            {
                Time.Tick();
                Thread.Sleep(100);
                //Console.WriteLine("{0} {1} {2} {3} {4}", Time.deltaTime, Time.frameCount, Time.ticks, Time.time, Time.realtimeSinceStartup);
            }
        }
    }
}
