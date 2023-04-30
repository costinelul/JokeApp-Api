namespace server.Services.JokeService
{

    public interface IJokeService
    {
        Task<Joke> CreateJoke(int publisherId, JokeDto dto);
        Task<List<Joke>> GetAllJokes();
        Task<Joke?> UpdateJoke(int id, JokeDto dto);
        Task<Joke?> DeleteJoke(int id);
        Task<int> Laugh(int jokeId, int userId);
        Task<bool> HasLaughed(int jokeId, int userId);
    }
}