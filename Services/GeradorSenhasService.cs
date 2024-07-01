using System.Security.Cryptography;
using System.Text;
using TO_DO___API.Data.Dto;
using TO_DO___API.Data;
using TO_DO___API.Models;

namespace TO_DO___API.Services;

public class GeradorSenhasService
{
    private readonly EntidadesContext _entidadesContext;
    private readonly HttpClient _httpClient;
    private readonly byte[] _encryptionKey;

    public GeradorSenhasService(EntidadesContext entidadesContext, HttpClient httpClient)
    {
        _entidadesContext = entidadesContext;
        _httpClient = httpClient;
        _encryptionKey = Encoding.UTF8.GetBytes("value"); // Troque por sua chave de 32 bytes
    }

    // Método para gerar senha
    public async Task GerarSenhaAsync(CreateGeradorSenhasDto geradorSenhasDto, string userId)
    {
        // Verifica se o Id do usuário é válido
        if (string.IsNullOrEmpty(userId))
        {
            throw new ArgumentNullException("Não foi possível obter o Id do usuário.");
        }

        // Chama a API para gerar a senha, passando a quantidade de caracteres desejada
        var senhaGerada = await GerarSenhaExternaAsync(geradorSenhasDto.QuantidadeCaracteres);

        // Criptografa a senha gerada
        var senhaCriptografada = EncryptString(senhaGerada);

        GeradorSenhas novaSenha = new GeradorSenhas()
        {
            RedeSocial = geradorSenhasDto.RedeSocial,
            QuantidadeCaracteres = geradorSenhasDto.QuantidadeCaracteres,
            GerarSenha = senhaCriptografada,
            CreatedByUserId = userId
        };

        _entidadesContext.GeradorSenhas.Add(novaSenha);
        await _entidadesContext.SaveChangesAsync();
    }

    // Método para chamar a API externa e gerar a senha
    private async Task<string> GerarSenhaExternaAsync(int length)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"passwordgenerator?length={length}");

        using (var response = await _httpClient.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            return body;
        }
    }

    // Método para criptografar uma string
    private string EncryptString(string plainText)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = _encryptionKey;
            aes.GenerateIV();

            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            using (var ms = new MemoryStream())
            {
                // Escreve o IV no início do fluxo de bytes
                ms.Write(aes.IV, 0, aes.IV.Length);

                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                }

                // Retorna a concatenação do IV com os dados criptografados, convertidos para Base64
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    // Método para descriptografar uma string
    public string DecryptString(string cipherText)
    {
        byte[] fullCipher;
        try
        {
            // Tenta decodificar a string de cifra Base64 de volta para bytes
            fullCipher = Convert.FromBase64String(cipherText);
        }
        catch (FormatException)
        {
            // Se a string não estiver em um formato Base64 válido, retorne uma mensagem de erro ou lance uma exceção
            throw new ArgumentException("A string de cifra não está em um formato Base64 válido.");
        }

        using (var aes = Aes.Create())
        {
            aes.Key = _encryptionKey;

            // O IV tem o tamanho do bloco do algoritmo de criptografia
            byte[] iv = new byte[aes.BlockSize / 8];
            // Copia os primeiros bytes do array fullCipher para o array iv
            Array.Copy(fullCipher, 0, iv, 0, iv.Length);

            // O restante do array fullCipher contém os dados criptografados
            byte[] cipher = new byte[fullCipher.Length - iv.Length];
            // Copia os bytes restantes de fullCipher para o array cipher
            Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);

            aes.IV = iv;

            using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            using (var ms = new MemoryStream(cipher))
            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (var sr = new StreamReader(cs))
            {
                return sr.ReadToEnd();
            }
        }
    }


}