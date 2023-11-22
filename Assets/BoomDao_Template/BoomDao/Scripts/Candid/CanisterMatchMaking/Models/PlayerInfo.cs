using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using UserId = EdjCase.ICP.Candid.Models.Principal;

namespace CanisterPK.CanisterMatchMaking.Models
{
	public class PlayerInfo
	{
		[CandidName("characterSelected")]
		public UnboundedUInt CharacterSelected { get; set; }

		[CandidName("deckSavedKeyIds")]
		public List<string> DeckSavedKeyIds { get; set; }

		[CandidName("elo")]
		public UnboundedUInt Elo { get; set; }

		[CandidName("id")]
		public UserId Id { get; set; }

		[CandidName("matchAccepted")]
		public bool MatchAccepted { get; set; }

		public PlayerInfo(UnboundedUInt characterSelected, List<string> deckSavedKeyIds, UnboundedUInt elo, UserId id, bool matchAccepted)
		{
			this.CharacterSelected = characterSelected;
			this.DeckSavedKeyIds = deckSavedKeyIds;
			this.Elo = elo;
			this.Id = id;
			this.MatchAccepted = matchAccepted;
		}

		public PlayerInfo()
		{
		}
	}
}