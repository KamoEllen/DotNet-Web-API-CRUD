using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // api/workers
    public class WorkersController:ControllerBase
    {
        private readonly AppDbContext _context;
        public WorkersController(AppDbContext context)
        {
            _context=context;
        }

        [HttpGet]
        public async Task<IEnumerable<Worker>> GetWorkers()
        {
            var workers = await _context.Workers.AsNoTracking().ToListAsync();

            return workers;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Worker worker)
        {
            
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _context.AddAsync(worker);

            var result = await _context.SaveChangesAsync();

            if(result > 0)
            {
                return Ok();
            }

            return BadRequest();
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<Worker>> GetWorker(int id)
        {
            var worker = await _context.Workers.FindAsync(id);

            if(worker is null)
                return NotFound();

            return Ok(worker);
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var worker = await _context.Workers.SingleOrDefaultAsync(x=>x.Id==id);

            if(worker is null)
            {

                return NotFound();
            }
            _context.Remove(worker);

            var result =await _context.SaveChangesAsync();
            if(result > 0){

                return Ok();
            }

            
            return BadRequest("Unable to delted worker");
        }


        [HttpPut("{id:int}")]
        // api/workers/1
        public async Task<IActionResult> EditWorker(int id, Worker worker)
        {
            var workerFromDb = await _context.Workers.FindAsync(id);

            if(workerFromDb is null)
            {
                return BadRequest("Worker Not found");
            }

            workerFromDb.Name = worker.Name;
            workerFromDb.Address = worker.Address;
            workerFromDb.Email = worker.Email;
            worker.PhoneNumber = worker.PhoneNumber;

            var result = await _context.SaveChangesAsync();

            if(result > 0)
            {
                return Ok();
            }

            return BadRequest();


        }



    }
}