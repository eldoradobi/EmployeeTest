using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using MetinvestTest.DAL;
using MetinvestTest.Models;
using Newtonsoft.Json;

namespace MetinvestTest.Controllers
{
    public class EmployeesController : ApiController
    {
        private readonly AppDbContext db;
        public EmployeesController(AppDbContext db)
        {
            this.db = db;
        }

        
        [HttpPost]
        public IQueryable<Employee> Get(Paging paging)
        {
            return db.Employees.OrderBy(e => e.Name).Skip((paging.Page - 1) * paging.Rows).Take(paging.Rows);
        }
       
        [ResponseType(typeof(Employee))]
        [HttpPost]
        public async Task<IHttpActionResult> Get(int id)
        {
            Employee employee = await db.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [ResponseType(typeof(object))]
        [HttpPost]
        public async Task<IHttpActionResult> Edit(int id, Employee employee)
        {
            employee.Id = id;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employee.Id)
            {
                return BadRequest();
            }

            db.Entry(employee).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { success = true, employee });
        }

        [HttpPost]
        [ResponseType(typeof(Employee))]
        public async Task<IHttpActionResult> Create(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new { Error = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))});
            }

            if(!string.IsNullOrEmpty(employee.PersonnelNumber) && db.Employees.Any(e => e.PersonnelNumber == employee.PersonnelNumber && e.Staffed == employee.Staffed))
            {
                return Ok(new { Error = $"Пользователь с табельным номером {employee.PersonnelNumber} существует" });
            }

            db.Employees.Add(employee);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = employee.Id }, employee);
        }

        [ResponseType(typeof(Employee))]
        [HttpPost]
        public async Task<IHttpActionResult> Delete(ObjId objId)
        {
            Employee employee = await db.Employees.FindAsync(objId.Id);
            if (employee == null)
            {
                return NotFound();
            }

            db.Employees.Remove(employee);
            await db.SaveChangesAsync();

            return Ok(new { success = true, employee });
        }
        [ResponseType(typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<HttpResponseMessage> Upload()
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            StringBuilder stringBuilder = new StringBuilder();
           
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                
                var docfiles = new List<string>();
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var serializer = new JsonSerializer();
                    using(var streamReader = new StreamReader(postedFile.InputStream))
                    {
                        using (var jsonTextReader = new JsonTextReader(streamReader))
                        {
                            IList<Employee> employees = null;
                            try
                            {
                                employees = serializer.Deserialize<List<Employee>>(jsonTextReader);
                            }
                            catch (Exception ex)
                            {
                                string jsonFile = JsonConvert.SerializeObject(new Employee());
                                stringBuilder.AppendLine($"Error: Не возсожно прочитать файл, он должен быть в формате \"[{{{jsonFile}}}, {{{jsonFile}}}]\"\n {ex.Message}");
                                break;
                            }
                            foreach (Employee employee in employees)
                            {
                                try
                                {
                                    if (employee.Id != 0 && db.Employees.Find(employee.Id) != null)
                                    {
                                        stringBuilder.AppendLine($"Обновление пользователя {employee.Name}");
                                        db.Employees.Attach(employee);
                                        db.Entry(employee).State = EntityState.Modified;
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(employee.PersonnelNumber) && db.Employees.Any(e => e.PersonnelNumber == employee.PersonnelNumber && e.Staffed == employee.Staffed))
                                        {
                                            throw new Exception($"Пользователь с табельным номером {employee.PersonnelNumber} существует");
                                        }
                                        stringBuilder.AppendLine($"Добавление пользователя {employee.Name}");
                                        db.Employees.Add(employee);
                                    }
                                    await db.SaveChangesAsync();
                                    stringBuilder.AppendLine($"Данные пользователя {employee.Name} добавлены/изменены успешно.");
                                }
                                catch (Exception ex)
                                {
                                    stringBuilder.AppendLine($"Ошибка обновления/добавления: {ex.Message}");
                                }
                            }
                        }
                    }
                }
               
            }
            else
            {
                stringBuilder.AppendLine("Запрос не содержит файлов");
            }
            result.Content = new ByteArrayContent(Encoding.UTF8.GetBytes(stringBuilder.ToString()));
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentDisposition.FileName = "Result.txt";
            
            return result;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmployeeExists(int id)
        {
            return db.Employees.Count(e => e.Id == id) > 0;
        }
    }
}