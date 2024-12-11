using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;

namespace ControleEnderecos.Application
{
    public class Helper
    {

        /// <summary>
        /// Gera um hash seguro de uma senha utilizando o algoritmo PBKDF2 com a função de hash HMACSHA256.
        /// </summary>
        /// <param name="password">A senha que será processada para gerar o hash.</param>
        /// <param name="salt">O valor do salt em bytes, utilizado para proteger a senha contra ataques de rainbow table.</param>
        /// <returns>
        /// Uma string em Base64 que representa o hash derivado da senha.
        /// </returns>
        /// <remarks>
        /// Este método utiliza o algoritmo PBKDF2 (Password-Based Key Derivation Function 2) com a função de hash HMACSHA256.
        /// O número de iterações definido é de 100.000, o que torna o processo de hash mais resistente a ataques de força bruta.
        /// O tamanho da chave derivada (subkey) é de 256 bits (32 bytes).
        /// </remarks>
        /// <example>
        /// Exemplo de uso:
        /// <code>
        /// string senha = "minhaSenhaSegura";
        /// byte[] salt = new byte[16];
        ///
        /// // Gera um salt aleatório (normalmente gerado e armazenado junto com o hash)
        /// using (var rng = new RNGCryptoServiceProvider())
        /// {
        ///     rng.GetBytes(salt);
        /// }
        ///
        /// // Gera o hash da senha
        /// string hashDaSenha = GetPasswordHash(senha, salt);
        /// Console.WriteLine($"Hash da senha: {hashDaSenha}");
        /// </code>
        /// </example>
        public static string GetPasswordHash(String password, byte[] salt)
        {   
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            return hashed;
        }

        /// <summary>
        /// Gera um salt criptográfico em formato Base64.
        /// </summary>
        /// <param name="bits">O tamanho do salt em bits. O valor padrão é 128 bits.</param>
        /// <param name="baseBits">A base de conversão de bits para bytes. O valor padrão é 8 bits por byte.</param>
        /// <returns>
        /// Um string em Base64 representando o salt gerado.
        /// </returns>
        /// <remarks>
        /// O salt gerado é convertido para Base64 para facilitar o armazenamento ou transporte.
        /// Este método utiliza o método <see cref="GenerateSaltToBytes"/> internamente.
        /// </remarks>
        /// <example>
        /// Exemplo de uso:
        /// <code>
        /// string salt = GenerateSalt();
        /// Console.WriteLine($"Salt gerado: {salt}");
        /// </code>
        /// </example>
        public static string GenerateSalt(int bits = 128, int baseBits = 8)
        {
            return Convert.ToBase64String(GenerateSaltToBytes(bits, baseBits));
        }

        /// <summary>
        /// Gera um salt criptográfico como um array de bytes.
        /// </summary>
        /// <param name="bits">O tamanho do salt em bits. O valor padrão é 128 bits.</param>
        /// <param name="baseBits">A base de conversão de bits para bytes. O valor padrão é 8 bits por byte.</param>
        /// <returns>
        /// Um array de bytes representando o salt gerado.
        /// </returns>
        /// <exception cref="ArgumentException">Lançada quando o valor de <paramref name="bits"/> ou <paramref name="baseBits"/> é menor que 8.</exception>
        /// <remarks>
        /// O salt é utilizado em processos criptográficos, como derivação de chaves, para adicionar aleatoriedade e proteger contra ataques de rainbow table.
        /// </remarks>
        /// <example>
        /// Exemplo de uso:
        /// <code>
        /// byte[] saltBytes = GenerateSaltToBytes();
        /// Console.WriteLine($"Salt em bytes gerado: {BitConverter.ToString(saltBytes)}");
        /// </code>
        /// </example>
        public static byte[] GenerateSaltToBytes(int bits = 128, int baseBits = 8)
        {
            if (bits < 8 && baseBits < 8)
                throw new ArgumentException("Parâmetro bits ou baseBits devem ser maior que 8");

            byte[] salt = RandomNumberGenerator.GetBytes(bits / baseBits); // divide by 8 to convert bits to bytes            
            return salt;
        }        


