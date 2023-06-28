using System.Net;
using AutoSale.Domain.Enum;
using AutoSale.Domain.Enum.Car;
using AutoSale.Domain.Models;
using AutoSale.Domain.Response;
using AutoSale.Domain.ViewModels.CarAd;
using AutoSale.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace AutoSaleMVC.Controllers
{
    public class CarAdController : Controller
    {
        private readonly ICarAdService _carAdService;
        private readonly ICurrencyService _currencyService;
        private readonly ICarBrandService _carBrandService;
        private readonly ICarModelService _carModelService;
        private readonly UserManager<User> _userManager;
        private readonly ICarImageService _carImageService;
        private readonly IImageService _imageService;


        public CarAdController(ICarAdService carAdService,
            ICurrencyService currencyService,
            ICarBrandService carBrandService,
            ICarModelService carModelService, 
            UserManager<User> userManager,
            ICarImageService carImageService,
            IImageService imageService)
        {
            _carAdService = carAdService;
            _currencyService = currencyService;
            _carBrandService = carBrandService;
            _carModelService = carModelService;
            _userManager = userManager;
            _carImageService = carImageService;
            _imageService = imageService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            CreateCarAdViewModel createCarAdViewModel = new();
            
            var carBrandsResponse = await _carBrandService.GetAllAsync();
            var currenciesResponse = await _currencyService.GetAllAsync();
            
            if (carBrandsResponse.Code is not ResponseCode.Ok 
                || currenciesResponse.Code is not ResponseCode.Ok)
            {
                return View("Error");
            }

            foreach (var carBrand in carBrandsResponse.Data)
            {
                createCarAdViewModel.CarBrands.Add(new SelectListItem
                {
                    Text = carBrand.Name,
                    Value = carBrand.Id.ToString()
                });
            }
            
            foreach (var currency in currenciesResponse.Data)
            {
                createCarAdViewModel.Currencies.Add(new SelectListItem
                {
                    Text = currency.Sign + " (" +currency.Name + ")",
                    Value = currency.Id.ToString()
                });
            }

            var userName = User.Identity?.Name;

            if (userName is null)
            {
                return View("Error");
            }

            var user = await _userManager.FindByNameAsync(userName);

            if (user is null)
            {
                return View("Error");
            }

            createCarAdViewModel.UserId = user.Id;

            return View(createCarAdViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCarAdViewModel createCarAdViewModel)
        {
            
            var carBrandsResponse = await _carBrandService.GetAllAsync();
            
            IResponse<List<CarModel>>? carModelsResponse = null;
            if (createCarAdViewModel.CarBrandId != 0)
            {
                carModelsResponse = await _carModelService.GetByCarBrandIdAsync(createCarAdViewModel.CarBrandId);
            }
            
            var currenciesResponse = await _currencyService.GetAllAsync();
            
            if (carBrandsResponse.Code is not ResponseCode.Ok 
                || currenciesResponse.Code is not ResponseCode.Ok)
            {
                return View("Error");
            }

            foreach (var carBrand in carBrandsResponse.Data)
            {
                createCarAdViewModel.CarBrands.Add(new SelectListItem
                {
                    Text = carBrand.Name,
                    Value = carBrand.Id.ToString()
                });
            }
            
            foreach (var currency in currenciesResponse.Data)
            {
                createCarAdViewModel.Currencies.Add(new SelectListItem
                {
                    Text = currency.Sign + " (" +currency.Name + ")",
                    Value = currency.Id.ToString()
                });
            }

            if (carModelsResponse is not null)
            {
                if (carModelsResponse.Code is not ResponseCode.Ok)
                {
                    return View("Error");
                }
                foreach (var carModel in carModelsResponse.Data)
                {
                    createCarAdViewModel.CarModels.Add(new SelectListItem
                    {
                        Text = carModel.Name,
                        Value = carModel.Id.ToString()
                    });
                }
            }
            
            if (createCarAdViewModel.Fuel != Fuel.Electric && createCarAdViewModel.EngineCapacity is null)
            {
                ModelState.AddModelError("EngineCapacity", "You have to enter engine capacity");
                return View(createCarAdViewModel);
            }

            if (createCarAdViewModel.EngineCapacity is not null)
            {
                if (createCarAdViewModel.EngineCapacity.Contains("."))
                {
                    createCarAdViewModel.EngineCapacity = createCarAdViewModel.EngineCapacity.Replace(".", ",");
                    if (!double.TryParse(createCarAdViewModel.EngineCapacity, out _))
                    {
                        ModelState.AddModelError("EngineCapacity", "Engine capacity was entered in wrong format");
                        return View(createCarAdViewModel);
                    }

                    if (createCarAdViewModel.EngineCapacity == "0")
                    {
                        ModelState.AddModelError("EngineCapacity", "You have to enter engine capacity");
                        return View(createCarAdViewModel);
                    }
                }
            }
            
            if (ModelState.IsValid)
            {
                var response = await _carAdService.CreateAsync(createCarAdViewModel);
                if (response.Code is not ResponseCode.Ok)
                {
                    return View("Error");
                }
                return RedirectToAction("Index", "Home");
            }
            
            return View(createCarAdViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> View(int id)
        {
            var result = await _carAdService.GetByIdAsync(id, true);

            if (result.Code is ResponseCode.NotFound)
            {
                return NotFound();
            }
            
            else if (result.Code is ResponseCode.Ok)
            {
                var isAuthenticated = User.Identity.IsAuthenticated;

                var carAdUser = await _userManager.FindByIdAsync(result.Data.UserId);

                if (carAdUser is null)
                {
                    return View("Error");
                }
                
                var currentUserId = isAuthenticated 
                    ? (await _userManager.FindByNameAsync(User.Identity.Name)).Id 
                    : null;
                
                string? carAdUserImageName = null;
                
                if (carAdUser.ImageId is not null)
                {
                    var carAdUserImageResponse = await _imageService.GetByIdAsync((int)carAdUser.ImageId);
                    if (carAdUserImageResponse.Code is ResponseCode.InternalServerError)
                    {
                        return View("Error");
                    }
                    if (carAdUserImageResponse.Code is ResponseCode.Ok)
                    {
                        carAdUserImageName = carAdUserImageResponse.Data.Name;
                    }
                }

                if (isAuthenticated
                    && currentUserId != result.Data.UserId
                    && !result.Data.IsActive)
                {
                    return StatusCode(403);
                }
                else
                {
                    ViewCarAdViewModel viewCarAdViewModel = new()
                    {
                        CurrentUserId = currentUserId,
                        IsAuthenticated = isAuthenticated,
                        UserImageName = carAdUserImageName
                    };

                    if (isAuthenticated
                        && currentUserId == result.Data.UserId
                        && !result.Data.IsActive)
                    {
                        viewCarAdViewModel.Message = "(The ad is inactive. Other users will not see it.)";
                    }
                    
                    var carImagesResponse = await _carImageService.GetByCarIdAsync(result.Data.CarId, true);
                    if (carImagesResponse.Code is not ResponseCode.Ok)
                    {
                        return View("Error");
                    }

                    viewCarAdViewModel.CarAd = result.Data;
                    viewCarAdViewModel.Images = carImagesResponse.Data.OrderByDescending(ci => ci.IsMain).ToList();
                
                    return View(viewCarAdViewModel);
                }
            }
            else
            {
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page, int carBrandId, int carModelId, Region region, int yearFrom, int yearTo, int priceFrom, int priceTo, CarAdsOrderByOptions carAdsOrderByOptions)
        {
            var pageSize = 10;
            page = page == 0 
                ? 1 
                : page;

            var carAdsResponse = _carAdService.GetAllAsQueryable(true);

            if (carAdsResponse.Code is not ResponseCode.Ok)
            {
                return View("Error");
            }

            var carAdsQueryable = carAdsResponse.Data.Where(ca => ca.IsActive);

            if (carBrandId != 0)
            {
                carAdsQueryable = carAdsQueryable.Where(ca => ca.Car.CarBrandId == carBrandId);
            }
            
            if (carModelId != 0)
            {
                carAdsQueryable = carAdsQueryable.Where(ca => ca.Car.CarModelId == carModelId);
            }
            
            if (region != 0)
            {
                carAdsQueryable = carAdsQueryable.Where(ca => ca.Car.Region == region);
            }
            
            if (yearFrom != 0 && yearTo == 0)
            {
                carAdsQueryable = carAdsQueryable.Where(ca => ca.Car.YearOfProduction >= yearFrom);
            }
            else if (yearFrom == 0 && yearTo != 0)
            {
                carAdsQueryable = carAdsQueryable.Where(ca => ca.Car.YearOfProduction <= yearTo);
            }
            else if (yearTo != 0 && yearFrom != 0)
            {
                if (yearFrom <= yearTo)
                {
                    carAdsQueryable = carAdsQueryable.Where(ca => ca.Car.YearOfProduction >= yearFrom && ca.Car.YearOfProduction <= yearTo);
                }
            }
            
            if (priceFrom != 0 && priceTo == 0)
            {
                carAdsQueryable = carAdsQueryable.Where(ca => ca.Car.Price >= priceFrom);
            }
            else if (priceTo != 0 && priceFrom == 0)
            {
                carAdsQueryable = carAdsQueryable.Where(ca => ca.Car.Price <= priceTo);
            }
            else if (priceTo != 0 && priceFrom != 0)
            {
                if (priceFrom <= priceTo)
                {
                    carAdsQueryable = carAdsQueryable.Where(ca => ca.Car.YearOfProduction >= priceFrom && ca.Car.YearOfProduction <= priceTo);
                }
            }

            if (carAdsOrderByOptions is CarAdsOrderByOptions.FromCheapToExpensive)
            {
                carAdsQueryable = carAdsQueryable.OrderBy(ca => ca.Car.Price);
            }
            else if (carAdsOrderByOptions is CarAdsOrderByOptions.FromExpensiveToCheap)
            {
                carAdsQueryable = carAdsQueryable.OrderByDescending(ca => ca.Car.Price);
            }
            else if (carAdsOrderByOptions is CarAdsOrderByOptions.YearOfProductionInAscendingOrder)
            {
                carAdsQueryable = carAdsQueryable.OrderBy(ca => ca.Car.YearOfProduction);
            }
            else if (carAdsOrderByOptions is CarAdsOrderByOptions.YearOfProductionDescending)
            {
                carAdsQueryable = carAdsQueryable.OrderByDescending(ca => ca.Car.YearOfProduction);
            }
            else if (carAdsOrderByOptions is CarAdsOrderByOptions.MileageInAscendingOrder)
            {
                carAdsQueryable = carAdsQueryable.OrderBy(ca => ca.Car.Mileage);
            }
            else if (carAdsOrderByOptions is CarAdsOrderByOptions.MileageDescending)
            {
                carAdsQueryable = carAdsQueryable.OrderByDescending(ca => ca.Car.Mileage);
            }
            
            var carAds = carAdsQueryable.ToPagedList(page, pageSize);

            var carBrands = await _carBrandService.GetAllAsync();

            if (carBrands.Code is not ResponseCode.Ok)
            {
                return View("Error");
            }
            
            List<CarModel> carModels = new(); 
            
            if (carBrandId != 0)
            {
                var response = await _carModelService.GetByCarBrandIdAsync(carBrandId);
                if (response.Code is not ResponseCode.Ok)
                {
                    return View("Error");
                }

                carModels = response.Data;
            }
            
            IndexCarAdViewModel indexCarAdViewModel = new()
            {
                CarAds = carAds,
                AllCarAdsCount = carAdsQueryable.Count(),
            };

            foreach (var carBrand in carBrands.Data)
            {
                indexCarAdViewModel.CarBrands.Add(new SelectListItem()
                {
                    Text = carBrand.Name,
                    Value = carBrand.Id.ToString()
                });
            }

            if (carModels.Any())
            {
                foreach (var carModel in carModels)
                {
                    indexCarAdViewModel.CarModels.Add(new SelectListItem()
                    {
                        Text = carModel.Name,
                        Value = carModel.Id.ToString()
                    });
                }
            }

            return View(indexCarAdViewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MyAds()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

            var currentUserId = currentUser.Id;

            var carAdsResponse = await _carAdService.GetByUserIdAsync(currentUserId, true);

            if (carAdsResponse.Code is ResponseCode.InternalServerError)
            {
                return View("Error");
            }

            MyAdsViewModel myAdsViewModel = new()
            {
                CarAds = carAdsResponse.Data
            };

            return View(myAdsViewModel);
        }
        
        
    }
}