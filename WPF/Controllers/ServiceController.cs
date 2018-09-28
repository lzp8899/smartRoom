using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Web.Controllers
{
    /// <summary>
    /// ServiceController
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : BaseController
    {

        /// <summary>
        /// 通过构造函数注入
        /// </summary>
        public ServiceController()
        {
        }

        /// <summary>
        /// Gets the information.
        /// </summary>
        /// <returns>ActionResult&lt;ApiDisplayInfo&gt;.</returns>
        // GET api/values
        [HttpGet("allInfos")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiDisplayInfo))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(string))]
        public ActionResult<ApiDisplayInfo> GetInfos()
        {
            return Ok(MainApp.Instance.ApiDisplayInfo);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //// GET api/values
        //[HttpGet("ex")]
        //[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<string>))]
        //[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        //[ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(string))]
        //public ActionResult<IEnumerable<string>> Get()
        //{
        //    //return StatusCode(401, "xx");

        //    return Ok(manager.GetValues());
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //// GET api/values/5
        //[Authorize]
        //[HttpGet("{id}")]
        //public ActionResult<string> Get(int id)
        //{
        //    if (id <= 0)
        //    {
        //        return BadRequest("参数错误");
        //    }

        //    return Ok(new { Flag = true });
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //// POST api/values
        //[HttpPost]
        //public IActionResult Post([FromBody] string value)
        //{
        //    return Ok(new { Flag = true });
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public IActionResult Put(int id, [FromBody] string value)
        //{
        //    return Ok(new { Flag = true });
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //[Authorize]
        //public IActionResult Delete(int id)
        //{
        //    return Ok(new { Flag = false });
        //}
    }
}
