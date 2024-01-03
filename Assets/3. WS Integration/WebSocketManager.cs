
using System;
using System.Threading;
using EdjCase.ICP.Agent;
using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.WebSockets;
using UnityEngine;

using EdjCase.ICP.Agent;
using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Agent.Identities;
using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.WebSockets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Debug = UnityEngine.Debug;


public class AppMessage
{
	[CandidName("text")]
	public string Text { get; set; }

	public AppMessage(string text)
	{
		this.Text = text;
	}
	
}

public class WebSocketManager : MonoBehaviour
{
	private Principal devCanisterId = Principal.FromText("j4n55-giaaa-aaaap-qb3wq-cai");
	private Uri devGatewayUri = new Uri("ws://localhost:8080");
	private Uri devBoundryNodeUri = new Uri("http://localhost:4943");

	private Principal prodCanisterId = Principal.FromText("j4n55-giaaa-aaaap-qb3wq-cai");
	private Uri prodGatewayUri = new Uri("wss://icwebsocketgateway.app.runonflux.io");
	//gatewayUri = new Uri("wss://gateway.icws.io");
	
	private IWebSocketAgent<AppMessage>? websocket;
	private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
	private Stopwatch stopwatch = Stopwatch.StartNew();
	
	async void Start()
	{
		bool development = true;
		Principal canisterId;
		Uri gatewayUri;
		
		if (development)
		{
			canisterId = devCanisterId;
			gatewayUri = devGatewayUri;
		}
		else
		{
			canisterId = prodCanisterId;
			gatewayUri = prodGatewayUri;
		}
		var builder = new WebSocketBuilder<AppMessage>(canisterId, gatewayUri)
			.OnMessage(this.OnMessage)
			.OnOpen(this.OnOpen)
			.OnError(this.OnError)
			.OnClose(this.OnClose);
		if (development)
		{
			// Set the root key as the dev network key
			SubjectPublicKeyInfo devRootKey = await new HttpAgent(
				httpBoundryNodeUrl: devBoundryNodeUri
			).GetRootKeyAsync();
			builder = builder.WithRootKey(devRootKey);
		}
		this.websocket = await builder.BuildAndConnectAsync(cancellationToken: cancellationTokenSource.Token);
		await this.websocket.ReceiveAllAsync(cancellationTokenSource.Token);
	}

	void OnOpen()
	{
		Debug.Log("Opened: " + this.stopwatch.Elapsed);
	}
	void OnMessage(AppMessage message)
	{
		Debug.Log("Received message: " + message.Text);
		ICTimestamp.Now().NanoSeconds.TryToUInt64(out ulong now);
		Debug.Log("Sending message:  pong" + this.stopwatch.Elapsed);
		this.websocket!.SendAsync(new AppMessage("pong"));
	}
	void OnError(Exception ex)
	{
		Debug.Log("Error: " + ex.ToString());
	}
	void OnClose()
	{
		Debug.Log("Closed" + this.stopwatch.Elapsed);
	}

	void OnDestroy()
	{
		cancellationTokenSource.Cancel(); // Cancel any ongoing operations
		websocket?.DisposeAsync();
	}
}