using System;
using System.IO;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using System.Net;
using Newtonsoft.Json.Linq;

public class ChatBot
{
	//Adding client - our Twitchclient and credintials - our authorization data
	private TwitchClient client;
	private ConnectionCredentials credentials;

	//create client and intialize connection
	public ChatBot(string botName, string token, string channelName)
	{
		client = new TwitchClient();
		credentials = new ConnectionCredentials(botName, token);

		client.Initialize(credentials, channelName);

		client.OnLog += Client_OnLog;
		client.OnJoinedChannel += Client_OnJoinedChannel;
        client.OnChatCommandReceived += Client_OnChatCommandReceived;
        
		client.Connect();
	}
    //Comand for chat
    private async void Client_OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
    {
        switch (e.Command.CommandText.ToLower())
        {
            case "rank":
                //create get request and launch stream to read response
                WebRequest reqGET = WebRequest.Create(@"there must be APEX API");
                WebResponse resp = reqGET.GetResponse();
                Stream stream = resp.GetResponseStream();
                StreamReader sr = new StreamReader(stream);
                string s = sr.ReadToEnd();
                //make json from strig
                JObject parsed = JObject.Parse(s);
                //parse for needs
                string parsedName = (string)parsed["global"]["name"];
                string parsedRankScore = (string)parsed["global"]["rank"]["rankScore"];
                string parsedRankName = (string)parsed["global"]["rank"]["rankName"];
                string parsedRankDiv = (string)parsed["global"]["rank"]["rankDiv"];
                client.SendMessage(e.Command.ChatMessage.Channel, $"Мой ник: {parsedName} \n Мой ранк: {parsedRankName} {parsedRankDiv} - {parsedRankScore} \n || Да я рак _(");
                break;
        }

    }
    //On boy joined to chat
    private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
    {
		client.SendMessage(e.Channel, "Hi everyone bot is in chat now");
    }

    //Log of bot
    private void Client_OnLog(object sender, OnLogArgs e)
    {
		Console.WriteLine(e.Data);
    }

}
