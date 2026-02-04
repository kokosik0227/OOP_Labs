using System.Collections.Generic;
using Models;
using IOLib;
using Tests.Mocks;

namespace Tests
{
    public class EntityServiceTests
    {
        public EntityService CreateService(out MockProvider mockProvider)
        {
            mockProvider = new MockProvider();
            var context = new EntityContext(mockProvider);
            return new EntityService(context);
        }

        [Fact]
        public void CountExcellentFemale5Course_ShouldCountCorrectly()
        {
            var service = CreateService(out _);

            var students = new List<Student>
            {
                new Student { LastName="Ivanova", Course=5, Sex="F", AverageScore=5 },
                new Student { LastName="Petrenko", Course=5, Sex="F", AverageScore=4 },
                new Student { LastName="Kovalenko", Course=5, Sex="F", AverageScore=5 },
                new Student { LastName="Oleg", Course=4, Sex="F", AverageScore=5 },
                new Student { LastName="Sergiy", Course=5, Sex="M", AverageScore=5 }
            };

            var count = service.Count(students);
            Assert.Equal(2, count);
        }

        [Fact]
        public void Count_EmptyList_ShouldReturnZero()
        {
            var service = CreateService(out _);
            var students = new List<Student>();
            var count = service.Count(students);
            Assert.Equal(0, count);
        }

        [Fact]
        public void SaveAll_ShouldStoreAllEntities()
        {
            var service = CreateService(out var mockProvider);

            var students = new List<Student>
            {
                new Student { LastName="Ivanova", Course=5, Sex="F", AverageScore=5 }
            };

            var dentists = new List<Dentist>
            {
                new Dentist { Name="Dr. Smith", HasTools=true }
            };

            var storytellers = new List<Storyteller>
            {
                new Storyteller { Name="Anna", HasTools=true }
            };

            service.SaveAll(students, dentists, storytellers, "mockpath");

            var wrapper = (EntityWrapper)mockProvider.StoredData;
            Assert.Single(wrapper.Students);
            Assert.Single(wrapper.Dentists);
            Assert.Single(wrapper.Storytellers);
        }

        [Fact]
        public void LoadAll_ShouldReturnAllEntities()
        {
            var service = CreateService(out var mockProvider);

            var wrapper = new EntityWrapper
            {
                Students = new List<StudentEntity>
                {
                    new StudentEntity { LastName="Ivanova" }
                },
                Dentists = new List<DentistEntity>
                {
                    new DentistEntity { Name="Dr. Smith" }
                },
                Storytellers = new List<StorytellerEntity>
                {
                    new StorytellerEntity { Name="Anna" }
                }
            };
            mockProvider.StoredData = wrapper;

            service.LoadAll(out var students, out var dentists, out var storytellers, "mockpath");

            Assert.Single(students);
            Assert.Equal("Ivanova", students[0].LastName);

            Assert.Single(dentists);
            Assert.Equal("Dr. Smith", dentists[0].Name);

            Assert.Single(storytellers);
            Assert.Equal("Anna", storytellers[0].Name);
        }

        [Fact]
        public void SaveAndLoad_EmptyLists_ShouldWork()
        {
            var service = CreateService(out var mockProvider);

            var emptyStudents = new List<Student>();
            var emptyDentists = new List<Dentist>();
            var emptyStorytellers = new List<Storyteller>();

            service.SaveAll(emptyStudents, emptyDentists, emptyStorytellers, "mockpath");

            service.LoadAll(out var students, out var dentists, out var storytellers, "mockpath");

            Assert.Empty(students);
            Assert.Empty(dentists);
            Assert.Empty(storytellers);
        }

        [Fact]
        public void CountExcellentFemale5Course_MixedCases()
        {
            var service = CreateService(out _);

            var students = new List<Student>
            {
                new Student { LastName="F1", Course=5, Sex="f", AverageScore=5 },
                new Student { LastName="F2", Course=5, Sex="FEMALE", AverageScore=5 },
                new Student { LastName="F3", Course=5, Sex="Female", AverageScore=4.5 },
                new Student { LastName="M1", Course=5, Sex="M", AverageScore=5 },
                new Student { LastName="F4", Course=4, Sex="F", AverageScore=5 }
            };

            var count = service.Count(students);
            Assert.Equal(2, count);
        }
    }
}