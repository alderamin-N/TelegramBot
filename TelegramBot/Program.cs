using Dapper;
using Npgsql;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ConsoleApp1
{
    public class Program
    {       
        private static string _connectionString  = $"Server=localhost;Port=5432;UserName=postgres;Password={Environment.GetEnvironmentVariable("PG_Telegram_password")};Database=myTelegramBot;";
        private static InlineKeyboardMarkup _workInlineKeyboard = CreateWorkInlineKeyboard();
        private static InlineKeyboardMarkup _entertainmentInlineKeyboard = CreateEntertainmentInlineKeyboard();
        private static InlineKeyboardMarkup _lifeInlineKeyboard = CreateLifeInlineKeyboard();

        static void Main(string[] args)
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            var client = new TelegramBotClient("6213510833:AAEQBLhOupM8_N6KOkEX3dUU9jSysztH4h0");
            client.StartReceiving(Update, Error); //начинаю слушать из бота
            Console.ReadLine();           
        }

        private static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            Console.WriteLine(arg2.Message);
            return Task.CompletedTask;
        }

        private static async Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            var message = update.Message; //то что приходит из бота
            var replyKeyboard = CreateReplyKeyboard();          
            replyKeyboard.ResizeKeyboard = true;   

            if (message != null)
            {
                AddUser(message.Chat.Id, message.From.FirstName);
                switch (message.Text)
                {
                    case "/start":
                    LogAction("Начало работы", message.Chat.Id);
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                            $"Привет,{message.From.FirstName}, как хочешь провести эти выходные?",
                            replyMarkup: replyKeyboard);
                        break;
                    case "Хочу поработать!":
                        LogAction("Хочу поработать", message.Chat.Id);
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                            "Отлично! У меня есть для тебя несколько мест, где можно продуктивно поработать:",
                            replyMarkup: _workInlineKeyboard);
                        break;
                    case "Хочу отдохнуть!":
                        LogAction("Хочу отдохнуть", message.Chat.Id);
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                            "Отлично! У меня есть для тебя несколько идей, как можно провести время весело и с пользой!",
                            replyMarkup: _entertainmentInlineKeyboard);
                        break;
                    case "Хочу остаться дома!":
                        LogAction("Хочу остаться дома", message.Chat.Id);
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                            "Для тебя есть хорошая новость: можно и дома хорошо провести время! Что ты выберишь?",
                            replyMarkup: _lifeInlineKeyboard);
                        break;
                }   
            }

            if (update.Type == UpdateType.CallbackQuery)
            {
                await CallbackHandler(botClient, update);
            }
        }

        private static async Task CallbackHandler(ITelegramBotClient botClient, Update update)
        {
            var chatId = update.CallbackQuery!.Message.Chat.Id;
            switch (update.CallbackQuery!.Data)
            {
                case "WatchFilm":
                    LogAction("Просмотр дома фильма", chatId);
                    await botClient.SendTextMessageAsync(chatId,
                    "https://www.kinopoisk.ru/chance/?ysclid=lnlzuan2t120174603");
                    break;               
               case "PlayGame":
                    LogAction("Игра в настольную игру", chatId);
                    await botClient.SendTextMessageAsync(chatId,
                    "Хорошая идея! Есть несколько игр на примете:\r\n" +
                    "\r\n"+
                    "Игры для одного:\r\n" +
                    "1.\"Катамино (Katamino)\"\r\n" +
                    "2.\"Деревни\"\r\n" +
                    "3.\"Город мечты\"\r\n" +
                    "4.\"Место преступления\"\r\n" +
                     "\r\n" +
                    "Игры для двоих:\r\n" +
                    "1.\"Кортекс\"\r\n" +
                    "2.\"Цитадели\"\r\n" +
                    "3.\"Эволюция\"\r\n" +
                    "4.\"Каркассон\"\r\n" +  
                    "\r\n" +
                    "Игры для большой компании:\r\n" +
                    "1.\"Цивилизация\"\r\n" +
                    "2.\"Монополия\"\r\n" +
                    "3.\"Мафия\"\r\n" +
                    "4.\"Catan: Колонизаторы\"\r\n" +
                    "\r\n" +
                    "Где купить https://hobbygames.ru/ \r\n"                    
                    );
                    break;                   
                case "WorkInLibrary":
                    LogAction("Поработать в библиотеке им. Н.В. Гоголя", chatId);
                    await botClient.SendVenueAsync(chatId,
                   latitude: 59.948068f, longitude: 30.412942f, title: "\"Центральная районная библиотека им. Н.В. Гоголя\"", address: "​Среднеохтинский проспект,8");                   
                    break;
                case "WorkInCentre":
                    LogAction("Поработать в Kazanskaya-Page", chatId);
                    await botClient.SendVenueAsync(chatId,
                   latitude: 59.933847f, longitude: 30.322682f, title: "\"Kazanskaya-Page\"", address: "​Казанская, 3а");
                    break;
                case "WorkInCentre2":
                    LogAction("Поработать в Case", chatId);
                    await botClient.SendVenueAsync(chatId,
                    latitude: 59.928023f, longitude: 30.346763f, title: "\"Case\"", address: "​Владимирский проспект, 23");
                    break;
                case "WorkInSouth":
                    LogAction("Поработать в Ugol-Page", chatId);
                    await botClient.SendVenueAsync(chatId,
                    latitude: 59.889276f, longitude: 30.277889f, title: "\"Ugol-Page\"", address: "Улица Маршала Говорова, 35а");
                    break;
                case "WorkOnPetrogradskaya":
                    LogAction("Поработать в Ясная поляна", chatId);
                    await botClient.SendVenueAsync(chatId,
                    latitude: 59.965598f, longitude: 30.313264f, title: "\"Ясная поляна\"", address: "​Улица Льва Толстого, 1/3");
                    break;
                case "WorkInNotrth":
                    LogAction("Поработать в Smart", chatId);
                    await botClient.SendVenueAsync(chatId,
                   latitude: 60.008221f, longitude: 30.29668f, title: "\"Smart\"", address: "Коломяжский проспект, 19");
                    break;
                case "Theatre":
                    LogAction("Пойти в театр", chatId);
                    await botClient.SendTextMessageAsync(chatId,
                       text: "Большая сцена театра имени ЛенСовета предлагает в эти выходные:");
                    await botClient.SendTextMessageAsync(chatId,
                      text: "\"МАКБЕТ.КИНО.\"\r\nПо пьесе Шекспира\r\n" +
                      "На большой сцене\r\n"+
                      "Дата: 21.10.2023 в 18-00\r\n" +
                      "Продолжительность спектакля: 5 часов\r\nСпектакль идет с тремя антрактами\r\n" +
                      "Успей купить билеты тут: https://lensov-theatre.spb.ru/buy/bileter/#performance=19299598"
                      );
                    await botClient.SendMediaGroupAsync(chatId,
                    media: new IAlbumInputMedia[]
                     {new InputMediaPhoto(
                    InputFile.FromUri("https://lensov-theatre.spb.ru/images/watermark/watermark2.php?image=877_12151.jpg"))
                    });

                    await botClient.SendTextMessageAsync(chatId,
                    text: "\"ТАРТЮФ\"\r\n" +
                    "На большой сцене\r\n" +
                    "Дата: 22.10.2023 в 18-00\r\n"+
                    "Продолжительность спектакля: 2 часа 40 минут\r\nСпектакль идет с одним антрактом\r\n"+
                    "Купить билеты можно тут: https://lensov-theatre.spb.ru/buy/bileter/#performance=19299599"
                       );
                    await botClient.SendMediaGroupAsync(chatId,
                    media: new IAlbumInputMedia[]
                     {new InputMediaPhoto(
                    InputFile.FromUri("https://lensov-theatre.spb.ru/images/watermark/watermark2.php?image=3744_10692.jpg"))
                    });
                    break;
                case "Ballet":
                    LogAction("Пойти на балет", chatId);
                    await botClient.SendTextMessageAsync(chatId,
                     text: "Балет прекрасен в это время года. Есть превосходные постановки!\r\n"+ "\"Мариинский теарт предлагает на эти выходные:\r\n");
                   await botClient.SendTextMessageAsync(chatId,
                      text:
                      "балет Бориса Асафьева \"Бахчисарайский фонтан\"\r\n" +
                      "Дата: 21.10.2023 в 19-00\r\n" +
                      "О балете:\r\n" +
                      "ИСПОЛНИТЕЛИ\r\nДирижер – Борис Грузин\r\nМария – Елена Евсеева\r\nВацлав – Алексей Тимофеев\r\nГирей – Юрий Смекалов\r\nЗарема – Мария Буланова\r\nНурали – Григорий Попов\r\n" +
                      "Продолжительность спектакля 2 часа 45 минут\r\nСпектакль идет с двумя антрактами\r\n" +
                      "Купить билеты можно тут:https://tickets.mariinsky.ru/ru/performance/bG5BbWhiWWxHZjhUWXd5QUkvNFF4QT09/?_gl=1*1j9gjvk*_ga*MTIzNTIxNDE3MC4xNjk3MTA4MDAy*_ga_BW69QBE1S3*MTY5NzExMTQyNy4yLjAuMTY5NzExMTQyNy4wLjAuMA.. \r\n"
                      );
                    await botClient.SendMediaGroupAsync(chatId,
                   media: new IAlbumInputMedia[]
                    {new InputMediaPhoto(
                    InputFile.FromUri("https://www.mariinsky.ru/images/cms/data/ballet_repertoire/bahfontan/evseeva_bah_fontan_by_razina.jpg"))
                   });                    
                    await botClient.SendTextMessageAsync(chatId,
                       text: 
                       "балет Хермана Левенскьольда \"Сильфида\"\r\n" +
                       "Дата: 22.10.2023 в 19-00\r\n" +
                       "О балете:\r\n" +
                       "ИСПОЛНИТЕЛИ\r\nДирижер – Борис Грузин\r\nСильфида – Мария Ширинкина\r\nДжеймс – Максим Изместьев\r\nЭффи – Валерия Беспалова\r\nМедж – Дмитрий Пыхачов\r\n" +
                       "Продолжительность спектакля 1 час 40 минут\r\nСпектакль идет с одним антрактом\r\n" +                      
                       "Купить билеты можно тут:https://tickets.mariinsky.ru/ru/performance/VFFFWjVzbVVRanp0cHZDQlRDc0tIQT09/?_gl=1*j9yxum*_ga*MTIzNTIxNDE3MC4xNjk3MTA4MDAy*_ga_BW69QBE1S3*MTY5NzEwODAwMS4xLjEuMTY5NzEwODA2OS4wLjAuMA.. \r\n"
                    );
                    await botClient.SendMediaGroupAsync(chatId,
                   media: new IAlbumInputMedia[]
                    {new InputMediaPhoto(
                    InputFile.FromUri("https://www.mariinsky.ru/images/cms/data/ballet_repertoire/sylphide/shirinkina_la_sylphide_by_baranovsky.png"))
                   });
                    break;
            }
        }

        private static ReplyKeyboardMarkup CreateReplyKeyboard()
        {
            return new ReplyKeyboardMarkup(new[]
            {
                new[]
                {
                    new KeyboardButton("Хочу поработать!"),                
                },
                new[]
                {
                    new KeyboardButton("Хочу отдохнуть!"),  
                },
                new[]
                {
                    new KeyboardButton("Хочу остаться дома!")
                }
            });
        }

        private static InlineKeyboardMarkup CreateWorkInlineKeyboard()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                   InlineKeyboardButton.WithCallbackData(text: "\"Центральная районная библиотека им. Н.В. Гоголя\" адрес: ​Среднеохтинский проспект,8", callbackData: "WorkInLibrary")                   
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "\"Kazanskaya-Page\" ​адрес: Казанская, 3а", callbackData: "WorkInCentre"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "\"Case\" ​адрес: Владимирский проспект, 23", callbackData: "WorkInCentre2"),
                },
                 new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "\"Ugol-Page\" ​адрес: Улица Маршала Говорова, 35а", callbackData: "WorkInSouth"),
                },
                 new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "\"Ясная поляна\" адрес: ​Улица Льва Толстого, 1/3", callbackData: "WorkOnPetrogradskaya"),
                },
                 new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "\"Smart\" ​адрес: Коломяжский проспект, 19", callbackData: "WorkInNotrth"),
                }
            });
        }
        
        private static InlineKeyboardMarkup CreateEntertainmentInlineKeyboard()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Пойти в театр", callbackData: "Theatre"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Пойти на балет", callbackData: "Ballet"),
                }
            });
        }

        private static InlineKeyboardMarkup CreateLifeInlineKeyboard()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Посмотреть фильм", callbackData: "WatchFilm"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Поиграть в настольную игру", callbackData: "PlayGame"),
                }
            });
        }

        private static void LogAction(string actionName, long chatid)
        {          
            using var connection = new NpgsqlConnection(_connectionString);
            string query = $"select * from public.user where chat_id = {chatid}";
            TelegramBot.User user = connection.QueryFirstOrDefault<TelegramBot.User>(query);           
            string req1 = $"insert into public.action (user_id, name, date_time) values ({user.Id},'{actionName}','{DateTime.Now}')";
            connection.Query(req1);
        }
        private static void AddUser(long chatid, string nameUser)
        {
           using var connection = new NpgsqlConnection(_connectionString);
            string query = $"select id from public.user where chat_id = {chatid}";
            int? id1 = connection.QueryFirstOrDefault<int?>(query);
            if (id1 == null)
            {
                string req2 = $"insert into public.user ( name, chat_id) values ('{nameUser}',{chatid})";
                connection.Query(req2);
            }
        }
    }
}