        /// <summary>
        /// Criptografa uma string de texto simples usando criptografia AES.
        /// </summary>
        /// <param name="plainText">A string de texto simples a ser criptografada.</param>
        /// <param name="key">A chave de criptografia como um array de bytes. Deve ter um comprimento válido para AES (128, 192 ou 256 bits).</param>
        /// <param name="iv">O vetor de inicialização (IV) como um array de bytes. Deve ter o mesmo comprimento que o tamanho do bloco AES (16 bytes).</param>
        /// <returns>
        /// Uma string codificada em base64 que representa a versão criptografada do texto simples de entrada.
        /// </returns>
        /// <exception cref="ArgumentNullException">Lançada quando o plainText, key ou iv é nulo.</exception>
        /// <exception cref="CryptographicException">Lançada quando a criptografia falha devido a uma chave ou IV inválido, ou outro problema criptográfico.</exception>
        /// <example>
        /// Exemplo de uso:
        /// <code>
        /// string texto = "Texto para criptografar";
        /// byte[] chave = new byte[32]; // Chave de 256 bits (32 bytes)
        /// byte[] vetorInicializacao = new byte[16]; // Vetor de inicialização (16 bytes)
        /// 
        /// // Gera uma chave e IV aleatórios para fins de demonstração
        /// using (var rng = new RNGCryptoServiceProvider())
        /// {
        ///     rng.GetBytes(chave);
        ///     rng.GetBytes(vetorInicializacao);
        /// }
        /// // ou utilizar um chave e um IV fixo
        /// 
        /// // byte[] chave = Encoding.UTF8.GetBytes("0123456789abcdef0123456789abcdef"); // Chave de 32 bytes
        /// // byte[] vetorInicializacao = Encoding.UTF8.GetBytes("abcdef0123456789");  // IV de 16 bytes
        /// 
        /// 
        /// // Chama o método de criptografia
        /// string textoCriptografado = EncryptString(texto, chave, vetorInicializacao);
        /// Console.WriteLine($"Texto criptografado: {textoCriptografado}");
        /// </code>
        /// </example>
        public static string EncryptString(string plainText, byte[] key, byte[] iv)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }

        public static string DecryptString(string cipherText, byte[] key, byte[] iv)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }


        public static string? GenerateAesSecretKey(byte[] salt)
        {
            DateTime validUntil = DateTime.UtcNow.AddSeconds(30);
            var ticksValue = validUntil.Ticks;
            var ticksBytes = BitConverter.GetBytes(ticksValue);

            using (var aes = Aes.Create())
            {
                aes.GenerateKey();
                salt = ticksBytes.Concat(salt).ToArray();
                byte[] data = aes.Key.Concat(salt).ToArray();
                return Convert.ToBase64String(data);
            }
        }

        // Gera o hash SHA-256
        public static string GenerateSHA256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static bool IsValidLogin(string login)
        {
            /**             
             *[a-zA-Z]: Indica que o nome de usuário deve começar com uma letra, seja minúscula(a-z) ou maiúscula(A-Z).
             *  (?!.*[._@]{2})(Negative Lookahead) Aqui estamos tratando para ter mais de dois caracteres consecutivos
             *  ?!(Negative Lookahead) Garante que a string não contenha um padrão especifico    
             *  .* Corresponde a qualquer sequência de caractes
             *  [._@]{2} Corresponde exatamente dois caracters consecutivos             
            */
            string regexString = @"^[a-zA-Z](?!.*[._\-@]{2})[a-zA-Z0-9._\-@]{2,23}[a-zA-Z0-9]$";

            return IsValid(regexString, login);// || IsValidEmail(login);
        }

        public static bool IsValidURL(string email)
        {
            Uri? uri = null;
            return Uri.TryCreate(email, UriKind.Absolute, out uri)
                && (uri.Scheme == Uri.UriSchemeHttp
                 || uri.Scheme == Uri.UriSchemeHttps
                 || uri.Scheme == Uri.UriSchemeFtp
                 || uri.Scheme == Uri.UriSchemeMailto);
        }

        public static double Noise(double factor, double y = 2.5)
        {
            long timeticks = DateTime.Now.AddSeconds(30).Ticks;
            return (Math.Sin(timeticks / factor) * Math.Sin(timeticks / factor)) * y;
        }

        public static bool IsValidEmail(string email)
        {
            //[a-zA-Z](? !.* [._@]{ 2})[a-zA - Z0 - 9._@]
            //string regexString = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            //[.!#$%&'*+/=?^_`{|}~-]
            //string regexString = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,253}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,253}[a-zA-Z0-9])?)*$";
            string regexString = @"^[a-zA-Z0-9](?!.*[._@]{2})[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,253}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,253}[a-zA-Z])?)*$";
            return IsValid(regexString, email);
        }

        public static bool IsValid(string regex, string value)
        {

            try
            {
                bool isMatch = Regex.IsMatch(value, regex);
                return isMatch;
            }
            catch (ArgumentException e)
            {
                return false;
            }

        }

    }
}
