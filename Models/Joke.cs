namespace server.Model
{
    public class Joke
    {
        public int Id { get; set; }
        public string? Question { get; set; }
        public string? Punchline { get; set; }
        public int PublisherId { get; set; }
        public int Laughs { get; set; }
        public DateTime Date { get; set; }

        public Joke()
        {
            Date = DateTime.UtcNow;
            Laughs = 0;
        }
    }
}