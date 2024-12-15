using System;
using Backend.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;
[Route("api/[controller]")]
[ApiController]
[ServiceFilter(typeof(LastActive))]
//[Authorize]
public class BaseApiController:ControllerBase
{

}
