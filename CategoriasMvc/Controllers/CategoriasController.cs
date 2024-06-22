﻿using CategoriasMvc.Models;
using CategoriasMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace CategoriasMvc.Controllers;

public class CategoriasController : Controller
{
    private readonly ICategoriaService _categoriaService;

    public CategoriasController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaViewModel>>?> Index()
    {
        var result = await _categoriaService.GetCategorias();

        if (result is null)
            return View("Error");

        return View(result);
    }

    [HttpGet]
    public IActionResult CriarNovaCategoria()
    {
        return View();
    }
    [HttpPost]
    public async Task<ActionResult<CategoriaViewModel>> CriarNovaCategoria(CategoriaViewModel categoriaVM)
    {
        if (ModelState.IsValid)
        {
            var result = await _categoriaService.CriaCategoria(categoriaVM);

            if (result != null)
                return RedirectToAction(nameof(Index));
        }

        ViewBag.Erro = "Erro ao criar a categoria";
        return View(categoriaVM);
    }

    [HttpGet]
    public async Task<ActionResult> DeletarCategoria(int id)
    {
        var result = await _categoriaService.GetCategoriaPorId(id);

        if (result is null)
            return View("Error");

        return View(result);
    }
    [HttpPost(), ActionName("DeletarCategoria")]
    public async Task<ActionResult> DeletaConfirmado(int id)
    {
        var result = await _categoriaService.DeletaCategoria(id);

        if (result)
            return RedirectToAction("Index");

        return View("Error");
    }
}