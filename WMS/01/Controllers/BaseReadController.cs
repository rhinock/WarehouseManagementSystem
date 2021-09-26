using WMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;


namespace WMS.Controllers
{
    /// <summary>
    /// Базовый контроллер для чтения данных
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Route("api/[controller]")]
    [ApiController]
    public class BaseReadController<T> : ControllerBase where T: ModelBase
    {
        protected readonly DbContext _dbContext;
        public BaseReadController(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public virtual IEnumerable<T> Get()
        {
            return _dbContext.Set<T>().ToList();
        }
        [HttpGet("{id}")]
        public virtual IActionResult Get(long id)
        {
            T entity = _dbContext.Set<T>().FirstOrDefault(w => w.Id == id);

            if (entity == null)
            {
                return NotFound();
            }

            return Ok(entity);
        }
    }
}
