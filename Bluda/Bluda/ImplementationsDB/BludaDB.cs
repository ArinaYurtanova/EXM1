using ClassLibrary.BindingModel;
using ClassLibrary.Interface;
using ClassLibrary.Models;
using ClassLibrary.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text.RegularExpressions;

namespace ClassLibrary.ImplementationsDB
{
    public class BludaDB : IBluda
    {
        private DBContext context;

        public BludaDB(DBContext context)
        {
            this.context = context;
        }

        public void Add(BludaBindingModel model)
        {
            if (!Regex.IsMatch(model.NameBluda, @"^\D{1,100}") || !Regex.IsMatch(model.TypeBluda, @"^\D{1,100}"))
            {
                throw new Exception("Неправильно введены данные");
            }

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Bluda element = context.Bludas.FirstOrDefault(rec => rec.NameBluda == model.NameBluda);
                    if (element != null)
                    {
                        throw new Exception("Уже есть изделие с таким названием");
                    }
                    element = new Bluda
                    {
                        NameBluda = model.NameBluda,
                        DateCreate = DateTime.Now,
                        TypeBluda = model.TypeBluda
                    };
                    context.Bludas.Add(element);
                    context.SaveChanges();
                    var groupMaterials = model.ListProduct.GroupBy(rec => rec.ProductName)
                                                .Select(rec => new
                                                {
                                                    ProductName = rec.Key,
                                                    IdBluda = rec.FirstOrDefault().IdBluda,
                                                    Count = rec.Sum(r => r.Count),
                                                    PlaceProizvod = rec.FirstOrDefault().PlaceProizvod

                                                });
                    foreach (var groupMaterial in groupMaterials)
                    {
                        context.Products.Add(new Produckt
                        {
                            IdBluda = element.Id,
                            ProductName = groupMaterial.ProductName,
                            PlaceProizvod = groupMaterial.PlaceProizvod,
                            Count = groupMaterial.Count
                        });
                        context.SaveChanges();
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void Delete(int id)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Bluda element = context.Bludas.FirstOrDefault(rec => rec.Id == id);
                    if (element != null)
                    {
                        context.Products.RemoveRange(
                                            context.Products.Where(rec => rec.IdBluda == id));
                        context.Bludas.Remove(element);
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Элемент не найден");
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public BludaViewModel GetElement(int id)
        {
            Bluda element = context.Bludas.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new BludaViewModel
                {
                    Id = element.Id,
                    NameBluda = element.NameBluda,
                    TypeBluda = element.TypeBluda,
                    DateCreate = element.DateCreate.ToString(),
                    Products = context.Products.Where(rec => rec.IdBluda == element.Id).Select(rec => new ProductViewModel
                    {
                        Id = rec.Id,
                        ProductName = rec.ProductName,
                        DatePostavki = rec.DatePostavki.ToString(),
                        Count = rec.Count,
                        IdBluda = rec.IdBluda,
                        PlaceProizvod = rec.PlaceProizvod
                    }).ToList()
                };

            }
            throw new Exception("Элемент не найден");
        }

        public List<BludaViewModel> GetList()
        {
            List<BludaViewModel> result = context.Bludas
                .Select(rec => new BludaViewModel
                {
                    Id = rec.Id,
                    NameBluda = rec.NameBluda,
                    TypeBluda = rec.TypeBluda,
                    DateCreate = SqlFunctions.DateName("dd", rec.DateCreate) + " " +
                                SqlFunctions.DateName("mm", rec.DateCreate) + " " +
                                SqlFunctions.DateName("yyyy", rec.DateCreate),

                })
                .ToList();
            return result;
        }

        public List<BludaViewModel> GetListOrder(ReportBindingModel model)
        {
            List<BludaViewModel> result = context.Bludas
                .Where(rec => rec.DateCreate >= model.DateFrom && rec.DateCreate <= model.DateTo)
                .Select(rec => new BludaViewModel
                {
                    Id = rec.Id,
                    NameBluda = rec.NameBluda,
                    TypeBluda = rec.TypeBluda,
                    DateCreate = SqlFunctions.DateName("dd", rec.DateCreate) + " " +
                                SqlFunctions.DateName("mm", rec.DateCreate) + " " +
                                SqlFunctions.DateName("yyyy", rec.DateCreate),

                })
                .ToList();
            return result;
        }

        public void Update(BludaBindingModel model)
        {
            if (!Regex.IsMatch(model.NameBluda, @"^\D{1,100}") || !Regex.IsMatch(model.TypeBluda, @"^\D{1,100}"))
            {
                throw new Exception("Неправильно введены данные");
            }

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Bluda element = context.Bludas.FirstOrDefault(rec =>
                                  rec.NameBluda == model.NameBluda && rec.Id != model.Id);
                    if (element != null)
                    {
                        throw new Exception("Уже есть изделие с таким названием");
                    }
                    element = context.Bludas.FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    element.NameBluda = model.NameBluda;
                    element.TypeBluda = model.TypeBluda;
                    context.SaveChanges();

                    var compNumbers = model.ListProduct.Select(rec => rec.Id).Distinct();
                    var updateMaterials = context.Products
                                                    .Where(rec => rec.IdBluda == model.Id &&
                                                        compNumbers.Contains(rec.Id));
                    foreach (var updateMaterial in updateMaterials)
                    {
                        updateMaterial.Count = model.ListProduct
                                                        .FirstOrDefault(rec => rec.Id == updateMaterial.Id).Count;
                    }
                    context.SaveChanges();
                    context.Products.RemoveRange(
                                        context.Products.Where(rec => rec.IdBluda == model.Id &&
                                                                            !compNumbers.Contains(rec.Id)));
                    context.SaveChanges();
                    var groupMaterials = model.ListProduct
                                                .Where(rec => rec.Id == 0)
                                                .GroupBy(rec => rec.ProductName)
                                                .Select(rec => new
                                                {
                                                    ProductName = rec.Key,
                                                    rec.FirstOrDefault().IdBluda,
                                                    Count = rec.Sum(r => r.Count),
                                                    rec.FirstOrDefault().PlaceProizvod
                                                });
                    foreach (var groupMaterial in groupMaterials)
                    {
                        Produckt elementPC = context.Products
                                                 .FirstOrDefault(rec => rec.IdBluda == model.Id &&
                                                                 rec.IdBluda == groupMaterial.IdBluda);
                        if (elementPC != null)
                        {
                            elementPC.Count += groupMaterial.Count;
                            context.SaveChanges();
                        }
                        else
                        {
                            context.Products.Add(new Produckt
                            {
                                IdBluda = model.Id,
                                DatePostavki = DateTime.Now,
                                PlaceProizvod = groupMaterial.PlaceProizvod,
                                ProductName = groupMaterial.ProductName,
                                Count = groupMaterial.Count
                            });
                            context.SaveChanges();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
