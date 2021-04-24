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
    public class EpisodesController : ApiController
    {

        [HttpGet]
        [Route("api/episodes/getepisodesbyseriesid")]
        public IHttpActionResult GetEpisodesBySeriesId(int seriesid)
        {
            try
            {
                using (WebSeriesDBEntities dbcontext = new WebSeriesDBEntities())
                {
                    return Ok(dbcontext.Episodes.Where(e => e.seriesid == seriesid).ToList());
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error : " + ex);
            }
        }

        [HttpGet]
        [Route("api/episodes/getepisodescountforseriesid")]
        public IHttpActionResult GetEpisodesCountForSeriesId(int seriesid)
        {
            try
            {
                using (WebSeriesDBEntities dbcontext = new WebSeriesDBEntities())
                {
                    return Ok(dbcontext.Episodes.Where(e => e.seriesid == seriesid).ToList().Count());
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
                    return Ok(dbcontext.Episodes.FirstOrDefault(e => e.id == id));
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
                        Episode episode = new Episode();
                        episode.name = request["name"];
                        episode.seriesid = Convert.ToInt32(request["seriesid"]);
                        episode.date = System.DateTime.Now.ToShortDateString();
                        episode.views = 0;
                        episode.likes = 0;
                        episode.dislikes = 0;
                        episode.imageurl = "/uploads/" + uniqueImageFileName;
                        episode.videourl = "/uploads/" + uniquevideoFileName;

                        dbcontext.Episodes.Add(episode);
                        dbcontext.SaveChanges();
                        return Ok(episode);
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
                    Episode existing = dbcontext.Episodes.FirstOrDefault(e => e.id == id);
                    if (existing == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        dbcontext.Episodes.Remove(existing);
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


        [HttpPut]
        [Route("api/episodes/incrviews")]
        public IHttpActionResult IncrViews(int id)
        {
            try
            {
                using (WebSeriesDBEntities dbcontext = new WebSeriesDBEntities())
                {
                    Episode existing = dbcontext.Episodes.FirstOrDefault(e => e.id == id);
                    if (existing == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        existing.views = existing.views + 1;
                        dbcontext.SaveChanges();
                        return Ok(existing.views);
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
