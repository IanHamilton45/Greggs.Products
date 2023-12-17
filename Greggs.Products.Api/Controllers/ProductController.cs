using System;
using System.Collections.Generic;
using System.Linq;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Greggs.Products.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private static readonly string[] Products = new[]
    {
        "Sausage Roll", "Vegan Sausage Roll", "Steak Bake", "Yum Yum", "Pink Jammie"
    };

    private readonly ILogger<ProductController> _logger;

    public ProductController(ILogger<ProductController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<Product> Get(int pageStart = 0, int pageSize = 5)
    {
        if (pageSize > Products.Length)
            pageSize = Products.Length;

        var rng = new Random();
        return Enumerable.Range(1, pageSize).Select(index => new Product
            {
                PriceInPounds = rng.Next(0, 10),
                Name = Products[rng.Next(Products.Length)]
            })
            .ToArray();
    }

    [HttpGet("/UserStory1/")]
    public IEnumerable<Product> GetUS1(int pageStart = 0, int pageSize = 5){
        var products = new ProductAccess();
        
        if (pageSize > products.MaxLength())
            pageSize = products.MaxLength();

        return products.List(pageStart, pageSize)
            .ToArray();
    }

    [HttpGet("/UserStory2/")]
    public IEnumerable<Product> GetUS2(int pageStart = 0, int pageSize = 5){
        var products = new ProductAccess();
        var eurExchange = 1.11m;

        if (pageSize > products.MaxLength())
            pageSize = products.MaxLength();

        var dataToReturn = products.List(pageStart, pageSize).ToList();
        dataToReturn.ForEach (a => {a.PriceInPounds = Math.Round((a.PriceInPounds * eurExchange), 2);});

        return dataToReturn.ToArray();
    }

}