using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebSeriesDataAccessLayer;

namespace WebSeriesWebAPIServer.Controllers
{
    public class LanguagesController : ApiController
    {
        public IHttpActionResult Get()
        {
            try
            {
                using (WebSeriesDBEntities dbcontext = new WebSeriesDBEntities())
                {
                    return Ok(dbcontext.Languages.ToList());
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
                    return Ok(dbcontext.Languages.FirstOrDefault(l => l.id == id));
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error : " + ex);
            }
        }

        public IHttpActionResult Post(Language language)
        {
            try
            {
                using (WebSeriesDBEntities dbcontext = new WebSeriesDBEntities())
                {
                    dbcontext.Languages.Add(language);
                    dbcontext.SaveChanges();
                    return Ok(language);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error : " + ex);
            }
        }

        public IHttpActionResult Put(int id, Language language)
        {
            try
            {
                using (WebSeriesDBEntities dbcontext = new WebSeriesDBEntities())
                {
                    Language existing = dbcontext.Languages.FirstOrDefault(l => l.id == id);
                    if (existing == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        existing.name = language.name;
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

        public IHttpActionResult Delete(int id)
        {
            try
            {
                using (WebSeriesDBEntities dbcontext = new WebSeriesDBEntities())
                {
                    Language existing = dbcontext.Languages.FirstOrDefault(l => l.id == id);
                    if (existing == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        dbcontext.Languages.Remove(existing);
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
