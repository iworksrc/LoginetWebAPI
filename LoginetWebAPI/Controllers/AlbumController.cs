using LoginetWebAPI.DTO;
using LoginetWebAPI.Helpers;
using LoginetWebAPI.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using NLog;

namespace LoginetWebAPI.Controllers
{
    /// <summary>
    /// Контроллер для работы с сущностями Album
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class AlbumController : ApiController
    {
        /// <summary>
        /// Служба получения данных 
        /// </summary>
        private IDataService ds;


        /// <summary>
        /// Инициализирует новый экземпляр <see cref="AlbumController"/> класса.
        /// </summary>
        /// <param name="ds">Любой класс реализующий интерфейс <i>IDataService</i>.<br/>Внедрение зависимости через Ninject Framework. http://www.ninject.org/ </param>
        public AlbumController(IDataService ds)
        {
            this.ds = ds;
        }

        /// <summary>
        /// Логирование NLog
        /// </summary>
        private Logger logger = LogManager.GetCurrentClassLogger();

        
        /// <summary>
        /// Получить список всех альбомов
        /// </summary>
        /// <returns>
        /// Возвращает массив объектов "альбом" <i>(IEnumerable&lt;Album&gt;)</i>, <br/>
        /// результат (<b>xml</b>, <b>json</b>) зависит от параметров заголовка <b>Accept</b> в запросе клиента (application/xhtml+xml ,application/json).<br/>
        /// Пример зпроса: GET: (Адрес сервера)api/Album
        /// </returns>
        public IEnumerable<Album> Get()
        {
            try
            {
                IEnumerable<Album> albums = JsonConvert.DeserializeObject<IEnumerable<Album>>(ds.getAllAlbums());

                return albums.LongCount() > 0 ? albums : null;

            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, e, "GET: api/album");
                return null;
            }
        }

        
        /// <summary>
        /// Получить альбом по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор альбома</param>
        /// <returns>
        /// Возвращает объект "альбом" <i>(Album)</i>, <br/>
        /// результат (<b>xml</b>, <b>json</b>) зависит от параметров заголовка <b>Accept</b> в запросе клиента (application/xhtml+xml ,application/json).<br/>
        /// Пример зпроса: GET: (Адрес сервера)api/Album/5
        /// </returns>
        public Album Get(int id)
        {
            try
            {
                Album album = JsonConvert.DeserializeObject<Album>(ds.getAlbum(id));

                return album.id != 0 ? album : null;

            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, e, "GET: api/album/{id}");
                return null;
            }
        }

        /// <summary>
        /// Получить все альбомы пользователя по идентификатору пользователя
        /// </summary>
        /// <param name="userId">идентификатор пользователя</param>
        /// <returns>
        /// Возвращает массив объектов "альбом" определённого пользователя <i>(IEnumerable&lt;Album&gt;)</i>, <br/>
        /// результат (<b>xml</b>, <b>json</b>) зависит от параметров заголовка <b>Accept</b> в запросе клиента (application/xhtml+xml ,application/json).<br/>
        /// Пример зпроса: GET: (Адрес сервера)api/Album?userId=5
        /// </returns>
        public IEnumerable<Album> Get([FromUri] string userId)
        {
            try
            {
                IEnumerable<Album> albums = JsonConvert.DeserializeObject<IEnumerable<Album>>(ds.getAlbumsByUserId(Int32.Parse(userId)));
                return albums.LongCount() > 0 ? albums : null;
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, e, "GET: api/album?userId={id}");
                return null;
            }
        }

    }
}
