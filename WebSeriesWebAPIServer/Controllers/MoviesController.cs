using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebSeriesDataAccessLayer;

namespace WebSeriesWebAPIServer.Controllers
{
    public class MoviesController : ApiController
    {
        public IHttpActionResult Get()
        {
            try
            {
                using (WebSeriesDBEntities dbcontext = new WebSeriesDBEntities())
                {
                    return Ok(dbcontext.Movies.ToList());
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error : " + ex);
            }
        }

        public IHttpActionResult Get(int id)
        {
            try
            {
                using (WebSeriesDBEntities dbcontext = new WebSeriesDBEntities())
                {
                    return Ok(dbcontext.Movies.FirstOrDefault(m => m.id == id));
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error : " + ex);
            }
        }

        public IHttpActionResult Post()
        {
            var request = HttpContext.Current.Request;
            try
            {
                if (request.Files.Count == 2)
                {
                    //imagefile
                    var imageurl = request.Files["imageurl"];
                    var uploadFolder = HttpContext.Current.Server.MapPath("~/uploads");

                    String uniqueImageFileName = (Guid.NewGuid().ToString() + imageurl.FileName);
                    String imagefilePath = uploadFolder + "/" + uniqueImageFileName;
                    imageurl.SaveAs(imagefilePath);

                    //video file
                    var videourl = request.Files["videourl"];

                    String uniquevideoFileName = (Guid.NewGuid().ToString() + videourl.FileName);
                    String videofilePath = uploadFolder + "/" + uniquevideoFileName;
                    videourl.SaveAs(videofilePath);

                    using (WebSeriesDBEntities dbcontext = new WebSeriesDBEntities())
                    {
                        Movie movie = new Movie();
                        movie.name = request["name"];
                        movie.language = request["language"];
                        movie.categories = request["categories"];
                        //movie.duration = request["duration"];
                        movie.releaseddate = request["releaseddate"];
                        movie.story = request["story"];
                        movie.forkid = request["forkid"] == "1";
                        movie.views = 0;
                        movie.imageurl = "/uploads/" + uniqueImageFileName;
                        movie.videourl = "/uploads/" + uniquevideoFileName;

                        dbcontext.Movies.Add(movie);
                        dbcontext.SaveChanges();
                        return Ok(movie);
                    }
                }
                else
                {
                    return BadRequest("Please upload both image and video files");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error : " + ex);
            }
        }

      
        public IHttpActionResult Delete(int id)
        {
            try
            {
                using (WebSeriesDBEntities dbcontext = new WebSeriesDBEntities())
                {
                    Movie existing = dbcontext.Movies.FirstOrDefault(m => m.id == id);
                    if (existing == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        dbcontext.Movies.Remove(existing);
                        dbcontext.SaveChanges();
                        return Ok(existing);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error : " + ex);
            }
        }
    }
}
