using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;


[ApiController]
[Route("api/cities/{cityId}/pointsofinterest")] //this is a child of city controller
public class PointsOfInterestController : ControllerBase
{
    [HttpGet]
    public IActionResult GetPointsOfInterest(int cityId)
    {
        var city = CitiesDataStore.Current.Cities
            .FirstOrDefault(c => c.Id == cityId);

        if (city == null)
        {
            return NotFound();
        }

        return Ok(city.PointsOfInterest);
    }

    [HttpGet("{id}")]
    public IActionResult GetPointOfInterest(int cityId, int id)
    {
        var city = CitiesDataStore.Current.Cities
            .FirstOrDefault(c => c.Id == cityId);

        if (city == null)
        {
            return NotFound();
        }

        // find point of interest
        var pointOfInterest = city.PointsOfInterest
            .FirstOrDefault(c => c.Id == id);

        if (pointOfInterest == null)
        {
            return NotFound();
        }

        return Ok(pointOfInterest);
    }
}