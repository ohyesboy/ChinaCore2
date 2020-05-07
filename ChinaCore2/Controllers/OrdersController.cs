using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ChinaCore.Controllers
{
	[Route("api/[controller]")]
	[EnableCors("MyPolicy")]
	public class OrdersController : ControllerBase
	{

		private ILogger<OrdersController> _logger;
		private IConfiguration _config;
		private ChinaContext _db;

		public OrdersController(
			ILogger<OrdersController> logger,
			IConfiguration config,
			ChinaContext db)
		{
			_db = db;
			_config = config;
			_logger = logger;

		}
		[HttpPost]
		public IActionResult Post([FromBody]Order order)
		{
			_db.Orders.Add(order);
			_db.SaveChanges();
			return Ok(order);
		}

		[HttpGet]
		public IActionResult Get()
		{
			_db.Database.EnsureCreated();
			var orders = _db.Orders
				.Include(x => x.Items)
				.OrderBy(x => x.Done)
				.ThenByDescending(x => x.Time)
				.ToList();

			return Ok(orders);
		}

		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			var order = _db.Orders.Include(x => x.Items).FirstOrDefault(x => x.Id == id);

			return Ok(order);
		}

		[HttpGet("clearDone")]
		public IActionResult ClearDone()
		{
			var orders = _db.Orders
				.Where(x => x.Done)
				.ToList();
			_db.RemoveRange(orders);
			_db.SaveChanges();
			return Ok();
		}

		[HttpGet("done/{id}")]
		public IActionResult Done(int id)
		{
			var order = _db.Orders
				.First(x => x.Id == id);
			order.Done = true;

			_db.SaveChanges();
			return Ok();
		}
	}

	public class Order
	{
		[Key]
		public int Id { get; set; }
		public string CustomerName { get; set; }
		public DateTime Time { get; set; }

		public IList<OrderItem> Items { get; set; }

		public bool Done { get; set; }
	}

	public class OrderItem
	{

		[Key]
		public int Id { get; set; }

		public int OrderId { get; set; }

		public int Count { get; set; }
		public string Title { get; set; }
		public string Label { get; set; }

		public float Price { get; set; }


	}
}