using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillBridge.Message;
using GameServer.Entities;

namespace GameServer.Managers
{
    class CharacterManager : Singleton<CharacterManager>
    {
        public Dictionary<int, Character> Characters = new Dictionary<int, Character>();//用字典，不用列表，不需要做遍历

        public CharacterManager()
        {

        }

        public void Dispose()
        {

        }

        public void Init()
        {

        }
        //管理器常用方法，clear，add，remove
        public void Clear()
        {
            this.Characters.Clear();
        }

        public Character AddCharacter(TCharacter cha)
        {
            Character character = new Character(CharacterType.Player, cha);
            this.Characters[cha.ID] = character;
            return character;
        }

        public void RemoveCharacter(int characterId)
        {
            this.Characters.Remove(characterId);
        }
    }
}
