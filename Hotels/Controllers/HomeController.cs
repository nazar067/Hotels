﻿using Hotels.Models;
using Hotels.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Hotels.Controllers
{
    public class HomeController : Controller
    {
        HotelsContext context;
        public HomeController(HotelsContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult> SendMessage()
        {
            EmailService emailService = new EmailService();
            await emailService.SendEmailAsync("kasich.nszar@gmail.com", "Thank you", "Thank you for ordering our hotel");
            return RedirectToAction("Index");
        }
        public IActionResult Index()
        {
            var hotels = context.Hotels.ToList();
            return View(hotels);
        }
        [HttpGet]
        public IActionResult More(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.HostelsId = id;
            var info = context.Hotels.Where(x => x.Id == id);
            return View(info);
        }
        [HttpGet]
        public IActionResult Buy(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");
            ViewBag.HotelsId = id;
            return View();
        }
        [HttpPost]
        public IActionResult Buy(Orders orders)
        {
            if (orders.Name?.ToLower() == null)
            {
                ModelState.AddModelError("Name", "");
            }
            if (orders.Lastname?.ToLower() == null)
            {
                ModelState.AddModelError("Lastname", "");
            }
            if (orders.Phone == 0)
            {
                ModelState.AddModelError("Phone", "");
            }
            if (orders.Email?.ToLower() == null)
            {
                ModelState.AddModelError("Email", "");
            }
            if (ModelState.IsValid)
            {
                context.Orders.Add(orders);
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Orders ON;");
                context.SaveChanges();
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Orders OFF;");
                SendMessage();
                return RedirectToAction("Index");
            }
            else
            {
                return View(orders);
            }
        }
    }
}
