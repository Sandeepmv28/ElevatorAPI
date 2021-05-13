using ElevatorAPI.Models;
using ElevatorAPI.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElevatorAPI.Controllers
{
    [Route("api/elevator")]
    [ApiController]
    public class ElevatorController : ControllerBase
    {
        private readonly IElevatorService _elevatorService;
        private readonly ILogger<ElevatorController> _logger;
        public ElevatorController(ILogger<ElevatorController> logger, IElevatorService elevatorService)
        {
            _logger = logger;
            _elevatorService = elevatorService;
        }

        [HttpGet("request-to-floor/{floorNumber}")]
        public IActionResult GetRequestCurrentFloor(int floorNumber)
        {
            var result = new ResultModel { IsSuccess = true };
            try
            {
                var elevatorResult = _elevatorService.FloorPress(floorNumber);
                result.Data = elevatorResult;
                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}, StachTrace: {ex.StackTrace}");
                result.IsSuccess = false;
                result.Message = "Error:";
                return StatusCode(500, result);
            }           
        }

        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            var result = new ResultModel { IsSuccess = true };
            try
            {
                var elevatorResult = _elevatorService.GetStatus();
                result.Data = elevatorResult;
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}, StachTrace: {ex.StackTrace}");
                result.IsSuccess = false;
                result.Message = "Error:";
                return StatusCode(500, result);
            }
        }

        [HttpGet("next-floor")]
        public IActionResult GetNextFloor()
        {
            var result = new ResultModel { IsSuccess = true };
            try
            {
                var elevatorResult = _elevatorService.GetNextFloor();
                result.Data = elevatorResult;
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}, StachTrace: {ex.StackTrace}");
                result.IsSuccess = false;
                result.Message = "Error:";
                return StatusCode(500, result);
            }
        }

    }
}
