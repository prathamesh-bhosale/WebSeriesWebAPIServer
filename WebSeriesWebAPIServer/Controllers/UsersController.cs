using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using WebSeriesDataAccessLayer;

namespace WebSeriesWebAPIServer.Controllers
{
    public class UsersController : ApiController
    {
        public IHttpActionResult Post(User user)
        {
            try
            {
                using (WebSeriesDBEntities dbcontext = new WebSeriesDBEntities())
                {
                    User existing = dbcontext.Users.FirstOrDefault(u => u.name == user.name);
                    if(existing != null)
                    {
                        return BadRequest("Username exist, please select another username.");
                    }
                    existing = dbcontext.Users.FirstOrDefault(u => u.emailid == user.emailid);
                    if(existing != null)
                    {
                        return BadRequest("Emailid exist, please select another emailid.");
                    }

                    dbcontext.Users.Add(user);
                    dbcontext.SaveChanges();
                    return Ok(user);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error : " + ex);
            }
        }

        [HttpPost]
        [Route("api/users/checklogin")]
        public IHttpActionResult CheckLogin(User user)
        {
            try
            {
                using (WebSeriesDBEntities dbcontext = new WebSeriesDBEntities())
                {
                    User existing = dbcontext.Users.FirstOrDefault(u => u.name == user.name);
                    if (existing == null)
                        return BadRequest("Coundn't find your Movify Account");
                    else
                    {
                        if(existing.password == user.password)
                        {
                            existing.password = null;
                            return Ok(existing);
                        }
                        else
                        {
                            return BadRequest("Wrong password. Try again or click Forgot password to reset it.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error : " + ex);
            }
        }

        [HttpPost]
        [Route("api/users/forget")]
        public IHttpActionResult ForgetPassword(User user)
        {
            try
            {
                using (WebSeriesDBEntities dbcontext = new WebSeriesDBEntities())
                {
                    User existing = dbcontext.Users.FirstOrDefault(u => u.emailid == user.emailid);
                    if (existing == null)
                        return BadRequest("Emailid does not exist");
                    else
                    {
                        string body = "Username = " + existing.name + " Password = " + existing.password;
                        MovifyLibrary.SendMail("Password Recovery", body, user.emailid);
                        return Ok("Login details are sent to registered emailid");
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