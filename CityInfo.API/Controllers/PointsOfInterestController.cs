using CityInfo.API.Models;
using Microsoft.AspNetCore.JsonPatch;
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

    [HttpGet("{id}", Name = "GetPointOfInterest")]
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

    [HttpPost]
    public IActionResult CreatePointOfInterest(int cityId,
            [FromBody] PointOfInterestForCreationDto pointOfInterest)
    {
        if (pointOfInterest.Description == pointOfInterest.Name)
        {
            ModelState.AddModelError(
                "Description",
                "The provided description should be different from the name.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
        if (city == null)
        {
            return NotFound();
        }

        // demo purposes - to be improved
        var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(
                         c => c.PointsOfInterest).Max(p => p.Id);

        var finalPointOfInterest = new PointOfInterestDto()
        {
            Id = ++maxPointOfInterestId,
            Name = pointOfInterest.Name,
            Description = pointOfInterest.Description
        };

        city.PointsOfInterest.Add(finalPointOfInterest);

        return CreatedAtRoute("GetPointOfInterest", new { cityId, id = finalPointOfInterest.Id }, finalPointOfInterest);
    }


    [HttpPut("{id}")]
    public IActionResult UpdatePointOfInterest(int cityId, int id,
    [FromBody] PointOfInterestForUpdateDto pointOfInterest)
    {
        if (pointOfInterest.Description == pointOfInterest.Name)
        {
            ModelState.AddModelError(
                "Description",
                "The provided description should be different from the name.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
        if (city == null)
        {
            return NotFound();
        }

        var pointOfInterestFromStore = city.PointsOfInterest
            .FirstOrDefault(p => p.Id == id);
        if (pointOfInterest == null)
        {
            return NotFound();
        }

        pointOfInterestFromStore.Name = pointOfInterest.Name;
        pointOfInterestFromStore.Description = pointOfInterest.Description;

        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id,
        [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
    {
        var city = CitiesDataStore.Current.Cities
            .FirstOrDefault(c => c.Id == cityId);
        if (city == null)
        {
            return NotFound();
        }

        var pointOfInterestFromStore = city.PointsOfInterest
            .FirstOrDefault(c => c.Id == id);
        if (pointOfInterestFromStore == null)
        {
            return NotFound();
        }

        var pointOfInterestToPatch =
               new PointOfInterestForUpdateDto()
               {
                   Name = pointOfInterestFromStore.Name,
                   Description = pointOfInterestFromStore.Description
               };

        patchDoc.ApplyTo(pointOfInterestToPatch);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (pointOfInterestToPatch.Description == pointOfInterestToPatch.Name)
        {
            ModelState.AddModelError(
                "Description",
                "The provided description should be different from the name.");
        }

        if (!TryValidateModel(pointOfInterestToPatch))
        {
            return BadRequest(ModelState);
        }

        pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
        pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

        return NoContent();
    } /// doesnt work in .net 6 :(


    [HttpDelete("{id}")]
    public IActionResult DeletePointOfInterest(int cityId, int id)
    {
        var city = CitiesDataStore.Current.Cities
            .FirstOrDefault(c => c.Id == cityId);
        if (city == null)
        {
            return NotFound();
        }

        var pointOfInterestFromStore = city.PointsOfInterest
            .FirstOrDefault(c => c.Id == id);
        if (pointOfInterestFromStore == null)
        {
            return NotFound();
        }

        city.PointsOfInterest.Remove(pointOfInterestFromStore);

        return NoContent();
    }
}