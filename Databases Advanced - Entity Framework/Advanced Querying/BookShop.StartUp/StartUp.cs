namespace BookShop
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using BookShop.Models;
    using BookShop.Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {
            }
        }

        public static int RemoveBooks(BookShopContext db)
        {
            var books = db.Books
                .Where(x => x.Copies < 4200)
                .ToList();

            var booksCount = books.Count;

            db.Books.RemoveRange(books);

            db.SaveChanges();

            return booksCount;
        }

        public static void IncreasePrices(BookShopContext db)
        {
            var books = db.Books
                .Where(x => x.ReleaseDate.Value.Year < 2010)
                .ToList();

            foreach (var book in books)
            {
                book.Price += 5m;
            }
            db.SaveChanges();
        }

        public static string GetMostRecentBooks(BookShopContext db)
        {
            var categories = db.Categories
                .Include(x => x.CategoryBooks)
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    x.Name,
                    Books = x.CategoryBooks.Select(b => new
                        {
                            b.Book.Title,
                            b.Book.ReleaseDate
                        })
                        .OrderByDescending(b => b.ReleaseDate)
                        .Take(3)
                        .ToList()
                });
            var result = new StringBuilder();
            foreach (var category in categories)
            {
                result.AppendLine($"--{category.Name}");
                foreach (var book in category.Books)
                {
                    result.AppendLine($"{book.Title} ({book.ReleaseDate.Value.Year})");
                }
            }
            return result.ToString();
        }

        public static string GetTotalProfitByCategory(BookShopContext db)
        {
            var categories = db.Categories
                .Include(x => x.CategoryBooks)
                .Select(x => new
                {
                    x.Name,
                    TotalSum = x.CategoryBooks.Select(b => b.Book.Copies * b.Book.Price).Sum(),
                })
                .OrderByDescending(x => x.TotalSum)
                .ToList();

            var result = new List<string>();

            foreach (var category in categories)
            {
                result.Add($"{category.Name} ${category.TotalSum:F2}");
            }

            return string.Join(Environment.NewLine, result);
        }

        public static string CountCopiesByAuthor(BookShopContext db)
        {
            var authors = db.Authors
                .Select(x => new
                {
                    FullName = $"{x.FirstName} {x.LastName}",
                    Copies = x.Books.Select(b => b.Copies).Sum()
                })
                .OrderByDescending(x => x.Copies)
                .ToList();

            var result = new List<string>();

            foreach (var author in authors)
            {
                result.Add($"{author.FullName} - {author.Copies}");
            }

            return string.Join(Environment.NewLine, result);
        }

        public static int CountBooks(BookShopContext db, int input)
        {
            var books = db.Books.Where(x => x.Title.Length > input)
                .ToList();

            return books.Count;
        }

        public static string GetBooksByAuthor(BookShopContext db, string input)
        {
            var books = db.Books
                .OrderBy(x => x.BookId)
                .Include(x => x.Author)
                .Where(x => x.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .Select(x => new
                {
                    Name = $"{x.Author.FirstName} {x.Author.LastName}",
                    Title = x.Title
                })
                .ToList();

            var result = new List<string>();

            foreach (var book in books)
            {
                result.Add($"{book.Title} ({book.Name})");
            }

            return string.Join(Environment.NewLine, result);
        }

        public static string GetBookTitlesContaining(BookShopContext db, string input)
        {
            var books = db.Books
                .Where(x => x.Title.ToLower().Contains(input.ToLower()))
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToList();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetAuthorNamesEndingIn(BookShopContext db, string input)
        {
            var authors = db.Authors
                .Where(e => e.FirstName.EndsWith(input))
                .Select(x => new
                {
                    Name = $"{x.FirstName} {x.LastName}"
                })
                .OrderBy(x => x.Name)
                .ToList();
            var result = new List<string>();
            foreach (var author in authors)
            {
                result.Add($"{author.Name}");
            }

            return string.Join(Environment.NewLine, result);
        }

        public static string GetBooksByCategory(BookShopContext db, string input)
        {
            var tokens = input.ToLower()
                .Split(new[] {"\t", " ", Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            var books = db.Books
                .Where(b => b.BookCategories.Any(c => tokens.Contains(c.Category.Name.ToLower())))
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToList();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksReleasedBefore(BookShopContext db, string input)
        {
            var books = db.Books
                .Include(e => e.BookCategories)
                .ThenInclude(e => e.Category)
                .Where(x => x.ReleaseDate.Value <
                            DateTime.ParseExact(input, "dd-MM-yyyy", CultureInfo.InvariantCulture))
                .OrderByDescending(x => x.ReleaseDate)
                .Select(e => new
                {
                    Title = e.Title,
                    Edition = e.EditionType,
                    Price = e.Price
                }).ToList();

            var result = new List<string>();

            foreach (var book in books)
            {
                result.Add($"{book.Title} - {book.Edition} - ${book.Price:F2}");
            }

            return string.Join(Environment.NewLine, result);
        }

        public static string GetBooksNotRealeasedIn(BookShopContext db, int input)
        {
            var books = db.Books
                .Where(x => x.ReleaseDate.Value.Year != input)
                .OrderBy(x => x.BookId)
                .Select(x => x.Title)
                .ToList();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetGoldenBooks(BookShopContext db)
        {
            var books = db.Books
                .Where(e => e.EditionType == EditionType.Gold && e.Copies < 5000)
                .OrderBy(e => e.BookId)
                .Select(e => e.Title)
                .ToList();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksByAgeRestriction(BookShopContext db, string input)
        {
            var books = db.Books
                .Where(e => string.Equals(e.AgeRestriction.ToString(), input,
                    StringComparison.InvariantCultureIgnoreCase))
                .Select(e => e.Title)
                .OrderBy(e => e)
                .ToList();

            var result = string.Join(Environment.NewLine, books);

            return result;
        }

        public static string GetBooksByPrice(BookShopContext db)
        {
            var books = db.Books
                .Where(x => x.Price > 40)
                .Select(e => new
                    {
                        Title = e.Title,
                        Price = e.Price
                    }
                )
                .OrderByDescending(e => e.Price)
                .ToList();

            var result = books.Select(book => $"{book.Title} - ${book.Price:F2}").ToList();

            return string.Join(Environment.NewLine, result);
        }
    }
}