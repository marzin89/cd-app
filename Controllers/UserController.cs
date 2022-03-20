using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CdApp.Data;
using CdApp.Models;

namespace CdApp.Controllers
{
    public class UserController : Controller
    {
        private readonly CdContext _context;
        public UserController(CdContext context)
        {
            _context = context;
        }

        // Inloggning
        public async Task<IActionResult> Login(int id)
        {
            // Returnerar vyn
            return View();
        }

        // Inloggning när formuläret skickas
        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            // Hämtar alla användare ur databasen
            List<User> users = await _context.User.ToListAsync();

            // Loopar igenom användarna
            foreach(var item in users)
            {
                // Kontrollerar att användarnamn och lösenord matchar
                if(item.UserName == user.UserName & item.Password == user.Password)
                {
                    // Lagrar inloggningen i ViewData och en session
                    ViewData["userID"] = item.UserId;
                    HttpContext.Session.SetInt32("UserId", item.UserId);
                    HttpContext.Session.SetString("SignedIn", "true");

                    // Skickar användaren till Admin
                    return RedirectToAction("Admin", new { UserName = item.UserName, Password = item.Password });
                }
                // Returnerar ett felmeddelande och vyn
                else
                {
                    ViewData["loginError"] = "Fel användarnamn eller lösenord";
                    return View();
                }
            }

            // Returnerar vyn
            return View();
        }

        // Admin-sida för utlåning
        public async Task<IActionResult> Admin()
        {
            // Hämtar alla skivor ur databasen
            List<Cd> albums = await _context.Cd.ToListAsync();

            // Här lagras skivan som ska lånas
            Cd cd = new Cd();

            // Kontrollerar att sessionen inte har löpt ut
            if(HttpContext.Session.GetString("Name") != null)
            {
                // Lagrar skivans ID
                int id = (int)HttpContext.Session.GetInt32("CdId");

                // Loopar igenom skivorna
                foreach (var album in albums)
                {
                    // Lagrar skivan om ID matchar
                    if(album.CdId == id)
                    {
                        cd = album;
                    }
                }
            }

            // Returnerar vyn och skivan
            return View(cd);
        }

        // Admin-sidan när formuläret skickas
        [HttpPost, ActionName("Admin")]
        public async Task<IActionResult> AdminBorrowCD(int? id)
        {
            // Kontrollerar att sessionen inte har löpt ut
            if(HttpContext.Session.GetInt32("CdId") != null)
            {
                // Lagrar skivans ID
                id = (int)HttpContext.Session.GetInt32("CdId");
            }

            // Hämtar skivan ur databasen
            var cd = await _context.Cd.FindAsync(id);

            // Returnerar ett felmeddelande om skivan inte finns
            if(cd == null)
            {
                return NotFound();
            }
            else
            {
                // Kontrollerar att sessionen inte har löpt ut
                if (HttpContext.Session.GetInt32("UserId") != null)
                {
                    // Lagrar användarens ID
                    int userId = (int)HttpContext.Session.GetInt32("UserId");

                    // Lagrar att skivan är utlånad, när den lånades, av vem samt när den lämnas tillbaka
                    cd.IsAvailable = false;
                    cd.Borrowed = DateTime.Now.Date;
                    cd.BorrowedBy = userId;
                    cd.BackInStock = DateTime.Now.Date.AddDays(30);

                    // Uppdaterar databasen
                    _context.Cd.Update(cd);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    ViewData["UserId"] = "null";
                }
            }

            // Returnerar vyn
            return View(cd);
        }

        // Registrering
        public IActionResult Signup()
        {
            // Returnerar vyn
            return View();
        }

        // Registrering när formuläret skickas
        [HttpPost]
        public async Task<IActionResult> Signup(User user)
        {
            // Kontrollerar att användarnamn och lösenord har angetts
            if(user.UserName != null & user.Password != null)
            {
                // Lägger till användaren i databasen
                _context.Add(user);
                await _context.SaveChangesAsync();
            }

            // Returnerar vyn
            return View();
        }
    }
}
