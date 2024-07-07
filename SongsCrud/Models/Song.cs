using System.Security.Policy;

namespace SongsCrud.Models
{
    public class Song
    {
       

        public int SongId { get; set; }
        public string SongName { get; set; }
        public string SongAlbum { get; set; }


        public Song(int v1, string v2, string v3)
        {
            this.SongId = v1;
            this.SongName = v2;
            this.SongAlbum = v3;
        }
        public Song()
        {
          
        }
    }
}
