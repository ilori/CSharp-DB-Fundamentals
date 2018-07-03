namespace P01_BillsPaymentSystem.App
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Data;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main()
        {
            var db = new BillsPaymentSystemContext();

            Seed(db);

            var userId = int.Parse(Console.ReadLine());

            using (db)
            {
                UserInformation(db, userId);

                PayBills(db, userId, 500);
            }
        }

        private static void UserInformation(BillsPaymentSystemContext db, int userId)
        {
            var user = db.Users
                .Where(x => x.UserId == userId)
                .Select(x => new
                {
                    Name = $"{x.FirstName} {x.LastName}",
                    BankAccounts = x.PaymentMethods
                        .Where(ba => ba.Type == PaymentMethodType.BankAccount)
                        .Select(e => e.BankAccount).ToList(),
                    CreditCards = x.PaymentMethods
                        .Where(pm => pm.Type == PaymentMethodType.CreditCard)
                        .Select(cc => cc.CreditCard).ToList()
                }).FirstOrDefault();

            Console.WriteLine($"User: {user.Name}");

            if (user.BankAccounts.Any())
            {
                Console.WriteLine($"Bank Accounts:");

                foreach (var ba in user.BankAccounts)
                {
                    Console.WriteLine($"-- ID: {ba.BankAccountId}");
                    Console.WriteLine($"--- Balance: {ba.Balance:F2}");
                    Console.WriteLine($"--- Bank: {ba.BankName}");
                    Console.WriteLine($"--- SWIFT: {ba.SwiftCode}");
                }
            }
            if (user.CreditCards.Any())
            {
                Console.WriteLine($"Credit Cards:");

                foreach (var cc in user.CreditCards)
                {
                    Console.WriteLine($"-- ID: {cc.CreditCardId}");
                    Console.WriteLine($"--- Limit: {cc.Limit:F2}");
                    Console.WriteLine($"--- Money Owed: {cc.MoneyOwed:F2}");
                    Console.WriteLine($"--- Limit Left: {cc.LimitLeft}");
                    Console.WriteLine(
                        $"--- Expiration Date: {cc.ExpirationDate.ToString("yyyy/MM", CultureInfo.InvariantCulture)}");
                }
            }
        }

        private static void Seed(BillsPaymentSystemContext db)
        {
            using (db)
            {
                db.Database.EnsureDeleted();
                db.Database.Migrate();

                var user = new User[]
                {
                    new User()
                    {
                        FirstName = "Stamat",
                        LastName = "Stamatov",
                        Email = "stamat@mail.bg",
                        Password = "stamat123"
                    },
                    new User()
                    {
                        FirstName = "Grigor",
                        LastName = "Petrov",
                        Email = "hgoo@mail.bg",
                        Password = "gosh555"
                    },
                    new User()
                    {
                        FirstName = "Niki",
                        LastName = "Ivanov",
                        Email = "niks@abv.bg",
                        Password = "naigolemiq123"
                    },
                };

                var creditCards = new CreditCard[]
                {
                    new CreditCard()
                    {
                        ExpirationDate = DateTime.ParseExact("20-05-2023", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                        Limit = 2500m,
                        MoneyOwed = 1000m
                    },
                    new CreditCard()
                    {
                        ExpirationDate = DateTime.ParseExact("15-02-2020", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                        Limit = 200m,
                        MoneyOwed = 5m
                    },
                    new CreditCard()
                    {
                        ExpirationDate = DateTime.ParseExact("01-01-2018", "dd-MM-yyyy", CultureInfo.InvariantCulture),
                        Limit = 5000m,
                        MoneyOwed = 10m
                    }
                };

                var bankAccount = new BankAccount()
                {
                    Balance = 6000m,
                    BankName = "UniCredit",
                    SwiftCode = "UNCR90"
                };

                var paymentMethods = new PaymentMethod[]
                {
                    new PaymentMethod()
                    {
                        User = user[0],
                        CreditCard = creditCards[1],
                        Type = PaymentMethodType.CreditCard,
                    },
                    new PaymentMethod()
                    {
                        User = user[2],
                        CreditCard = creditCards[2],
                        Type = PaymentMethodType.CreditCard,
                    },
                    new PaymentMethod()
                    {
                        User = user[1],
                        BankAccount = bankAccount,
                        Type = PaymentMethodType.BankAccount,
                    },
                };

                db.Users.AddRange(user);
                db.BankAccounts.Add(bankAccount);
                db.CreditCards.AddRange(creditCards);
                db.PaymentMethods.AddRange(paymentMethods);
                db.SaveChanges();
            }
        }

        private static void PayBills(BillsPaymentSystemContext db, int userId, decimal amount)
        {
            try
            {
                var user = db.Users.Find(userId);

                if (user == null)
                {
                    throw new ArgumentException($"User with ID: {userId} not found!");
                }

                var totalMoney = 0m;

                var bankAccounts = db.BankAccounts
                    .Select(x => new
                    {
                        UserId = x.PaymentMethod.UserId,
                        BankAccountId = x.BankAccountId,
                        Balance = x.Balance
                    })
                    .Where(x => x.UserId == userId)
                    .ToList();


                var creditCards = db.CreditCards
                    .Select(x => new
                    {
                        UserId = x.PaymentMethod.UserId,
                        CreditCardId = x.CreditCardId,
                        LimitLeft = x.LimitLeft
                    })
                    .Where(x => x.UserId == userId)
                    .ToList();

                totalMoney += bankAccounts.Sum(x => x.Balance);
                totalMoney += creditCards.Sum(x => x.LimitLeft);

                if (totalMoney < amount)
                {
                    throw new ArgumentException($"Insufficient funds!");
                }
                var areBillsPayed = false;

                foreach (var b in bankAccounts.OrderBy(x => x.BankAccountId))
                {
                    var currentAccount = db.BankAccounts.Find(b.BankAccountId);

                    if (amount <= currentAccount.Balance)
                    {
                        currentAccount.Withdraw(amount);
                        areBillsPayed = true;
                    }
                    else
                    {
                        amount -= currentAccount.Balance;
                        currentAccount.Withdraw(currentAccount.Balance);
                    }

                    if (areBillsPayed)
                    {
                        db.SaveChanges();
                        return;
                    }
                }
                foreach (var c in creditCards.OrderBy(x => x.CreditCardId))
                {
                    var currentCreditCard = db.CreditCards.Find(c.CreditCardId);

                    if (amount <= currentCreditCard.LimitLeft)
                    {
                        currentCreditCard.Withdraw(amount);
                        areBillsPayed = true;
                    }
                    else
                    {
                        amount -= currentCreditCard.LimitLeft;
                        currentCreditCard.Withdraw(currentCreditCard.LimitLeft);
                    }

                    if (areBillsPayed)
                    {
                        db.SaveChanges();
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}