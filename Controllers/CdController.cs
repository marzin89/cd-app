#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CdApp.Data;
using CdApp.Models;

namespace CdApp
{
    public class CdController : Controller
    {
        private readonly CdContext _context;

        public CdController(CdContext context)
        {
            _context = context;
        }

        // Startsidan
        // GET: Cd
        public async Task<IActionResult> Index()
        {
            // Returnerar befintliga skivor
            return View(await _context.Cd.ToListAsync());
        }

        // Enskild skiva
        // GET: Cd/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // Returnerar ett felmeddelande om id == null
            if (id == null)
            {
                return NotFound();
            }

            // Hämtar skivan ur databasen
            var cd = await _context.Cd
                .FirstOrDefaultAsync(m => m.CdId == id);

            // Returnerar ett felmeddelande om skivan inte finns
            if (cd == null)
            {
                return NotFound();
            }

            // Om sessionen inte har gått ut, lagras inloggningen i ViewData
            if(HttpContext.Session.GetString("SignedIn") != null)
            {
                ViewData["SignedIn"] = "true";
            }

            // Lagrar skivan i ViewData
            HttpContext.Session.SetInt32("CdId", cd.CdId);
            HttpContext.Session.SetString("Name", cd.Name);
            HttpContext.Session.SetString("Artist", cd.Artist);
            HttpContext.Session.SetString("Year", cd.Year);

            // Lagrar eventuellt längd och skivbolag (ej obligatoriska uppgifter)
            if(cd.Length != null)
            {
                HttpContext.Session.SetString("Length", cd.Length);
            }

            if(cd.Label != null)
            {
                HttpContext.Session.SetString("Label", cd.Label);
            }
            
            HttpContext.Session.SetString("Genre", cd.Genre);

            // Returnerar skivan
            return View(cd);
        }

        // Lägg till
        // GET: Cd/Create
        public IActionResult Create()
        {
            // Returnerar vyn
            return View();
        }

        // Lägger till skivan när formuläret skickas
        // POST: Cd/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CdId,Name,ArtistId,Artist,Year,Length,Label,Genre")] Cd cd)
        {
            // Kontrollerar att formuläret är korrekt ifyllt
            if (ModelState.IsValid)
            {
                // Lägger till ett ID för artisten
                List<Artist> artists = await _context.Artist.ToListAsync();
                cd.ArtistId = artists.Count;

                // Lägger till skivan i databasen
                _context.Add(cd);
                await _context.SaveChangesAsync();

                // Skickar tillbaka användaren till startsidan
                return RedirectToAction(nameof(Index));
            }

            // Returnerar vyn
            return View(cd);
        }

        // Redigera
        // GET: Cd/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // Returnerar ett felmeddelande om id == null
            if (id == null)
            {
                return NotFound();
            }

            // Hämtar skivan ut databasen
            var cd = await _context.Cd.FindAsync(id);

            // Returnerar ett felmeddelande om skivan inte finns
            if (cd == null)
            {
                return NotFound();
            }

            // Returnerar skivan
            return View(cd);
        }

        // Uppdaterar skivan när formuläret skickas
        // POST: Cd/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CdId,Name,Artist,ArtistId,Year,Length,Label,Genre")] Cd cd)
        {
            // Returnerar ett felmeddelande om id inte stämmer överens med skivan
            if (id != cd.CdId)
            {
                return NotFound();
            }

            // Kontrollerar om formuläret är korrekt ifyllt
            if (ModelState.IsValid)
            {
                // Lägger till skivan i databasen
                try
                {
                    _context.Update(cd);
                    await _context.SaveChangesAsync();
                }
                // Returnerar ett felmeddelande om skivan inte finns
                catch (DbUpdateConcurrencyException)
                {
                    if (!CdExists(cd.CdId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                // Skickar tillbaka användaren till startsidan
                return RedirectToAction(nameof(Index));
            }

            // Returnerar vyn
            return View(cd);
        }

        // Radera
        // GET: Cd/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // Returnerar ett felmeddelande om id == null
            if (id == null)
            {
                return NotFound();
            }

            // Hämtar skivan ut databasen
            var cd = await _context.Cd
                .FirstOrDefaultAsync(m => m.CdId == id);

            // Returnerar ett felmeddelande om skivan inte finns
            if (cd == null)
            {
                return NotFound();
            }

            // Returnerar vyn
            return View(cd);
        }

        // Raderar skivan när formuläret skickas
        // POST: Cd/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Hämtar och tar bort skivan ur databasen
            var cd = await _context.Cd.FindAsync(id);
            _context.Cd.Remove(cd);
            await _context.SaveChangesAsync();

            // Skickar tillbaka användaren till startsidan
            return RedirectToAction(nameof(Index));
        }

        private bool CdExists(int id)
        {
            return _context.Cd.Any(e => e.CdId == id);
        }

        // Sökformulär
        public async Task<IActionResult> Search()
        {
            return View();
        }

        // När sökformuläret skickas
        [HttpPost, ActionName("Search")]
        public async Task<IActionResult> Search(string Name, string Artist)
        {
            // Hämtar alla skivor ur databasen
            List<Cd> albums = await _context.Cd.ToListAsync();

            // Listan lagrar eventuella resultat
            List<Cd> result = new List<Cd>();

            // Kontrollerar att Name != null
            if(Name != null)
            {
                // Loopar igenom skivorna
                foreach(var album in albums)
                {
                    // Lägger till skivan i den andra listan om skivan matchar
                    if(album.Name == Name)
                    {
                        result.Add(album);
                    }
                }
            }
            // Kontrollerar att Artist != null
            else if(Artist != null)
            {
                // Loopar igenom skivorna
                foreach (var album in albums)
                {
                    // Lägger till skivan i den andra listan om artisten matchar
                    if (album.Artist == Artist)
                    {
                        result.Add(album);
                    }
                }
            }

            // Lagrar listan i ViewBag
            ViewBag.Result = result;

            // Returnerar vyn
            return View();
        }
    }
}
