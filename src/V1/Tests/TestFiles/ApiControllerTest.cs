using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceQuery;

namespace ServiceBricks.Xunit
{
    public abstract class ApiControllerTest<TDto> : IDisposable
        where TDto : class, IDataTransferObject
    {
        public virtual ISystemManager SystemManager { get; set; }

        public virtual ITestManager<TDto> TestManager { get; set; }

        public virtual void Dispose()
        {
            SystemManager?.StopSystem();
        }

        [Fact]
        public virtual async Task Create_NullData()
        {
            var controller = TestManager.GetController(SystemManager.ServiceProvider);

            //Call Create
            var respCreate = await controller.CreateAsync(null);

            if (respCreate is BadRequestObjectResult badResult)
            {
                Assert.True(badResult.Value != null);
                if (badResult.Value is ProblemDetails problemDetails)
                    Assert.True(problemDetails.Title == LocalizationResource.ERROR_SYSTEM);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        public virtual async Task<TDto> CreateBase(TDto model)
        {
            int existingCount = 0;
            List<TDto> existingList = new List<TDto>();

            //Call GetAll
            var controller = TestManager.GetController(SystemManager.ServiceProvider);
            var respGetAll = await controller.QueryAsync(new ServiceQueryRequest());
            if (respGetAll is OkObjectResult okResultGetAll)
            {
                Assert.True(okResultGetAll.Value != null);
                if (okResultGetAll.Value is ServiceQueryResponse<TDto> resplist)
                {
                    existingCount = resplist.List.Count;
                    existingList = resplist.List;
                }
            }
            else
                Assert.Fail("");

            //Call Create
            var respCreate = await controller.CreateAsync(model);
            if (respCreate is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is TDto obj)
                {
                    //Validate
                    TestManager.ValidateObjects(model, obj, HttpMethod.Post);

                    //Call GetItem
                    var respGetItem = await controller.Get(obj.StorageKey);
                    if (respGetItem is OkObjectResult okResultGetItem && model != null)
                    {
                        Assert.True(okResultGetItem.Value != null);
                        if (okResultGetItem.Value is TDto gobj)
                        {
                            //Validate
                            TestManager.ValidateObjects(model, gobj, HttpMethod.Post);

                            //Call GetAll
                            respGetAll = await controller.Query(new ServiceQueryRequest());
                            if (respGetAll is OkObjectResult okResultGetAll2)
                            {
                                Assert.True(okResultGetAll2.Value != null);
                                if (okResultGetAll2.Value is ServiceQueryResponse<TDto> resplist)
                                {
                                    Assert.True(resplist.List.Count == 1 + existingCount);
                                    var foundObject = TestManager.FindObject(resplist.List, gobj);
                                    Assert.True(foundObject != null);

                                    //Validate
                                    TestManager.ValidateObjects(gobj, foundObject, HttpMethod.Get);
                                    return gobj;
                                }
                                else
                                    Assert.Fail("");
                            }
                            else
                                Assert.Fail("");
                        }
                        else
                            Assert.Fail("");
                    }
                    else
                        Assert.Fail("");
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            throw new Exception();
        }

        [Fact]
        public virtual async Task Create_MinData()
        {
            var model = TestManager.GetMinimumDataObject();

            await CreateBase(model);
        }

        [Fact]
        public virtual async Task Create_MaxData()
        {
            var model = TestManager.GetMaximumDataObject();

            await CreateBase(model);
        }

        [Fact]
        public virtual async Task Create_Two()
        {
            int existingCount = 0;
            //Call GetAll before creating (possible pre-populated)
            var controller = TestManager.GetController(SystemManager.ServiceProvider);
            var respGetAll = await controller.Query(new ServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is ServiceQueryResponse<TDto> resplist)
                {
                    existingCount = resplist.List.Count;
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            await Create_MinData();

            await Create_MaxData();

            //Call GetAll again after create
            controller = TestManager.GetController(SystemManager.ServiceProvider);
            respGetAll = await controller.Query(new ServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult2)
            {
                Assert.True(okResult2.Value != null);
                if (okResult2.Value is ServiceQueryResponse<TDto> resplist)
                {
                    Assert.True(resplist.List.Count == 2 + existingCount);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual async Task GetAll_MinData()
        {
            int existingCount = 0; //possibly pre-populated
            List<TDto> existingList = new List<TDto>();
            //Call GetAll
            var controller = TestManager.GetController(SystemManager.ServiceProvider);
            var respGetAll = await controller.Query(new ServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is ServiceQueryResponse<TDto> resplist)
                {
                    existingCount = resplist.List.Count;
                    existingList = resplist.List;
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            var model = TestManager.GetMinimumDataObject();
            var dto = await CreateBase(model);

            respGetAll = await controller.Query(new ServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult2)
            {
                Assert.True(okResult2.Value != null);
                if (okResult2.Value is ServiceQueryResponse<TDto> resplist)
                {
                    Assert.True(resplist.List.Count == 1 + existingCount);
                    var foundObject = TestManager.FindObject(resplist.List, dto);
                    Assert.True(foundObject != null);

                    //Validate
                    TestManager.ValidateObjects(dto, foundObject, HttpMethod.Get);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual async Task GetAll_MaxData()
        {
            int existingCount = 0;
            List<TDto> existingList = new List<TDto>();
            //Call GetAll
            var controller = TestManager.GetController(SystemManager.ServiceProvider);
            var respGetAll = await controller.Query(new ServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is ServiceQueryResponse<TDto> resplist)
                {
                    existingCount = resplist.List.Count;
                    existingList = resplist.List;
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            var model = TestManager.GetMaximumDataObject();
            var dto = await CreateBase(model);

            respGetAll = await controller.Query(new ServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult2)
            {
                Assert.True(okResult2.Value != null);
                if (okResult2.Value is ServiceQueryResponse<TDto> resplist)
                {
                    Assert.True(resplist.List.Count == 1 + existingCount);
                    var foundObject = TestManager.FindObject(resplist.List, dto);
                    Assert.True(foundObject != null);

                    //Validate
                    TestManager.ValidateObjects(dto, foundObject, HttpMethod.Get);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual async Task GetItem_NullData()
        {
            //Call GetItem
            var controller = TestManager.GetController(SystemManager.ServiceProvider);
            var respGetItem = await controller.Get(null);
            if (respGetItem is BadRequestObjectResult badResult)
            {
                Assert.True(badResult.Value != null);
                if (badResult.Value is ProblemDetails problemDetails)
                    Assert.True(problemDetails.Title == LocalizationResource.ERROR_SYSTEM);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual async Task GetItem_NotFound()
        {
            var model = TestManager.GetObjectNotFound();

            //Call GetItem
            var controller = TestManager.GetController(SystemManager.ServiceProvider);
            var respGetItem = await controller.Get(model.StorageKey);

            if (respGetItem is OkObjectResult okResult)
                Assert.True(okResult.Value == null);
            else
                Assert.Fail("");

            //Call GetItem with null
            controller = TestManager.GetController(SystemManager.ServiceProvider);
            respGetItem = await controller.Get(null);

            if (respGetItem is BadRequestObjectResult badResult)
            {
                Assert.True(badResult.Value != null);
                if (badResult.Value is ProblemDetails problemDetails)
                    Assert.True(problemDetails.Title == LocalizationResource.ERROR_SYSTEM);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual async Task GetAllPaging_Multi()
        {
            await Create_Two();

            int existingCount = 0;
            //Call GetAll
            var controller = TestManager.GetController(SystemManager.ServiceProvider);
            var respGetAll = await controller.Query(new ServiceQueryRequest());
            if (respGetAll is OkObjectResult okResultAll)
            {
                Assert.True(okResultAll.Value != null);
                if (okResultAll.Value is ServiceQueryResponse<TDto> resplist)
                {
                    existingCount = resplist.List.Count;
                    Assert.True(existingCount >= 2); //Possible pre-loaded data
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Call GetAllPaging get one
            controller = TestManager.GetController(SystemManager.ServiceProvider);
            var paging = new Paging() { PageNumber = 1, PageSize = 1 };
            var serviceQueryRequest = ServiceQueryRequestBuilder.New().Paging(1, 1, false).Build();
            respGetAll = await controller.Query(serviceQueryRequest);
            if (respGetAll is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is ServiceQueryResponse<TDto> respPaging)
                {
                    Assert.True(respPaging.List != null);
                    if (respPaging.List == null)
                        throw new ArgumentNullException(nameof(respPaging.List));
                    //Assert.True(respPaging.Count == existingCount);
                    Assert.True(respPaging.List.Count == 1);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Call GetAllPaging get two
            serviceQueryRequest = ServiceQueryRequestBuilder.New().Paging(1, 2, false).Build();
            respGetAll = await controller.Query(serviceQueryRequest);
            if (respGetAll is OkObjectResult okResult2)
            {
                Assert.True(okResult2.Value != null);
                if (okResult2.Value is ServiceQueryResponse<TDto> respPaging)
                {
                    Assert.True(respPaging.List != null);
                    if (respPaging.List == null)
                        throw new ArgumentNullException(nameof(respPaging.List));
                    //Assert.True(respPaging.Count == existingCount);
                    Assert.True(respPaging.List.Count == 2);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Call GetAllPaging get more than total
            serviceQueryRequest = ServiceQueryRequestBuilder.New().Paging(1, existingCount + 1, false).Build();
            respGetAll = await controller.Query(serviceQueryRequest);
            if (respGetAll is OkObjectResult okResult3)
            {
                Assert.True(okResult3.Value != null);
                if (okResult3.Value is ServiceQueryResponse<TDto> respPaging)
                {
                    Assert.True(respPaging.List != null);
                    if (respPaging.List == null)
                        throw new ArgumentNullException(nameof(respPaging.List));
                    //Assert.True(respPaging.Count == existingCount);
                    Assert.True(respPaging.List.Count == existingCount);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Call GetAllPaging page two of one
            serviceQueryRequest = ServiceQueryRequestBuilder.New().Paging(2, 1, false).Build();
            respGetAll = await controller.Query(serviceQueryRequest);
            if (respGetAll is OkObjectResult okResultpage2)
            {
                Assert.True(okResultpage2.Value != null);
                if (okResultpage2.Value is ServiceQueryResponse<TDto> respPaging)
                {
                    Assert.True(respPaging.List != null);
                    if (respPaging.List == null)
                        throw new ArgumentNullException(nameof(respPaging.List));
                    //Assert.True(respPaging.Count == existingCount);
                    Assert.True(respPaging.List.Count == 1);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Call GetAllPaging page two (over max)
            serviceQueryRequest = ServiceQueryRequestBuilder.New().Paging(2, existingCount, false).Build();
            respGetAll = await controller.Query(serviceQueryRequest);
            if (respGetAll is OkObjectResult okResultpage2ofmax)
            {
                Assert.True(okResultpage2ofmax.Value != null);
                if (okResultpage2ofmax.Value is ServiceQueryResponse<TDto> respPaging)
                {
                    Assert.True(respPaging.List != null);
                    if (respPaging.List == null)
                        throw new ArgumentNullException(nameof(respPaging.List));
                    //Assert.True(respPaging.Count == existingCount);
                    Assert.True(respPaging.List.Count == 0);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual async Task Update_NullData()
        {
            //Call Update
            var controller = TestManager.GetController(SystemManager.ServiceProvider);
            var respUpdate = await controller.Update(null);

            if (respUpdate is BadRequestObjectResult badResult)
            {
                Assert.True(badResult.Value != null);
                if (badResult.Value is ProblemDetails problemDetails)
                    Assert.True(problemDetails.Title == LocalizationResource.ERROR_SYSTEM);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        protected virtual async Task UpdateNoChangeBase(TDto model)
        {
            //Call Update
            var controller = TestManager.GetController(SystemManager.ServiceProvider);
            var respUpdate = await controller.Update(model);
            if (respUpdate is OkObjectResult okResult && model != null)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is TDto obj)
                {
                    //Validate
                    TestManager.ValidateObjects(model, obj, HttpMethod.Put);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual async Task Update_NoChange_MinData()
        {
            var model = TestManager.GetMinimumDataObject();

            var dto = await CreateBase(model);

            await UpdateNoChangeBase(dto);
        }

        [Fact]
        public virtual async Task Update_NoChange_MaxData()
        {
            var model = TestManager.GetMaximumDataObject();

            var dto = await CreateBase(model);

            await UpdateNoChangeBase(dto);
        }

        protected virtual async Task UpdateBase(TDto model)
        {
            var controller = TestManager.GetController(SystemManager.ServiceProvider);
            //Update the object
            if (model == null)
                throw new ArgumentNullException("model");
            TestManager.UpdateObject(model);

            //Call Update
            var respUpdate = await controller.Update(model);
            if (respUpdate is OkObjectResult okResult && model != null)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is TDto obj)
                {
                    //Validate
                    TestManager.ValidateObjects(model, obj, HttpMethod.Put);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual async Task Update_MinData()
        {
            var model = TestManager.GetMinimumDataObject();
            var dto = await CreateBase(model);
            await UpdateBase(dto);
        }

        [Fact]
        public virtual async Task Update_MaxData()
        {
            var model = TestManager.GetMaximumDataObject();
            var dto = await CreateBase(model);
            await UpdateBase(dto);
        }

        protected virtual async Task DeleteBase(TDto model)
        {
            var controller = TestManager.GetController(SystemManager.ServiceProvider);

            //Call Delete
            var respDelete = await controller.Delete(model.StorageKey);
            if (respDelete is OkObjectResult okResultDelete)
            {
                Assert.True(okResultDelete.Value != null);
                if (okResultDelete.Value is bool retVal)
                {
                    Assert.True(retVal == true);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Verify GetItem not found
            var respGetItem = await controller.Get(model.StorageKey);
            if (respGetItem is OkObjectResult okResult)
                Assert.True(okResult.Value == null);
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual async Task Delete_MinData()
        {
            var model = TestManager.GetMinimumDataObject();
            var dto = await CreateBase(model);

            await DeleteBase(dto);
        }

        [Fact]
        public virtual async Task Delete_MaxData()
        {
            var model = TestManager.GetMaximumDataObject();
            var dto = await CreateBase(model);

            await DeleteBase(dto);
        }

        [Fact]
        public virtual async Task Query()
        {
            int existingCount = 0;
            List<TDto> existingList = new List<TDto>();
            //Call GetAll
            var controller = TestManager.GetController(SystemManager.ServiceProvider);
            var respGetAll = await controller.Query(new ServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is ServiceQueryResponse<TDto> resplist)
                {
                    existingCount = resplist.List.Count;
                    existingList = resplist.List;
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            // Query all
            var qb = ServiceQueryRequestBuilder.New();
            var respQueryAll = await controller.Query(qb.Build());
            if (respQueryAll is OkObjectResult qokResult)
            {
                Assert.True(qokResult.Value != null);
                if (qokResult.Value is ServiceQueryResponse<TDto> querycount)
                {
                    //Assert.True(existingCount == querycount.Count);
                    Assert.True(existingCount == querycount.List.Count);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            var model = TestManager.GetMaximumDataObject();
            var dto = await CreateBase(model);

            respGetAll = await controller.Query(new ServiceQueryRequest());
            if (respGetAll is OkObjectResult okResult2)
            {
                Assert.True(okResult2.Value != null);
                if (okResult2.Value is ServiceQueryResponse<TDto> resplist)
                {
                    Assert.True(resplist.List.Count == 1 + existingCount);
                    var foundObject = TestManager.FindObject(resplist.List, dto);
                    Assert.True(foundObject != null);

                    //Validate
                    TestManager.ValidateObjects(dto, foundObject, HttpMethod.Get);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            // Query all again
            respQueryAll = await controller.Query(new ServiceQueryRequest());
            if (respQueryAll is OkObjectResult qaokResult)
            {
                Assert.True(qaokResult.Value != null);
                if (qaokResult.Value is ServiceQueryResponse<TDto> querycount)
                {
                    Assert.True(querycount.List.Count == 1 + existingCount);
                    Assert.True(querycount.List.Count == 1 + existingCount);
                    var foundObject = TestManager.FindObject(querycount.List, dto);
                    Assert.True(foundObject != null);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public virtual async Task Query_ByProperties()
        {
            var controller = TestManager.GetController(SystemManager.ServiceProvider);

            var model = TestManager.GetMaximumDataObject();
            var dto = await CreateBase(model);

            var queries = TestManager.GetQueriesForObject(dto);
            foreach (var query in queries)
            {
                // Query by each property
                var respQueryAll = await controller.Query(query);
                if (respQueryAll is OkObjectResult qaokResult)
                {
                    Assert.True(qaokResult.Value != null);
                    if (qaokResult.Value is ServiceQueryResponse<TDto> querycount)
                    {
                        Assert.True(querycount.List.Count >= 1);
                        var foundObject = TestManager.FindObject(querycount.List, dto);
                        Assert.True(foundObject != null);
                    }
                    else
                        Assert.Fail("");
                }
                else
                    Assert.Fail("");
            }
        }
    }
}