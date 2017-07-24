using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace LoginetWebAPI.Helpers
{
    /// <summary>
    /// Шифворальщик/Дешифровальщик по стандарту <b>Advanced Encryption Standard (AES)</b>
    /// </summary>
    public class Encryptor
    {
        /// <summary>
        /// Байты ключа
        /// </summary>
        private byte[] Key = new byte[32];

        /// <summary>
        /// Байты вектора
        /// </summary>
        private byte[] Vector = new byte[16];

        /// <summary>
        /// Хранит признак того, что сгенерированные для шифрования <b>Ключ</b> и <b>Вектор</b> сохранены в файл
        /// </summary>
        private bool isKeysSaved;

        /// <summary>
        /// Полный путь и имя файла для сохранения <b>Ключа</b> и <b>Вектора</b>
        /// </summary>
        private readonly string keyfile = ConfigurationManager.AppSettings["keyfile"];
        
        /// <summary>
        /// Логирование NLog
        /// </summary>
        private Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="Encryptor"/> класса.<br/>
        /// При создании экземпляра класса будет произведена попытка прочитать <b>ключ</b> и <b>вектор</b> из файла.<br/>
        /// В случае провала будут сгенерированы новые <b>ключ</b> и <b>вектор</b> и будет произведена попытка записать их в файл,<br/>
        /// В случае провала сохранения ключей в файл шифрование/дешифрование производиться не будет,<br/>
        /// соответствующие методы будут возвращать <i>null</i>
        /// </summary>
        public Encryptor()
        {
            if (obtainKeysFromFile())
            {
                logger.Log(LogLevel.Info, "keys obtained from file as well");

                isKeysSaved = true;
            }
            else
            {
                logger.Log(LogLevel.Info, "create keys");

                using (RijndaelManaged Rijndael = new RijndaelManaged())
                {
                    Rijndael.GenerateKey();
                    Rijndael.GenerateIV();
                    if (saveKeysToFile(Rijndael.Key, Rijndael.IV))
                    {
                        Key = Rijndael.Key;
                        Vector = Rijndael.IV;
                        logger.Log(LogLevel.Info, "keys saved as well");
                        isKeysSaved = true;
                    }
                    else
                    {
                        logger.Log(LogLevel.Error, "Decryption not possible, emails will be omitted");
                        isKeysSaved = false;
                    }
                }
            }
        }

        /// <summary>
        /// Чтение <b>Ключа</b> и <b>Вектора</b> из файла
        /// </summary>
        /// <returns>
        /// В случае удачного чтения из файла - <i><b>true</b></i>, иначе <i><b>false</b></i>
        /// </returns>
        private bool obtainKeysFromFile()
        {
            try
            {
                using (FileStream fs = File.OpenRead(keyfile))
                {
                    fs.Read(Key, 0, Key.Length);
                    fs.Read(Vector, 0, Vector.Length);
                }
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Info, e, "AES keys are NOT obtained from file");
                return false;
            }

            return true;
        }


        /// <summary>
        /// Запись <b>Ключа</b> и <b>Вектора</b> в файл
        /// </summary>
        /// <param name="key">Байты ключа</param>
        /// <param name="vector">Байты вектора</param>
        /// <returns>
        /// В случае удачной записи в файл - <i><b>true</b></i>, иначе <i><b>false</b></i>
        /// </returns>
        private bool saveKeysToFile(byte[] key, byte[] vector)
        {
            try
            {
                using (FileStream fs = new FileStream(keyfile, FileMode.OpenOrCreate))
                {
                    fs.Write(key, 0, key.Length);
                    fs.Write(vector, 0, vector.Length);
                }
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, e, "AES Keys are NOT saved");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Шифрует переданную строку
        /// </summary>
        /// <param name="plaintext">открытый текст</param>
        /// <returns>
        /// В случае успешно прочитанных/сгенерированных/сохранённых <b>Ключа</b> и <b>Вектора</b> - зашифрованный текст, иначе <i>null</i>
        /// </returns>
        public string Encrypt(string plaintext)
        {

            if (!isKeysSaved) return null;

            byte[] encrypted;
            using (RijndaelManaged Rijndael = new RijndaelManaged())
            {

                ICryptoTransform encryptor = Rijndael.CreateEncryptor(Key, Vector);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plaintext);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(encrypted);
        }

        /// <summary>
        /// Дешифрует переданную строку
        /// </summary>
        /// <param name="chiped">зашифрованный текст</param>
        /// <returns>
        /// В случае успешно прочитанных/сгенерированных/сохранённых <b>Ключа</b> и <b>Вектора</b> - открытый текст, иначе <i>null</i>
        /// </returns>
        public string Decrypt(string chiped)
        {
            if (!isKeysSaved) return null;
            string plaintext = null;
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(Key, Vector);

                using (MemoryStream msDecrypt = new MemoryStream( Convert.FromBase64String(chiped)) )
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }
            return plaintext;
        }
    }
}