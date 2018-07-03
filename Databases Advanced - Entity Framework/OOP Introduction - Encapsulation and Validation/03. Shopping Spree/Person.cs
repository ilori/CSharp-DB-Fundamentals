using System;
using System.Collections.Generic;
using System.Linq;

public class Person
{
    private string name;
    private decimal money;
    private List<Product> products;

    public Person(string name, decimal money)
    {
        this.Name = name;
        this.Money = money;
        this.Products = new List<Product>();
    }

    public string Name
    {
        get { return this.name; }
        set
        {
            if (value == "")
            {
                throw new ArgumentException("Name cannot be empty");
            }
            this.name = value;
        }
    }

    public decimal Money
    {
        get { return this.money; }
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("Money cannot be negative");
            }
            this.money = value;
        }
    }

    public List<Product> Products
    {
        get { return this.products; }
        set { this.products = value; }
    }


    public void BuyProducts(Product product)
    {
        if (this.Money >= product.Price)
        {
            this.Money -= product.Price;
            this.Products.Add(product);
        }
        else
        {
            throw new Exception($"{this.Name} can't afford {product.Name}");
        }
    }
    
    public override string ToString()
    {
        if (this.Products.Count > 0)
        {
            var purchases = this.Products.Select(p => p.Name).ToArray();
            return $"{this.Name} - {string.Join(", ", purchases)}";
        }
        else
        {
            return $"{this.Name} - Nothing bought";
        }
    }

}