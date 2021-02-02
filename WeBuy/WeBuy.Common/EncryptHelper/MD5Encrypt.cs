using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace WeBuy.Common.MD5
{
    /// <summary>
    /// 不可逆加密
    /// 1 防止被篡改
    /// 2 防止明文存储
    /// 3 防止抵赖，数字签名
    /// </summary>
    public class MD5Encrypt
    {
        #region MD5
        /// <summary>
        /// MD5加密,和动网上的16/32位MD5加密结果相同,
        /// 使用的UTF8编码
        /// </summary>
        /// <param name="strSource">待加密的字符串</param>
        /// <param name="length">16或32值之一,其它则采用.net默认MD5加密算法</param>
        /// <returns>加密后的字串</returns>
        public static string Encrypt(string source, int length = 32)//默认参数
        {
            HashAlgorithm provider = CryptoConfig.CreateFromName("MD5") as HashAlgorithm;
            if (string.IsNullOrEmpty(source)) return string.Empty;

            byte[] bytes = Encoding.UTF8.GetBytes(source);// Encoding.ASCII.GetBytes(source);
            byte[] hashValue = provider.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();
            switch (length)
            {
                case 16://16位密文是32位密文的9到24位字符
                    for (int i = 4; i < 12; i++)
                        sb.Append(hashValue[i].ToString("x2"));
                    break;
                case 32:
                    for (int i = 0; i < 16; i++)
                    {
                        sb.Append(hashValue[i].ToString("x2"));
                    }
                    break;
                default:
                    for (int i = 0; i < hashValue.Length; i++)
                    {
                        sb.Append(hashValue[i].ToString("x2"));
                    }
                    break;
            }
            return sb.ToString();
        }
        /// <summary>
        /// MD5加密,和动网上的16/32位MD5加密结果相同,
        /// 使用的UTF8编码
        /// 为文件加密方法
        /// </summary>
        /// <param name="strSource">待加密的文件路径</param>
        /// <param name="length">16或32值之一,其它则采用.net默认MD5加密算法</param>
        /// <returns>加密后的字串</returns>
        public static string EncryptFile(string pathFile, int length = 32)//默认参数
        {
            using (FileStream fsRead = new FileStream(pathFile, FileMode.Open))
            {
                int fsLen = (int)fsRead.Length;
                byte[] heByte = new byte[fsLen];
                int r = fsRead.Read(heByte, 0, heByte.Length);
                string myStr = System.Text.Encoding.UTF8.GetString(heByte);
                return Encrypt(myStr, length);
            }
        }
        #endregion MD5
    }
}
