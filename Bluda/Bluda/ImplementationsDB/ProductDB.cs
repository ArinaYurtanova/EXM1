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
    public class ProductDB : IProduct
    {
        private DBContext context;

        public ProductDB(DBContext context)
        {
            this.context = context;
        }

        public void Add(ProductBindingModel model)
        {
            if (!Regex.IsMatch(model.PlaceProizvod, @"^\D{1,100}") || !Regex.IsMatch(model.ProductName, @"^\D{1,100}"))
            {
                throw new Exception("НАФИГ НАМ ТАКИЕ ПРОДУКТЫ");
            }
            context.Products.Add(new Produckt
            {
                IdBluda = model.IdBluda,
                Count = model.Count,
                PlaceProizvod = model.PlaceProizvod,
                ProductName = model.ProductName,
                DatePostavki = DateTime.Now
            });
            //Bluda b = context.Bludas.FirstOrDefault(rec => rec.Id == model.IdBluda);
            //b.Products.Add(new Produckt
            //{
            //    IdBluda = model.IdBluda,
            //    Count = model.Count,
            //    PlaceProizvod = model.PlaceProizvod,
            //    ProductName = model.ProductName,
            //    DatePostavki = DateTime.Now
            //});
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            Produckt element = context.Products.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                context.Products.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }

        public ProductViewModel GetElement(int id)
        {
            Produckt element = context.Products.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new ProductViewModel
                {

                    Id = element.Id,
                    Count = element.Count,
                    IdBluda = element.IdBluda,
                    PlaceProizvod = element.PlaceProizvod,
                    ProductName = element.ProductName,
                    DatePostavki = element.DatePostavki.ToLongDateString()



                };
            }
            throw new Exception("Элемент не найден");
        }

        public List<ProductViewModel> GetList()
        {
            List<ProductViewModel> result = context.Products
                   .Select(rec => new ProductViewModel
                   {
                       Id = rec.Id,
                       Count = rec.Count,
                       IdBluda = rec.IdBluda,
                       PlaceProizvod = rec.PlaceProizvod,
                       ProductName = rec.ProductName,
                       DatePostavki = SqlFunctions.DateName("dd", rec.DatePostavki) + " " +
                                   SqlFunctions.DateName("mm", rec.DatePostavki) + " " +
                                   SqlFunctions.DateName("yyyy", rec.DatePostavki),

                   })
                   .ToList();
            return result;
        }

        public void Update(ProductBindingModel model)
        {
            if (!Regex.IsMatch(model.PlaceProizvod, @"^\D{1,100}") || !Regex.IsMatch(model.ProductName, @"^\D{1,100}"))
            {
                throw new Exception("НАФИГ НАМ ТАКИЕ ПРОДУКТЫ");
            }
            Produckt element = context.Products.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Count = model.Count;
            element.PlaceProizvod = model.PlaceProizvod;
            element.ProductName = model.PlaceProizvod;
            element.IdBluda = model.IdBluda;
            context.SaveChanges();
        }
    }
}
