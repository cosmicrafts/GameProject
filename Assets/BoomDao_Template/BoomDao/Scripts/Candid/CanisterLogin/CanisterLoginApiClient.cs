using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using System.Threading.Tasks;
using CanisterPK.CanisterLogin;
using EdjCase.ICP.Agent.Responses;
using EdjCase.ICP.Candid.Mapping;

namespace CanisterPK.CanisterLogin
{
	public class CanisterLoginApiClient
	{
		public IAgent Agent { get; }

		public Principal CanisterId { get; }

		public CandidConverter? Converter { get; }

		public CanisterLoginApiClient(IAgent agent, Principal canisterId, CandidConverter? converter = default)
		{
			this.Agent = agent;
			this.CanisterId = canisterId;
			this.Converter = converter;
		}

		public async Task<(bool Arg0, string Arg1)> CreatePlayer(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "createPlayer", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<CanisterLoginApiClient.GetICPBalanceArg0> GetICPBalance()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getICPBalance", arg);
			return reply.ToObjects<CanisterLoginApiClient.GetICPBalanceArg0>(this.Converter);
		}

		public async Task<OptionalValue<Models.Player>> GetMyPlayerData()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMyPlayerData", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.Player>>(this.Converter);
		}

		public async Task<OptionalValue<Models.Player>> GetPlayer()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getPlayer", arg);
			return reply.ToObjects<OptionalValue<Models.Player>>(this.Converter);
		}

		public async Task<OptionalValue<Models.Player>> GetPlayerData(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getPlayerData", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.Player>>(this.Converter);
		}

		public async Task<OptionalValue<Models.PlayerPreferences>> GetPlayerPreferences()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getPlayerPreferences", arg);
			return reply.ToObjects<OptionalValue<Models.PlayerPreferences>>(this.Converter);
		}

		public async Task<(bool Arg0, string Arg1)> SavePlayerChar(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "savePlayerChar", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool Arg0, string Arg1)> SavePlayerLanguage(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "savePlayerLanguage", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<bool> SavePlayerName(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "savePlayerName", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public class GetICPBalanceArg0
		{
			[CandidName("e8s")]
			public ulong E8s { get; set; }

			public GetICPBalanceArg0(ulong e8s)
			{
				this.E8s = e8s;
			}

			public GetICPBalanceArg0()
			{
			}
		}
	}
}