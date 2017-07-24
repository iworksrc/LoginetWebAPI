using LoginetWebAPI.DTO;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System;
using NLog;
using System.Configuration;

namespace LoginetWebAPI.Helpers
{
    /// <summary>
    /// Вспомогательный функционал
    /// </summary>
    public class Helper
    {
        /// <summary>
        /// Шифворальщик/Дешифровальщик 
        /// </summary>
        private Encryptor encryptor = new Encryptor();
        
        /// <summary>
        /// Логирование NLog
        /// </summary>
        private Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// Шифрует все поля <b>email</b> пользователей в переданной коллекции 
        /// </summary>
        /// <param name="users">Коллекция пользователей</param>
        /// <returns>
        /// Коллекцию пользователей с зашифрованными полями <b>email</b>
        /// </returns>
        public IEnumerable<User> EncryptUsersEmails(IEnumerable<User> users)
        {
            foreach(User u in users)
            {
                EncryptUsersEmails(u);
            }
            return users;
        }

        /// <summary>
        /// Шифрует поле <b>email</b> переданного пользователя
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>
        /// Пользователя с зашифрованным полем <b>email</b>
        /// </returns>
        internal User EncryptUsersEmails(User user)
        {
            user.email = encryptor.Encrypt(user.email);
            return user;
        }

    }
}