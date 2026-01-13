using Api.Infrastructure.Services.ListingPhones;
using Api.Shared.DTOs.ListingPhones;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ListingPhonesController : ControllerBase
    {
        private readonly IListingPhonesServices _listingPhonesServices;

        public ListingPhonesController(IListingPhonesServices listingPhonesServices)
        {
            _listingPhonesServices = listingPhonesServices;
        }

        // ===============================
        // GET ALL
        // ===============================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _listingPhonesServices.GetAllAsync();
            return Ok(result);
        }

        // ===============================
        // GET BY ID
        // ===============================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _listingPhonesServices.GetByIdAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // ===============================
        // CREATE
        // ===============================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddListingPhonesDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _listingPhonesServices.CreateAsync(userId!, dto);
            return Ok(result);
        }

        // ===============================
        // UPDATE
        // ===============================
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateListingPhonesDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _listingPhonesServices.UpdateAsync(userId!, dto);
            return Ok(result);
        }
    }
}
