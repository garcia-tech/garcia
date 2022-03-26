using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace GarciaCore.CodeGenerator.Tests
{
    public class SolutionTests
    {
        private ISolutionService _solutionService;
        private readonly ITestOutputHelper _output;

        public SolutionTests(ITestOutputHelper output)
        {
            _solutionService = new SolutionService();
            _output = output;
        }

        [Fact]
        public async Task CreateSolutionAsync()
        {
            var json = await _solutionService.GetSampleJsonAsync();
            var result = await _solutionService.CreateSolutionAsync(json);
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetSolutionJsonAsync()
        {
            var solution = await _solutionService.CreateSampleSolutionAsync();
            var result = await _solutionService.GetSolutionJsonAsync(solution);
            var solution2 = JsonSerializer.Deserialize<Solution>(result);
            result.ShouldNotBeNullOrEmpty();
            _output.WriteLine(result);
        }

        [Fact]
        public async Task GetSampleJsonAsync()
        {
            var result = await _solutionService.GetSampleJsonAsync();
            result.ShouldNotBeNullOrEmpty();
            _output.WriteLine(result);
        }

        [Fact]
        public async Task CreateSampleSolutionAsync()
        {
            var result = await _solutionService.CreateSampleSolutionAsync();
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task GenerateItemsAsync()
        {
            var items = new List<Item>()
            {
                new Item()
                {
                    Name = "User",
                    IdType = IdType.Guid,
                    Properties = new List<ItemProperty>()
                    {
                        new ItemProperty() { Name = "Name", Type = ItemPropertyType.String, MappingType = ItemPropertyMappingType.Property },
                        new ItemProperty() { Name = "Surname", Type = ItemPropertyType.String, MappingType = ItemPropertyMappingType.Property },
                        new ItemProperty() { Name = "HomeAddress", Type = ItemPropertyType.Class, MappingType = ItemPropertyMappingType.Property, InnerType = new Item() { Name = "Address" } },
                        new ItemProperty() { Name = "WorkAddress", Type = ItemPropertyType.Class, MappingType = ItemPropertyMappingType.Property, InnerType = new Item() { Name = "Address" } }
                    }
                },
                new Item()
                {
                    Name = "Address",
                    IdType = IdType.Int,
                    Properties = new List<ItemProperty>()
                    {
                        new ItemProperty() { Name = "Addressline", Type = ItemPropertyType.String, MappingType = ItemPropertyMappingType.Property },
                    }
                }
            };

            //Console.WriteLine(JsonConvert.SerializeObject(items, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
            _output.WriteLine(JsonSerializer.Serialize(items));
            var solution = await _solutionService.CreateSampleSolutionAsync();
            var result = await solution.Generate(items);
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();

            foreach (var item in result)
            {
                item.Folder.ShouldNotBeNullOrEmpty();
                item.Generator.ShouldNotBeNull();
                item.Code.ShouldNotBeNullOrEmpty();
                _output.WriteLine($"// Folder: {item.Folder}, Generator: {item.Generator.GetType().Name}");
                _output.WriteLine(item.Code);
            }

            foreach (var item in result)
            {
                var allMessages = item.AllMessages;

                if (!string.IsNullOrEmpty(allMessages))
                    _output.WriteLine($"// Messages: {allMessages}");
            }
        }

        [Fact]
        public async Task CreateSampleSolution2Async()
        {
            var result = await _solutionService.CreateSampleSolution2Async();
            result.ShouldNotBeNull();
            result.Projects.ShouldNotBeNull();
            var json = JsonSerializer.Serialize(result, new JsonSerializerOptions() { WriteIndented = true });
            _output.WriteLine(json);

            var solution = await _solutionService.CreateSolutionAsync(json);
            solution.ShouldNotBeNull();
            solution.Solution.ShouldNotBeNull();
            solution.Messages.ShouldNotBeNull();
            _output.WriteLine(JsonSerializer.Serialize(solution.Solution, new JsonSerializerOptions() { WriteIndented = true, ReferenceHandler = ReferenceHandler.Preserve }));

            foreach (var message in solution.Messages)
            {
                _output.WriteLine(message);
            }
        }

        [Fact]
        public async Task CreateItemsFromFileAsync()
        {
            //var itemsModel = new ItemsModel()
            //{
            //    Items = new List<ItemModel>()
            //    {
            //        new ItemModel()
            //        {
            //            AddApplication = true,
            //            IdType = IdType.Long.ToString(),
            //            IsEnum = false,
            //            Name = "Content",
            //            Properties = new List<ItemPropertyModel>()
            //            {
            //                new ItemPropertyModel()
            //                {
            //                    InnerType = "DateTime",
            //                    MappingType = "Property",
            //                },
            //                new ItemPropertyModel()
            //                {
            //                    InnerType = "ContentItem",
            //                    MappingType = "List",
            //                    Type = "Class",
            //                    Name = "Items"
            //                }
            //            }
            //        }
            //    }
            //};

            //_output.WriteLine(JsonSerializer.Serialize(itemsModel));
            //return;
            var json = await File.ReadAllTextAsync("Farmi.json");
            var solution = await _solutionService.CreateSolutionAsync(json);
            var itemsJson = await File.ReadAllTextAsync("FarmiItems.json");
            var items = await _solutionService.CreateItemsAsync(itemsJson);
            var result = await solution.Solution.Generate(items);
        }

        [Fact]
        public async Task CreateSolutionFromFileAsync()
        {
            var json = await File.ReadAllTextAsync("Farmi.json");
            //_output.WriteLine(json);
            var solution = await _solutionService.CreateSolutionAsync(json);
            solution.ShouldNotBeNull();
            solution.Solution.ShouldNotBeNull();
            solution.Messages.ShouldNotBeNull();
            //_output.WriteLine(JsonSerializer.Serialize(solution.Solution, new JsonSerializerOptions() { WriteIndented = true, ReferenceHandler = ReferenceHandler.Preserve }));

            foreach (var message in solution.Messages)
            {
                _output.WriteLine(message);
            }

            var items = new List<Item>()
            {
                new Item()
                {
                    Name = "Content",
                    IdType = IdType.Long,
                    Properties = new List<ItemProperty>()
                    {
                        new ItemProperty() { Name = "Date", Type = ItemPropertyType.DateTime, MappingType = ItemPropertyMappingType.Property },
                        new ItemProperty() { Name = "Files", Type = ItemPropertyType.Class, MappingType = ItemPropertyMappingType.List, InnerType = new Item() { Name = "ContentFile" } },
                        new ItemProperty() { Name = "Milestones", Type = ItemPropertyType.Class, MappingType = ItemPropertyMappingType.List, InnerType = new Item() { Name = "ContentMilestone" } },
                    },
                    AddApplication = true,
                    MultipartUpload = true
                },
                new Item()
                {
                    Name = "ContentFile",
                    IdType = IdType.Long,
                    Properties = new List<ItemProperty>()
                    {
                        new ItemProperty() { Name = "Url", Type = ItemPropertyType.String, MappingType = ItemPropertyMappingType.Property },
                        new ItemProperty() { Name = "Description", Type = ItemPropertyType.String, MappingType = ItemPropertyMappingType.Property },
                    }
                },
                new Item()
                {
                    Name = "ContentMilestone",
                    IdType = IdType.Long,
                    Properties = new List<ItemProperty>()
                    {
                        new ItemProperty() { Name = "FinishedOn", Type = ItemPropertyType.DateTime, MappingType = ItemPropertyMappingType.Property },
                    }
                },
                new Item()
                {
                    Name = "Milestone",
                    IdType = IdType.Long,
                    Properties = new List<ItemProperty>()
                    {
                        new ItemProperty() { Name = "Title", Type = ItemPropertyType.String, MappingType = ItemPropertyMappingType.Property },
                        new ItemProperty() { Name = "Description", Type = ItemPropertyType.String, MappingType = ItemPropertyMappingType.Property },
                        new ItemProperty() { Name = "Logo", Type = ItemPropertyType.String, MappingType = ItemPropertyMappingType.Property },
                    },
                    AddApplication = true
                },
            };

            _output.WriteLine(JsonSerializer.Serialize(items));
            var result = await solution.Solution.Generate(items);
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();

            foreach (var item in result)
            {
                var allMessages = item.AllMessages;

                if (!string.IsNullOrEmpty(allMessages))
                    _output.WriteLine($"// Messages: {allMessages}");
            }

            foreach (var item in result)
            {
                item.Folder.ShouldNotBeNullOrEmpty();
                item.Generator.ShouldNotBeNull();
                item.Code.ShouldNotBeNullOrEmpty();
                // _output.WriteLine($"// Folder: {item.Folder}, File: {item.File},  Generator: {item.Generator.GetType().Name}");
                // _output.WriteLine(item.Code);
                Directory.CreateDirectory(item.Folder);
                await File.WriteAllTextAsync($"{item.Folder}\\{item.File}", item.Code);
            }
        }
    }
}
