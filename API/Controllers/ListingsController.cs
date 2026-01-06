using Api.Infrastructure.Jwt;
using Api.Infrastructure.Services.Listings;
using Api.Shared.DTOs.Listings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingsController : ControllerBase
    {
        private readonly IListingsServices _listingsServices;
        public ListingsController(IListingsServices listingsServices)
        {
            _listingsServices = listingsServices;
        }
  
        /// <summary>
        /// Method to create a new listing
        /// </summary>
        /// <param name="listingDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateListing([FromBody] AddListingDTO listingDto)
        {
            var IdUser = Jwt_Helpers.GetIdUserByToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));

            //Verifico que el usuario esté autenticado

            if (string.IsNullOrEmpty(IdUser))
            {
                return Unauthorized("User is not authenticated.");
            }
            var createdListing = await _listingsServices.CreateListingAsync(IdUser, listingDto);
            return CreatedAtAction(nameof(CreateListing), new { id = createdListing.Id }, createdListing);
        }


       /// <summary>
        /// Method to get all listings
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllListings()
        {
            var IdUser = Jwt_Helpers.GetIdUserByToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));

            //Verifico que el usuario esté autenticado
 
            if (string.IsNullOrEmpty(IdUser))
            {
                return Unauthorized("User is not authenticated.");
            }

            var listings = await _listingsServices.GetAllListingsAsync();
            if (listings == null || listings.Count == 0)
            {
                return NotFound("No listings found.");
            }
            return Ok(listings);
        }


        /// <summary>
        /// Method to get listing by id
        /// </summary>
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetListingById(int id)
        {
            var IdUser = Jwt_Helpers.GetIdUserByToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));

            //Verifico que el usuario esté autenticado

            if (string.IsNullOrEmpty(IdUser))
            {
                return Unauthorized("User is not authenticated.");
            }
            var listing = await _listingsServices.GetListingByIdAsync(id);
            if (listing == null)
            {
                return NotFound();
            }
            return Ok(listing);
        }

        /// <summary>
        /// Method to update a listing
        /// </summary>

        [HttpPut]
        public async Task<IActionResult> UpdateListing( [FromBody] ListingDTO listingDto)
        {
            var IdUser = Jwt_Helpers.GetIdUserByToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));

            //Verifico que el usuario esté autenticado

            if (string.IsNullOrEmpty(IdUser))
            {
                return Unauthorized("User is not authenticated.");
            }
            // Verifico que el ID del listing en la URL coincida con el ID en el DTO
            var id = _listingsServices.GetListingByIdAsync(listingDto.Id);
            if (id == null)
            {
                return BadRequest("Listing ID mismatch.");
            }
            var updatedListing = await _listingsServices.UpdateListingAsync(IdUser, listingDto);
            if (updatedListing == null)
            {
                return NotFound();
            }
            return Ok(updatedListing);
        }

    }
}
