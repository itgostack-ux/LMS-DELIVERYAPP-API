using DeliveryAPI.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogisticsController : ControllerBase
    {
        private readonly ILogisticsService _service;

        public LogisticsController(ILogisticsService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get Active Companies
        /// </summary>
        [HttpGet("companies")]
        public IActionResult GetCompanies()
        {
            var result = _service.GetCompanies();
            return Ok(result);
        }

        /// <summary>
        /// Get Location Types by Company
        /// </summary>
        [HttpGet("location-types/{companyId}")]
        public IActionResult GetLocationTypes(int companyId)
        {
            var result = _service.GetLocationTypes(companyId);
            return Ok(result);
        }

        /// <summary>
        /// Get Locations by Company and Location Type
        /// </summary>
        [HttpGet("locations/{companyId}/{locationTypeId}")]
        public IActionResult GetLocations(int companyId, int locationTypeId)
        {
            var result = _service.GetLocations(companyId, locationTypeId);
            return Ok(result);
        }


        [HttpGet("roles")]
        public IActionResult GetRoles()
        {
            return Ok(_service.GetRoles());
        }

        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            return Ok(_service.GetUsers());
        }
    }
}