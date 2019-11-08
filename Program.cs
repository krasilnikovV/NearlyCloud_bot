using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Text.Json;
using MihaZupan;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using NearlyCloud_bot.Utils;
using File = System.IO.File;

namespace NearlyCloud_bot
{
    class Program
    {
        
        


        public static ITelegramBotClient botClient;

        static void Main(string[] args)
        {
            string settingsString = "";
            try
            {
                using (StreamReader sr = new StreamReader(Path.GetFullPath("Settings.json")))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        settingsString += line;
                    }
                };
                

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            var settings = JsonSerializer.Deserialize<Settings>(settingsString);


            List < IWebProxy > proxyList = new List<IWebProxy>();
            getProxyList();

           
            var proxy = new HttpToSocks5Proxy("104.227.102.171", 9219, settings.ProxyLogin, settings.ProxyPassword);
           
            

            botClient =  new TelegramBotClient(settings.TelegramApiKey, proxy) {Timeout = TimeSpan.FromSeconds(5)};
            
            var me = botClient.GetMeAsync().Result;
            var chatId = me.Id;
            botClient.OnMessage += _handleMessage;
            botClient.StartReceiving();
           
            Console.WriteLine($"{me.Id}    {botClient.GetMeAsync().Status}");
            Console.ReadKey();
            botClient.StopReceiving();
         
            void getProxyList()
            {
                using (WebClient wc = new WebClient())
                {
                    string html = wc.DownloadString("https://www.socks-proxy.net/");
                    Regex regex = new Regex(@"\d+(.)\d+(.)\d+(.)\d+(.)(<\/td><td>)\d+");
                    MatchCollection mc = regex.Matches(html);
                    if (mc.Count > 0)
                    {
                        foreach (Match item in mc)
                        {
                            string[] _ip = item.Value.Split(new string[] {"</td><td>"}, StringSplitOptions.None);
                            proxyList.Add(new WebProxy(_ip[0], int.Parse(_ip[1])));
                        }
                    } 
                }

            }

            
        }

        
        private async static void _handleMessage(
            object sender, MessageEventArgs messageEventArgs)
        {
            //var mess = messageEventArgs.Message.Text;
            var chatId = messageEventArgs.Message.Chat.Id;
            
            ComandMess cmdMess = new ComandMess();
            cmdMess.chatId = chatId;
            cmdMess.botClient = botClient;
            cmdMess.messageEventArgs = messageEventArgs;
            cmdMess.doComand();           
        }

        


    }
}
