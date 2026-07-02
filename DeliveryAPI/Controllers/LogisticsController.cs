using DeliveryAPI.Models.Response;
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


        [HttpGet("couriers")]
        public IActionResult GetCouriers()
        {
            return Ok(_service.GetCouriers());
        }

        [HttpGet("delivery-lifecycle")]
        public List<DeliveryLifecycleModel> GetDeliveryLifecycles()
        {
            return _service.GetDeliveryLifecycles();
        }
        [HttpPost("delivery-lifecycle")]
        public IActionResult SaveDeliveryLifecycle([FromBody] DeliveryLifecycleModel model)
        {
            var result = _service.SaveDeliveryLifecycle(model);

            if (!result)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Status Code or Status Name already exists."
                });
            }

            string message;

            if (model.LifecycleId == 0)
                message = "Delivery Lifecycle created successfully.";
            else if (!model.IsActive)
                message = "Delivery Lifecycle deleted successfully.";
            else
                message = "Delivery Lifecycle updated successfully.";

            return Ok(new
            {
                Success = true,
                Message = message
            });
        }


        [HttpGet("company-user-lifecycle-access")]
        public IActionResult GetCompanyUserLifecycleAccess()
        {
            return Ok(_service.GetCompanyUserLifecycleAccess());
        }

        [HttpPost("company-user-lifecycle-access")]
        public IActionResult SaveCompanyUserLifecycleAccess([FromBody] CompanyUserLifecycleAccessModel model)
        {
            bool result = _service.SaveCompanyUserLifecycleAccess(model);

            if (result)
            {
                return Ok(new
                {
                    success = true,
                    message = model.MappingId == 0
                        ? "Company User Lifecycle Access created successfully."
                        : model.IsActive
                            ? "Company User Lifecycle Access updated successfully."
                            : "Company User Lifecycle Access deleted successfully."
                });
            }

            return BadRequest(new
            {
                success = false,
                message = "Duplicate Company, User, Role and Lifecycle combination already exists."
            });
        }
        [HttpGet("company-user-role")]
        public IActionResult GetCompanyUserRole(int userId, int companyId = 0)
        {
            return Ok(_service.GetCompanyUserRole(userId, companyId));
        }
        [HttpGet("role-lifecycle-mapping")]
        public IActionResult GetRoleLifecycleMappings()
        {
            return Ok(_service.GetRoleLifecycleMappings());
        }

        [HttpPost("role-lifecycle-mapping")]
        public IActionResult SaveRoleLifecycleMapping([FromBody] RoleLifecycleMappingModel model)
        {
            var message = _service.SaveRoleLifecycleMapping(model);

            if (message.Contains("already"))
            {
                return BadRequest(new
                {
                    success = false,
                    message = message
                });
            }

            return Ok(new
            {
                success = true,
                message = message
            });
        }


        [HttpGet("transfer-stock-log-detail")]
        public IActionResult GetTransferStockLogDetail(
    int companyId,
    string? locationIds,
    DateTime? fromDate,
    DateTime? toDate,
    string? locationTypeIds)
        {
            var result = _service.GetTransferStockLogDetailDelivery(
                companyId,
                locationIds,
                fromDate,
                toDate,
                locationTypeIds);

            return Ok(result);
        }
    }
}