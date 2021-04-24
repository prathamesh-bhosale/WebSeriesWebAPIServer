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
    public class SeriesController : ApiController
    {

        public IHttpActionResult Get()
        {
            try
            {
                using (WebSeriesDBEntities dbcontext = new WebSeriesDBEntities())
                {
                    var seriesList = dbcontext.Series.Select(s => new
                    {
                        s.id,
                        s.name,
                        s.categories,
                        s.language,
                        s.story,
                        s.forkid,
                        s.videourl,
                        s.imageurl,
                        count = dbcontext.Episodes.Where(e => e.seriesid == s.id).ToList().Count()
                    }).ToList();
                    return Ok(seriesList);
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
                    return Ok(dbcontext.Series.FirstOrDefault(s => s.id == id));
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
                        Series series = new Series();
                        series.name = request["name"];
                        series.language = request["language"];
                        series.categories = request["categories"];

                        series.story = request["story"];
                        series.forkid = request["forkid"] == "1";                  
                        series.imageurl = "/uploads/" + uniqueImageFileName;
                        series.videourl = "/uploads/" + uniquevideoFileName;

                        dbcontext.Series.Add(series);
                        dbcontext.SaveChanges();
                        return Ok(series);
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
                    Series existing = dbcontext.Series.FirstOrDefault(s => s.id == id);
                    if (existing == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        dbcontext.Series.Remove(existing);
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
