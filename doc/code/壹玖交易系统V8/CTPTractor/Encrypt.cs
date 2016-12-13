using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CTPTractor
{
	public class Encrypt
	{
		internal string ToEncrypt(string encryptKey, string str)
		{
			string result;
			try
			{
				byte[] bytes = System.Text.Encoding.Unicode.GetBytes(encryptKey);
				byte[] bytes2 = System.Text.Encoding.Unicode.GetBytes(str);
				System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
				System.Security.Cryptography.CryptoStream cryptoStream = new System.Security.Cryptography.CryptoStream(memoryStream, new System.Security.Cryptography.DESCryptoServiceProvider().CreateEncryptor(bytes, bytes), System.Security.Cryptography.CryptoStreamMode.Write);
				cryptoStream.Write(bytes2, 0, bytes2.Length);
				cryptoStream.FlushFinalBlock();
				byte[] inArray = memoryStream.ToArray();
				cryptoStream.Close();
				memoryStream.Close();
				result = System.Convert.ToBase64String(inArray);
			}
			catch (System.Security.Cryptography.CryptographicException ex)
			{
				throw new System.Exception(ex.Message);
			}
			return result;
		}

		internal string ToDecrypt(string encryptKey, string str)
		{
			string @string;
			try
			{
				byte[] bytes = System.Text.Encoding.Unicode.GetBytes(encryptKey);
				byte[] buffer = System.Convert.FromBase64String(str);
				System.IO.MemoryStream stream = new System.IO.MemoryStream(buffer);
				System.Security.Cryptography.CryptoStream cryptoStream = new System.Security.Cryptography.CryptoStream(stream, new System.Security.Cryptography.DESCryptoServiceProvider().CreateDecryptor(bytes, bytes), System.Security.Cryptography.CryptoStreamMode.Read);
				byte[] array = new byte[1000];
				System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
				int count;
				while ((count = cryptoStream.Read(array, 0, array.Length)) > 0)
				{
					memoryStream.Write(array, 0, count);
				}
				@string = System.Text.Encoding.Unicode.GetString(memoryStream.ToArray());
			}
			catch (System.Security.Cryptography.CryptographicException ex)
			{
				throw new System.Exception(ex.Message);
			}
			return @string;
		}
	}
}
