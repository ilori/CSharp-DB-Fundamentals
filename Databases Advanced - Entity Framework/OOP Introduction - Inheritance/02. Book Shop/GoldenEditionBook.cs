public class GoldenEditionBook : Book
{
    public GoldenEditionBook(string title, string author, decimal price) : base(title, author, price)
    {
        this.Price *= 1.3m;
    }

    public override string ToString()
    {
        return
            $"Type: {typeof(GoldenEditionBook)}\nTitle: {this.Title}\nAuthor: {this.Author}\nPrice: {this.Price:f2}";
    }
}