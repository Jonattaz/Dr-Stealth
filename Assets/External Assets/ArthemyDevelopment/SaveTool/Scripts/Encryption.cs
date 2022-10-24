using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;


namespace ArthemyDevelopment.Save
{
    public static class Encryption
    {
        
          public static byte[] EncryptData(string data)
            {
                AesManaged _aes = new AesManaged();
                byte[] key = KeyGenerator();
                byte[] iv = KeyGenerator();
        
                ICryptoTransform encryptor = _aes.CreateEncryptor(key, iv);
                
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
                StreamWriter sw = new StreamWriter(cs);
                
                sw.WriteLine(data);
                
                sw.Close();
                cs.Close();
                ms.Close();
        
                return ms.ToArray();
            }
          
          public static byte[] EncryptData(string data, string custkey)
          {
              AesManaged _aes = new AesManaged();
              byte[] key = KeyGenerator(custkey);
              byte[] iv = KeyGenerator(custkey);
        
              ICryptoTransform encryptor = _aes.CreateEncryptor(key, iv);
                
              MemoryStream ms = new MemoryStream();
              CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
              StreamWriter sw = new StreamWriter(cs);
                
              sw.WriteLine(data);
                
              sw.Close();
              cs.Close();
              ms.Close();
        
              return ms.ToArray();
          }
        
            public static string DecryptData(byte[] data)
            {
                AesManaged _aes = new AesManaged();
        
                byte[] key = KeyGenerator();
                byte[] iv = KeyGenerator();
        
                ICryptoTransform decryptor = _aes.CreateDecryptor(key, iv);
                
                MemoryStream ms = new MemoryStream(data);
                CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                
                string decrypted = sr.ReadToEnd();
                
                ms.Close();
                cs.Close();
                sr.Close();
                
        
                return decrypted;
            }
            
            public static string DecryptData(byte[] data, string custkey)
            {
                AesManaged _aes = new AesManaged();
        
                byte[] key = KeyGenerator(custkey);
                byte[] iv = KeyGenerator(custkey);
        
                ICryptoTransform decryptor = _aes.CreateDecryptor(key, iv);
                
                MemoryStream ms = new MemoryStream(data);
                CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                
                string decrypted = sr.ReadToEnd();
                
                ms.Close();
                cs.Close();
                sr.Close();
                
        
                return decrypted;
            }
        
            public static byte[] KeyGenerator()
            {
                byte[] key;
                string userKey = SaveDataPreferences.current.EncryptionKey;
        
                if (userKey.Length == 16)
                {
                    key = Encoding.ASCII.GetBytes(userKey);
                }
                else
                {
                    char[] _key = userKey.ToCharArray();
                    Array.Resize(ref _key, 16);
                    key = Encoding.ASCII.GetBytes(_key);
        
                }
        
                return key;
            }
            
            public static byte[] KeyGenerator(string customkey)
            {
                byte[] key;
                string userKey = customkey;
        
                if (userKey.Length == 16)
                {
                    key = Encoding.ASCII.GetBytes(userKey);
                }
                else
                {
                    char[] _key = userKey.ToCharArray();
                    Array.Resize(ref _key, 16);
                    key = Encoding.ASCII.GetBytes(_key);
        
                }
        
                return key;
            }
            
        
    }
    
}
