using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeBuy.Controllers.Base
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class MyBaseController : ControllerBase
    {
    }
}
