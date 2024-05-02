using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OnlineShop.Core.Schemas.Base;
using OnlineShop.Utils;
using OnlineShop.Core;
using OnlineShop.Core.Models;
using OnlineShop.Core.Business.Rule;
using OnlineShop.Core.Interfaces;
using OnlineShop.Core.Schemas;
using OnlineShop.Core.Business.GetDiscountPercentVoucherCode;
using Microsoft.IdentityModel.Tokens;

namespace OnlineShop.UseCases.SyncAllPerm
{
    public class SeedFlow
    {
        private readonly IDataContext dbContext;
        public SeedFlow(IDataContext ctx)
        {
            dbContext = ctx;
        }

        public async Task<Response> Seed(List<RouterModel> routers)
        {
            Response response = new Response();
            var executionStrategy = dbContext.Database.CreateExecutionStrategy();

            await executionStrategy.Execute(async () =>
            {
                using IDbContextTransaction transaction = dbContext.Database.BeginTransaction();
                try
                {
                    if (dbContext.Perms.Any() || dbContext.GroupsPerms.Any() || dbContext.UsersGroups.Any())
                    {
                        dbContext.Perms.RemoveRange(dbContext.Perms);
                        dbContext.GroupsPerms.RemoveRange(dbContext.GroupsPerms);
                        dbContext.UsersGroups.RemoveRange(dbContext.UsersGroups);
                    }
                    if (dbContext.Users.Count() == 0)
                    {
                        SeedGroup();
                        SeedUser();
                    }
                    List<PermSchema> perms = await CreatePerms(routers);
                    List<GroupSchema> groups = await CreateGroupPerm(perms);
                    AddUserToGroup(groups);
                    response.Status = "success";
                    response.Result = true;
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    response.Status = "error";
                    response.Result = false;
                    transaction.Rollback();
                    throw;
                }
            });

            return response;
        }


        private async Task<List<GroupSchema>> CreateGroupPerm(List<PermSchema> perms)
        {
            List<GroupSchema> groups = dbContext.Groups.ToList();
            List<GroupPerm> groupsPerms = new();
            foreach (var perm in perms)
            {
                foreach (var group in groups)
                {
                    if (group.ProfileType.Contains(perm.ProfileTypes) || perm.ProfileTypes == PermissionUtil.PUBLIC_PROFILE)
                    {
                        GroupPerm gp = new GroupPerm();
                        gp.Perm = perm;
                        gp.Group = group;
                        groupsPerms.Add(gp);
                    }
                }
            }
            await dbContext.GroupsPerms.AddRangeAsync(groupsPerms);
            await dbContext.SaveChangesAsync();
            return groups;
        }

        private async Task<List<PermSchema>> CreatePerms(List<RouterModel> routers)
        {
            List<PermSchema> perms = new List<PermSchema>();
            foreach (var router in routers)
            {
                PermSchema permSchema = new PermSchema();
                permSchema.Action = router.Method;
                permSchema.Title = router.Method + " " + router.Module;
                permSchema.ProfileTypes = router.ProfileType;
                permSchema.Module = router.Module;
                perms.Add(permSchema);
            }
            await dbContext.Perms.AddRangeAsync(perms);
            await dbContext.SaveChangesAsync();
            return perms;
        }

        private void AddUserToGroup(List<GroupSchema> groups)
        {
            List<UsersGroups> usersGroups = new List<UsersGroups>();
            List<UserSchema> users = dbContext.Users.ToList();

            foreach (var user in users)
            {
                foreach (var group in groups)
                {
                    if (user.GroupIds.Contains(group.ProfileType))
                    {
                        UsersGroups ug = new UsersGroups();
                        ug.User = user;
                        ug.Group = group;
                        usersGroups.Add(ug);
                    }
                }
            }
            dbContext.UsersGroups.AddRangeAsync(usersGroups);
            dbContext.SaveChanges();
        }

        private void SeedGroup()
        {
            var ids = new int[] { 1, 2 };
            var currentGroups = dbContext.Users.Where(x => ids.Contains(x.Id)).ToList();
            if (currentGroups.Count == 0)
            {
                GroupSchema admin = new GroupSchema { Id = 1, Title = "Admin", Description = "", ProfileType = "[" + PermissionUtil.ADMIN_PROFILE + "]" };
                GroupSchema staff = new GroupSchema { Id = 2, Title = "Staff", Description = "", ProfileType = "[" + PermissionUtil.STAFF_PROFILE + "]" };
                List<GroupSchema> groups = new List<GroupSchema> { admin, staff };
                dbContext.Groups.AddRangeAsync(groups);
                dbContext.SaveChanges();
            }
        }

