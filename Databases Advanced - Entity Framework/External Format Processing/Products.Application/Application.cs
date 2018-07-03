namespace Products.Application
{
    using System.Linq;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Linq;
    using Data;
    using Models;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Application
    {
        public static void Main()
        {
            DatabaseTools.ResetDatabase();


            ProductsInRange();
            ProductsInRangeXml();

            SuccessfullySoldProducts();
            SuccessfullySoldProductsXml();

            CategoriesByProductsCount();
            CategoriesByProductsCountXml();

            UsersAndProducts();
            UsersAndProductsXml();
        }

        //                        XML SECTION

        private static void UsersAndProductsXml()
        {
            using (var context = new ProductsDbContext())
            {
                var users = context.Users
                    .Where(x => x.ProductsSold.Count >= 1)
                    .OrderByDescending(x => x.ProductsSold.Count)
                    .ThenBy(x => x.LastName)
                    .Select(x => new
                    {
                        x.FirstName,
                        x.LastName,
                        x.Age,
                        Products = x.ProductsSold.Select(p => new
                        {
                            p.Name,
                            p.Price
                        })
                    }).ToList();

                var xDoc = new XDocument(new XElement("users", new XAttribute("count", users.Count)));

                foreach (var user in users)
                {
                    var finalUser = new XElement("user",
                        new XAttribute("first-name", user.FirstName ?? "[no first-name]"),
                        new XAttribute("last-name", user.LastName ?? "[no last-name]"),
                        new XAttribute("age", user.Age.ToString() ?? "[no age]"),
                        new XElement("sold-products", new XAttribute("count", user.Products.Count())));


                    foreach (var product in user.Products)
                    {
                        finalUser.Element("sold-products").Add(new XElement("product",
                            new XAttribute("name", product.Name),
                            new XAttribute("price", product.Price))
                        );
                    }

                    xDoc.Root.Add(finalUser);
                }

                var xmlString = xDoc.ToString();

                File.WriteAllText("XmlResults/UsersAndProductsXml.xml", xmlString);
            }
        }

        private static void CategoriesByProductsCountXml()
        {
            using (var db = new ProductsDbContext())
            {
                var categories = db.Categories
                    .OrderBy(x => x.CategoryProducts.Select(p => p.Product).Count())
                    .Select(x => new
                    {
                        x.Name,
                        ProductCount = x.CategoryProducts.Select(p => p.Product).Count(),
                        AveragePrice = x.CategoryProducts.Select(p => p.Product.Price).Average(),
                        TotalRevenue = x.CategoryProducts.Select(p => p.Product.Price).Sum()
                    }).ToList();

                var xDoc = new XDocument(new XElement("categories"));

                foreach (var c in categories)
                {
                    xDoc.Root.Add(new XElement("category",
                        new XAttribute("name", c.Name),
                        new XElement("products-count", c.ProductCount),
                        new XElement("average-price", c.AveragePrice),
                        new XElement("total-revenue", c.TotalRevenue)
                    ));
                }

                var xmlString = xDoc.ToString();

                File.WriteAllText("XmlResults/CategoriesByProductsCountXml.xml", xmlString);
            }
        }

        private static void SuccessfullySoldProductsXml()
        {
            using (var db = new ProductsDbContext())
            {
                var users = db.Users
                    .Where(x => x.ProductsSold.Count >= 1)
                    .OrderBy(x => x.LastName)
                    .ThenBy(x => x.FirstName)
                    .Select(x => new
                    {
                        x.FirstName,
                        x.LastName,
                        Products = x.ProductsSold.Select(p => new
                        {
                            p.Name,
                            p.Price
                        })
                    }).ToList();

                var xDoc = new XDocument(new XElement("users"));

                foreach (var u in users)
                {
                    var user = new XElement("user",
                        new XAttribute("first-name", u.FirstName ?? "[no first-name]"),
                        new XAttribute("last-name", u.LastName ?? "[no last-name]"),
                        new XElement("sold-products")
                    );

                    foreach (var p in u.Products)
                    {
                        user.Element("sold-products").Add(new XElement("product", new XElement("name", p.Name),
                            new XElement("price", $"{p.Price:f2}")));
                    }

                    xDoc.Root.Add(user);
                }

                var xmlString = xDoc.ToString();

                File.WriteAllText("XmlResults/SuccessfullySoldProductsXml.xml", xmlString);
            }
        }

        private static void ProductsInRangeXml()
        {
            using (var db = new ProductsDbContext())
            {
                var products = db.Products
                    .Where(x => x.Price >= 1000 && x.Price <= 2000 && x.BuyerId != null)
                    .Select(x => new
                    {
                        x.Name,
                        x.Price,
                        BuyerName = $"{x.Buyer.FirstName} {x.Buyer.LastName}"
                    }).ToList();


                var xmlDoc = new XDocument(new XElement("products"));

                foreach (var p in products)
                {
                    xmlDoc.Root.Add(new XAttribute("name", p.Name), new XAttribute("price", p.Price),
                        new XAttribute("buyer", p.BuyerName));
                }

                var xmlString = xmlDoc.ToString();

                File.WriteAllText("XmlResults/ProductsInRangeXml.xml", xmlString);
            }
        }

        private static void ImportXmlProducts()
        {
            var xmlString = File.ReadAllText(@"..\Products.Data\XmlData\products.xml");
            var xmlDocument = XDocument.Parse(xmlString);

            var elements = xmlDocument.Root.Elements();


            using (var db = new ProductsDbContext())
            {
                var categoryProducts = new List<CategoryProduct>();

                var userIds = db.Users.Select(x => x.Id).ToList();

                var categoryIds = db.Categories.Select(x => x.Id).ToList();

                var random = new Random();

                foreach (var e in elements)
                {
                    var name = e.Element("name").Value;
                    var price = decimal.Parse(e.Element("price").Value);

                    var sellerIndex = random.Next(0, userIds.Count);
                    var sellerId = userIds[sellerIndex];

                    var product = new Product()
                    {
                        Name = name,
                        Price = price,
                        SellerId = sellerId
                    };

                    var categoryIndex = random.Next(0, categoryIds.Count);
                    var categoryId = categoryIds[categoryIndex];

                    var categoryProduct = new CategoryProduct()
                    {
                        Product = product,
                        CategoryId = categoryId
                    };

                    categoryProducts.Add(categoryProduct);
                }

                db.CategoryProducts.AddRange(categoryProducts);
                db.SaveChanges();

                Console.WriteLine($"{categoryProducts.Count} category products were successfully added !");
            }
        }

        private static void ImportXmlCategories()
        {
            var xmlString = File.ReadAllText(@"..\Products.Data\XmlData\categories.xml");
            var xmlDocument = XDocument.Parse(xmlString);

            var elements = xmlDocument.Root.Elements();

            var categories = new List<Category>();

            foreach (var e in elements)
            {
                categories.Add(new Category
                {
                    Name = e.Element("name").Value
                });
            }

            using (var db = new ProductsDbContext())
            {
                db.Categories.AddRange(categories);
                db.SaveChanges();
            }

            Console.WriteLine($"{categories.Count} categories were successfully added !");
        }

        private static void ImportXmlUsers()
        {
            var xmlString = File.ReadAllText(@"..\Products.Data\XmlData\users.xml");
            var xmlDocument = XDocument.Parse(xmlString);

            var elements = xmlDocument.Root.Elements();

            var users = new List<User>();

            foreach (var e in elements)
            {
                var firstName = e.Attribute("firstName")?.Value;
                var lastName = e.Attribute("lastName")?.Value;
                int? age = null;

                if (e.Attribute("age") != null)
                {
                    age = int.Parse(e.Attribute("age").Value);
                }


                users.Add(new User()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Age = age
                });
            }
            using (var db = new ProductsDbContext())
            {
                db.Users.AddRange(users);
                db.SaveChanges();
            }

            Console.WriteLine($"{users.Count} users were successfully added !");
        }


        //                         JSON SECTION


        private static void UsersAndProducts()
        {
            using (var db = new ProductsDbContext())
            {
                var users = db.Users
                    .Where(x => x.ProductsSold.Count >= 1)
                    .OrderByDescending(x => x.ProductsSold.Count)
                    .ThenBy(x => x.LastName)
                    .Select(x => new
                    {
                        x.FirstName,
                        x.LastName,
                        x.Age,
                        SoldProducts = new
                        {
                            x.ProductsSold.Count,
                            Products = x.ProductsSold.Select(p => new
                            {
                                p.Name,
                                p.Price
                            })
                        }
                    }).ToList();

                var usersToSerialize = new
                {
                    UsersCount = users.Count,
                    users
                };

                var jsonString = JsonConvert.SerializeObject(usersToSerialize, Formatting.Indented,
                    new JsonSerializerSettings()
                    {
                        DefaultValueHandling = DefaultValueHandling.Include
                    });

                File.WriteAllText("JsonResults/UsersAndProducts.json", jsonString);
            }
        }

        private static void CategoriesByProductsCount()
        {
            using (var db = new ProductsDbContext())
            {
                var categories = db.Categories
                    .OrderBy(x => x.Name)
                    .Select(x => new
                    {
                        x.Name,
                        ProductCount = x.CategoryProducts.Select(p => p.Product).Count(),
                        AveragePrice = x.CategoryProducts.Select(a => a.Product.Price).Average(),
                        TotalRevenue = x.CategoryProducts.Select(t => t.Product.Price).Sum()
                    }).ToList();

                var jsonString = JsonConvert.SerializeObject(categories, Formatting.Indented,
                    new JsonSerializerSettings()
                    {
                        DefaultValueHandling = DefaultValueHandling.Ignore
                    });

                File.WriteAllText("JsonResults/CategoriesByProductsCount.json", jsonString);
            }
        }

        private static void SuccessfullySoldProducts()
        {
            using (var db = new ProductsDbContext())
            {
                var users = db.Users
                    .Where(x => x.ProductsSold.Any(e => e.BuyerId != null))
                    .OrderBy(x => x.LastName)
                    .ThenBy(x => x.FirstName)
                    .Select(x => new
                    {
                        x.FirstName,
                        x.LastName,
                        SoldProducts = x.ProductsSold.Select(p => new
                        {
                            p.Name,
                            p.Price,
                            BuyerFirstName = p.Buyer.FirstName,
                            BuyerLastName = p.Buyer.LastName
                        })
                    }).ToList();

                var jsonString = JsonConvert.SerializeObject(users, Formatting.Indented, new JsonSerializerSettings()
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore
                });

                File.WriteAllText("JsonResults/SuccessfullySoldProducts.json", jsonString);
            }
        }

        private static void ProductsInRange()
        {
            using (var db = new ProductsDbContext())
            {
                var products = db.Products
                    .Where(x => x.Price >= 500 && x.Price <= 1000)
                    .OrderBy(x => x.Price)
                    .Select(x => new
                    {
                        x.Name,
                        x.Price,
                        Seller = $"{x.Seller.FirstName} {x.Seller.LastName}"
                    })
                    .ToList();

                var jsonString = JsonConvert.SerializeObject(products, Formatting.Indented);

                File.WriteAllText("JsonResults/PriceInRange.json", jsonString);
            }
        }

        private static void SetCategories()
        {
            using (var db = new ProductsDbContext())
            {
                var products = db.Products.Select(x => x.Id).ToList();

                var categories = db.Categories.Select(x => x.Id).ToList();

                var categoryProducts = new List<CategoryProduct>();

                foreach (var product in products.OrderBy(x => x))
                {
                    var forRandom = new Random();
                    for (var i = 0; i < forRandom.Next(1, 5); i++)
                    {
                        var random = new Random();

                        var categoryProduct = new CategoryProduct()
                        {
                            ProductId = product,
                            CategoryId = categories[random.Next(0, categories.Count)]
                        };

                        while (categoryProducts.Any(e =>
                            e.CategoryId == categoryProduct.CategoryId && e.ProductId == product))
                        {
                            random = new Random();
                            categoryProduct.CategoryId = categories[random.Next(0, categories.Count)];
                        }

                        categoryProducts.Add(categoryProduct);
                    }
                }

                db.CategoryProducts.AddRange(categoryProducts);
                db.SaveChanges();

                Console.WriteLine($"{categoryProducts.Count} category products were added to the database !");
            }
        }

        private static void ImportJsonProducts()
        {
            var products = ImportJson<Product>(@"..\Products.Data\JsonData\products.json");

            var random = new Random();

            using (var db = new ProductsDbContext())
            {
                var users = db.Users.Select(x => x.Id).ToList();

                foreach (var product in products)
                {
                    var sellerId = users[random.Next(0, users.Count)];

                    int? buyerId = sellerId;
                    while (buyerId == sellerId)
                    {
                        buyerId = users[random.Next(0, users.Count)];
                    }

                    if (buyerId - sellerId < 5 && buyerId - sellerId > 0)
                    {
                        buyerId = null;
                    }
                    product.SellerId = sellerId;
                    product.BuyerId = buyerId;
                }

                db.Products.AddRange(products);
                db.SaveChanges();
            }

            Console.WriteLine($"{products.Length} products were added to the database !");
        }

        private static void ImportJsonCategories()
        {
            var categories = ImportJson<Category>(@"..\Products.Data\JsonData\categories.json");

            using (var db = new ProductsDbContext())
            {
                db.Categories.AddRange(categories);
                db.SaveChanges();
            }

            Console.WriteLine($"{categories.Length} categories were added to the database !");
        }

        private static void ImportJsonUsers()
        {
            var users = ImportJson<User>(@"..\Products.Data\JsonData\users.json");

            using (var db = new ProductsDbContext())
            {
                db.Users.AddRange(users);
                db.SaveChanges();
            }

            Console.WriteLine($"{users.Length} users were added to the database !");
        }

        private static T[] ImportJson<T>(string path)
        {
            var stringJson = File.ReadAllText(path);

            var objects = JsonConvert.DeserializeObject<T[]>(stringJson);

            return objects;
        }
    }
}