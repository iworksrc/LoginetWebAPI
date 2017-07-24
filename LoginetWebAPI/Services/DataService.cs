using System;
using System.Net.Http;
using NLog;
using System.Configuration;

namespace LoginetWebAPI.Services
{
    /// <summary>
    /// Служба забирающая данные с WebAPI http://jsonplaceholder.typicode.com/
    /// </summary>
    public class DataService : IDataService
    {
        private HttpClient httpclient;

        /// <summary>
        /// Базовый адрес внешнего сервиса API.<br/> 
        /// Настраивается через конфигурационный файл <i>Web.config</i>
        /// </summary>
        private readonly string baselink = ConfigurationManager.AppSettings["baselink"];

        /// <summary>
        /// Часть адреса внешнего сервиса API для работы c Users.<br/> 
        /// Настраивается через конфигурационный файл <i>Web.config</i>
        /// </summary>
        private readonly string users = ConfigurationManager.AppSettings["users"];

        /// <summary>
        /// Часть адреса внешнего сервиса API для работы c Albums.<br/>
        /// Настраивается через конфигурационный файл <i>Web.config</i>
        /// </summary>
        private readonly string albums = ConfigurationManager.AppSettings["albums"];

        /// <summary>
        /// Часть адреса внешнего сервиса API для работы c Albums с передачей значения userId.<br/>
        /// Настраивается через конфигурационный файл <i>Web.config</i>
        /// </summary>
        private readonly string albumsByUserId = ConfigurationManager.AppSettings["albumsByUserId"];

        /// <summary>
        /// Логирование NLog
        /// </summary>
        private Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Конструктор класса <see cref="DataService"/> class.
        /// </summary>
        public DataService()
        {
            httpclient = new HttpClient()
            {
                BaseAddress = new Uri(baselink)
            };
            
        }


        /// <summary>
        /// Закрытый метод выполняющий звпрос к стороннему WebAPI согласно переданному параметру
        /// </summary>
        /// <param name="query">строка запроса</param>
        /// <returns>
        /// строка, содержащая массив JSON-объектов 
        /// </returns>
        private string getData(string query)
        {
            try
            {
                var resp = httpclient.GetAsync(query);
                resp.Wait();
                var jsonstring = resp.Result.Content.ReadAsStringAsync();
                jsonstring.Wait();
                return jsonstring.Result;
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error,e,query);
                return null;
            }

        }

        /// <summary>
        /// Получить JSON-объект пользователя по его идентификатору.<br/>
        /// Метод-враппер над <see cref="DataService.getData(string)"/>
        /// </summary>
        /// <param name="id">идентификатор пользователя</param>
        /// <returns>
        /// строка, содержащая JSON-объект
        /// </returns>
        public string getUser(int id)
        {
            return getData(users +"/" + id);
        }

        /// <summary>
        /// Получить массив JSON-объектов всех пользователей.<br/>
        /// Метод-враппер над <see cref="DataService.getData(string)"/>
        /// </summary>
        /// <returns>
        /// строка, содержащая массив JSON-объектов 
        /// </returns>
        public string getAllUsers()
        {
            return getData(users);
        }


        /// <summary>
        /// Получить JSON-объект албома по его идентификатору.<br/>
        /// Метод-враппер над <see cref="DataService.getData(string)"/>
        /// </summary>
        /// <param name="id">идентификатор альбома</param>
        /// <returns>
        /// строка, содержащая JSON-объект
        /// </returns>
        public string getAlbum(int id)
        {
            return getData(albums+ "/" + id);
        }

        /// <summary>
        /// Получить массив JSON-объектов всех альбомов.<br/>
        /// Метод-враппер над <see cref="DataService.getData(string)"/>
        /// </summary>
        /// <returns>
        /// строка, содержащая массив JSON-объектов 
        /// </returns>
        public string getAllAlbums()
        {
            return getData(albums);
        }

        /// <summary>
        /// Получить массив JSON-объектов всех альбомов определённого пользователя по его идентификатору.<br/>
        /// Метод-враппер над <see cref="DataService.getData(string)"/>
        /// </summary>
        /// <param name="id">идентификатор пользователя</param>
        /// <returns>
        /// строка, содержащая массив JSON-объектов 
        /// </returns>
        public string getAlbumsByUserId(int id)
        {
            return getData(albumsByUserId + id);
        }
    }
}