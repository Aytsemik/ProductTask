using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Tasks.Context;
using Tasks.Helpers;
using Tasks.Models;

namespace Tasks.Controllers
{
    public class HomeController : Controller
    {
        public  static object List = System.Web.HttpContext.Current.Session["_List"];
        /// <summary>
        /// #Task1
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="currentFilter"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult Index(string searchString, string currentFilter, int? page, int? pageSize)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            ViewBag.PageSize = pageSize;

            //Use Cache  for List,  create generic list
            List<ProductViewModel> lst = List as List<ProductViewModel>;
            if (lst == null)
            {
                var r = new Random();
                lst = GenerateProducts(100).Select(p => new ProductViewModel
                {
                    ID = r.Next(1, 100),
                    Name = p.Name,
                    Code = p.Code,
                    Barcode = p.Barcode,
                    Price = p.Price
                }).ToList();
                List = lst;
            }

            //Search
            if (searchString != null)
                searchString = searchString.Trim();

            if (!String.IsNullOrEmpty(searchString))
                lst = lst.Where(p => p.Name.Contains(searchString.ToLower())).ToList();

            //Paging
            int nPageSize = (pageSize ?? 10);
            int nPageNumber = (page ?? 1);
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_List", lst.ToPagedList(nPageNumber, nPageSize))
                : View(lst.ToPagedList(nPageNumber, nPageSize));
        }

        public ActionResult Create()
        {
            return PartialView("_Edit");
        }

        public ActionResult Edit(int id)
        {
            var model =( List as List<ProductViewModel>).FirstOrDefault(p=>p.ID==id);
            return PartialView("_Edit",model);
        }
        [HttpPost]
        public ActionResult Save(ProductViewModel model)
        {
            try
            {
                var lst = List as List<ProductViewModel>;

                if (model.ID==0)
                {
                    var r = new Random();
                    // required unique id  in list ...todo
                    model.ID = r.Next(1, 102);

                    lst.Add(model);
                    return Json(new { success = true });
                }

                lst[lst.FindIndex(p => p.ID == model.ID)] = model;

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView("_Edit", model);
            }
        }

        public ActionResult Delete(int id)
        {
            var lst =List as List<ProductViewModel>;
            var model = lst.FirstOrDefault(p => p.ID == id);

            return PartialView("_Delete", model);
        }

        [HttpPost]
        public ActionResult Delete(ProductViewModel model)
        {
            try
            {
                var lst = List as List<ProductViewModel>;

                lst.Remove(lst.FirstOrDefault(p => p.ID == model.ID));
                return Json(new { success = true });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            return PartialView("_Delete", model);
        }

        /// <summary>
        /// #Task2
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Insert()
        {
            return View();
        }
        [HttpPost]
        public async Task <ActionResult> InsertSubmit()
        {
            try
            {
                var productRepository = new ProductRepository();

                var lst = GenerateProducts(50000);
                await productRepository.Add(lst);

                ViewBag.Message = new CustomMessage("Uploaded successfully.", "alert-success");
            }
            catch (Exception ex)
            {

                ViewBag.Message = new CustomMessage(ex.Message, "alert-danger");
            }
            return View("Insert");
        }
        private List<Product> GenerateProducts(int count)
        {
            var c = 10;
            var random = new Random();
            var lst = new List<Product>();

            for (int i = 0; i < count; i++)
            {
                lst.Add(new Product
                {
                    Code = random.Next(1, 1000000),
                    Name = "Product" + i,
                    Price = ((int)Math.Round(random.Next(10, 5000) / 10.0)) * 10,
                    Barcode = i % c == 0 ? null : StringHelper.Get(random.Next(5, 16))
                });
            }
            return lst;
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}