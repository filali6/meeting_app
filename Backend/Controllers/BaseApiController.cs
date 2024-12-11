using System;
using Backend.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;
[Route("api/[controller]")]
[ApiController]
[ServiceFilter(typeof(LastActive))]
public class BaseApiController:ControllerBase
{

}
