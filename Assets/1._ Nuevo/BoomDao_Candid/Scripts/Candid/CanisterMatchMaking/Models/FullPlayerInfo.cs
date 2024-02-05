using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using UserId = EdjCase.ICP.Candid.Models.Principal;

namespace CanisterPK.CanisterMatchMaking.Models
{
	public class FullPlayerInfo
	{
		[CandidName("elo")]
		public UnboundedUInt Elo { get; set; }

		[CandidName("id")]
		public UserId Id { get; set; }

		[CandidName("matchAccepted")]
		public bool MatchAccepted { get; set; }

		[CandidName("playerGameData")]
		public string PlayerGameData { get; set; }

		[CandidName("playerName")]
		public string PlayerName { get; set; }

		public FullPlayerInfo(UnboundedUInt elo, UserId id, bool matchAccepted, string playerGameData, string playerName)
		{
			this.Elo = elo;
			this.Id = id;
			this.MatchAccepted = matchAccepted;
			this.PlayerGameData = playerGameData;
			this.PlayerName = playerName;
		}

		public FullPlayerInfo()
		{
		}
	}
}