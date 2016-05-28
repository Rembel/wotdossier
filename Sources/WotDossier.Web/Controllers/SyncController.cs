using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using ProtoBuf;
using WotDossier.Domain;
using WotDossier.Web.Logic;

namespace WotDossier.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Sync")]
    public class SyncController : Controller
    {
        private readonly SyncManager _syncManager;

        public SyncController(SyncManager syncManager)
        {
            _syncManager = syncManager;
        }

        // GET: api/Sync
        [HttpGet]
        [Route("dbversion")]
        public IActionResult GetDbVersion()
        {
            var dbversion = _syncManager.GetDbVersion();
            return Ok(dbversion);
        }

        // post: api/Sync/statistic
        [HttpPost]
        [Route("statistic")]
        public IActionResult PostStatistic()
        {
            try
            {
                var bodyStream = Request.Body;

                using (var streamReader = new BinaryReader(bodyStream))
                {
                    const int bufferSize = 4096;
                    using (var ms = new MemoryStream())
                    {
                        byte[] buffer = new byte[bufferSize];
                        int count;
                        while ((count = streamReader.Read(buffer, 0, buffer.Length)) != 0)
                            ms.Write(buffer, 0, count);
                        ms.Position = 0;
                        var stat = Serializer.Deserialize<ClientStat>(ms);

                        _syncManager.ProcessStatistic(stat);
                    }
                }
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e);
            }
        }

        //// GET: api/Sync/5
        //[HttpGet("{id}", Name = "GetApplicationUser")]
        //public IActionResult GetApplicationUser([FromRoute] string id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return HttpBadRequest(ModelState);
        //    }

        //    ApplicationUser applicationUser = _context.ApplicationUser.Single(m => m.Id == id);

        //    if (applicationUser == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    return Ok(applicationUser);
        //}

        //// PUT: api/Sync/5
        //[HttpPut("{id}")]
        //public IActionResult PutApplicationUser(string id, [FromBody] ApplicationUser applicationUser)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return HttpBadRequest(ModelState);
        //    }

        //    if (id != applicationUser.Id)
        //    {
        //        return HttpBadRequest();
        //    }

        //    _context.Entry(applicationUser).State = EntityState.Modified;

        //    try
        //    {
        //        _context.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ApplicationUserExists(id))
        //        {
        //            return HttpNotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return new HttpStatusCodeResult(StatusCodes.Status204NoContent);
        //}

        //// POST: api/Sync
        //[HttpPost]
        //public IActionResult PostApplicationUser([FromBody] ApplicationUser applicationUser)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return HttpBadRequest(ModelState);
        //    }

        //    _context.ApplicationUser.Add(applicationUser);
        //    try
        //    {
        //        _context.SaveChanges();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (ApplicationUserExists(applicationUser.Id))
        //        {
        //            return new HttpStatusCodeResult(StatusCodes.Status409Conflict);
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtRoute("GetApplicationUser", new { id = applicationUser.Id }, applicationUser);
        //}

        // DELETE: api/Sync/player/ru/5
        [HttpDelete("player/{server}/{id}")]
        public IActionResult DeletePlayer(string server, int id)
        {
            var player = _syncManager.GetPlayer(server, id);
            if (player == null)
            {
                return NotFound();
            }

            _syncManager.DeletePlayer(player);

            return Ok();
        }

        // Get: api/Sync/player/ru/5
        [HttpGet("player/{server}/{id}")]
        public IActionResult GetPlayer(string server, int id)
        {
            var player = _syncManager.GetPlayer(server, id);
            if (player == null)
            {
                return NotFound();
            }

            return Ok(player);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _syncManager.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}