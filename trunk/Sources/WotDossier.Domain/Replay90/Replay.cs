using System;

namespace WotDossier.Domain.Replay90
{
    [Serializable]
    public class Replay 
    {
        public FirstBlock Datablock_1;
        public BattleResult Datablock_battle_result;
        public ReplayIdentify Identify;
    }
}