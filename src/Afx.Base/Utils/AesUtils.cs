using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Afx.Utils
{
    /// <summary>
    /// AES 加密、解密
    /// </summary>
    public static class AesUtils
    {
        /// <summary>
        /// 加密、解密默认 key
        /// </summary>
        private const string DefaultKey = "cj9@i8+&";
        /// <summary>
        /// 加密、解密默认 CipherMode
        /// </summary>
        public const CipherMode DefaultMode = CipherMode.CBC;
        /// <summary>
        /// 加密、解密默认CipherMode
        /// </summary>
        public const PaddingMode DefaultPadding = PaddingMode.PKCS7;

        /// <summary>
        /// 生成 ASCII字符的 aes key
        /// </summary>
        /// <param name="len"></param>
        /// <returns>ASCII字符</returns>
        public static string CreateKey(int len = 24)
        {
            return StringUtils.GetRandomString(len);
        }

        /// <summary>
        /// 生成 ASCII字符的 aes iv
        /// </summary>
        /// <param name="len"></param>
        /// <returns>ASCII字符</returns>
        public static string CreateIV(int len = 8)
        {
            return StringUtils.GetRandomString(len);
        }

        #region bytes
        /// <summary>
        /// 加密 byte[]
        /// </summary>
        /// <param name="input">byte[]</param>
        /// <returns>加密成功返回byte[]</returns>
        public static byte[] Encrypt(byte[] input)
        {
            return Encrypt(input, DefaultKey);
        }

        /// <summary>
        /// 加密 byte[]
        /// </summary>
        /// <param name="input">byte[]</param>
        /// <param name="key">ASCII字符 key</param>
        /// <returns>加密成功返回byte[]</returns>
        public static byte[] Encrypt(byte[] input, string key)
        {
            return Encrypt(input, key, null, DefaultMode, DefaultPadding);
        }

        /// <summary>
        /// 加密 byte[]
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key">ASCII字符 key</param>
        /// <param name="iv">ASCII字符 iv</param>
        /// <returns>加密成功返回byte[]</returns>
        public static byte[] Encrypt(byte[] input, string key, string iv)
        {
            return Encrypt(input, key, iv, DefaultMode, DefaultPadding);
        }

        /// <summary>
        /// 加密 byte[]
        /// </summary>
        /// <param name="input">byte[]</param>
        /// <param name="key">ASCII字符 key</param>
        /// <param name="mode">指定用于加密的块密码模式</param>
        /// <param name="padding">指定在消息数据块比加密操作所需的全部字节数短时应用的填充类型</param>
        /// <returns>加密成功返回byte[]</returns>
        public static byte[] Encrypt(byte[] input, string key, CipherMode mode, PaddingMode padding)
        {
            return Encrypt(input, key, null, mode, padding);
        }

        /// <summary>
        /// 加密 byte[]
        /// </summary>
        /// <param name="input">byte[]</param>
        /// <param name="key">ASCII字符 key</param>
        /// <param name="iv">ASCII字符 iv</param>
        /// <param name="mode">指定用于加密的块密码模式</param>
        /// <param name="padding">指定在消息数据块比加密操作所需的全部字节数短时应用的填充类型</param>
        /// <returns>加密成功返回byte[]</returns>
        public static byte[] Encrypt(byte[] input, string key, string iv, CipherMode mode, PaddingMode padding)
        {
            byte[] output = null;
            if (input != null && input.Length > 0)
            {
                if (string.IsNullOrEmpty(key)) throw new ArgumentNullException("key");
                byte[] keyBytes = Encoding.ASCII.GetBytes(key);
                byte[] ivBytes = null;
                if (!string.IsNullOrEmpty(iv))
                {
                    ivBytes = Encoding.ASCII.GetBytes(iv);
                }
                else
                {
                    var tiv = CreateIV(8);
                    ivBytes = Encoding.ASCII.GetBytes(tiv);
                }

                output = Encrypt(input, keyBytes, ivBytes, mode, padding);

                if (string.IsNullOrEmpty(iv))
                {
                    var buffer = new byte[output.Length + ivBytes.Length];
                    Array.Copy(output, 0, buffer, 0, output.Length);
                    Array.Copy(ivBytes, 0, buffer, output.Length, ivBytes.Length);
                    output = buffer;
                }
            }
            else if (input != null && input.Length == 0)
            {
                output = new byte[0];
            }

            return output;
        }

        /// <summary>
        /// 加密 byte[]
        /// </summary>
        /// <param name="input">byte[]</param>
        /// <param name="key">key</param>
        /// <param name="iv"> iv</param>
        /// <param name="mode">指定用于加密的块密码模式</param>
        /// <param name="padding">指定在消息数据块比加密操作所需的全部字节数短时应用的填充类型</param>
        /// <returns>加密成功返回byte[]</returns>
        public static byte[] Encrypt(byte[] input, byte[] key, byte[] iv, CipherMode mode, PaddingMode padding)
        {
            byte[] output = null;
            if (input != null && input.Length > 0)
            {
                if (key == null) throw new ArgumentNullException("key");
                if (key.Length < 8 || key.Length % 8 != 0) throw new ArgumentException("key.Length is error!", "key");
                if (iv.Length < 8 || iv.Length % 8 != 0) throw new ArgumentException("iv.Length is error!", "iv");

                using (var aes = Aes.Create())
                {
                    aes.Mode = mode;
                    aes.Padding = padding;
                    aes.Key = key;
                    aes.IV = iv;

                    using (ICryptoTransform cryptoTransform = aes.CreateEncryptor())
                    {
                        output = cryptoTransform.TransformFinalBlock(input, 0, input.Length);
                    }
                }
            }
            else if (input != null && input.Length == 0)
            {
                output = new byte[0];
            }

            return output;
        }

        /// <summary>
        /// 解密 byte[]
        /// </summary>
        /// <param name="input">byte[]</param>
        /// <returns>解密成功返回byte[]</returns>
        public static byte[] Decrypt(byte[] input)
        {
            return Decrypt(input, DefaultKey);
        }

        /// <summary>
        /// 解密 byte[]
        /// </summary>
        /// <param name="input">byte[]</param>
        /// <param name="key">ASCII字符 key</param>
        /// <returns>解密成功返回byte[]</returns>
        public static byte[] Decrypt(byte[] input, string key)
        {
            return Decrypt(input, key, null, DefaultMode, DefaultPadding);
        }

        /// <summary>
        /// 解密 byte[]
        /// </summary>
        /// <param name="input">byte[]</param>
        /// <param name="key">ASCII字符 key</param>
        /// <returns>解密成功返回byte[]</returns>
        public static byte[] Decrypt(byte[] input, string key, string iv)
        {
            return Decrypt(input, key, iv, DefaultMode, DefaultPadding);
        }

        /// <summary>
        /// 解密 byte[]
        /// </summary>
        /// <param name="input">byte[]</param>
        /// <param name="key">ASCII字符 key</param>
        /// <param name="mode">指定用于解密的块密码模式</param>
        /// <param name="padding">指定在消息数据块比解密操作所需的全部字节数短时应用的填充类型</param>
        /// <returns>解密成功返回byte[]</returns>
        public static byte[] Decrypt(byte[] input, string key, CipherMode mode, PaddingMode padding)
        {
            return Decrypt(input, key, null, mode, padding);
        }

        /// <summary>
        /// 解密 byte[]
        /// </summary>
        /// <param name="input">byte[]</param>
        /// <param name="key">ASCII字符 key</param>
        /// <param name="iv">ASCII字符 iv</param>
        /// <param name="mode">指定用于解密的块密码模式</param>
        /// <param name="padding">指定在消息数据块比解密操作所需的全部字节数短时应用的填充类型</param>
        /// <returns>解密成功返回byte[]</returns>
        public static byte[] Decrypt(byte[] input, string key, string iv, CipherMode mode, PaddingMode padding)
        {
            byte[] output = null;
            if (input != null && input.Length > 0)
            {
                if (string.IsNullOrEmpty(key)) throw new ArgumentNullException("key");
                byte[] keyBytes = Encoding.ASCII.GetBytes(key);
                var input_len = input.Length;
                byte[] ivBytes = null;
                if (!string.IsNullOrEmpty(iv))
                {
                    ivBytes = Encoding.ASCII.GetBytes(iv);
                }
                else
                {
                    if (input != null && input.Length > 0 && input.Length <= 8)
                        throw new ArgumentException("input.Length is error!", "input");

                    ivBytes = new byte[8];
                    Array.Copy(input, input.Length - ivBytes.Length, ivBytes, 0, ivBytes.Length);
                    input_len = input.Length - ivBytes.Length;
                }

                output = Decrypt(input, keyBytes, ivBytes, mode, padding, input_len);
            }
            else if (input != null && input.Length == 0)
            {
                output = new byte[0];
            }

            return output;
        }

        /// <summary>
        /// 解密 byte[]
        /// </summary>
        /// <param name="input">byte[]</param>
        /// <param name="key">key</param>
        /// <param name="iv">iv</param>
        /// <param name="mode">指定用于解密的块密码模式</param>
        /// <param name="padding">指定在消息数据块比解密操作所需的全部字节数短时应用的填充类型</param>
        /// <param name="input_len">需要解密的 input 长度， -1.表示所有</param>
        /// <returns>解密成功返回byte[]</returns>
        public static byte[] Decrypt(byte[] input, byte[] key, byte[] iv, CipherMode mode, PaddingMode padding, int input_len = -1)
        {
            byte[] output = null;
            if (input != null && input.Length > 0)
            {
                if (key == null || key.Length < 8 || key.Length % 8 != 0) throw new ArgumentException("key");
                if (iv == null || iv.Length < 8 || iv.Length % 8 != 0) throw new ArgumentException("iv");

                var len = input.Length;
                if (0 < input_len && input_len < input.Length) len = input_len;

                using (var aes = Aes.Create())
                {
                    aes.Mode = mode;
                    aes.Padding = padding;
                    aes.Key = key;
                    aes.IV = iv;

                    using (ICryptoTransform cryptoTransform = aes.CreateDecryptor())
                    {
                        output = cryptoTransform.TransformFinalBlock(input, 0, len);
                    }
                }
            }
            else if (input != null && input.Length == 0)
            {
                output = new byte[0];
            }

            return output;
        }
        #endregion

        #region string
        /// <summary>
        /// 加密 string
        /// </summary>
        /// <param name="input">string</param>
        /// <param name="resultType"></param>
        /// <returns>加密成功返回string</returns>
        public static string Encrypt(string input, StringByteType resultType = StringByteType.Hex)
        {
            return Encrypt(input, DefaultKey, resultType);
        }

        /// <summary>
        /// 加密 string
        /// </summary>
        /// <param name="input">string</param>
        /// <param name="key">ASCII字符 key</param>
        /// <param name="resultType"></param>
        /// <returns>加密成功返回string</returns>
        public static string Encrypt(string input, string key, StringByteType resultType = StringByteType.Hex)
        {
            return Encrypt(input, key, null, DefaultMode, DefaultPadding, resultType);
        }

        /// <summary>
        /// 加密 string
        /// </summary>
        /// <param name="input">string</param>
        /// <param name="key">ASCII字符 key</param>
        /// <param name="iv">ASCII字符 iv</param>
        /// <param name="resultType"></param>
        /// <returns>加密成功返回string</returns>
        public static string Encrypt(string input, string key, string iv, StringByteType resultType = StringByteType.Hex)
        {
            return Encrypt(input, key, iv, DefaultMode, DefaultPadding, resultType);
        }

        /// <summary>
        /// 加密 string
        /// </summary>
        /// <param name="input">string</param>
        /// <param name="key">ASCII字符 key</param>
        /// <param name="mode">指定用于加密的块密码模式</param>
        /// <param name="padding">指定在消息数据块比加密操作所需的全部字节数短时应用的填充类型</param>
        /// <param name="resultType"></param>
        /// <returns>加密成功返回string</returns>
        public static string Encrypt(string input, string key, CipherMode mode, PaddingMode padding, StringByteType resultType = StringByteType.Hex)
        {
            return Encrypt(input, key, null, mode, padding, resultType);
        }

        /// <summary>
        /// 加密 string
        /// </summary>
        /// <param name="input">string</param>
        /// <param name="key">ASCII字符 key</param>
        /// <param name="iv">ASCII字符 iv</param>
        /// <param name="mode">指定用于加密的块密码模式</param>
        /// <param name="padding">指定在消息数据块比加密操作所需的全部字节数短时应用的填充类型</param>
        /// <param name="resultType"></param>
        /// <returns>加密成功返回string</returns>
        public static string Encrypt(string input, string key, string iv, CipherMode mode, PaddingMode padding, StringByteType resultType = StringByteType.Hex)
        {
            string output = null;
            if (!string.IsNullOrEmpty(input))
            {
                byte[] inputdata = Encoding.UTF8.GetBytes(input);
                byte[] outputdata = Encrypt(inputdata, key, iv, mode, padding);
                output = resultType == StringByteType.Hex ? StringUtils.ByteToHexString(outputdata)
                    : Convert.ToBase64String(outputdata);
            }
            else if(input == string.Empty)
            {
                output = string.Empty;
            }

            return output;
        }


        /// <summary>
        /// 解密 string
        /// </summary>
        /// <param name="input">string</param>
        /// <param name="inputType"></param>
        /// <returns>解密成功返回string</returns>
        public static string Decrypt(string input, StringByteType inputType = StringByteType.Hex)
        {
            return Decrypt(input, DefaultKey, inputType);
        }

        /// <summary>
        /// 解密 string
        /// </summary>
        /// <param name="input">string</param>
        /// <param name="key">ASCII字符 key</param>
        /// <param name="inputType"></param>
        /// <returns>解密成功返回string</returns>
        public static string Decrypt(string input, string key, StringByteType inputType = StringByteType.Hex)
        {
            return Decrypt(input, key, null, DefaultMode, DefaultPadding, inputType);
        }

        /// <summary>
        /// 解密 string
        /// </summary>
        /// <param name="input">string</param>
        /// <param name="key">ASCII字符 key</param>
        /// <param name="iv">ASCII字符 iv</param>
        /// <param name="inputType"></param>
        /// <returns>解密成功返回string</returns>
        public static string Decrypt(string input, string key, string iv, StringByteType inputType = StringByteType.Hex)
        {
            return Decrypt(input, key, iv, DefaultMode, DefaultPadding, inputType);
        }

        /// <summary>
        /// 解密 string
        /// </summary>
        /// <param name="input">string</param>
        /// <param name="key">ASCII字符 key</param>
        /// <param name="mode">指定用于解密的块密码模式</param>
        /// <param name="padding">指定在消息数据块比解密操作所需的全部字节数短时应用的填充类型</param>
        /// <param name="inputType"></param>
        /// <returns>解密成功返回string</returns>
        public static string Decrypt(string input, string key, CipherMode mode, PaddingMode padding, StringByteType inputType = StringByteType.Hex)
        {
            return Decrypt(input, key, null, mode, padding, inputType);
        }

        /// <summary>
        /// 解密 string
        /// </summary>
        /// <param name="input">string</param>
        /// <param name="key">ASCII字符 key</param>
        /// <param name="iv">ASCII字符 iv</param>
        /// <param name="mode">指定用于解密的块密码模式</param>
        /// <param name="padding">指定在消息数据块比解密操作所需的全部字节数短时应用的填充类型</param>
        /// <param name="inputType"></param>
        /// <returns>解密成功返回string</returns>
        public static string Decrypt(string input, string key, string iv, CipherMode mode, PaddingMode padding, StringByteType inputType = StringByteType.Hex)
        {
            string output = null;
            if (!string.IsNullOrEmpty(input))
            {
                if (input.Length % 2 != 0) throw new ArgumentException("input is error!", "input");
                byte[] inputdata = inputType == StringByteType.Hex ? StringUtils.HexStringToByte(input)
                    : Convert.FromBase64String(input);
                byte[] outputdata = Decrypt(inputdata, key, iv, mode, padding);
                if (outputdata != null && outputdata.Length > 0)
                {
                    output = Encoding.UTF8.GetString(outputdata);
                }
            }
            else if(input == string.Empty)
            {
                output = string.Empty;
            }

            return output;
        }
        #endregion

    }
}
