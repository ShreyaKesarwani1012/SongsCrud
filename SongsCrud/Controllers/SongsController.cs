using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using SongsCrud.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SongsCrud.Controllers
{
    public class SongsController : Controller
    {
       //static List<Song> list = new List<Song>();

        // GET: SongsController = display all recored at once!
        public ActionResult Index()
        {
            List<Song> list = GetAllSongs();
            return View(list);
        }


        static List<Song> GetAllSongs()
        {
            List<Song> list = new List<Song>();

            SqlConnection cn = new SqlConnection();
            try
            {
                cn.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Songs;Integrated Security=True";
                cn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "select * from SongData";

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                   list.Add(new Song((int)dr[0], (string)dr[1], (string)dr[2]));
                }
                dr.NextResult();

                dr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                cn.Close();
            }

            return list;
        }

        // GET: SongsController/Details/5 - gat only particular song!
        public ActionResult Details(int id)
        {
           
            //getting from view and passing to model

            Song s = getSingleSong(id);

            return View(s);
        }
        public Song getSingleSong(int id)
        {
            Song s = new Song();

            SqlConnection cn = new SqlConnection();
            try
            {
                cn.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Songs;Integrated Security=True";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "SELECT SongId,SongName,SongAlbum FROM SongData WHERE SongId = @id";

                cmd.Parameters.AddWithValue("@id",id);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    s.SongId = (int)dr[0];
                    s.SongName = (string)dr[1];
                    s.SongAlbum = (string)dr[2];
                }
                dr.Close ();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                cn.Close();
            }
            return s;
        }

        // GET: SongsController/Create
        [HttpGet]
        public ActionResult Create()
        {

            return View();
        }

        // POST: SongsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]  //==========??????????

        public ActionResult Create(Song s)
        {
            try
            {
                InsertSongIntoDatabase(s);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private void InsertSongIntoDatabase(Song song)
        {
            Console.WriteLine(song.SongName);
            SqlConnection cn = new SqlConnection();

            try {

                cn.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Songs;Integrated Security=True";
                cn.Open();
                SqlCommand cmdInsert = new SqlCommand();
                cmdInsert.Connection = cn;
                cmdInsert.CommandType = System.Data.CommandType.Text;
                cmdInsert.CommandText = "insert into SongData values(@SongId, @SongName, @SongAlbum)";

                cmdInsert.Parameters.AddWithValue("@SongId", song.SongId);
                cmdInsert.Parameters.AddWithValue("@SongName", song.SongName);
                cmdInsert.Parameters.AddWithValue("@SongAlbum", song.SongAlbum);
               
                cmdInsert.ExecuteNonQuery();
                Console.WriteLine("wokay");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally 
            { cn.Close();
            }  
        }
        
    

        // GET: SongsController/Edit/5
        public ActionResult Edit(int id, string name, string album)
        {
            Song s = EditData(id , name , album);
           
            return View(s);
           
        }

        public Song EditData(int id, string name , string album)
        
        {
            Song s = new Song();
            SqlConnection cn = new SqlConnection();
            try
            {
                cn.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Songs;Integrated Security=True";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "UPDATE SongData SET SongName = @name , SongAlbum = @album Where SongId = @id";

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@album", album);
                cn.Open();

                int rowaffected = cmd.ExecuteNonQuery();
                cn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                cn.Close();
            }
       


            return s;
        }

        // POST: SongsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Song song)
        {
            try
            {
               
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }



        // GET: SongsController/Delete/5
        public ActionResult Delete(int id)
        {
            Song s = new Song();
            s.SongId = id;
            s.SongName = "ON";
            s.SongAlbum = "Love Yourself";
            return View(s);
            //return View();
        }

        // POST: SongsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id ,Song s)
        {
            try
            {

                //Song s = DeleteData(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public void DeleteData(int id)
        
        {
            Console.WriteLine("hi");
        }
    }
}
