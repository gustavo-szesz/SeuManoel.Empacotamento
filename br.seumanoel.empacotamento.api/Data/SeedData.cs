using br.seumanoel.empacotamento.api.Models;
using br.seumanoel.empacotamento.api.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace br.seumanoel.empacotamento.api.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());

            if (context.Users.Any())
            {
                Console.WriteLine("Banco de dados já está populado com usuários.");
                return;
            }

            Console.WriteLine("Iniciando a criação de usuários de seed...");

            // User test to be created on the database
            var userDto = new UserDto
            {
                Username = "teste-user",
                Password = "password1456"
            };
            
            Console.WriteLine($"Criando usuário de teste: {userDto.Username}");

            // Criar entidade de usuário a partir do DTO
            var user = new User
            {
                Username = userDto.Username
            };
            
            // Hash da senha usando o mesmo algoritmo do serviço de autenticação
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(userDto.Password));
            user.Password = Convert.ToBase64String(hash);
            
            // Adicionar e salvar no banco de dados
            context.Users.Add(user);
            context.SaveChanges();
            
            // Verificar se o usuário foi realmente salvo
            var savedUser = context.Users.FirstOrDefault(u => u.Username == userDto.Username);
            Console.WriteLine($"Usuário criado com sucesso: {savedUser != null}");

            // Adicionar um usuário admin também
            var adminUser = new User
            {
                Username = "admin",
                Password = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes("admin123")))
            };

            context.Users.Add(adminUser);
            context.SaveChanges();
            Console.WriteLine("Usuário admin criado com sucesso");
        }
    }
}