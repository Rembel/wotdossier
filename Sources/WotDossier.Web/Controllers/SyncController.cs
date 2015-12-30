using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using Newtonsoft.Json;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Web.Models;
using WotDossier.Web.Services;

namespace WotDossier.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Sync")]
    public class SyncController : Controller
    {
        private dossierContext _context;

        public SyncController(dossierContext context)
        {
            _context = context;
        }

        // GET: api/Sync
        [HttpGet]
        [Route("dbversion")]
        public IActionResult GetDbVersion()
        {
            var dbversion = _context.dbversion.First();
            return Ok(dbversion);
        }

        // post: api/Sync/statistic
        [HttpPost]
        [Route("statistic")]
        public IActionResult PostStatistic(Statistic statistic)
        {
            try
            {
                var bodyStream = Request.Body;

                using (var streamReader = new StreamReader(bodyStream))
                {
                    var body = streamReader.ReadToEnd();

                    statistic = JsonConvert.DeserializeObject<Statistic>(body);
                }

                var stat = DeserializeStatistic(statistic);

                var player = _context.player.FirstOrDefault(x => x.accountid == stat.Player.AccountId && x.server == stat.Player.Server);

                if (player == null)
                {
                    _context.player.Add(new player
                    {
                        id = stat.Player.Id,
                        uid = stat.Player.UId.Value,
                        accountid = stat.Player.AccountId,
                        server = stat.Player.Server,
                        creaded = stat.Player.Creaded,
                        rev = stat.Player.Rev,
                        name = stat.Player.Name
                    });

                }
                else
                {
                    player.rev = stat.Player.Rev;
                }

                _context.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return HttpBadRequest(e);
            }
        }

        private static ClientStat DeserializeStatistic(Statistic statistic)
        {
            byte[] bytes = Convert.FromBase64String(statistic.CompressedData);
            string decompress = CompressHelper.Decompress(bytes);
            ClientStat stat = JsonConvert.DeserializeObject<ClientStat>(decompress);
            return stat;
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

        // DELETE: api/Sync/5
        [HttpDelete("player/{server}/{id}")]
        public IActionResult DeleteStatistic(string server, int id)
        {
        //    if (!ModelState.IsValid)
        //    {
        //        return HttpBadRequest(ModelState);
        //    }

            player player = _context.player.FirstOrDefault(m => m.server == server && m.accountid == id);
            if (player == null)
            {
                return HttpNotFound();
            }

            _context.player.Remove(player);
            _context.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }

        //private bool ApplicationUserExists(string id)
        //{
        //    return _context.ApplicationUser.Count(e => e.Id == id) > 0;
        //}
    }
}