namespace app.MVVM.Model.Data.Repositories
{
    public interface IGenericSongRepo
    {
        List<int> Songs { get; set; }

        int Id { get; }

        string Name { get; }

        int Owner { get; }

        bool AddSong(int songId);

        bool RemoveSong(int songId);

        int GetSongsNumber();
    }

    public class GenericSongRepo : IGenericSongRepo
    {
        public List<int> Songs { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public int Owner { get; }

        public GenericSongRepo(int owner, int id, string name)
        {
            this.Owner = owner;
            this.Id = id;
            this.Name = name;
            this.Songs = [];
        }

        public bool AddSong(int songId)
        {
            if (this.Songs.Contains(songId))
            {
                return false;
            }

            this.Songs.Add(songId);
            return true;
        }

        public bool RemoveSong(int songId)
        {
            return this.Songs.Remove(songId);
        }

        public int GetSongsNumber()
        {
            return this.Songs.Count;
        }
    }
}