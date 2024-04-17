using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MyApp.DTOs;
using MyApp.Models;

namespace MyApp.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class AnimalsController: ControllerBase
{
    private readonly IConfiguration _configuration;
    public AnimalsController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    [HttpGet]
    public ActionResult GetAnimals([FromQuery]  string? parametr)
    {
        SqlConnection connection = 
            new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();
        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        
        List<Animal> animals = new List<Animal>();
          
        if (string.IsNullOrEmpty(parametr))
        {
            parametr = "Name";

        }
        command.CommandText = $"select * from Animal order by {@parametr}";
        command.Parameters.AddWithValue("@parametr", parametr);

        var reader = command.ExecuteReader();
        int idAnimalsOrdinal = reader.GetOrdinal("IdAnimal");
        int nameOrdinal = reader.GetOrdinal("Name");
        int descOrdinal = reader.GetOrdinal("Description");
        int categoryOrdinal = reader.GetOrdinal("Category");
        int areaOrdinal = reader.GetOrdinal("Area");
        
        while (reader.Read())
        {
            animals.Add(new Animal()
            {
                IdAnimal = reader.GetInt32(idAnimalsOrdinal),
                Name = reader.GetString(nameOrdinal),
                Description = reader.GetString(descOrdinal),
                Category = reader.GetString(categoryOrdinal),
                Area = reader.GetString(areaOrdinal)
            });
        }

        return Ok(animals);
    }

    [HttpPost]
    public ActionResult AddAnimal(AddAnimal animal)
    {
        using SqlConnection connection = 
            new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();
        using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        
        command.CommandText = $"insert into Animal values (@animalId, @animalN, @animalD, @animalC, @animalA)";
        // command.Parameters.AddWithValue("@animId", animal.IdAnimal);

        command.Parameters.AddWithValue("@animalId", animal.IdAnimal);
        command.Parameters.AddWithValue("@animalN", animal.Name);
        command.Parameters.AddWithValue("@animalD", animal.Description);
        command.Parameters.AddWithValue("@animalC", animal.Category);
        command.Parameters.AddWithValue("@animalA", animal.Area);
        command.ExecuteReader();

        return Created("", null);
    }
    
    
    
    [HttpPut]
    public ActionResult UpdateAnimal(int id, UpdateAnimal animal)
    { 
        using SqlConnection connection = 
            new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();
        using SqlCommand command = new SqlCommand();
        command.Connection = connection;

        
        command.CommandText = $"update Animal set Animal.Description = @desc, Animal.Name = @name, Animal.Category = @category, Animal.Area = @area  where  Animal.IdAnimal = @id";
        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@desc", animal.Description);
        command.Parameters.AddWithValue("@name", animal.Name);
        command.Parameters.AddWithValue("@category", animal.Category);
        command.Parameters.AddWithValue("@area", animal.Area);

        command.ExecuteReader();
        
        return Ok();
    }

    [HttpDelete]
    public ActionResult DeleteAnimal(int idAnimal)
    {
        using SqlConnection connection = 
            new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();
        using SqlCommand command = new SqlCommand();
        command.Connection = connection;

        command.CommandText = $"Delete Animal where Animal.IdAnimal = @id";
        command.Parameters.AddWithValue("@id", idAnimal);
        int deletedRow = command.ExecuteNonQuery();

        if (deletedRow > 0)
        {
            return Ok();
        }
        return NotFound();




    }
    
    
}