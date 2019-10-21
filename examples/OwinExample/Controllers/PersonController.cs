using OwinExample.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace OwinExample.Controllers
{
    [RoutePrefix("api/people")]
    public class PersonController : ApiController
    {
        IPersonService service;

        public PersonController(IPersonService service)
        {
            this.service = service;
        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(service.GetAll());
        }

        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            return Ok(service.GetById(id));
        }

    }
}