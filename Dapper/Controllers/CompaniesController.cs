using DapperApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace DapperApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompaniesController : Controller
    {

        private readonly IConfiguration _configuration;
        public CompaniesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult<List<Company>> GetAll()
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var sql = "SELECT * FROM COMPANIES";
                    var result = connection.Query<Company>(sql);
                    return Ok(result);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        [HttpGet("{id}")]
        public ActionResult<Company> Get(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var parameters = new { CompanyId = id };
                    var sql = "SELECT * FROM COMPANIES WHERE CompanyId = @CompanyId";
                    var result = connection.Query<Company>(sql, parameters);
                    return Ok(result);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }
        [HttpPost]
        public ActionResult<Company> Create(Company company)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var parameters = new {
                        Name = company.Name,
                        Address = company.Address,
                        City = company.City,
                        State = company.State,
                        PostalCode= company.PostalCode
                    };
                    var sql = "INSERT INTO Companies (Name, Address, City, State, PostalCode)" +
                        " VALUES (@Name, @Address, @City, @State, @PostalCode);";
                    var result = connection.Execute(sql, parameters);
                    if(result == 1)
                    {
                        return Ok(company);
                    }
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }
        [HttpPut("{id}")]
        public ActionResult<Company> Update(int id, Company newCompany)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var parameters = new {
                        CompanyId = id,
                        Name = newCompany.Name,
                        Address = newCompany.Address,
                        City = newCompany.City,
                        State = newCompany.State,
                        PostalCode = newCompany.PostalCode
                    };
                    var sql = "UPDATE Companies SET Name = @Name, Address = @Address, City= @City," +
                        " State = @State, PostalCode = @PostalCode WHERE CompanyId = @CompanyId;";
                    var result = connection.Execute(sql, parameters);
                    if (result == 1)
                    {
                        return Ok(newCompany);
                    }
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        [HttpDelete("{id}")]
        public ActionResult<Company> Delete(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var parameters = new { CompanyId = id };
                    var sql = "DELETE FROM COMPANIES WHERE CompanyId = @CompanyId";
                    var result = connection.Execute(sql, parameters);
                    if (result == 1)
                    {
                        return Ok();
                    }
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

    }
}
