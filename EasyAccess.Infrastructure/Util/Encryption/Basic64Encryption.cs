﻿using System;
using System.Text;

namespace EasyAccess.Infrastructure.Util.Encryption
{
    /// <summary>
    /// 使用Basic64加密和解密密码的工具类
    /// </summary>
    public class Basic64Encryption
    {
        /// <summary>
        /// 使用Base64加密
        /// </summary>
        /// <param name="codeName">加密采用的编码方式</param>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        public static string Encrypt(Encoding encode, string source)
        {
            byte[] bytes = encode.GetBytes(source);
            string code;
            try
            {
                code = Convert.ToBase64String(bytes);
            }
            catch
            {
                code = source;
            }
            return code;
        }

        /// <summary>
        /// Base64加密，采用utf8编码方式加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public static string Encrypt(string source)
        {
            return Encrypt(Encoding.UTF8, source);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="codeName">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string Decrypte(Encoding encode, string result)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encode.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }

        /// <summary>
        /// Base64解密，采用utf8编码方式解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string Decrypte(string result)
        {
            return Decrypte(Encoding.UTF8, result);
        }
    }
}
