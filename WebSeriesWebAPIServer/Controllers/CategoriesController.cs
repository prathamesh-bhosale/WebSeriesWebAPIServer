using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using WebSeriesDataAccessLayer;

namespace WebSeriesWebAPIServer.Controllers
{
    public class CategoriesController : ApiController
    {
        public IHttpActionResult Get()
        {
            try
            {
                using(WebSeriesDBEntities dbcontext = new WebSeriesDBEntities())
                {
                    return Ok(dbcontext.Categories.ToList());
                }
            }
            catch(Exception ex)
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
                    return Ok(dbcontext.Categories.FirstOrDefault(c => c.id == id));
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error : " + ex);
            }
        }

        public IHttpActionResult Post(Category category)
        {
            try
            {
                using (WebSeriesDBEntities dbcontext = new WebSeriesDBEntities())
                {
                    dbcontext.Categories.Add(category);
                    dbcontext.SaveChanges();
                    return Ok(category);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error : " + ex);
            }
        }

        public IHttpActionResult Put(int id, Category category)
        {
            try
            {
                using (WebSeriesDBEntities dbcontext = new WebSeriesDBEntities())
                {
                    Category existing = dbcontext.Categories.FirstOrDefault(c => c.id == id);
                    if (existing == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        existing.name = category.name;
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
                    Category existing = dbcontext.Categories.FirstOrDefault(c => c.id == id);
                    if (existing == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        dbcontext.Categories.Remove(existing);
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
