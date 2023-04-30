using server.Data;

namespace server.Services.JokeService;

public class JokeService : IJokeService
{
    private readonly JokeContext _jokeContext;
    private readonly UserJokeLaughContext _laughContext;
    public JokeService(JokeContext jokeContext, UserJokeLaughContext laughContext)
    {
        _jokeContext = jokeContext;
        _laughContext = laughContext;
    }
    public async Task<Joke> CreateJoke(int publisherId, JokeDto dto)
    {
        var newJoke = new Joke()
        {
            Question = dto.Question,
            Punchline = dto.Punchline,
            PublisherId = publisherId,
        };
        _jokeContext.Jokes.Add(newJoke);
        await _jokeContext.SaveChangesAsync();

        return newJoke;
    }
    public async Task<List<Joke>> GetAllJokes()
    {
        var jokes = await _jokeContext.Jokes.ToListAsync();
        return jokes;
    }

    public async Task<Joke?> DeleteJoke(int id)
    {
        var joke = await _jokeContext.Jokes.FindAsync(id);
        if (joke is null) return null;

        _jokeContext.Jokes.Remove(joke);
        await _jokeContext.SaveChangesAsync();

        return joke;
    }

    public async Task<Joke?> UpdateJoke(int id, JokeDto dto)
    {
        var joke = await _jokeContext.Jokes.FindAsync(id);
        if (joke is null) return null;

        joke.Question = dto.Question;
        joke.Punchline = dto.Punchline;

        await _jokeContext.SaveChangesAsync();

        return joke;
    }

    public async Task<int> Laugh(int jokeId, int userId)
    {
        var joke = await _jokeContext.Jokes.FindAsync(jokeId);
        if (joke is null) return -1;
        //// check if user laughed already
        var hasLaughed = await _laughContext.UserJokeLaugh.FirstOrDefaultAsync(e => e.UserId == userId && e.JokeId == jokeId);
        if (hasLaughed is null)
        {
            joke.Laughs += 1;
            var relation = new UserJokeLaugh()
            {
                UserId = userId,
                JokeId = jokeId,
            };
            await _laughContext.AddAsync(relation);
        }
        else
        {
            joke.Laughs -= 1;
            _laughContext.Remove(hasLaughed);
        }
        await _laughContext.SaveChangesAsync();
        await _jokeContext.SaveChangesAsync();
        return joke.Laughs;
    }
    public async Task<bool> HasLaughed(int jokeId, int userId)
    {
        var hasLaughed = await _laughContext.UserJokeLaugh.FirstOrDefaultAsync(e => e.UserId == userId && e.JokeId == jokeId);
        if (hasLaughed is null) return false;
        return true;
    }
}