using System;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Reflection.Metadata;
using System.Text.Json;
using MihaZupan;
using Telegram.Bot;
using Telegram.Bot.Args;
using NearlyCloud_bot.Utils;
using Telegram.Bot.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using MySql.Data.MySqlClient;
using MySQLApp;
using File = System.IO.File;
using NearlyCloud_bot;


namespace NearlyCloud_bot
{
    class Program
    {
        public static ITelegramBotClient botClient;

        static void Main(string[] args)
        {

            
            using (ApplicationContext db = new ApplicationContext())
            {
               
                TgUser tgUser1 = new TgUser();
                tgUser1.userId = 3212142;
                tgUser1.username = "dvsdvds";
                tgUser1.firstname = "frstnm1";
                tgUser1.lastname = "lstname1";
                TgUser tgUser2 = new TgUser { userId = 111111, username = "username2", firstname = "firstname2", lastname = "lastname2" };
                db.Add(tgUser1);
                db.Add(tgUser2);
                db.SaveChanges();
                
                Console.WriteLine("Объекты успешно сохранены");

                var users = db.Users;
                
                Console.WriteLine("Список:");
                foreach (var user in users)
                {
                    
                    Console.WriteLine("--------");
                    Console.WriteLine(user.id);
                    Console.WriteLine(user.userId);
                    Console.WriteLine(user.username);
                    Console.WriteLine(user.firstname);
                    Console.WriteLine(user.lastname);
                    Console.WriteLine("--------");
                    
                }
            }




          
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            
            

            var settings = JsonSerializer.Deserialize<Settings>(settingsString);


            //List<IWebProxy> proxyList = new List<IWebProxy>();
            //getProxyList();

            
            
            
                                

            var proxy = new HttpToSocks5Proxy("104.227.102.171", 9219, settings.ProxyLogin, settings.ProxyPassword);
            botClient = new TelegramBotClient(settings.TelegramApiKey, proxy) {Timeout = TimeSpan.FromSeconds(5)};

            var me = botClient.GetMeAsync().Result;
            var chatId = me.Id;
            botClient.OnMessage += _handleMessage;
           
            botClient.StartReceiving();

            Console.WriteLine($"{me.Id}    {botClient.GetMeAsync().Status}");

            

            Console.ReadKey();


            botClient.StopReceiving();

            //void getProxyList()
            //{
            //    using (WebClient wc = new WebClient())
            //    {
            //        string html = wc.DownloadString("https://www.socks-proxy.net/");
            //        Regex regex = new Regex(@"\d+(.)\d+(.)\d+(.)\d+(.)(<\/td><td>)\d+");
            //        MatchCollection mc = regex.Matches(html);
            //        if (mc.Count > 0)
            //        {
            //            foreach (Match item in mc)
            //            {
            //                string[] _ip = item.Value.Split(new string[] {"</td><td>"}, StringSplitOptions.None);
            //                proxyList.Add(new WebProxy(_ip[0], int.Parse(_ip[1])));
            //            }
            //        }
            //    }

            //}
        }

        private async static void _handleMessage(
            object sender, MessageEventArgs messageEventArgs)
        {
            //var mess = messageEventArgs.Message.Text;
            var chatId = messageEventArgs.Message.Chat.Id;

            TgUser user = new TgUser();
            user.userId = messageEventArgs.Message.Chat.Id;
            user.username = messageEventArgs.Message.Chat.Username;
            user.firstname = messageEventArgs.Message.Chat.FirstName;
            user.lastname = messageEventArgs.Message.Chat.LastName;

            Console.WriteLine("------------");
            Console.WriteLine($"id:  {user.userId} \nusername:  {user.username} \nuserFirstName:  {user.firstname}\nuserLastName:  {user.lastname}");
            Console.WriteLine("------------");
            ComandMess cmdMess = new ComandMess();
            cmdMess.chatId = chatId;
            cmdMess.botClient = botClient;
            cmdMess.messageEventArgs = messageEventArgs;
            cmdMess.doComand();
        }
    }
}
namespace MySQLApp
{
    public class ApplicationContext : DbContext
    {
        public DbSet<TgUser> Users { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {          
            optionsBuilder.UseMySql("server=localhost;user=root;database=telegramuserlib;password=vik22232;");
        }
    }
}


