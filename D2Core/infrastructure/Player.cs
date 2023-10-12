using System;
namespace D2Core.core
{
	public class Player
	{
        private string accountName { get; set; }
		private string playerName { get; set; }
		private int _instanceId { get; set; }
		private ushort playerLevel { get; set; }
		private uint seqNum { get; set; }

        public Player(string accountName,string playerName,ushort playerLevel,int instanceId=0,uint seqNum=0)
		{
			this.accountName = accountName;
			this.playerName = playerName;
			this._instanceId = instanceId;
			this.playerLevel = playerLevel;
			this.seqNum = 0;
		
		}
	}
}

