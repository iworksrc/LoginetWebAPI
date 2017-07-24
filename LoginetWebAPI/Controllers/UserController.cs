using LoginetWebAPI.DTO;
using LoginetWebAPI.Helpers;
using LoginetWebAPI.Services;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Ninject;

namespace LoginetWebAPI.Controllers
{
    /// <summary>
    /// Контроллер для работы с сущностями User
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController"/>
    public class UserController : ApiController
    {
        /// <summary>
        /// Служба получения данных 
        /// </summary>
        private IDataService ds;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="UserController"/> класса.
        /// </summary>
        /// <param name="ds">Любой класс реализующий интерфейс <i>IDataService</i>.<br/>Внедрение зависимости через Ninject Framework. http://www.ninject.org/  </param>
        public UserController(IDataService ds)
        {
            this.ds = ds;
        }

        /// <summary>
        /// Экземпляр класса-помошника осуществляющего шифрование полей email пользователей перед возвратом результата
        /// </summary>
        private Helper helper = new Helper();

        /// <summary>
        /// Логирование NLog
        /// </summary>
        private Logger logger = LogManager.GetCurrentClassLogger();
        
        /// <summary>
        /// Получить всех пользователей
        /// </summary>
        /// <returns>
        /// Возвращает массив объектов "пользователь" <i>(IEnumerable&lt;User&gt;)</i>, <br/>
        /// результат (<b>xml</b>, <b>json</b>) зависит от параметров заголовка <b>Accept</b> в запросе клиента (application/xhtml+xml ,application/json).<br/>
        /// Пример зпроса: GET: (Адрес сервера)api/User
        /// </returns>
        public IEnumerable<User> Get()
        {
            try
            {
                //не забыть сделать шифрование email юзеров
                //logger.Log(LogLevel.Info,"GET: api/user");

                IEnumerable<User> users =  JsonConvert.DeserializeObject<IEnumerable<User>>(ds.getAllUsers());

                return helper.EncryptUsersEmails(users.LongCount() > 0 ? users : null);
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, e, "GET: api/user");
                return null;
            }

        }

        
        /// <summary>
        /// Получить пользователя по его идентификатору
        /// </summary>
        /// <param name="id">идентификатор пользователя</param>
        /// <returns>
        /// Возвращает объекто "пользователь" <i>(User)</i>, <br/>
        /// результат (<b>xml</b>, <b>json</b>) зависит от параметров заголовка <b>Accept</b> в запросе клиента (application/xhtml+xml ,application/json).<br/>
        /// Пример зпроса: GET: (Адрес сервера)api/User/5
        /// </returns>
        public User Get(int id)
        {
            try
            {
                User user = JsonConvert.DeserializeObject<User>(ds.getUser(id));

                return helper.EncryptUsersEmails(user.id != 0 ? user : null);
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, e, "GET: api/user/{id}");
                return null;
            }
        }

        
    }
}
