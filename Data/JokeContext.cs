namespace server.Data
{
    public class JokeContext : DbContext
    {

        public JokeContext(DbContextOptions<JokeContext> options) : base(options)
        { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql("Host=dpg-ch791qrhp8u9bo5spu7g-a.frankfurt-postgres.render.com;Port=5432;Database=jokes_7d9j;Username=costinelul;Password=qGOp5MaI9aUu3VL7KHO8pmPwEKzpRWXV;SSL Mode=Require;Trust Server Certificate=true;");
        }
        public DbSet<Joke> Jokes { get; set; }

    }
}
