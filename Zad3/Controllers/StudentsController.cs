using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zad3.Models;
using Zad3.Services;

namespace Zad3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {

        private const string ConString = "Data Source=db-mssql;Initial Catalog=s16061;Integrated Security=True";

        private IStudentsDal _dbService;

        public StudentsController(IStudentsDal dbService)
        {
            _dbService = dbService;
        }


        [HttpGet]
        public string GetStudent(string orderBy)
        {
            return $"Kowalski, Malewski, Andrzejewski sortowanie={orderBy}";
        }

        [HttpGet("{id}")]
        public IActionResult GetStudent(int id)
        {
            if (id == 1)
            {
                return Ok("Kowalski");
            }
            else if (id == 2)
            {
                return Ok("Malewski");
            }
            return NotFound("Nie znaleziono studenta");
        }

        [HttpGet("list")]
        public IActionResult GetStudents()
        {
            using (SqlConnection con = new SqlConnection(ConString))
            {
                using (SqlCommand cmd = new SqlCommand("Select Student.FirstName, Student.LastName, Student.BirthDate, Studies.Name, Enrollment.Semester FROM Student inner join Enrollment on Student.IdEnrollment = Enrollment.IdEnrollment inner join Studies on Enrollment.IdStudy = Studies.IdStudy", con))
                {
                    con.Open();
                    var dr = cmd.ExecuteReader();
                    var list = new List<Student>();
                    while (dr.Read())
                    {
                        var st = new Student();
                        st.FirstName = dr["FirstName"].ToString();
                        st.LastName = dr["LastName"].ToString();
                        st.BirthDate = dr["BirthDate"].ToString();
                        st.Name = dr["Name"].ToString();
                        st.Semester = dr["Semester"].ToString();
                        list.Add(st);


                    }
                    return Ok(list);

                }
            }

        }

        [HttpGet("studies/{id}")]
        public IActionResult UpdateStudent(int id) { 
                using (SqlConnection con = new SqlConnection(ConString))
        {
            using (SqlCommand cmd = new SqlCommand("Select Student.IndexNumber, Student.FirstName, Student.LastName, Enrollment.Semester FROM Student inner join Enrollment on Student.IdEnrollment = Enrollment.IdEnrollment where	IndexNumber = @id", con))
            {
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    var dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        var st = new Student();
                        st.IndexNumber = dr["IndexNumber"].ToString();
                        st.FirstName = dr["FirstName"].ToString();
                        st.LastName = dr["LastName"].ToString();
                        st.Semester = dr["Semester"].ToString();

                        return Ok(st);
                    }
                }
        }

            return StatusCode((int) HttpStatusCode.OK);
    }



        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id) { 
    using (SqlConnection con = new SqlConnection(ConString))
{
    using (SqlCommand cmd = new SqlCommand("DELETE FROM Student WHERE IndexNumber = @id", con))
    {
      
        cmd.Parameters.AddWithValue("@id", id);
        con.Open();
        int rowsAffected = cmd.ExecuteNonQuery();
                    cmd.CommandText = "Deleted";
                    con.Close();
    }
}

            return StatusCode((int)HttpStatusCode.OK);
        }




    }
}