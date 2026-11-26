using System;
using System.Linq;
using HousingRegistration.Model;
using PasswordLibrary;

namespace HousingRegistration
{
    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine("Создание учетной записи для жильца");
            Console.Write("Введите номер лицевого счета: ");
            string accountNumber = Console.ReadLine();
            Console.Write("Введите логин: ");
            string login = Console.ReadLine();
            Console.Write("Введите пароль: ");
            string password = Console.ReadLine();

            if (String.IsNullOrWhiteSpace(accountNumber) || String.IsNullOrWhiteSpace(login) || String.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Поля не должны быть пустыми.");
                return;
            }

            login = login.Trim();
            if (login.Length < 3 || login.Length > 50)
            {
                Console.WriteLine("Логин должен содержать от 3 до 50 символов.");
                return;
            }

            try
            {
                using (HousingContext db = new HousingContext())
                {
                    Account account = db.Accounts.FirstOrDefault(a => a.AccountNumber == accountNumber.Trim());
                    if (account == null)
                    {
                        Console.WriteLine("Лицевой счет не найден.");
                        return;
                    }

                    if (db.Users.Any(u => u.Login == login))
                    {
                        Console.WriteLine("Такой логин уже занят.");
                        return;
                    }

                    if (db.Users.Any(u => u.AccountID == account.AccountID))
                    {
                        Console.WriteLine("Для этого лицевого счета пользователь уже зарегистрирован.");
                        return;
                    }

                    string passwordHash = PasswordHasher.HashPassword(password);
                    User user = new User
                    {
                        AccountID = account.AccountID,
                        Login = login,
                        PasswordHash = passwordHash,
                        CreatedAt = DateTime.Now
                    };

                    db.Users.AddObject(user);
                    db.SaveChanges();
                    Console.WriteLine("Учетная запись добавлена.");
                    Console.WriteLine("Хэш пароля: " + passwordHash);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Не удалось сохранить запись: " + ex.Message);
            }

            Console.WriteLine("Нажмите любую клавишу для выхода.");
            Console.ReadKey();
        }
    }
}
