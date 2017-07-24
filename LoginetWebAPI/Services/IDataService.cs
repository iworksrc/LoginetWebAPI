namespace LoginetWebAPI.Services
{
    /// <summary>
    /// Методы необходимые для реализации внедряемой через <b>DI</b> службы получения даных.
    /// </summary>
    public interface IDataService
    {
        string getAlbum(int id);
        string getAlbumsByUserId(int id);
        string getAllAlbums();
        string getAllUsers();
        string getUser(int id);
    }
}