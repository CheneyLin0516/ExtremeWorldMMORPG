using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Models;

namespace GameServer.Managers
{
    class MapManager: Singleton<MapManager>
    {
        Dictionary<int, Map> Maps = new Dictionary<int, Map>();

        public void Init()
        {
            foreach (var mapdefine in DataManager.Instance.Maps.Values)
            {
                Map map = new Map(mapdefine);
                Log.InfoFormat("MapManager.Init > Map:{0}:{1}", map.Define.ID, map.Define.Name);
                this.Maps[mapdefine.ID] = map;
            }
        }

        public Map this[int key]
        {
            get
            {
                return this.Maps[key];
            }
        }

        public void Update()//其他大部分manager是请求响应的，不带有自主服务，所以不存在update。意思就是地图管理器除了别人请求它做事，自己还要做事
        {
            foreach(var map in this.Maps.Values)
            {
                map.Update();
            }
        }
    }

}
