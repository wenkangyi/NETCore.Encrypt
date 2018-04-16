using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace NETCore.Encrypt.Tests
{
    public class AES_Tests
    {
        [Fact(DisplayName = "AES ke test")]
        public void Cretea_AESKey_Test()
        {
            //Ack
            var aesKey = EncryptProvider.CreateAesKey();

            //Assert
            Assert.NotNull(aesKey);

            Assert.NotEmpty(aesKey.Key);
            Assert.Equal(32, aesKey.Key.Length);

            Assert.NotEmpty(aesKey.IV);
            Assert.Equal(16, aesKey.IV.Length);
        }


        [Fact(DisplayName = "AES encrypt success test")]
        public void Aes_Encryt_Success_Test()
        {
            var aesKey = EncryptProvider.CreateAesKey();
            var key = aesKey.Key;
            var srcString = "test aes encrypt";

            //Ack
            var result = EncryptProvider.AESEncrypt(srcString, key);

            //Assert
            Assert.NotEmpty(result);
        }

        [Fact(DisplayName = "AES encrypt with empty data test")]
        public void Aes_Encryt_EmptyData_Fail_Test()
        {
            var aesKey = EncryptProvider.CreateAesKey();
            var key = aesKey.Key;
            var srcString = string.Empty;

            //Assert
            Assert.Throws<ArgumentException>(() => EncryptProvider.AESDecrypt(srcString, key));
        }


        [Fact(DisplayName = "AES encrypt with error key test")]
        public void Aes_Encrypt_ErrorKey_Fail_Test()
        {
            var aesKey = EncryptProvider.CreateAesKey();
            var key = "1hyhuo";
            var srcString = "test aes encrypt";

            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => EncryptProvider.AESEncrypt(srcString, key));
        }

        [Fact(DisplayName = "AES decrypt success test")]
        public void Aes_Decryt_Success_Test()
        {
            var aesKey = EncryptProvider.CreateAesKey();
            var key = aesKey.Key;

            var srcString = "test aes encrypt";

            //Ack
            var encrypted = EncryptProvider.AESEncrypt(srcString, key);
            var decrypted = EncryptProvider.AESDecrypt(encrypted, key);

            //Assert
            Assert.NotEmpty(encrypted);
            Assert.NotEmpty(decrypted);
            Assert.Equal(srcString, decrypted);
        }

        [Fact(DisplayName = "AES decrypt with empty data test")]
        public void Aes_Decrypt_EmptyData_Fail_Test()
        {
            var aesKey = EncryptProvider.CreateAesKey();
            var key = aesKey.Key;
            var srcString = string.Empty;

            //Assert
            Assert.Throws<ArgumentException>(() => EncryptProvider.AESDecrypt(srcString, key));
        }

        [Fact(DisplayName = "AES decrypt with error key test")]
        public void Aes_Decrypt_ErrorKey_Fail_Test()
        {
            var aesKey = EncryptProvider.CreateAesKey();
            var key = "dfafa";  //must be 32 bit
            var srcString = "test aes encrypt";

            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => EncryptProvider.AESDecrypt(srcString, key));
        }

        [Fact(DisplayName = "AES encrypt with iv success test")]
        public void Aes_Encryt_WithIV_Success_Test()
        {
            var aesKey = EncryptProvider.CreateAesKey();

            var key = aesKey.Key;
            var iv = aesKey.IV;

            var srcString = "test aes encrypt";

            //Ack
            var result = EncryptProvider.AESEncrypt(srcString, key, iv);

            //Assert
            Assert.NotEmpty(result);
        }

        [Fact(DisplayName = "AES encrypt with error key test")]
        public void Aes_Encrypt_With_ErrorKey_Fail_Test()
        {
            var aesKey = EncryptProvider.CreateAesKey();

            var key = aesKey.Key;
            var iv = "ikojpoi";  //must be 16 bit

            var srcString = "test aes encrypt";

            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => EncryptProvider.AESEncrypt(srcString, key, iv));
        }

        [Fact(DisplayName = "AES encrypt with error iv test")]
        public void Aes_Encrypt_With_ErrorIV_Fail_Test()
        {
            var aesKey = EncryptProvider.CreateAesKey();

            var key = "j1l23kj1j"; //must be 32 bit
            var iv = aesKey.IV;

            var srcString = "test aes encrypt";
            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => EncryptProvider.AESEncrypt(srcString, key, iv));
        }

        [Fact(DisplayName = "AES decrypt with iv success test")]
        public void Aes_Decryt_WithIV_Success_Test()
        {
            var aesKey = EncryptProvider.CreateAesKey();
            var key = aesKey.Key;
            var iv = aesKey.IV;

            var srcString = "test aes encrypt";

            //Ack
            var encrypted = EncryptProvider.AESEncrypt(srcString, key, iv);
            var decrypted = EncryptProvider.AESDecrypt(encrypted, key, iv);

            //Assert
            Assert.NotEmpty(encrypted);
            Assert.NotEmpty(decrypted);
            Assert.Equal(srcString, decrypted);
        }

        [Fact(DisplayName = "AES decrypt with error key test")]
        public void Aes_Decrypt_With_ErrorKey_Fail_Test()
        {
            var aesKey = EncryptProvider.CreateAesKey();
            var key = aesKey.Key;
            var iv = "ikojpoi";  //must be 16 bit
            var srcString = "test aes encrypt";

            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => EncryptProvider.AESDecrypt(srcString, key, iv));
        }

        [Fact(DisplayName = "AES decrypt with error iv test")]
        public void Aes_Decrypt_With_ErrorIV_Fail_Test()
        {
            var aesKey = EncryptProvider.CreateAesKey();
            var key = "j1l23kj1j"; //must be 32 bit
            var iv = aesKey.IV;
            var srcString = "test aes encrypt";

            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => EncryptProvider.AESDecrypt(srcString, key, iv));
        }

        [Fact(DisplayName = "AES encrypt and decrypt about file stream")]
        public void Aes_File_Test()
        {
            var rootDir = Path.Combine(AppContext.BaseDirectory, "Assets");
            if (!Directory.Exists(rootDir))
            {
                Directory.CreateDirectory(rootDir);
            }

            var file = Path.Combine(rootDir, "AES_File_Test.txt");
            if (!File.Exists(file))
            {
                using (var fileStream = File.OpenWrite(file))
                {
                    var tempDatas = Encoding.UTF8.GetBytes("May be you shoud get all the buffer of file,and then decrypt by EncryptProvider.");
                    fileStream.Write(tempDatas, 0, tempDatas.Length);
                }
            }

            var aesKey = EncryptProvider.CreateAesKey();

            //encrypt
            var text = File.ReadAllText(file);
            var datas = File.ReadAllBytes(file);

            var encryptedDatas = EncryptProvider.AESEncrypt(datas, aesKey.Key, aesKey.IV);
            var encryptedFile = Path.Combine(rootDir, "AES_File_Test_Encrypted.txt");

            using (var fileStream = File.OpenWrite(encryptedFile))
            {
                fileStream.Write(encryptedDatas, 0, encryptedDatas.Length);
            }

            //decrypt
            var encryptedFileDatas = File.ReadAllBytes(encryptedFile);
            var decryptedDatas = EncryptProvider.AESDecrypt(encryptedFileDatas, aesKey.Key, aesKey.IV);

            var decryptedText = Encoding.UTF8.GetString(decryptedDatas);

            //assert
            Assert.NotNull(decryptedDatas);
            Assert.Equal(decryptedText, text);


        }
    }
}
