using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace NearlyCloud_bot
{
    class ComandMess
    {
        public ITelegramBotClient botClient;
        public MessageEventArgs messageEventArgs;
        public long chatId;
        private CategoryMenu categoryMenu = new CategoryMenu();


        public async void doComand()
        {
                   
            string typeMess;
            ReplyKeyboardMarkup menuLabiary = new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new KeyboardButton("Вывод бибилотек пользователя"),

                    },
                    new[]
                    {
                        new KeyboardButton("Создание новой библиотеки"),

                    },
                    new[]
                    {
                        new KeyboardButton("Поиск библиоотеки"),

                    },
                    new[]
                    {
                        new KeyboardButton("Временная библиотека"),

                    },
                    new[]
                    {
                        new KeyboardButton("Главное меню"),

                    },
                }
            };
            if (messageEventArgs.Message.Text == null)
            {
                if (messageEventArgs.Message.Document == null)
                {
                    typeMess = "Other";
                    await botClient.SendTextMessageAsync(chatId, "Bad type, pls do again");
                }
                else
                {
                    var mess = messageEventArgs.Message.Document;
                    typeMess = "Document";
                    categoryMenu.inDocument = true;
                   
                    await botClient.SendTextMessageAsync(chatId, "В какую библиотеку добавить документ?",
                        replyMarkup: menuLabiary);
                    ComandWithLabiary();
                }
            }
            else
            {

                var mess = messageEventArgs.Message.Text;
                if (mess.StartsWith("https://"))
                {
                    categoryMenu.inLink = true;
                    typeMess = "Link";
                    

                        
                    await botClient.SendTextMessageAsync(chatId, "В какую библиотеку добавить ссылку?",
                        replyMarkup: menuLabiary);
                }
                else
                {
                    typeMess = "Text";
                    ComandWithLabiary();
                    //there will be search
                }
               
                

            }
            await botClient.SendTextMessageAsync(chatId, $"is {typeMess}").ConfigureAwait(false);
        }

        public async void ComandInHeadMenu()
        {
            string mess = messageEventArgs.Message.Text;

            var headMenu = new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new KeyboardButton("Редактировать"),                        
                    },
                    new[]
                    {
                        new KeyboardButton("Добавить")
                    },
                    new[]
                    {
                    new KeyboardButton("Выйти")
                    }
                }
            };

            if (mess == "Редактировать")
            {
                //Редактировать библиотеку

            }

            if (mess == "Добавить")
            {
                //Добавить биюлиотеку
            }

            if (mess == "Выйти")
            {
                categoryMenu.inLink = false;
                categoryMenu.inDocument = false;
            }
            await botClient.SendTextMessageAsync(chatId, "Heloo ", replyMarkup: headMenu);
        }

        public async void ComandWithLabiary()
        {
            string mess = messageEventArgs.Message.Text;
            

            if (mess == "Вывод библиотек")
            {
                //Логика вывода библиотек
            }

            if (mess == "Создание новой библиотеки")
            {

            }

            if (mess == "Поиск библиоотеки")
            {

            }

            if (mess == "Временная библиотека")
            {

            }
            if (mess == "Главное меню")
            {
                ComandInHeadMenu();
            }
        }

        
    }
}
