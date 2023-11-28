using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using System.Threading.Tasks;
using CanisterPK.CanisterMatchMaking;
using EdjCase.ICP.Agent.Responses;

namespace CanisterPK.CanisterMatchMaking
{
	public class CanisterMatchMakingApiClient
	{
		public IAgent Agent { get; }

		public Principal CanisterId { get; }

		public CandidConverter? Converter { get; }

		public CanisterMatchMakingApiClient(IAgent agent, Principal canisterId, CandidConverter? converter = default)
		{
			this.Agent = agent;
			this.CanisterId = canisterId;
			this.Converter = converter;
		}

		public async Task<(bool Arg0, string Arg1)> AcceptMatch(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "acceptMatch", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<bool> AddMatchPlayerData(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "addMatchPlayerData", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<(bool Arg0, UnboundedUInt Arg1)> AddPlayerSearching()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "addPlayerSearching", arg);
			return reply.ToObjects<bool, UnboundedUInt>(this.Converter);
		}

		public async Task<(bool Arg0, string Arg1)> AssignPlayer2(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "assignPlayer2", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool Arg0, string Arg1)> CancelMatchmaking()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "cancelMatchmaking", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<OptionalValue<Models.MatchData>> GetMatchData(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMatchData", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.MatchData>>(this.Converter);
		}

		public async Task<(Models.SearchStatus Arg0, UnboundedUInt Arg1, string Arg2)> GetMatchSearching(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getMatchSearching", arg);
			return reply.ToObjects<Models.SearchStatus, UnboundedUInt, string>(this.Converter);
		}

		public async Task<(OptionalValue<Models.MatchData> Arg0, UnboundedUInt Arg1)> GetMyMatchData()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMyMatchData", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.MatchData>, UnboundedUInt>(this.Converter);
		}

		public async Task<(UnboundedUInt Arg0, string Arg1)> IsGameAccepted()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "isGameAccepted", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<UnboundedUInt, string>(this.Converter);
		}

		public async Task<(bool Arg0, string Arg1)> IsGameMatched()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "isGameMatched", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool Arg0, string Arg1)> RejectMatch()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "rejectMatch", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool Arg0, string Arg1)> SetGameOver()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "setGameOver", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<Models.CanisterWsCloseResult> WsClose(Models.CanisterWsCloseArguments arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ws_close", arg);
			return reply.ToObjects<Models.CanisterWsCloseResult>(this.Converter);
		}

		public async Task<Models.CanisterWsGetMessagesResult> WsGetMessages(Models.CanisterWsGetMessagesArguments arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ws_get_messages", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.CanisterWsGetMessagesResult>(this.Converter);
		}

		public async Task<Models.CanisterWsMessageResult> WsMessage(Models.CanisterWsMessageArguments arg0, OptionalValue<ReservedValue> arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ws_message", arg);
			return reply.ToObjects<Models.CanisterWsMessageResult>(this.Converter);
		}

		public async Task<Models.CanisterWsOpenResult> WsOpen(Models.CanisterWsOpenArguments arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ws_open", arg);
			return reply.ToObjects<Models.CanisterWsOpenResult>(this.Converter);
		}
	}
}