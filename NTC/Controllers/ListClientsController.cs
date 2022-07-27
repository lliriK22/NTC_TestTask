using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NTC.Data;
using NTC.Models;
using System.Text.RegularExpressions;

namespace NTC.Controllers
{
    public class ListClientsController : Controller
    {
        private readonly ApplicationDbContext db;
        public ListClientsController(ApplicationDbContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            IEnumerable<Client> list_clients = db.Clients.ToList();
            return View(list_clients);
        }
        [HttpGet]
        public IActionResult Create()
        {
            Client cl = new Client();
            return PartialView(cl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Client obj, string phone, string email)
        {
            if (obj == null)
                return NotFound();

            obj.Contacts = "Телефон:" + phone + ";  " + "Email:" + email + ";";

            if (string.IsNullOrEmpty(obj.FIO) || string.IsNullOrEmpty(obj.Contacts) || string.IsNullOrEmpty(obj.JobTitle) || string.IsNullOrEmpty(obj.Job))
            {
                return RedirectToAction("Index");
            }

            db.Clients.Add(obj);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            if(id == null)
                return NotFound();

            var client = db.Clients.FirstOrDefault(x => x.Id == id);

            if (client == null)
                return NotFound();

            return PartialView(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Client obj, string phone, string email)
        {
            if (obj == null)
                return NotFound();

            obj.Contacts = "Телефон:" + phone + ";  " + "Email:" + email + ";";

            if (string.IsNullOrEmpty(obj.FIO) || string.IsNullOrEmpty(obj.Contacts) || string.IsNullOrEmpty(obj.JobTitle) || string.IsNullOrEmpty(obj.Job))
            {
                return RedirectToAction("Index");
            }

            db.Clients.Update(obj);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var client = db.Clients.FirstOrDefault(x => x.Id == id);

            if (client == null)
                return NotFound();

            return PartialView(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int? id)
        {
            var obj = db.Clients.FirstOrDefault(x => x.Id == id);

            if (obj == null)
                return NotFound();

            db.Clients.Remove(obj);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Search(string searchString)
        {
            var clients = from m in db.Clients select m;

            if (!String.IsNullOrEmpty(searchString))
            {

                var res_clients = clients.Where(s => s.FIO.Contains(searchString) || s.Job.Contains(searchString) ||
                s.JobTitle.Contains(searchString) || s.Contacts.Contains(searchString));

                if (res_clients.ToList().Count > 0)
                    return View(await res_clients.ToListAsync());
            }

            return RedirectToAction("Index");
        }
    }
}