        private void SeedUser()
        {
            var ids = new int[] { 1, 2 };
            var currentUsers = dbContext.Users.Where(x => ids.Contains(x.Id)).ToList();
            if (currentUsers.Count == 0)
            {
                string defaultPassword = JwtUtil.MD5Hash(UserRule.DEFAULT_PASSWORD);
                UserSchema userAdmin = new UserSchema { Id = 1, UserName = "admin", Password = defaultPassword, Email = "admin@gmail.com", GroupIds = "[" + PermissionUtil.ADMIN_PROFILE + "]" };
                UserSchema userStaff = new UserSchema { Id = 2, UserName = "staff", Password = defaultPassword, Email = "staff@gmail.com", GroupIds = "[" + PermissionUtil.STAFF_PROFILE + "]" };
                List<UserSchema> users = new List<UserSchema> { userAdmin, userStaff };
                dbContext.Users.AddRange(users);
                dbContext.SaveChanges();
            }

        }
        private void SeedCategory()
        {
            var category = new List<CategorySchema>
            {
                new CategorySchema { Name = "Women"},
                new CategorySchema { Name = "Men"},
                new CategorySchema { Name = "Kid"},
                new CategorySchema { Name = "Cosmetics"},
                new CategorySchema { Name = "Accessories"},
            };
            dbContext.Categories.AddRange(category);
        }
        private void SeedBrand()
        {
            var brand = new List<BrandSchema>
            {
                new BrandSchema { Name = "49 Farm",Code = 1},
                new BrandSchema { Name = "49 OL",Code = 2}
            };
            dbContext.Brands.AddRange(brand);
        }
        private void SeedProducts()
        {
            var products = new List<ProductSchema>
            {
                new ProductSchema { Name = "Product 1", Url = "url",ShortDescription = "short", Description = "Desc 1", Price = 25.5f,Discount = 10f, Currentcy = "$", DefaultImage = "product-1.jpg",BrandID = 1, OriginLinkDetail = "link", Stock = 3,CategoryID = 1,Status = ProductStatus.Trend},
                new ProductSchema { Name = "Product 2", Url = "url",ShortDescription = "short", Description = "Desc 2", Price = 25.5f,Discount = 10f, Currentcy = "$", DefaultImage = "product-1.jpg",BrandID = 1, OriginLinkDetail = "link", Stock = 3,CategoryID = 1,Status = ProductStatus.BestSeller},
                new ProductSchema { Name = "Product 3", Url = "url",ShortDescription = "short", Description = "Desc 3", Price = 25.5f,Discount = 10f, Currentcy = "$", DefaultImage = "product-1.jpg",BrandID = 1, OriginLinkDetail = "link", Stock = 3,CategoryID = 1,Status = ProductStatus.Featured},
                new ProductSchema { Name = "Product 4", Url = "url",ShortDescription = "short", Description = "Desc 4", Price = 25.5f,Discount = 10f, Currentcy = "$", DefaultImage = "product-1.jpg",BrandID = 1, OriginLinkDetail = "link", Stock = 3,CategoryID = 1,Status = ProductStatus.BestSeller},
                new ProductSchema { Name = "Product 5", Url = "url",ShortDescription = "short", Description = "Desc 5", Price = 25.5f,Discount = 10f, Currentcy = "$", DefaultImage = "product-1.jpg",BrandID = 1, OriginLinkDetail = "link", Stock = 3,CategoryID = 2,Status = ProductStatus.Trend},
                new ProductSchema { Name = "Product 6", Url = "url",ShortDescription = "short", Description = "Desc 6", Price = 25.5f,Discount = 10f, Currentcy = "$", DefaultImage = "product-1.jpg",BrandID = 1, OriginLinkDetail = "link", Stock = 3,CategoryID = 2,Status = ProductStatus.BestSeller},
                new ProductSchema { Name = "Product 7", Url = "url",ShortDescription = "short", Description = "Desc 7", Price = 25.5f,Discount = 10f, Currentcy = "$", DefaultImage = "product-1.jpg",BrandID = 1, OriginLinkDetail = "link", Stock = 3,CategoryID = 2,Status = ProductStatus.Featured},
                new ProductSchema { Name = "Product 8", Url = "url",ShortDescription = "short", Description = "Desc 8", Price = 25.5f,Discount = 10f, Currentcy = "$", DefaultImage = "product-1.jpg",BrandID = 1, OriginLinkDetail = "link", Stock = 3,CategoryID = 2,Status = ProductStatus.Featured},
                new ProductSchema { Name = "Product 9", Url = "url",ShortDescription = "short", Description = "Desc 9", Price = 25.5f,Discount = 10f, Currentcy = "$", DefaultImage = "product-1.jpg",BrandID = 1, OriginLinkDetail = "link", Stock = 3,CategoryID = 3,Status = ProductStatus.Trend},
                new ProductSchema { Name = "Product 10", Url = "url",ShortDescription = "short", Description = "Desc 10", Price = 25.5f,Discount = 10f, Currentcy = "$", DefaultImage = "product-1.jpg",BrandID = 1, OriginLinkDetail = "link", Stock = 3,CategoryID = 3,Status = ProductStatus.BestSeller},
                new ProductSchema { Name = "Product 11", Url = "url",ShortDescription = "short", Description = "Desc 11", Price = 25.5f,Discount = 10f, Currentcy = "$", DefaultImage = "product-1.jpg",BrandID = 1, OriginLinkDetail = "link", Stock = 3,CategoryID = 3,Status = ProductStatus.Featured},
                new ProductSchema { Name = "Product 12", Url = "url",ShortDescription = "short", Description = "Desc 12", Price = 25.5f,Discount = 10f, Currentcy = "$", DefaultImage = "product-1.jpg",BrandID = 1, OriginLinkDetail = "link", Stock = 3,CategoryID = 3,Status = ProductStatus.BestSeller},
                new ProductSchema { Name = "Product 13", Url = "url",ShortDescription = "short", Description = "Desc 13", Price = 25.5f,Discount = 10f, Currentcy = "$", DefaultImage = "product-1.jpg",BrandID = 1, OriginLinkDetail = "link", Stock = 3,CategoryID = 4,Status = ProductStatus.Trend},
                new ProductSchema { Name = "Product 14", Url = "url",ShortDescription = "short", Description = "Desc 14", Price = 25.5f,Discount = 10f, Currentcy = "$", DefaultImage = "product-1.jpg",BrandID = 1, OriginLinkDetail = "link", Stock = 3,CategoryID = 4,Status = ProductStatus.BestSeller},
                new ProductSchema { Name = "Product 15", Url = "url",ShortDescription = "short", Description = "Desc 15", Price = 25.5f,Discount = 10f, Currentcy = "$", DefaultImage = "product-1.jpg",BrandID = 1, OriginLinkDetail = "link", Stock = 3,CategoryID = 4,Status = ProductStatus.Featured},
                new ProductSchema { Name = "Product 16", Url = "url",ShortDescription = "short", Description = "Desc 16", Price = 25.5f,Discount = 10f, Currentcy = "$", DefaultImage = "product-1.jpg",BrandID = 1, OriginLinkDetail = "link", Stock = 3,CategoryID = 4,Status = ProductStatus.Featured},
                new ProductSchema { Name = "Product 17", Url = "url",ShortDescription = "short", Description = "Desc 17", Price = 25.5f,Discount = 10f, Currentcy = "$", DefaultImage = "product-1.jpg",BrandID = 1, OriginLinkDetail = "link", Stock = 3,CategoryID = 5,Status = ProductStatus.Trend},
                new ProductSchema { Name = "Product 18", Url = "url",ShortDescription = "short", Description = "Desc 18", Price = 25.5f,Discount = 10f, Currentcy = "$", DefaultImage = "product-1.jpg",BrandID = 1, OriginLinkDetail = "link", Stock = 3,CategoryID = 5,Status = ProductStatus.Featured},
                new ProductSchema { Name = "Product 19", Url = "url",ShortDescription = "short", Description = "Desc 19", Price = 25.5f,Discount = 10f, Currentcy = "$", DefaultImage = "product-1.jpg",BrandID = 1, OriginLinkDetail = "link", Stock = 3,CategoryID = 5,Status = ProductStatus.BestSeller},
                new ProductSchema { Name = "Product 20", Url = "url",ShortDescription = "short", Description = "Desc 20", Price = 25.5f,Discount = 10f, Currentcy = "$", DefaultImage = "product-1.jpg",BrandID = 1, OriginLinkDetail = "link", Stock = 3,CategoryID = 5,Status = ProductStatus.Trend},
            };

            dbContext.Products.AddRange(products);
        }
        private void SeedVoucher()
        {
            DateTime dafaultExpiryDate = DateTime.UtcNow.AddDays(3);
            DateTime currentDate = DateTime.UtcNow;
            bool defaultActive = true;

            List<VoucherSchema> vouchers = new List<VoucherSchema> {
                new VoucherSchema { Code = VoucherRule.BLACK_FRIDAY, DiscountPercent = GetDiscountPercentVoucherCode.getDiscountPercentVoucherCode(VoucherRule.BLACK_FRIDAY), StartDate = currentDate, DiscountAmount = 0, EndDate = dafaultExpiryDate, Desc = "BLACK_FRIDAY", Status = defaultActive, CurrentUsageCount = 0 },
                new VoucherSchema { Code = VoucherRule.SPRING_SALE, DiscountPercent = GetDiscountPercentVoucherCode.getDiscountPercentVoucherCode(VoucherRule.SPRING_SALE), StartDate = currentDate, DiscountAmount = 0, EndDate = dafaultExpiryDate, Desc = "SPRING_SALE", Status = defaultActive, CurrentUsageCount = 0 },
                new VoucherSchema { Code = VoucherRule.VALENTINES_SALE, DiscountPercent = GetDiscountPercentVoucherCode.getDiscountPercentVoucherCode(VoucherRule.HALLOWEEN_SALE), StartDate = currentDate, DiscountAmount = 0, EndDate = dafaultExpiryDate, Desc = "VALENTINES_SALE", Status = defaultActive, CurrentUsageCount = 0 },
                new VoucherSchema { Code = VoucherRule.SUMMER_SALE, DiscountPercent = GetDiscountPercentVoucherCode.getDiscountPercentVoucherCode(VoucherRule.SUMMER_SALE), StartDate = currentDate, DiscountAmount = 0, EndDate = dafaultExpiryDate, Desc = "SUMMER_SALE", Status = defaultActive, CurrentUsageCount = 0 },
                new VoucherSchema { Code = VoucherRule.CHRISTMAS_SALE, DiscountPercent = GetDiscountPercentVoucherCode.getDiscountPercentVoucherCode(VoucherRule.CHRISTMAS_SALE), StartDate = currentDate, DiscountAmount = 0, EndDate = dafaultExpiryDate, Desc = "CHRISTMAS_SALE", Status = defaultActive, CurrentUsageCount = 0 },
                new VoucherSchema { Code = VoucherRule.NEW_YEAR, StartDate = currentDate, DiscountAmount = 0, DiscountPercent = GetDiscountPercentVoucherCode.getDiscountPercentVoucherCode(VoucherRule.NEW_YEAR), EndDate = dafaultExpiryDate, Desc = "NEW_YEAR", Status = defaultActive, CurrentUsageCount = 0 },
                new VoucherSchema { Code = VoucherRule.FREE_SHIP, StartDate = currentDate, DiscountAmount = 0, DiscountPercent = 0, EndDate = dafaultExpiryDate, Desc = "FREE_SHIP", Status = defaultActive, CurrentUsageCount = 0 },
                new VoucherSchema { Code = VoucherRule.HALLOWEEN_SALE, StartDate = currentDate, DiscountAmount = 0, DiscountPercent = GetDiscountPercentVoucherCode.getDiscountPercentVoucherCode(VoucherRule.HALLOWEEN_SALE), EndDate = dafaultExpiryDate, Desc = "HALLOWEEN_SALE", Status = defaultActive, CurrentUsageCount = 0 },
                new VoucherSchema { Code = VoucherRule.WOMAN_DAY, StartDate = currentDate, DiscountAmount = 0, DiscountPercent = GetDiscountPercentVoucherCode.getDiscountPercentVoucherCode(VoucherRule.WOMAN_DAY), EndDate = dafaultExpiryDate, Desc = "WOMAN_DAY", Status = defaultActive, CurrentUsageCount = 0 }
            };
            dbContext.Vouchers.AddRange(vouchers);
        }

    }
}
