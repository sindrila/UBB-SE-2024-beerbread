namespace app.MVVM.Model.Data.Utilities
{
    using System;
    using System.Collections.Generic;
    using app.MVVM.Model.Data.Repositories;
    using app.MVVM.Model.Domain;

    public class MockAnalyticsAPI
    {
        private List<int> posibleRecomendedSongs;

        public MockAnalyticsAPI(User user)
        {
            posibleRecomendedSongs = new List<int>();
            ISqlSongRepository sqlSongRepository = new SqlSongRepository();
            posibleRecomendedSongs = sqlSongRepository.GetAllSongIds();
        }

        public List<int> getRecomendedSongs(int nrRecomended)
        {
            // get 5 random songs
            List<int> result = new List<int>();
            Random random = new Random();
            for (int i = 0; i < nrRecomended && i < posibleRecomendedSongs.Count; i++)
            {
                result.Add(posibleRecomendedSongs[random.Next(posibleRecomendedSongs.Count)]);
            }

            result.Add(0);
            return result;
        }
    }
}
